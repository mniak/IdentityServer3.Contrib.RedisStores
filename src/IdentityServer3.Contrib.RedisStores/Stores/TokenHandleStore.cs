using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using StackExchange.Redis;

namespace IdentityServer3.Contrib.RedisStores
{
    public sealed class TokenHandleStore : RedisTransientStore<Token>, ITokenHandleStore
    {
        public TokenHandleStore(IScopeStore scopeStore, IDatabase redis, RedisOptions options) : base(redis, options)
        {

        }
        public TokenHandleStore(IScopeStore scopeStore, IDatabase redis) : base(redis)
        {

        }
        internal override string CollectionName => "tokens";
        internal override int GetTokenLifetime(Token token)
        {
            return token.Lifetime;
        }
    }
}
