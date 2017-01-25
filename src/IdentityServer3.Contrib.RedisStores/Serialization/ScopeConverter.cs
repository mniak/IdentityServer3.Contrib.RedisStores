using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            return objectType == typeof(Scope)
                || objectType == typeof(List<Scope>)
                || objectType == typeof(Scope[])
                || objectType == typeof(IEnumerable<Scope>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(Scope))
            {
                var scopeName = serializer.Deserialize<string>(reader);
                var scope = scopeStore.FindScopesAsync(new[] { scopeName }).Result.Single();
                return scope;
            }
            else
            {
                var scopeNames = serializer.Deserialize<IEnumerable<string>>(reader);
                var scopes = scopeStore.FindScopesAsync(scopeNames).Result;
                if (objectType == typeof(List<Scope>))
                    return scopes.ToList();
                else if (objectType == typeof(Scope[]))
                    return scopes.ToArray();
                return scopes;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var scopesCollection = value as IEnumerable<Scope>;
            if (scopesCollection != null)
            {
                var scopeNames = scopesCollection.Select(x => x.Name);
                serializer.Serialize(writer, scopeNames);
            }
            else
            {
                var scope = value as Scope;
                serializer.Serialize(writer, scope.Name);
            }
        }
    }
}
