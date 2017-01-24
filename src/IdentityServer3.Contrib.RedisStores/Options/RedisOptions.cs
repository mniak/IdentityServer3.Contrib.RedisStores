using IdentityServer3.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer3.Contrib.RedisStores
{
    public class RedisOptions
    {
        public string KeyPrefix { get; set; }
    }
}
