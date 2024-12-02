using Identity.Application.Commands.User.Create;
using Identity.Application.Interfaces;
using Identity.Core.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
                var userSeeds = GetUserSeeds();
                await identityService.PopulateUserData(userSeeds);
                logger.LogInformation($"Identity Database {typeof(IdentityContext).Name} seeded with {userSeeds.Count()} users.");
            }
        }

        #region seed data
        private static IEnumerable<LoadExistingUserCommand> GetUserSeeds()
        {
            // run by docker compose
            var path = Path.Combine("Persistence", "SeedData", "users.json");
            // run local
            //var path = "../Identity.Infrastructure/Persistence/SeedData/users.json";

            var userDataStr = File.ReadAllText(path);
            var userData = JsonConvert.DeserializeObject<List<LoadExistingUserCommand>>(userDataStr);
            #region default users
            var systemUser = new List<LoadExistingUserCommand>
            {
                 new LoadExistingUserCommand(){
                Id = "lxca7cdd-e81f-482d-ba63-f896ccf22b6a",
                UserName = "admin",
                Password = "adminadmin",
                ConfirmationPassword = "adminadmin",
                Email = "admin@gmail.com",
                FullName = "Admin",
                Roles = [SMRole.admin.ToString()],
                ProfileUrl = "lxca7cdd-e81f-482d-ba63-f896ccf22b6a",
                Gender = 0,
                City = "Ho Chi Minh City",
                Country = "Viet Nam",
                DateOfBirth = new DateTime(2000,1,1)
            },
            new LoadExistingUserCommand(){
                Id = "dqta7cdd-e81f-482d-ba63-f896ccf22b6a",
                UserName = "user",
                Password = "useruser",
                ConfirmationPassword = "useruser",
                Email = "user@gmail.com",
                FullName = "User",
                ProfileUrl = "dqta7cdd-e81f-482d-ba63-f896ccf22b6a",
                Roles = [SMRole.user.ToString()],
                Gender = 1,
                City = "Ho Chi Minh City",
                Country = "Viet Nam",
                DateOfBirth = new DateTime(2000,1,1)
            },
            };
            #endregion

            if (userData != null)
            {
                userData.AddRange(systemUser);
            }
            else
            {
                userData = systemUser;
            }

            return userData;
        }


        private static IEnumerable<string> GetRoleSeeds()
        => [SMRole.admin.ToString(), SMRole.user.ToString()];
    }
    #endregion
}
