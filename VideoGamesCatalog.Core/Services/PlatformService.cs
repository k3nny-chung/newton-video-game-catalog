using VideoGamesCatalog.Core.Data.Models;
using VideoGamesCatalog.Core.Data.Repository;

namespace VideoGamesCatalog.Core.Services
{
    public interface IPlatformService
    {
        Task<List<Platform>> GetAllPlatforms();
    }

    public class PlatformService : IPlatformService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlatformService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Platform>> GetAllPlatforms()
        {
            return await _unitOfWork.PlatformRepository.Get();
        }
    }
}
