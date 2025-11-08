namespace Backend.Application.Specifications
{
    public class PaginationParams
    {
        public int PageIndex { get; set; } = 1;
        private const int MaxPageSize = 100;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public string? Search { get; set; }

        public PaginationParams(int? pageIndex, int? pageSize, string? search)
        {
            PageIndex = pageIndex is null ? 1 : pageIndex.Value;
            PageSize = pageSize is null ? 10 : pageIndex.Value;
            Search = search;
        }
        public PaginationParams()
        {

        }
    }
}
