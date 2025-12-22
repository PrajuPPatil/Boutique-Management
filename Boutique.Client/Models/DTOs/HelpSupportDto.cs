namespace Boutique.Client.Models.DTOs
{
    public class HelpArticleDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string[] Tags { get; set; } = Array.Empty<string>();
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int ViewCount { get; set; }
        public bool IsPopular { get; set; }
        public string Difficulty { get; set; } = string.Empty;
        public int EstimatedReadTime { get; set; }
    }

    public class HelpCategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public int ArticleCount { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class HelpSearchFilterDto
    {
        public string SearchTerm { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public bool ShowPopularOnly { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "relevance";
    }

    public class PaginatedHelpResultDto
    {
        public List<HelpArticleDto> Articles { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}