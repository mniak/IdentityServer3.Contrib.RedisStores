using Newtonsoft.Json;
using System;
using System.Security.Claims;

namespace IdentityServer3.Contrib.RedisStores.Serialization
{
    internal class ClaimConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Claim);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var model = serializer.Deserialize<Models.ClaimModel>(reader);
            var result = new Claim(model.Type, model.Value);
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var claim = value as Claim;
            var model = new Models.ClaimModel(claim);
            serializer.Serialize(writer, model);
        }
    }
}
