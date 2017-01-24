using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace IdentityServer3.Contrib.RedisStores.Serialization
{
    internal class ScopeConverter : JsonConverter
    {
        private readonly IScopeStore scopeStore;

        public ScopeConverter(IScopeStore scopeStore)
        {
            this.scopeStore = scopeStore;
        }
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Scope);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var scopeName = serializer.Deserialize<string>(reader);
            var scope = scopeStore.FindScopesAsync(new[] { scopeName }).Result.Single();
            return scope;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var scope = value as Scope;
            serializer.Serialize(writer, scope.Name);
        }
    }
}
