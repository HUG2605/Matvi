namespace Matvi.Back.Application.Common.Grid
{
    public class FilterGrd
    {
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? SortBy { get; set; }
        public bool SortDesc { get; set; }
    }
}
