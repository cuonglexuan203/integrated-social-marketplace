using Feed.Core.Common.AuditProperties;

namespace Feed.Core.Entities
{
    public class User: IIdentifier
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public ICollection<string> Roles { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string ProfileUrl { get; set; }
    }
}
