namespace Chat.Application.Dtos
{
    public class MessageReadInfoDto
    {
        public string UserId { get; set; }
        public DateTimeOffset SeenAt { get; set; }
    }
}
