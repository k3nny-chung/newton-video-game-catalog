using System.ComponentModel.DataAnnotations;

namespace VideoGamesCatalog.Core.Data.Models
{
    public class AgeRating : IDataEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; }
    }
}
