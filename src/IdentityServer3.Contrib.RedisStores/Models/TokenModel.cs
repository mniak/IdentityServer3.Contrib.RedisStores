using IdentityModel;
using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System;

namespace IdentityServer3.Contrib.RedisStores.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class TokenModel : ITokenModel<Token>
    {
        /// <summary>
        /// 
        /// </summary>
        internal TokenModel()
        {
            Claims = new List<ClaimModel>();
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("aud")]
        public string Audience { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("iss")]
        public string Issuer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("iat")]
        public long IssuedAt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("ttl")]
        public int Lifetime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("client_id")]
        public string ClientId { get; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("version")]
        public int Version { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("claims")]
        public List<ClaimModel> Claims { get; set; }
    }
}
