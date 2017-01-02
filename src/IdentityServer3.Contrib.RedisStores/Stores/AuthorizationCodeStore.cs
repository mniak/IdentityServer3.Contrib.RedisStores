﻿using IdentityServer3.Contrib.RedisStores.Converters;
using IdentityServer3.Contrib.RedisStores.Models;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using StackExchange.Redis;

namespace IdentityServer3.Contrib.RedisStores
{
    /// <summary>
    /// A redis implementation of IAuthorizationCodeStore
    /// </summary>
    public sealed class AuthorizationCodeStore : RedisTransientStore<AuthorizationCode, AuthorizationCodeModel>, IAuthorizationCodeStore
    {
        /// <summary>
        /// Creates a new ReidsAuthorizationCodeStore
        /// </summary>
        /// <param name="redis">The redis database</param>
        /// <param name="clientStore">The client store</param>
        /// <param name="options">The options</param>
        public AuthorizationCodeStore(IDatabase redis, IClientStore clientStore, RedisOptions options) : base(redis, options, new AuthorizationCodeConverter(clientStore))
        {

        }
        /// <summary>
        /// Creates a new ReidsAuthorizationCodeStore
        /// </summary>
        /// <param name="redis">The redis database</param>
        /// <param name="clientStore">The client store</param>
        public AuthorizationCodeStore(IDatabase redis, IClientStore clientStore) : base(redis, new AuthorizationCodeConverter(clientStore))
        {

        }
        internal override string CollectionName => "authCodes";
        internal override int GetTokenLifetime(AuthorizationCode token)
        {
            return token.Client.AuthorizationCodeLifetime;
        }
    }
}
