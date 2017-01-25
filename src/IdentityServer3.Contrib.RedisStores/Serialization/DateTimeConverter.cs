using IdentityModel;
using Newtonsoft.Json;
using System;

namespace IdentityServer3.Contrib.RedisStores.Serialization
{
    internal class DateTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTimeOffset)
                || objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var num = serializer.Deserialize<long>(reader);
            if (objectType == typeof(DateTimeOffset))
            {
                return num.ToDateTimeOffsetFromEpoch();
            }
            else if (objectType == typeof(DateTime))
            {
                return num.ToDateTimeFromEpoch();
            }
            else throw new ArgumentException("Invalid type", nameof(objectType));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            long num;
            if (value.GetType() == typeof(DateTimeOffset))
            {
                var val = (DateTimeOffset)value;
                num = val.ToEpochTime();
            }
            else if (value.GetType() == typeof(DateTime))
            {
                var val = (DateTime)value;
                num = val.ToEpochTime();
            }
            else throw new ArgumentException("Invalid type", nameof(value));
            serializer.Serialize(writer, num);
        }
    }
}
