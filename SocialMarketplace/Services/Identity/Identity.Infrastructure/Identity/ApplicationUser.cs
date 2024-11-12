using Microsoft.AspNetCore.Identity;
namespace Identity.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
