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
    public class RedisTokenHandleStore : ITokenHandleStore
    {
        private const string TOKENS = "tokens";
        private const string SUBJECT = "sub";

        private readonly static ILog logger = LogProvider.GetCurrentClassLogger();

        private readonly RedisHelper redisHelper;
        private readonly IDatabase redis;

        /// <summary>
        /// Creates a new RedisTokenHandleStore instance
        /// </summary>
        /// <param name="redisConfiguration">The Redis configuration</param>
        public RedisTokenHandleStore(string redisConfiguration, RedisTokenHandleStoreOptions options = default(RedisTokenHandleStoreOptions))
        {
            var connection = ConnectionMultiplexer.Connect(redisConfiguration);
            this.redis = connection.GetDatabase();
            this.redisHelper = new RedisHelper(redis, options);
        }

        /// <summary>
        /// Creates a new RedisTokenHandleStore instance
        /// </summary>
        /// <param name="redisConfiguration">The Redis configuration</param>
        public RedisTokenHandleStore(ConfigurationOptions redisConfiguration, RedisTokenHandleStoreOptions options = default(RedisTokenHandleStoreOptions))
        {
            var connection = ConnectionMultiplexer.Connect(redisConfiguration);
            this.redis = connection.GetDatabase();
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
                var members = redis.SetMembers(redisHelper.GetIndexKey(TOKENS, SUBJECT, subject));
                var tasks = members.Select(x => redisHelper.RetrieveAsync<TokenModel>(TOKENS, x));
                var tokenModels = await Task.WhenAll(tasks);
                var result = tokenModels.Where(x => x != null)
                    .Select(x => x.GetTokenMetadata());
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
        public async Task<Token> GetAsync(string tokenKey)
        {
            await Task.Delay(0);
            try
            {
                var tokenModel = await redisHelper.RetrieveAsync<TokenModel>(TOKENS, tokenKey);
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
                await redisHelper.DeleteByIdAsync(TOKENS, tokenKey);
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

                var members = redis.SetMembers(redisHelper.GetIndexKey(TOKENS, SUBJECT, subject));
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
        public async Task StoreAsync(string tokenKey, Token token)
        {
            try
            {
                var tokenModel = new TokenModel(token);

                var accomplished = await redisHelper.StoreAsync(TOKENS, tokenKey, tokenModel, token.Lifetime);
                if (!accomplished)
                    throw new IOException("The Redis server returned FALSE to the SET command.");
                await Task.WhenAll(new[] {
                   redis.SetAddAsync(redisHelper.GetIdsKey(TOKENS), tokenKey),
                   redis.SetAddAsync(redisHelper.GetIndexKey(TOKENS, SUBJECT, token.SubjectId), tokenKey),
                });

            }
            catch (Exception ex)
            {
                logger.ErrorException("Could not store the token for key: '{Key}'", ex, tokenKey);
                throw;
            }
        }
    }
}
