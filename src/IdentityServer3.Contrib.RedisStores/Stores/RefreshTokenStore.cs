using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using StackExchange.Redis;

namespace IdentityServer3.Contrib.RedisStores
{
    public sealed class RefreshTokenStore : RedisTransientStore<RefreshToken>, IRefreshTokenStore
    {

        public RefreshTokenStore(IDatabase redis, RedisOptions options) : base(redis, options)
        {

        }
        public RefreshTokenStore(IDatabase redis) : base(redis)
        {

        }
        internal override string CollectionName => "refresh_tokens";
        internal override int GetTokenLifetime(RefreshToken token)
        {
            return token.LifeTime;
        }
    }
}
