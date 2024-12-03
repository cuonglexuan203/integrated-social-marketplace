using Feed.API.Extensions;
using Feed.Infrastructure.Persistence.SeedData;

namespace Feed.API
{
     public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .InitializeDatabase(context =>
                {
                    CommentContextSeed.SeedData(context.Comments);
                    PostContextSeed.SeedData(context.Posts);
                })
                .Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
