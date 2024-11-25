namespace VideoGamesCatalog.Core.Services
{
    public class PagedResult<T> where T: class
    {
        public List<T> Results { get; set; } = new List<T>();
        public int TotalCount { get; set; } 
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
