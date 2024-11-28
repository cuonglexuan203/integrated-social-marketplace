namespace Chat.Application.Dtos
{
    public class PostReferenceDto
    {
        public string Id { get; set; }
        public string ContentText { get; set; }
        public string Link { get; set; }
        public ICollection<MediaDto> Media { get; set; } = new List<MediaDto>();
    }
}
