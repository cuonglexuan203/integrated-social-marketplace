using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Feed.Infrastructure.Persistence.Serializer
{
    public class DateTimeOffsetAsStringSerializer : IBsonSerializer<DateTimeOffset>
    {
        public Type ValueType => typeof(DateTimeOffset);

        public DateTimeOffset Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var type = context.Reader.GetCurrentBsonType();

            if (type == BsonType.String)
            {
                // Parse the string back to DateTimeOffset
                return DateTimeOffset.Parse(context.Reader.ReadString());
            }

            throw new BsonSerializationException("Cannot deserialize DateTimeOffset from non-string type");
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTimeOffset value)
        {
            // Convert DateTimeOffset to ISO 8601 string format
            context.Writer.WriteString(value.ToString("O"));
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            if (value is DateTimeOffset dateTimeOffset)
            {
                Serialize(context, args, dateTimeOffset);
            }
            else
            {
                throw new BsonSerializationException("Value is not a DateTimeOffset");
            }
        }

        object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return Deserialize(context, args);
        }
    }

}
