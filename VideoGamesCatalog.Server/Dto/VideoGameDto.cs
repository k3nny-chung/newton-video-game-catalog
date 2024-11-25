using System.ComponentModel.DataAnnotations;
using VideoGamesCatalog.Core.Data.Models;

namespace VideoGamesCatalog.Server.Dto
{
    public class VideoGameDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [MaxLength(400)]
        public string Description { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int AgeRatingId { get; set; }

        [Required]
        public int PlatformId { get; set; }

        public string Platform { get; set; }
       
        public int? ImageId { get; set; }
        public string ImageUrl { get; set; }

        [Required]
        public List<GenreDto> Genres { get; set; }
    }
}
