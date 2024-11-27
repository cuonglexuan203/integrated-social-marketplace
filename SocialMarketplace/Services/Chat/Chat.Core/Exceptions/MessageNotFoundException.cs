namespace Chat.Core.Exceptions
{
    public class MessageNotFoundException : NotFoundException
    {
        public MessageNotFoundException(string messageId) : base($"Message with ID {messageId} was not found.")
        {

        }
    }
}
