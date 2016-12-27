using IdentityServer3.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using StackExchange.Redis;
using IdentityServer3.Contrib.RedisStores.Models;

namespace IdentityServer3.Contrib.RedisStores
{
    /// <summary>
    /// A redis implementation of IAuthorizationCodeStore
    /// </summary>
    public sealed class RedisAuthorizationCodeStore : RedisTransientStore<AuthorizationCode, AuthorizationCodeModel>, IAuthorizationCodeStore
    {
        /// <summary>
        /// Creates a new ReidsAuthorizationCodeStore
        /// </summary>
        /// <param name="redis">The redis database</param>
        /// <param name="options">The options</param>
        public RedisAuthorizationCodeStore(IDatabase redis, RedisOptions options) : base(redis, options)
        {

        }
        /// <summary>
        /// Creates a new ReidsAuthorizationCodeStore
        /// </summary>
        /// <param name="redis">The redis database</param>
        public RedisAuthorizationCodeStore(IDatabase redis) : base(redis)
        {

        }
        internal override string CollectionName { get { return "authcodes"; } }
        internal override int GetTokenLifetime(AuthorizationCode token)
        {
            return token.Client.AuthorizationCodeLifetime;
        }
    }
}
