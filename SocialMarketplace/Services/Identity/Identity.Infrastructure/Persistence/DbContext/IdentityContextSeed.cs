using Identity.Application.Commands.User.Create;
using Identity.Application.Interfaces;
using Identity.Core.Enums;
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
                    await identityService.CreateUserAsync(user);
                }
                logger.LogInformation($"Identity Database {typeof(IdentityContext).Name} seeded with users.");
            }
        }

        #region seed data
        private static IEnumerable<CreateUserCommand> GetUserSeeds()
        =>
        [
            new CreateUserCommand(){
                UserName = "admin",
                Password = "adminadmin",
                ConfirmationPassword = "adminadmin",
                Email = "admin@gmail.com",
                FullName = "Admin",
                Roles = [SMRole.admin.ToString()],
                Gender = 0,
                City = "Ho Chi Minh City",
                Country = "Viet Nam",
                DateOfBirth = new DateTime(2000,1,1)
            },
            new CreateUserCommand(){
                UserName = "user",
                Password = "useruser",
                ConfirmationPassword = "useruser",
                Email = "user@gmail.com",
                FullName = "User",
                Roles = [SMRole.user.ToString()],
                Gender = 1,
                City = "Ho Chi Minh City",
                Country = "Viet Nam",
                DateOfBirth = new DateTime(2000,1,1)
            },
        ];

            private static IEnumerable<string> GetRoleSeeds()
            => [SMRole.admin.ToString(), SMRole.user.ToString()];
        }
        #endregion
}
