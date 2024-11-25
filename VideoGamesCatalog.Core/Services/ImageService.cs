using VideoGamesCatalog.Core.Data.Models;
using VideoGamesCatalog.Core.Data.Repository;

namespace VideoGamesCatalog.Core.Services
{
    public interface IImageService
    {
        Task<VideoGameImage> AddVideoGameImage(VideoGameImage videoGameImage);
        Task<VideoGameImage> GetVideoGameImage(int id);
        Task<string> RemoveVideoGameImage(int id);
    }

    public class ImageService : IImageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ImageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<VideoGameImage> GetVideoGameImage(int id)
        {
            return await _unitOfWork.VideoGameImageRepository.GetByID(id);
        }

        public async Task<VideoGameImage> AddVideoGameImage(VideoGameImage videoGameImage)
        {
            await _unitOfWork.VideoGameImageRepository.Insert(videoGameImage);
            await _unitOfWork.Save();
            return videoGameImage;
        }

        public async Task<string> RemoveVideoGameImage(int id)
        {
            var vgImage = await _unitOfWork.VideoGameImageRepository.GetByID(id, "");
            if (vgImage == null)
            {
                return null;
            }

            // Look for any video games that a have a reference to this image
            var videoGames = await _unitOfWork.VideoGameRepository.Get([vg => vg.ImageId == id]);
            if (videoGames != null)
            {
                foreach (var vg in videoGames)
                {
                    vg.ImageId = null;
                    _unitOfWork.VideoGameRepository.Update(vg);
                }

                await _unitOfWork.Save();
            }

            var url = vgImage.ImageUrl;
            _unitOfWork.VideoGameImageRepository.Delete(vgImage);
            await _unitOfWork.Save();
            return url;
        }
    }
}
