using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer3.Contrib.RedisStores
{
    internal static class RedisHelper
    {
        public static string GetEntryKey(string collection, string id)
        {
            return $"{collection}:{id}";
        }
        public static string GetIdsKey(string collection)
        {
            return $"{collection}_ids";
        }
        public static string GetIndexKey(string collection, string index, string indexValue)
        {
            return $"{collection}_idx[{index}]:{indexValue ?? "NULL"}";
        }
        public async static Task<T> RetrieveAsync<T>(this IDatabase redis, string collection, string id)
        {
            var json = await redis.StringGetAsync(GetEntryKey(collection, id));
            if (json.IsNullOrEmpty)
            {
                await DeleteFromIndexIdAsync(redis, collection, id);
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(json);
        }
        public async static Task<bool> StoreAsync<T>(this IDatabase redis, string collection, string id, T obj, int? lifetime = null)
        {
            var entryKey = GetEntryKey(collection, id);
            var json = JsonConvert.SerializeObject(obj);
            var expiry = lifetime.HasValue ? new TimeSpan(0, 0, lifetime.Value) : (TimeSpan?)null;
            var result = await redis.StringSetAsync(entryKey, json, expiry: expiry);
            return result;
        }
        public async static Task DeleteByIdAsync(this IDatabase redis, string collection, string id)
        {
            await redis.KeyDeleteAsync(GetEntryKey(collection, id));
        }
        private async static Task DeleteFromIndexIdAsync(this IDatabase redis, string collection, string id)
        {
            var idsIndex = GetIdsKey(collection);
            await redis.SetRemoveAsync(idsIndex, id);
        }
    }
}
