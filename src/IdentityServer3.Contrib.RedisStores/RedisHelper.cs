using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace IdentityServer3.Contrib.RedisStores
{
    internal class RedisHelper
    {
        private readonly IDatabase redis;
        private readonly RedisTokenHandleStoreOptions options;

        public RedisHelper(IDatabase redis, RedisTokenHandleStoreOptions options)
        {
            this.redis = redis;
            this.options = options;
        }
        public string GetEntryKey(string collection, string id)
        {
            return $"{options.KeyPrefix}{collection}:{id}";
        }
        public string GetIdsKey(string collection)
        {
            return $"{options.KeyPrefix}{collection}_ids";
        }
        public string GetIndexKey(string collection, string index, string indexValue)
        {
            return $"{options.KeyPrefix}{collection}_idx[{index}]:{indexValue ?? "NULL"}";
        }
        public async Task<T> RetrieveAsync<T>(string collection, string id)
        {
            var json = await redis.StringGetAsync(GetEntryKey(collection, id));
            if (json.IsNullOrEmpty)
            {
                await DeleteFromIndexIdAsync(collection, id);
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(json);
        }
        public async Task<bool> StoreAsync<T>(string collection, string id, T obj, int? lifetime = null)
        {
            var entryKey = GetEntryKey(collection, id);
            var json = JsonConvert.SerializeObject(obj);
            var expiry = lifetime.HasValue ? new TimeSpan(0, 0, lifetime.Value) : (TimeSpan?)null;
            var result = await redis.StringSetAsync(entryKey, json, expiry: expiry);
            return result;
        }
        public async Task DeleteByIdAsync(string collection, string id)
        {
            await redis.KeyDeleteAsync(GetEntryKey(collection, id));
        }
        private async Task DeleteFromIndexIdAsync(string collection, string id)
        {
            var idsIndex = GetIdsKey(collection);
            await redis.SetRemoveAsync(idsIndex, id);
        }
    }
}
