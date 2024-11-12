using Identity.API.Extensions;
using Identity.Infrastructure.Persistence.DbContext;

namespace Identity.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .MigrateDatabase<IdentityContext>((context, services) =>
                {
                    IdentityContextSeed.SeedAsync(context, services).Wait();   
                })
                .Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
         =>   Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
