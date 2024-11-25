using System.ComponentModel.DataAnnotations;

namespace VideoGamesCatalog.Core.Data.Models
{
    public class VideoGameImage : IDataEntity
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string ImageUrl { get; set; }
    }
}
