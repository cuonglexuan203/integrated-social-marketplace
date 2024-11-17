using Microsoft.AspNetCore.Identity;
namespace Identity.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string ProfilePictureUrl { get; set; } = "https://res.cloudinary.com/dpnh3ytfm/image/upload/v1731844682/anonymous_wgerg5.jpg";
        public string? ProfileUrl { get; set; }
    }
}
