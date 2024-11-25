using System.Linq.Expressions;
using VideoGamesCatalog.Core.Data.Models;
using VideoGamesCatalog.Core.Data.Repository;

namespace VideoGamesCatalog.Core.Services
{
    public interface IVideoGameService
    {
        Task<VideoGame> AddVideoGame(VideoGame videoGame);
        Task<PagedResult<VideoGame>> FindVideoGames(VideoGameSearchFilter searchFilter);
        Task<VideoGame> GetVideoGame(int id);
        Task<bool> RemoveVideoGame(int id);
        Task<VideoGame> UpdateVideoGame(VideoGame videoGame);
    }

    public class VideoGameService : IVideoGameService
    {
        private const string IncludeProperties = "AgeRating,Platform,Genres,Image";
        private readonly IUnitOfWork _unitOfWork;

        public VideoGameService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<VideoGame>> FindVideoGames(VideoGameSearchFilter searchFilter)
        {
            List<Expression<Func<VideoGame, bool>>> filters = new();
            if (!string.IsNullOrEmpty(searchFilter.Title))
            {
                Expression<Func<VideoGame, bool>> containsTitle = (vg) => vg.Title.Contains(searchFilter.Title);
                filters.Add(containsTitle);
            }

            if (searchFilter.GenreIDs.Length > 0)
            {
                Expression<Func<VideoGame, bool>> hasGenres = (vg) => vg.Genres.Any(g => searchFilter.GenreIDs.Contains(g.Id));
                filters.Add(hasGenres);
            }

            if (searchFilter.AgeRatingIDs.Length > 0)
            {
                Expression<Func<VideoGame, bool>> hasAgeRatings = (vg) => searchFilter.AgeRatingIDs.Contains(vg.AgeRating.Id);
                filters.Add(hasAgeRatings);
            }

            if (searchFilter.PlatformID.HasValue)
            {
                Expression<Func<VideoGame, bool>> hasPlaftorm = (vg) => vg.Platform.Id == searchFilter.PlatformID.Value;
                filters.Add(hasPlaftorm);
            }

            Func<IQueryable<VideoGame>, IOrderedQueryable<VideoGame>> orderBy = (videoGame) => videoGame.OrderBy(v => v.Title);
            var offset = (searchFilter.PageNumber - 1) * searchFilter.PageSize;
            var limit = searchFilter.PageSize;

            var total = await _unitOfWork.VideoGameRepository.Count(filters);
            var result = await _unitOfWork.VideoGameRepository.Get(filters, orderBy, IncludeProperties, offset, limit);
            return new PagedResult<VideoGame>
            {
                Results = result,
                TotalCount = total,
                PageNumber = searchFilter.PageNumber,
                PageSize = searchFilter.PageSize,
            };
        }

        public async Task<VideoGame> GetVideoGame(int id)
        {

            return await _unitOfWork.VideoGameRepository.GetByID(id, IncludeProperties);
        }

        public async Task<VideoGame> AddVideoGame(VideoGame videoGameToAdd)
        {
            // Need to attach existing genres 
            var genresToAdd = new List<Genre>();
            foreach (var g in videoGameToAdd.Genres)
            {
                var genre = await _unitOfWork.GenreRepository.GetByID(g.Id);
                if (genre != null)
                {
                    genresToAdd.Add(genre);
                }
            }

            videoGameToAdd.Genres = genresToAdd;
            await _unitOfWork.VideoGameRepository.Insert(videoGameToAdd);
            await _unitOfWork.Save();
            return videoGameToAdd;
        }

        public async Task<VideoGame> UpdateVideoGame(VideoGame videoGameToUpdate)
        {
            var videoGame = await _unitOfWork.VideoGameRepository.GetByID(videoGameToUpdate.Id, "Genres");
            if (videoGame == null)
            {
                return null;
            }

            videoGame.Title = videoGameToUpdate.Title;
            videoGame.Description = videoGameToUpdate.Description;
            videoGame.ReleaseDate = videoGameToUpdate.ReleaseDate;
            videoGame.Price = videoGameToUpdate.Price;
            videoGame.AgeRatingId = videoGameToUpdate.AgeRatingId;
            videoGame.PlatformId = videoGameToUpdate.PlatformId;
            videoGame.ImageId = videoGameToUpdate.ImageId;

            var currentGenreIds = videoGame.Genres.Select(g => g.Id).ToList();
            var genresToAdd = videoGameToUpdate.Genres.Select(g => g.Id)
                                .Except(currentGenreIds)
                                .Select(id => new Genre { Id = id })
                                .ToList();

            var genresToRemove = videoGame.Genres.Where(g => !videoGameToUpdate.Genres.Select(g => g.Id).Contains(g.Id))
                                  .ToList();

            foreach (var g in genresToAdd)
            {
                var genre = await _unitOfWork.GenreRepository.GetByID(g.Id);
                if (genre != null)
                {
                    videoGame.Genres.Add(genre);
                }
            }

            foreach (var g in genresToRemove)
            {
                videoGame.Genres.Remove(g);
            }

            await _unitOfWork.Save();
            return videoGame;
        }

        public async Task<bool> RemoveVideoGame(int id)
        {
            var videoGame = await _unitOfWork.VideoGameRepository.GetByID(id, "Genres");
            if (videoGame == null)
            {
                return false;
            }

            videoGame.Genres.Clear();

            _unitOfWork.VideoGameRepository.Delete(videoGame);
            await _unitOfWork.Save();
            return true;
        }

    }
}
