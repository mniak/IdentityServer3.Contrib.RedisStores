using Newtonsoft.Json;
using System.Security.Claims;

namespace IdentityServer3.Contrib.RedisStores.Models
{
    internal class ClaimModel
    {
        public ClaimModel() { }
        public ClaimModel(Claim claim)
        {
            Type = claim.Type;
            Value = claim.Value;
        }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
