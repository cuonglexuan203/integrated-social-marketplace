using Microsoft.AspNetCore.Identity;
namespace Identity.Infrastructure.Identity
{
    public class ApplicationRole: IdentityRole
    {
        public ApplicationRole() { }
        public ApplicationRole(string roleName): base(roleName)
        {
        }
    }
}
