using VideoGamesCatalog.Core.Data.Models;
using VideoGamesCatalog.Core.Data.Repository;

namespace VideoGamesCatalog.Core.Services
{
    public interface IAgeRatingService
    {
        Task<List<AgeRating>> GetAllAgeRatings();
    }

    public class AgeRatingService : IAgeRatingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AgeRatingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<AgeRating>> GetAllAgeRatings()
        {
            return await _unitOfWork.AgeRatingRepository.Get();
        }
    }
}
