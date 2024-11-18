
namespace Feed.Core.Exceptions
{
    public class PostNotFoundException : NotFoundException
    {
        public PostNotFoundException(string postId) : base($"Post with ID {postId} was not found.")
        {
        }
    }
}
