using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace IdentityServer3.Contrib.RedisStores
{
    internal class RedisHelper<T>
    {
        private readonly IDatabase redis;
        private readonly RedisOptions options;
        private readonly JsonSerializerSettings serializerSettings;

        public RedisHelper(IDatabase redis, RedisOptions options, JsonSerializerSettings serializerSettings)
        {
            this.redis = redis;
            this.options = options;
            this.serializerSettings = serializerSettings;
        }
        public string GetEntryKey(string collection, string id)
        {
            return $"{options.KeyPrefix}{collection}[{id}]";
        }
        public string GetIdsKey(string collection)
        {
            return $"{options.KeyPrefix}{collection}_ids";
        }
        public string GetIndexKey(string collection, string indexName, string indexValue)
        {
            return $"{options.KeyPrefix}{collection}({indexName}={indexValue ?? "null"}))";
        }
        public async Task<T> RetrieveAsync(string collection, string id)
        {
            var json = await redis.StringGetAsync(GetEntryKey(collection, id));
            if (json.IsNullOrEmpty)
            {
                await DeleteFromIndexIdAsync(collection, id);
                return default(T);
            }
            return Deserialize(json);
        }


        public async Task<bool> StoreAsync(string collection, string id, T obj, int? lifetime = null)
        {
            var entryKey = GetEntryKey(collection, id);
            string json = Serialize(obj);
            var expiry = lifetime.HasValue ? new TimeSpan(0, 0, lifetime.Value) : (TimeSpan?)null;
            var result = await redis.StringSetAsync(entryKey, json, expiry: expiry);
            return result;
        }

        public async Task DeleteByIdAsync(string collection, string id)
        {
            await redis.KeyDeleteAsync(GetEntryKey(collection, id));
        }
        public async Task DeleteFromIndexIdAsync(string collection, string id)
        {
            var idsIndex = GetIdsKey(collection);
            await redis.SetRemoveAsync(idsIndex, id);
        }
        public async Task ExtendLifetimeAsync(string key, int? lifetime, bool setExpirationIfEternal = false)
        {
            if (!lifetime.HasValue)
                return;
            var ttl = await redis.KeyTimeToLiveAsync(key);
            if (setExpirationIfEternal && !ttl.HasValue || ttl.HasValue && lifetime.Value > ttl.Value.TotalSeconds)
                await redis.KeyExpireAsync(key, DateTime.Now.AddSeconds(lifetime.Value));
        }

        private string Serialize(T obj)
        {
            return JsonConvert.SerializeObject(obj, serializerSettings);
        }

        private T Deserialize(RedisValue json)
        {
            return JsonConvert.DeserializeObject<T>(json, serializerSettings);
        }
    }
}
