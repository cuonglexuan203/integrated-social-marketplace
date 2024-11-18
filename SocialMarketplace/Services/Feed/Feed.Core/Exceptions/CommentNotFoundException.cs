

namespace Feed.Core.Exceptions
{
    public class CommentNotFoundException : NotFoundException
    {
        public CommentNotFoundException(string commentId) : base($"Comment with ID {commentId} was not found.")
        {

        }
    }
}
