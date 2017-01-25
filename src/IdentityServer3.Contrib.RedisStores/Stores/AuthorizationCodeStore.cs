using IdentityServer3.Contrib.RedisStores.Serialization;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace IdentityServer3.Contrib.RedisStores
{
    public sealed class AuthorizationCodeStore : RedisTransientStore<AuthorizationCode>, IAuthorizationCodeStore
    {
        private readonly IScopeStore scopeStore;
        private readonly IClientStore clientStore;

        public AuthorizationCodeStore(IDatabase redis, RedisOptions options, IScopeStore scopeStore, IClientStore clientStore) : base(redis)
        {
            this.scopeStore = scopeStore;
            this.clientStore = clientStore;
        }
        public AuthorizationCodeStore(IDatabase redis, IScopeStore scopeStore, IClientStore clientStore) : base(redis)
        {
            this.scopeStore = scopeStore;
            this.clientStore = clientStore;
        }
        protected override void CustomizeSerializerSettings(JsonSerializerSettings settings)
        {
            base.CustomizeSerializerSettings(settings);
            settings.Converters.Add(new ScopeConverter(scopeStore));
            settings.Converters.Add(new AuthorizationCodeConverter(clientStore));
        }
        internal override string CollectionName => "auth_codes";
        internal override int GetTokenLifetime(AuthorizationCode token)
        {
            return token.Client.AuthorizationCodeLifetime;
        }
    }
}
