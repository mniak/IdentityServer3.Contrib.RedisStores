using IdentityServer3.Core.Models;
using System.Collections.Generic;

namespace IdentityServer3.Contrib.RedisStores.Models
{
    internal class TokenMetadata : ITokenMetadata
    {
        public string ClientId { get; set; }

        public IEnumerable<string> Scopes { get; set; }

        public string SubjectId { get; set; }
    }
}
