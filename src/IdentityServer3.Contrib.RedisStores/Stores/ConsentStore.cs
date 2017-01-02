using IdentityServer3.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using StackExchange.Redis;

namespace IdentityServer3.Contrib.RedisStores
{
    /// <summary>
    /// IdentityServer3 IConsentStore implementation using Redis
    /// </summary>
    public class ConsentStore : IConsentStore
    {
        const string CONSENTS = "consents";
        const string SUBJECT = "sub";

        private readonly IDatabase redis;
        private readonly RedisHelper redisHelper;

        /// <summary>
        /// Creates a new RedisConsentStore
        /// </summary>
        /// <param name="redis">The redis database</param>
        public ConsentStore(IDatabase redis) : this(redis, new RedisOptions())
        {

        }
        /// <summary>
        /// Creates a new RedisConsentStore
        /// </summary>
        /// <param name="redis">The redis database</param>
        /// <param name="options">Options</param>
        public ConsentStore(IDatabase redis, RedisOptions options)
        {
            this.redis = redis;
            this.redisHelper = new RedisHelper(redis, options);
        }

        /// <summary>
        /// Load all the consents for a specified subject
        /// </summary>
        /// <param name="subject"></param>
        /// <returns>A collection of consents</returns>
        public async Task<IEnumerable<Consent>> LoadAllAsync(string subject)
        {
            string subKey = redisHelper.GetIndexKey(CONSENTS, SUBJECT, subject);
            var entries = await redis.HashGetAllAsync(subKey);
            var result = entries.Select(x => CreateConsent(subject, x.Name, x.Value));
            return result;
        }

        /// <summary>
        /// Loads a consent by the subject and the client
        /// </summary>
        /// <param name="subject">The subject</param>
        /// <param name="client">The client</param>
        /// <returns>A single consent</returns>
        public async Task<Consent> LoadAsync(string subject, string client)
        {
            string subKey = redisHelper.GetIndexKey(CONSENTS, SUBJECT, subject);
            var scopes = await redis.HashGetAsync(subKey, client);
            var result = CreateConsent(subject, client, scopes);
            return result;
        }

        private static Consent CreateConsent(string subject, string client, RedisValue scopes)
        {
            return new Consent()
            {
                Subject = subject,
                ClientId = client,
                Scopes = scopes.IsNullOrEmpty
                        ? new string[0]
                        : scopes.ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
            };
        }

        /// <summary>
        /// Revoke a consent by the subject and the client
        /// </summary>
        /// <param name="subject">The subject</param>
        /// <param name="client">The client</param>
        /// <returns></returns>
        public async Task RevokeAsync(string subject, string client)
        {
            string subKey = redisHelper.GetIndexKey(CONSENTS, SUBJECT, subject);
            await redis.HashDeleteAsync(subKey, client);
        }

        /// <summary>
        /// Updates a consent
        /// </summary>
        /// <param name="consent">The consent</param>
        /// <returns></returns>
        public async Task UpdateAsync(Consent consent)
        {
            string subKey = redisHelper.GetIndexKey(CONSENTS, SUBJECT, consent.Subject);
            await redis.HashSetAsync(subKey, consent.ClientId, string.Join(" ", consent.Scopes));
        }
    }
}
