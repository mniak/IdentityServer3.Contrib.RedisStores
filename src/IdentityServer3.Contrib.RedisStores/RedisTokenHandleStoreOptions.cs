using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer3.Contrib.RedisStores
{
    /// <summary>
    /// Creates a new RedisTokenHandleStoreOptions instance
    /// </summary>
    public struct RedisTokenHandleStoreOptions
    {
        /// <summary>
        /// The prefix for all the Redis keys used
        /// </summary>
        public string KeyPrefix { get; set; }
    }
}
