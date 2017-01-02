using IdentityServer3.Contrib.RedisStores.Converters;
using IdentityServer3.Contrib.RedisStores.Models;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using StackExchange.Redis;

namespace IdentityServer3.Contrib.RedisStores
{
    /// <summary>
    /// IdentityServer3 ITokenHandleStore implementation using Redis
    /// </summary>
    public sealed class TokenHandleStore : RedisTransientStore<Token, TokenModel>, ITokenHandleStore
    {
        /// <summary>
        /// Creates a new RedisTokenHandleStore
        /// </summary>
        /// <param name="redis">The redis database</param>
        /// <param name="options">The options</param>
        public TokenHandleStore(IDatabase redis, RedisOptions options) : base(redis, options, new TokenConverter())
        {

        }
        /// <summary>
        /// Creates a new RedisTokenHandleStore
        /// </summary>
        /// <param name="redis">The redis database</param>
        public TokenHandleStore(IDatabase redis) : base(redis, new TokenConverter())
        {

        }
        internal override string CollectionName => "tokens";
        internal override int GetTokenLifetime(Token token)
        {
            return token.Lifetime;
        }
    }
}
