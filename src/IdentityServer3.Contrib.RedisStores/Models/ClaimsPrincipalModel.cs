using Newtonsoft.Json;
using System.Collections.Generic;

namespace IdentityServer3.Contrib.RedisStores.Models
{
    internal class ClaimsPrincipalModel
    {
        [JsonProperty("amr")]
        public string AuthenticationType { get; set; }
        [JsonProperty("claims")]
        public List<Models.ClaimModel> Claims { get; set; }
    }
}
