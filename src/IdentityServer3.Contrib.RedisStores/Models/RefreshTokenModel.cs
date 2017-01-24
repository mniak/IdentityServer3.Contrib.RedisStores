using IdentityServer3.Core.Models;
using Newtonsoft.Json;
using System;
using System.Security.Claims;

namespace IdentityServer3.Contrib.RedisStores.Models
{
    public class RefreshTokenModel
    {
        [JsonProperty("access_token")]
        public Token AccessToken { get; set; }
        [JsonProperty("subject")]
        public ClaimsPrincipal Subject { get; set; }
        [JsonProperty("created_at")]
        public DateTimeOffset CreationTime { get; set; }
        [JsonProperty("ttl")]
        public int LifeTime { get; set; }
        [JsonProperty("ver")]
        public int Version { get; set; }
    }
}
