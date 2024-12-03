using Feed.Infrastructure.Persistence.Serializer;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;

namespace Feed.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(typeof(DateTimeOffset), new DateTimeOffsetAsStringSerializer());
            return services;
        }
    }
}
