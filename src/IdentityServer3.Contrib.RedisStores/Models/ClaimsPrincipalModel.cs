using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace IdentityServer3.Contrib.RedisStores.Models
{
    internal class ClaimsPrincipalModel
    {
        public ClaimsPrincipalModel() { }
        public ClaimsPrincipalModel(ClaimsPrincipal claimsPrincipal)
        {
            this.AuthenticationType = claimsPrincipal.Identity.AuthenticationType;
            this.Claims = claimsPrincipal.Claims;
        }
        [JsonProperty("amr")]
        public string AuthenticationType { get; set; }
        [JsonProperty("claims")]
        public IEnumerable<Claim> Claims { get; set; }
    }
}
