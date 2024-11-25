using System.ComponentModel.DataAnnotations;

namespace VideoGamesCatalog.Core.Data.Models
{
    public class Genre : IDataEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public List<VideoGame> VideoGames { get; set; }
    }
}
