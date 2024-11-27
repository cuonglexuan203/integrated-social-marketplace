namespace Chat.Core.Exceptions
{
    public class ChatRoomNotFoundException : NotFoundException
    {
        public ChatRoomNotFoundException(string chatRoomId) : base($"Chat room with ID {chatRoomId} was not found.")
        {
        }
    }
}
