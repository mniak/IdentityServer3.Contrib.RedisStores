using IdentityServer3.Core.Models;
using Newtonsoft.Json;
using System;

namespace IdentityServer3.Contrib.RedisStores.Serialization
{
    internal class RefreshTokenConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(RefreshToken);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var model = serializer.Deserialize<Models.RefreshTokenModel>(reader);
            var refreshToken = new RefreshToken()
            {
                AccessToken = model.AccessToken,
                Subject = model.Subject,
                CreationTime = model.CreationTime,
                LifeTime = model.LifeTime,
                Version = model.Version,
            };
             return refreshToken;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var refreshToken = value as RefreshToken;
            var model = new Models.RefreshTokenModel()
            {
                AccessToken = refreshToken.AccessToken,
                Subject = refreshToken.Subject,
                CreationTime = refreshToken.CreationTime,
                LifeTime = refreshToken.LifeTime,
                Version = refreshToken.Version,
            };
            serializer.Serialize(writer, model);
        }
    }
}
