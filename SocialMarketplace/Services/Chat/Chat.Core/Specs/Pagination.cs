namespace Chat.Core.Specs
{
    public class Pagination<T> where T : class
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public long Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }

        public Pagination()
        {

        }

        public Pagination(int pageIndex, int pageSize, long count, IReadOnlyList<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }
    }
}
