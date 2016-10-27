using IdentityServer3.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Logging;
using StackExchange.Redis;
using IdentityServer3.Contrib.RedisStores.Models;
using System.IO;

namespace IdentityServer3.Contrib.RedisStores
{
    public class RedisTokenHandleStore : ITokenHandleStore
    {
        private const string TOKENS = "tokens";
        private const string SUBJECT = "sub";

        private readonly static ILog logger = LogProvider.GetCurrentClassLogger();

        private readonly ConnectionMultiplexer connection;
        private IDatabase redis;

        public RedisTokenHandleStore(string redisConfiguration)
        {
            //TODO use config
            this.connection = ConnectionMultiplexer.Connect("localhost");
            this.redis = connection.GetDatabase();
        }

        public async Task<IEnumerable<ITokenMetadata>> GetAllAsync(string subject)
        {
            try
            {
                var members = redis.SetMembers(RedisHelper.GetIndexKey(TOKENS, SUBJECT, subject));
                var tasks = members.Select(x => redis.RetrieveAsync<TokenModel>(TOKENS, x));
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

        public async Task<Token> GetAsync(string tokenKey)
        {
            await Task.Delay(0);
            try
            {
                var tokenModel = await redis.RetrieveAsync<TokenModel>(TOKENS, tokenKey);
                return tokenModel != null ? tokenModel.GetToken() : null;
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
                await redis.DeleteByIdAsync(TOKENS, tokenKey);
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

                var members = redis.SetMembers(RedisHelper.GetIndexKey(TOKENS, SUBJECT, subject));
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

        public async Task StoreAsync(string tokenKey, Token token)
        {
            try
            {
                var tokenModel = new TokenModel(token);

                var accomplished = await redis.StoreAsync(TOKENS, tokenKey, tokenModel, token.Lifetime);
                if (!accomplished)
                    throw new IOException("The Redis server returned FALSE to the SET command.");
                await Task.WhenAll(new[] {
                   redis.SetAddAsync(RedisHelper.GetIdsKey(TOKENS), tokenKey),
                   redis.SetAddAsync(RedisHelper.GetIndexKey(TOKENS, SUBJECT, token.SubjectId), tokenKey),
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
