
namespace Feed.Core.Specs
{
    public class CommentSpecParams
    {
        private const int MaxPageSize = 100;
        private int _pageSize = 10;
        public int PageSize { get => _pageSize; set => _pageSize = value > MaxPageSize ? MaxPageSize : value; }
        public int PageIndex { get; set; } = 1;
        public string? Sort { get; set; }
        public string? Search { get; set; }
    }
}
