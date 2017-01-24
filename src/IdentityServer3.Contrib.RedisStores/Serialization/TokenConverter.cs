using IdentityModel;
using IdentityServer3.Core.Models;
using Newtonsoft.Json;
using System;

namespace IdentityServer3.Contrib.RedisStores.Serialization
{
    class TokenConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Token);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var model = serializer.Deserialize<Models.TokenModel>(reader);
            var result = new Token()
            {
                Audience = model.Audience,
                Issuer = model.Issuer,
                CreationTime = model.IssuedAt.ToDateTimeOffsetFromEpoch(),
                Lifetime = model.Lifetime,
                Type = model.Type,
                Client = model.Client,
                Claims = model.Claims,
                Version = model.Version,
            };
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var token = value as Token;
            var model = new Models.TokenModel()
            {
                Audience = token.Audience,
                Issuer = token.Issuer,
                IssuedAt = token.CreationTime.ToEpochTime(),
                Lifetime = token.Lifetime,
                Type = token.Type,
                Version = token.Version
            };
            model.Claims.AddRange(token.Claims);
            serializer.Serialize(writer, model);
        }
    }
}
