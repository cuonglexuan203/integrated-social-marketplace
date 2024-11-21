
using Feed.Application.Interfaces;

namespace Feed.Infrastructure.Validation
{
    public class MongoIdValidator : IMongoIdValidator
    {
        public bool IsValid(string id)
        {
            return MongoDB.Bson.ObjectId.TryParse(id, out _);
        }
    }
}
