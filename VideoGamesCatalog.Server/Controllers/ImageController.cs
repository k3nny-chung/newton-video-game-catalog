using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VideoGamesCatalog.Core.Data.Models;
using VideoGamesCatalog.Core.Services;
using VideoGamesCatalog.Server.Dto;


namespace VideoGamesCatalog.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public ImageController(IWebHostEnvironment environment, IImageService imageService, IMapper mapper)
        {
            _environment = environment;
            _imageService = imageService;
            _mapper = mapper;
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Replace("-", "")
                      + Path.GetExtension(fileName);
        }

        [HttpPost]
        public async Task<VideoGameImageDto> Upload(IFormFile imageFile)
        {
            //TODO: Check if file is an image file
            var uniqueFileName = GetUniqueFileName(imageFile.FileName);
            var imagesDir = Path.Combine(_environment.WebRootPath, "images");
            var filePath = Path.Combine(imagesDir, uniqueFileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            imageFile.CopyTo(stream);

            var result = await _imageService.AddVideoGameImage(new VideoGameImage { ImageUrl = $"/images/{uniqueFileName}" });
            return _mapper.Map<VideoGameImageDto>(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var imageUrl = await _imageService.RemoveVideoGameImage(id);
            if (imageUrl == null)
            {
                return NotFound();
            }

            var filePath = _environment.WebRootPath + imageUrl;
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            return Ok();
        }
       
    }
}
