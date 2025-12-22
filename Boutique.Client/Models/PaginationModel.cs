namespace Boutique.Client.Models
{
    public class PaginationModel<T>
    {
        public List<T> Items { get; set; } = new();
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
    }

    public static class PaginationExtensions
    {
        public static PaginationModel<T> ToPaginatedList<T>(this IEnumerable<T> source, int page, int pageSize)
        {
            var totalItems = source.Count();
            var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PaginationModel<T>
            {
                Items = items,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };
        }
    }
}