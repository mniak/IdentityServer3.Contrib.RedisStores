using IdentityServer3.Contrib.RedisStores.Models;
using IdentityServer3.Core.Logging;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer3.Contrib.RedisStores
{
    /// <summary>
    /// IdentityServer3 ITokenHandleStore implementation using Redis
    /// </summary>
    public sealed class RedisTokenHandleStore : RedisTransientStore<Token, TokenModel>, ITokenHandleStore
    {
        /// <summary>
        /// Creates a new RedisTokenHandleStore
        /// </summary>
        /// <param name="redis">The redis database</param>
        /// <param name="options">The options</param>
        public RedisTokenHandleStore(IDatabase redis, RedisOptions options) : base(redis, options)
        {

        }
        /// <summary>
        /// Creates a new RedisTokenHandleStore
        /// </summary>
        /// <param name="redis">The redis database</param>
        public RedisTokenHandleStore(IDatabase redis) : base(redis)
        {

        }
        internal override string CollectionName { get { return "tokens"; } }
        internal override int GetTokenLifetime(Token token)
        {
            return token.Lifetime;
        }
    }
}
