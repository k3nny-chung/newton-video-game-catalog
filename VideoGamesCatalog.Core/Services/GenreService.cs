using VideoGamesCatalog.Core.Data.Models;
using VideoGamesCatalog.Core.Data.Repository;

namespace VideoGamesCatalog.Core.Services
{
    public interface IGenreService
    {
        Task<List<Genre>> GetAllGenres();
    }

    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenreService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Genre>> GetAllGenres()
        {
            return await _unitOfWork.GenreRepository.Get();
        }
    }
}
