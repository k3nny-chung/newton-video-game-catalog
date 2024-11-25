namespace VideoGamesCatalog.Core.Services
{
    public record VideoGameSearchFilter
    {
        private const int PAGE_SIZE_MAX = 50;
        public string Title { get; set; }
        public int[] GenreIDs { get; set; } = [];
        public int[] AgeRatingIDs { get; set; } = [];
        public int? PlatformID { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = PAGE_SIZE_MAX;
    }
}
