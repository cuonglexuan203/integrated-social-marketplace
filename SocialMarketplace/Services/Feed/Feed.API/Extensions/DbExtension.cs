using Feed.Infrastructure.Persistence.DbContext;
using MongoDB.Driver;
using Polly;
namespace Feed.API.Extensions
{
    public static class DbExtension
    {
        public static IHost InitializeDatabase(this IHost host, Action<IFeedContext> seeder)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<IFeedContext>>();
            var context = services.GetRequiredService<IFeedContext>();
            try
            {
                logger.LogInformation($"Started database initialization: {typeof(IFeedContext).Name}");
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
                logger.LogInformation($"Initialization completed: {typeof(IFeedContext).Name}");
            }
            catch (MongoException ex)
            {
                logger.LogError($"An error occured while populating the seed data of {typeof(IFeedContext).Name}: {ex.Message}");
            }
            return host;
        }

        public static void CallSeeder(Action<IFeedContext> seeder,IFeedContext feedContext)
        {
            feedContext.InitializeAsync().Wait();
            seeder(feedContext);
        }
    }
}
