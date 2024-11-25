using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideoGamesCatalog.Core.Data.Models
{
    public class VideoGame : IDataEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [MaxLength(400)]
        public string Description { get; set; }

        public DateTime ReleaseDate { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public int AgeRatingId { get; set; }
        public AgeRating AgeRating { get; set; }

        public int PlatformId { get; set; }
        public Platform Platform { get; set; }

        public int? ImageId { get; set; }
        public VideoGameImage Image { get; set; } = null;

        public List<Genre> Genres { get; set; } = [];
        
    }
}
