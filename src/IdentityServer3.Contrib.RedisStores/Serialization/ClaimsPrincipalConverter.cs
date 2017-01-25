using IdentityServer3.Contrib.RedisStores.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Claims;

namespace IdentityServer3.Contrib.RedisStores.Serialization
{
    internal class ClaimsPrincipalConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ClaimsPrincipal);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var model = serializer.Deserialize<ClaimsPrincipalModel>(reader);
            var claims = model.Claims.Select(x => new Claim(x.Type, x.Value));
            var identity = new ClaimsIdentity(claims, model.AuthenticationType);
            var result = new ClaimsPrincipal(identity);
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var claimsPrincipal = value as ClaimsPrincipal;
            var model = new Models.ClaimsPrincipalModel(claimsPrincipal);
            serializer.Serialize(writer, model);
        }
    }
}
