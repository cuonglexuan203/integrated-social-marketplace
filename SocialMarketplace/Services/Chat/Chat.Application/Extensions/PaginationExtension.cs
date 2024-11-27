using Chat.Application.Mappers;
using Chat.Core.Specs;

namespace Chat.Application.Extensions
{
    public static class PaginationExtension
    {
        public static Pagination<TOutput> Map<TInput, TOutput>(this Pagination<TInput> inputPage)
            where TInput : class
            where TOutput : class
        {
            var outputPage = ChatMapper.Mapper.Map<IReadOnlyList<TOutput>>(inputPage.Data);
            return new Pagination<TOutput>(inputPage.PageIndex, inputPage.PageSize, inputPage.Count, outputPage);
        }

        public static async Task<Pagination<TOutput>> MapAsync<TInput, TOutput>(this Pagination<TInput> inputPage, Func<IEnumerable<TInput>, CancellationToken, Task<List<TOutput>>> mapperFunc)
            where TInput : class
            where TOutput : class
        {
            var outputPage = await mapperFunc(inputPage.Data, default);
            return new Pagination<TOutput>(inputPage.PageIndex, inputPage.PageSize, inputPage.Count, outputPage);
        }
    }
}
