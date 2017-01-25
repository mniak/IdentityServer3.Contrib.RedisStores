using IdentityServer3.Contrib.RedisStores.Serialization;
using IdentityServer3.Core.Logging;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer3.Contrib.RedisStores
{
    public abstract class RedisTransientStore<TToken> : ITransientDataRepository<TToken>
        where TToken : class, ITokenMetadata
    {
        private const string SUBJECT = "sub";

        private readonly static ILog logger = LogProvider.GetCurrentClassLogger();

        private readonly IDatabase redis;
        private readonly Lazy<RedisHelper<TToken>> lazyRedisHelper;
        private RedisHelper<TToken> redisHelper { get { return lazyRedisHelper.Value; } }

        internal RedisTransientStore(IDatabase redis) : this(redis, new RedisOptions())
        {
        }
        internal RedisTransientStore(IDatabase redis, RedisOptions options)
        {
            this.redis = redis;
            this.lazyRedisHelper = new Lazy<RedisHelper<TToken>>(() => CreateRedisHelper(options));
        }
        private RedisHelper<TToken> CreateRedisHelper(RedisOptions options)
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.Converters.Add(new TokenConverter());
            serializerSettings.Converters.Add(new RefreshTokenConverter());
            serializerSettings.Converters.Add(new ClaimConverter());
            serializerSettings.Converters.Add(new ClaimsPrincipalConverter());

            CustomizeSerializerSettings(serializerSettings);
            var result = new RedisHelper<TToken>(this.redis, options, serializerSettings);
            return result;
        }
        protected virtual void CustomizeSerializerSettings(JsonSerializerSettings settings) { }
        public async Task<IEnumerable<ITokenMetadata>> GetAllAsync(string subject)
        {
            try
            {
                var members = redis.SetMembers(redisHelper.GetIndexKey(CollectionName, SUBJECT, subject));
                var tasks = members.Select(x => redisHelper.RetrieveAsync(CollectionName, x));
                var tokenModels = await Task.WhenAll(tasks);
                var result = tokenModels.Where(x => x != null)
                    .Cast<ITokenMetadata>();
                return result;
            }
            catch (Exception ex)
            {
                logger.ErrorException("Could not get all the tokens for sub: '{Subject}'", ex, subject);
                throw;
            }
        }

        public async Task<TToken> GetAsync(string tokenKey)
        {
            try
            {
                var token = await redisHelper.RetrieveAsync(CollectionName, tokenKey);
                return token;
            }
            catch (Exception ex)
            {
                logger.ErrorException("Could not get the token for key: '{Key}'", ex, tokenKey);
                throw;
            }
        }
        public async Task RemoveAsync(string tokenKey)
        {
            try
            {
                await redisHelper.DeleteByIdAsync(CollectionName, tokenKey);
            }
            catch (Exception ex)
            {
                logger.ErrorException("Could not remove the token for key: '{Key}'", ex, tokenKey);
                throw;
            }
        }
        public async Task RevokeAsync(string subject, string client)
        {
            try
            {
                logger.InfoFormat("Revoking token for sub: {Subject} and client: {Client}", subject, client);

                var members = redis.SetMembers(redisHelper.GetIndexKey(CollectionName, SUBJECT, subject));
                var tokensTasks = members.ToDictionary(key => key, key => GetAsync(key));
                await Task.WhenAll(tokensTasks.Values);
                var tokens = tokensTasks.ToDictionary(x => x.Key, x => x.Value.Result);
                var filtered = tokens.Where(x => x.Value.SubjectId == subject && x.Value.ClientId == client);

                if (!filtered.Any())
                {
                    logger.Info("Token not found");
                    return;
                }

                await Task.WhenAll(filtered.Select(t => RemoveAsync(t.Key)));
            }
            catch (Exception ex)
            {
                logger.ErrorException("Could not revoke the token for sub: '{Subject}' and client: {Client}", ex, subject, client);
                throw;
            }
        }

        public async Task StoreAsync(string tokenKey, TToken token)
        {
            try
            {
                var accomplished = await redisHelper.StoreAsync(CollectionName, tokenKey, token, GetTokenLifetime(token));
                if (!accomplished)
                    throw new IOException("The Redis server returned FALSE to the SET command.");

                var idskey = redisHelper.GetIdsKey(CollectionName);
                var taskSaveRecord = redis.SetAddAsync(idskey, tokenKey)
                    .ContinueWith(async t => await redisHelper.ExtendLifetimeAsync(idskey, GetTokenLifetime(token), true));

                var idxkey = redisHelper.GetIndexKey(CollectionName, SUBJECT, token.SubjectId);
                var taskSaveIndex = redis.SetAddAsync(idxkey, tokenKey)
                    .ContinueWith(async t => await redisHelper.ExtendLifetimeAsync(idxkey, GetTokenLifetime(token), true));

                await Task.WhenAll(taskSaveRecord, taskSaveIndex);
            }
            catch (Exception ex)
            {
                logger.ErrorException("Could not store the token for key: '{Key}'", ex, tokenKey);
                throw;
            }
        }
        internal abstract string CollectionName { get; }
        internal abstract int GetTokenLifetime(TToken token);
    }
}
