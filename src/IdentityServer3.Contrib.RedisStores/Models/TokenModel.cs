using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer3.Core.Models;
using System.Linq;
using IdentityServer3.Core;

namespace IdentityServer3.Contrib.RedisStores.Models
{
    public class TokenModel
    {
        [JsonProperty("aud")]
        public string Audience { get; set; }
        [JsonProperty("iss")]
        public string Issuer { get; set; }
        [JsonProperty("iat")]
        public long IssuedAt { get; set; }
        [JsonProperty("ttl")]
        public int Lifetime { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("claims")]
        public List<Claim> Claims { get; set; } = new List<Claim>();

        public Client Client
        {
            get
            {
                var clientId = Claims.Single(x => x.Type == Constants.ClaimTypes.ClientId)?.Value;
                var client = new Client() { ClientId = clientId };
                return client;
            }
        }
    }
}
