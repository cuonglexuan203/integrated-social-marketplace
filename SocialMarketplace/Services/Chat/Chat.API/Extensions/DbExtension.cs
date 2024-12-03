using Chat.Infrastructure.Persistence.DbContext;
using MongoDB.Driver;
using Polly;
namespace Chat.API.Extensions
{
    public static class DbExtension
    {
        public static IHost InitializeDatabase(this IHost host, Action<IChatContext> seeder)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<IChatContext>>();
            var context = services.GetRequiredService<IChatContext>();
            try
            {
                logger.LogInformation($"Started database initialization: {typeof(IChatContext).Name}");
                // retry stategy
                var retry = Policy.Handle<MongoException>()
                                  .WaitAndRetry(
                                    retryCount: 5,
                                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                    onRetry: (exception, span, cont) =>
                                    {
                                        logger.LogError($"Retrying because of {exception} {span}");
                                    }
                                  );
                retry.Execute(() => CallSeeder(seeder, context));
                logger.LogInformation($"Initialization completed: {typeof(IChatContext).Name}");
            }
            catch (MongoException ex)
            {
                logger.LogError($"An error occured while populating the seed data of {typeof(IChatContext).Name}: {ex.Message}");
            }
            return host;
        }

        public static void CallSeeder(Action<IChatContext> seeder, IChatContext feedContext)
        {
            feedContext.InitializeAsync().Wait();
            seeder(feedContext);
        }
    }
}
