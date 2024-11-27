namespace Chat.Infrastructure.Configurations
{
    public class DatabaseSettings
    {
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
        public string? ChatRoomCollection { get; set; }
        public string? MessageCollection { get; set; }
        public string? ChatParticipantCollection { get; set; }
    }
}
