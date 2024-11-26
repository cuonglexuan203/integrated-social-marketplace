
using Feed.Application.Mappers;
using Feed.Core.Specs;

namespace Feed.Application.Extensions
{
    public static class PaginationExtension
    {
        public static Pagination<TOutput> Map<TInput, TOutput>(this Pagination<TInput> inputPage)
            where TInput : class
            where TOutput : class
        {
            var outputPage = FeedMapper.Mapper.Map<IReadOnlyList<TOutput>>(inputPage.Data);
            return new Pagination<TOutput>(inputPage.PageIndex, inputPage.PageSize, inputPage.Count, outputPage);
        }
    }
}
