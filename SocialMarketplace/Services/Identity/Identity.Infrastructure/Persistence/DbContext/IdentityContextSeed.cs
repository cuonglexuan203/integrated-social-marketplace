using Identity.Application.Common.Interfaces;
using Identity.Core.Enums;
using Identity.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Identity.Infrastructure.Persistence.DbContext
{
    public class IdentityContextSeed
    {
        public static async Task SeedAsync(IdentityContext context, IServiceProvider services)
        {
            var logger = services.GetService<ILogger<IdentityContextSeed>>();
            var identityService = services.GetRequiredService<IIdentityService>();

            if (!context.Roles.Any())
            {
                foreach (var role in GetRoleSeeds())
                {
                    await identityService.CreateRoleAsync(role);
                }
                logger.LogInformation($"Identity Database {typeof(IdentityContext).Name} seeded with roles.");
            }

            if (!context.Users.Any())
            {
                foreach (var user in GetUserSeeds())
                {
                    await identityService.CreateUserAsync(user.userName, user.password, user.email, user.fullName, user.roles);
                }
                logger.LogInformation($"Identity Database {typeof(IdentityContext).Name} seeded with users.");
            }
        }

        #region seed data
        private static IEnumerable<(string userName, string password, string email, string fullName, List<string> roles)> GetUserSeeds()
        =>
        [
            (
                userName: "admin",
                password: "adminadmin",
                email: "admin@gmail.com",
                fullName: "Admin",
                roles: new List<string>(){SMRole.admin.ToString()}
            ),
             (
                userName: "user",
                password: "useruser",
                email: "user@gmail.com",
                fullName: "User",
                roles: new List<string>(){ SMRole.user.ToString() }
            )
        ];

            private static IEnumerable<string> GetRoleSeeds()
            => [SMRole.admin.ToString(), SMRole.user.ToString()];
        }
        #endregion
}
