
namespace Chat.Core.ValueObjects
{
    public class MessageReadInfo
    {
        public string UserId { get; set; }
        public DateTimeOffset SeenAt { get; set; }
    }
}
