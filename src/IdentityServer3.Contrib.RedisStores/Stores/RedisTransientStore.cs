using IdentityServer3.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using StackExchange.Redis;
using IdentityServer3.Core.Logging;
using System.IO;
using IdentityServer3.Contrib.RedisStores.Models;

namespace IdentityServer3.Contrib.RedisStores
{
    /// <summary>
    /// A base for the Redis Tokens Stores
    /// </summary>
    /// <typeparam name="TToken">The type of the token</typeparam>
    /// <typeparam name="TModel">The type of the of the model of the token. Must implement ITokenModel&lt;TToken&gt;</typeparam>
    public abstract class RedisTransientStore<TToken, TModel> : ITransientDataRepository<TToken>
        where TToken : class, ITokenMetadata, new()
        where TModel : ITokenModel<TToken>, new()
    {
        private const string SUBJECT = "sub";

        private readonly static ILog logger = LogProvider.GetCurrentClassLogger();

        private readonly IDatabase redis;
        private readonly RedisHelper redisHelper;

        internal RedisTransientStore(IDatabase redis) : this(redis, new RedisOptions())
        {
        }
        internal RedisTransientStore(IDatabase redis, RedisOptions options)
        {
            this.redis = redis;
            this.redisHelper = new RedisHelper(redis, options);
        }


        /// <summary>
        /// Retrieves all the tokens in the store for a given subject identifier
        /// </summary>
        /// <param name="subject">The subject identifier</param>
        /// <returns>A list of token metadata</returns>
        public async Task<IEnumerable<ITokenMetadata>> GetAllAsync(string subject)
        {
            try
            {
                var members = redis.SetMembers(redisHelper.GetIndexKey(CollectionName, SUBJECT, subject));
                var tasks = members.Select(x => redisHelper.RetrieveAsync<TModel>(CollectionName, x));
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

        /// <summary>
        /// Retrieves a token by its key
        /// </summary>
        /// <param name="tokenKey">The token key</param>
        /// <returns>The token</returns>
        public async Task<TToken> GetAsync(string tokenKey)
        {
            try
            {
                var tokenModel = await redisHelper.RetrieveAsync<TModel>(CollectionName, tokenKey);
                return tokenModel != null ? tokenModel.GetToken() : null;
            }
            catch (Exception ex)
            {
                logger.ErrorException("Could not get the token for key: '{Key}'", ex, tokenKey);
                throw;
            }
        }

        /// <summary>
        /// Removes a token by its key
        /// </summary>
        /// <param name="tokenKey">The token key</param>
        /// <returns></returns>
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

        /// <summary>
        /// Revokes a token given a subject identifier and a client identifier
        /// </summary>
        /// <param name="subject">The subject identifier</param>
        /// <param name="client">the client identifier</param>
        /// <returns></returns>
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

        /// <summary>
        /// Stores a token at the specified key
        /// </summary>
        /// <param name="tokenKey">The token key</param>
        /// <param name="token">The token</param>
        /// <returns></returns>
        public async Task StoreAsync(string tokenKey, TToken token)
        {
            try
            {
                var tokenModel = new TModel();
                tokenModel.ImportData(token);

                var accomplished = await redisHelper.StoreAsync(CollectionName, tokenKey, tokenModel, GetTokenLifetime(token));
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
