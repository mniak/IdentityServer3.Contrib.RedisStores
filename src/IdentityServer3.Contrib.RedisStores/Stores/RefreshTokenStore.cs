//using IdentityServer3.Core.Services;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using IdentityServer3.Core.Models;
//using IdentityServer3.Contrib.RedisStores.Models;
//using StackExchange.Redis;
//using IdentityServer3.Contrib.RedisStores.Converters;

//namespace IdentityServer3.Contrib.RedisStores
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public class RefreshTokenStore : RedisTransientStore<RefreshToken, RefreshTokenModel>, IRefreshTokenStore
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="redis"></param>
//        /// <param name="options"></param>
//        public RefreshTokenStore(IDatabase redis, RedisOptions options) : base(redis, options, new RefreshTokenConverter())
//        {

//        }
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="redis"></param>
//        public RefreshTokenStore(IDatabase redis) : base(redis, new RefreshTokenConverter())
//        {

//        }
//        internal override string CollectionName => "refreshTokens";

//        internal override int GetTokenLifetime(RefreshToken token)
//        {
//            return token.LifeTime;
//        }
//    }
//}
