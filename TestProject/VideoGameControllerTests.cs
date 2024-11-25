using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VideoGamesCatalog.Core.Services;
using VideoGamesCatalog.Server.Controllers;
using DataModels = VideoGamesCatalog.Core.Data.Models;
using Dto = VideoGamesCatalog.Server.Dto;

namespace TestProject
{
    public class VideoGameControllerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IVideoGameService> _mockVideoGameService; 
        private readonly VideoGameController _controller;
        private DataModels.VideoGame _videoGameData = new DataModels.VideoGame
        {
            Id = 1,
            Title = "Test1",
            Description = "description 1",
            AgeRatingId = 201,
            PlatformId = 501,
            Price = 10.99m,
            ReleaseDate = new DateTime(2024, 11, 24),
            ImageId = 283,
            Genres = new List<DataModels.Genre>
                            {
                                new DataModels.Genre { Id = 9201, Name = "Horror" },
                                new DataModels.Genre { Id = 2531, Name = "Action" }
                            }
        };
        private Dto.VideoGameDto _videoGameDto = new Dto.VideoGameDto
        {
            Id = 200,
            Title = "Test1",
            Description = "description 1",
            AgeRatingId = 201,
            PlatformId = 501,
            Price = 10.99m,
            ReleaseDate = new DateTime(2024, 11, 24),
            ImageId = 283,
            Genres = new List<Dto.GenreDto>
                            {
                                new Dto.GenreDto { Id = 9201, Name = "Horror" },
                                new Dto.GenreDto { Id = 2531, Name = "Action" }
                            }
        };

        public VideoGameControllerTests()
        {
            _mapper = MapperBuilder.Create();
            _mockVideoGameService = new Mock<IVideoGameService>();
            _controller = new VideoGameController(_mockVideoGameService.Object, _mapper);
        }

        [Fact]
        public async Task Get_Returns_PagedResult()
        {
            _mockVideoGameService.Setup(s => s.FindVideoGames(It.IsAny<VideoGameSearchFilter>()))
                .ReturnsAsync(new PagedResult<DataModels.VideoGame>
                {
                    PageNumber = 1,
                    PageSize = 50,
                    TotalCount = 10,
                    Results =
                    [
                        _videoGameData
                    ]
                });

            var actual = await _controller.Get(new VideoGameSearchFilter { Title = "The Last of Use" });

            Assert.IsType<ActionResult<PagedResult<Dto.VideoGameDto>>>(actual);
            Assert.True(actual.Value.TotalCount == 10);
            Assert.True(actual.Value.Results.First().Id == 1);
            Assert.Contains(actual.Value.Results, v => v.Genres.Count() == 2);
        }

        [Fact]
        public async Task GetByID_Returns_NotFound()
        {
            _mockVideoGameService.Setup(s => s.GetVideoGame(It.IsAny<int>()))
                .ReturnsAsync((int id) => {
                    if (id == _videoGameData.Id)
                        return _videoGameData;
                    else return null;
                    });

            var actual = await _controller.Get(9999);

            Assert.IsType<NotFoundResult>(actual.Result);
        }

        [Fact]
        public async Task Post_Returns_CreatedAtResult()
        {
            _mockVideoGameService.Setup(s => s.AddVideoGame(It.IsAny<DataModels.VideoGame>()))
                .ReturnsAsync((DataModels.VideoGame vg) =>  vg);

            var actual = await _controller.Post(_videoGameDto);

            Assert.IsType<CreatedAtActionResult>(actual.Result);
        }

        [Fact]
        public async Task Put_Returns_NotFound()
        {
            _mockVideoGameService.Setup(s => s.UpdateVideoGame(It.IsAny<DataModels.VideoGame>()))
                .ReturnsAsync((DataModels.VideoGame vg) =>
                {
                    if (vg.Id == _videoGameData.Id)
                        return _videoGameData;
                    else
                        return null;
                });

            var actual = await _controller.Put(7289, new Dto.VideoGameDto { Id = 2231 });

            Assert.IsType<NotFoundResult>(actual);
        }

        [Fact]
        public async Task Delete_Returns_Ok()
        {
            _mockVideoGameService.Setup(s => s.RemoveVideoGame(It.IsAny<int>()))
                .ReturnsAsync(true);
            var actual = await _controller.Delete(2837);

            Assert.IsType<OkResult>(actual);
        }

        [Fact]
        public async Task Delete_Returns_NotFound()
        {
            _mockVideoGameService.Setup(s => s.RemoveVideoGame(It.IsAny<int>()))
                .ReturnsAsync(false);
            var actual = await _controller.Delete(28112);

            Assert.IsType<NotFoundResult>(actual);
        }
    }
}