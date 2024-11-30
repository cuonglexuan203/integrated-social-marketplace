using Identity.Core.Enums;
using Microsoft.AspNetCore.Identity;
namespace Identity.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = default!;
        public string ProfilePictureUrl { get; set; } = "https://res.cloudinary.com/dpnh3ytfm/image/upload/v1731844682/anonymous_wgerg5.jpg";
        public string? ProfileUrl { get; set; }
        public Gender Gender { get; set; } = Core.Enums.Gender.Unknown;
        public DateTime? DateOfBirth { get; set; }
        public IList<string> Interests { get; set; } = new List<string>();
        public string? City { get; set; }
        public string? Country { get; set; }
        // Credibility
        public float BaseCredibility { get; set; } = 50;
        public float CredibilityScore { get; set; } = 50;
        public virtual ICollection<UserFollow> Followers { get; set; } = new List<UserFollow>();
        public virtual ICollection<UserFollow> Following { get; set; } = new List<UserFollow>();
    }
}
