using Moq;
using System.Linq.Expressions;
using VideoGamesCatalog.Core.Data.Models;
using VideoGamesCatalog.Core.Data.Repository;
using VideoGamesCatalog.Core.Services;

namespace TestProject
{
    public  class VideoGameServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IRepository<VideoGame>> _mockVideoGameRepo;
        private readonly Mock<IRepository<Genre>> _mockGenreRepo;
        private readonly VideoGameService _service;

        private List<VideoGame> _videoGameStore = [
            new VideoGame {
                Id = 111,
                Title = "Test 1",
                Description = "description 1",
                AgeRatingId = 201,
                PlatformId = 501,
                Price = 10.99m,
                ReleaseDate = new DateTime(2024, 11, 24),
                ImageId = 283,
                Genres = [
                            new Genre { Id = 9201, Name = "Horror" },
                            new Genre { Id = 2531, Name = "Action" }
                        ]
            },
            new VideoGame {
                Id = 222,
                Title = "Resident Evil",
                Description = "description 2",
                AgeRatingId = 201,
                PlatformId = 501,
                Price = 20.99m,
                ReleaseDate = new DateTime(2024, 11, 24),
                ImageId = 287,
                Genres = [
                            new Genre { Id = 9201, Name = "Horror" },
                            new Genre { Id = 2531, Name = "Action" }
                        ]
            },
            new VideoGame {
                Id = 333,
                Title = "Call of Duty",
                Description = "description 3",
                AgeRatingId = 201,
                PlatformId = 501,
                Price = 5.99m,
                ReleaseDate = new DateTime(2024, 11, 24),
                ImageId = 873,
                Genres = [ new Genre { Id = 8332, Name = "Sports" }]
            },
            new VideoGame {
                Id = 444,
                Title = "At the top",
                Description = "description 4",
                AgeRatingId = 201,
                PlatformId = 501,
                Price = 5.99m,
                ReleaseDate = new DateTime(2024, 11, 24),
                ImageId = 873,
                Genres = [ new Genre { Id = 4921, Name = "Family" }]
            },
        ];

        public VideoGameServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockVideoGameRepo = new Mock<IRepository<VideoGame>>();
            _mockGenreRepo = new Mock<IRepository<Genre>>();

            _mockUnitOfWork.Setup(u => u.VideoGameRepository)
                .Returns(_mockVideoGameRepo.Object);

            _mockUnitOfWork.Setup(u => u.GenreRepository)
                .Returns(_mockGenreRepo.Object);

            _mockVideoGameRepo.Setup(r =>
                r.Get(It.IsAny<List<Expression<Func<VideoGame, bool>>>>(), It.IsAny<Func<IQueryable<VideoGame>, IOrderedQueryable<VideoGame>>>(),
                It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))

                .ReturnsAsync(
                    (List<Expression<Func<VideoGame, bool>>> filters,
                    Func<IQueryable<VideoGame>, IOrderedQueryable<VideoGame>> orderBy,
                    string includeProperties,
                    int? offset,
                    int? limit) =>
                    {
                        var query = _videoGameStore.AsQueryable();
                        foreach (var filter in filters)
                        {
                            query = query.Where(filter);
                        }

                        if (orderBy != null)
                        {
                            query = orderBy(query);
                        }

                        if (offset.HasValue)
                        {
                            query = query.Skip(offset.Value);
                        }

                        if (limit.HasValue)
                        {
                            query = query.Take(limit.Value);
                        }

                        return query.ToList();
                    });

            _mockVideoGameRepo.Setup(
                r => r.Count(It.IsAny<List<Expression<Func<VideoGame, bool>>>>(), It.IsAny<Func<IQueryable<VideoGame>, IOrderedQueryable<VideoGame>>>(),
                It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>())
                )

                .ReturnsAsync(
                    (List<Expression<Func<VideoGame, bool>>> filters,
                    Func<IQueryable<VideoGame>, IOrderedQueryable<VideoGame>> orderBy,
                    string includeProperties,
                    int? offset,
                    int? limit) =>
                    {
                        var query = _videoGameStore.AsQueryable();
                        foreach (var filter in filters)
                        {
                            query = query.Where(filter);
                        }
                        return query.Count();
                    });
                
            _service = new VideoGameService( _mockUnitOfWork.Object );
        }

        [Fact]
        public async Task Should_Return_All_Without_Search_Filters()
        {
            var actual = await _service.FindVideoGames(new VideoGameSearchFilter());
            Assert.NotNull(actual);
            Assert.True(actual.TotalCount == _videoGameStore.Count);
            Assert.Contains(actual.Results, actualVg => _videoGameStore.Any(s => s.Id == actualVg.Id));
        }

        [Fact]
        public async Task Should_Return_By_Title_Search()
        {
            var actual = await _service.FindVideoGames(new VideoGameSearchFilter { Title = "Call" });
            Assert.NotNull(actual);
            Assert.True(actual.TotalCount == 1);
            Assert.True(actual.Results.First().Title == "Call of Duty");
        }

        [Fact]
        public async Task Should_Return_Paged_Results_Ordered_By_Title()
        {
            var actual = await _service.FindVideoGames(new VideoGameSearchFilter { 
                PageNumber = 1,
                PageSize = 2,
            });
            Assert.NotNull(actual);
            Assert.True(actual.TotalCount == 4);
            Assert.True(actual.Results.First().Title == "At the top");
        }

        [Fact]
        public async Task Update_Should_Save_And_Sync_Genres()
        {
            var currentVg = new VideoGame 
            {
                Id = 3672,
                Title = "Aliens",
                Description = "description",
                AgeRatingId = 201,
                PlatformId = 501,
                Price = 20.99m,
                ReleaseDate = new DateTime(2024, 11, 24),
                ImageId = 287,
                Genres = [
                            new Genre { Id = 9201, Name = "Horror" },
                            new Genre { Id = 2531, Name = "Action" }
                         ]
            };

            _mockVideoGameRepo.Setup(r => r.GetByID(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(currentVg);

            _mockGenreRepo.Setup(g => g.GetByID(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync((int id, string include) =>
                {
                    var genres = _videoGameStore.SelectMany(vg => vg.Genres).ToList();
                    return genres.FirstOrDefault(g => g.Id == id);
                });

            var toBeUpdated = new VideoGame
            {
                Id = 3672,
                Title = "Aliens 2",
                Description = "description",
                AgeRatingId = 201,
                PlatformId = 501,
                Price = 20.99m,
                ReleaseDate = new DateTime(2024, 11, 24),
                ImageId = 287,
                Genres = [new Genre { Id = 8332, Name = "Sports" }]
            };

            var actual = await _service.UpdateVideoGame(toBeUpdated);
            Assert.True(actual.Genres.Count == 1 && actual.Genres.First().Id == 8332 && actual.Title == "Aliens 2");
            
        }

        [Fact]
        public async Task Should_Insert_Video_Game_And_Genres()
        {
            var toBeAdded = new VideoGame
            {
                Id = 29371,
                Title = "The Smurfs",
                Description = "description",
                AgeRatingId = 201,
                PlatformId = 501,
                Price = 20.99m,
                ReleaseDate = new DateTime(2024, 11, 24),
                ImageId = 287,
                Genres = [new Genre { Id = 4921, Name = "Family" }]
            };

            _mockGenreRepo.Setup(g => g.GetByID(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync((int id, string include) =>
                {
                    var genres = _videoGameStore.SelectMany(vg => vg.Genres).ToList();
                    return genres.FirstOrDefault(g => g.Id == id);
                });

            var actual = await _service.AddVideoGame(toBeAdded);
            Assert.NotNull(actual);
            Assert.True(actual.Genres.Count == 1 && actual.Genres.First().Id == 4921);
        }
    }
}
