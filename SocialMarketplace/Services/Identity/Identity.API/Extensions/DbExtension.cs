using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace Identity.API.Extensions
{
    public static class DbExtension
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetService<TContext>();
            try
            {
                logger.LogInformation($"Started Db Migration: {typeof(TContext).Name}");
                // retry stategy
                var retry = Policy.Handle<SqlException>()
                                  .WaitAndRetry(
                                    retryCount: 5,
                                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                    onRetry: (exception, span, cont) =>
                                    {
                                        logger.LogError($"Retrying because of {exception} {span}");
                                    }
                                  );
                retry.Execute(() => CallSeeder(seeder, context, services));
                logger.LogInformation($"Migration completed: {typeof(TContext).Name}");
            }
            catch (SqlException ex) {
                logger.LogError($"An error occured while migrating db: {typeof(TContext).Name}");
            }
            return host;
        }
        private static void CallSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services) where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
