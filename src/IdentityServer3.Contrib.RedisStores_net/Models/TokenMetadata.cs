using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer3.Contrib.RedisStores.Models
{
    internal class TokenMetadata : ITokenMetadata
    {
        public string ClientId { get; set; }

        public IEnumerable<string> Scopes { get; set; }

        public string SubjectId { get; set; }
    }
}
