
namespace Feed.Application.Common.Models
{
    public class ReturnResult<T>
    {
        public T Result { get; set; }
        public string Message { get; set; }

    }
}
