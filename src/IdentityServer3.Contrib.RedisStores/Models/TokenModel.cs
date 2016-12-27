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
            Claims = new List<KeyValuePair<string, string>>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        public TokenModel(Token token) : this()
        {
            this.ImportData(token);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Token GetToken()
        {
            var result = new Token()
            {
                Audience = Audience,
                Issuer = Issuer,
                CreationTime = IssuedAt.ToDateTimeOffsetFromEpoch(),
                Lifetime = Lifetime,
                Type = Type,
                Client = new Client() { ClientId = ClientId },
                Claims = Claims.Select(x => new Claim(x.Key, x.Value)).ToList(),
                Version = Version,
            };
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        public void ImportData(Token token)
        {
            this.Audience = token.Audience;
            this.Issuer = token.Issuer;
            this.IssuedAt = token.CreationTime.ToEpochTime();
            this.Lifetime = token.Lifetime;
            this.Type = token.Type;
            this.Version = token.Version;
            this.Claims.AddRange(token.Claims.Select(x => new KeyValuePair<string, string>(x.Type, x.Value)));
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
        [JsonProperty("cli")]
        public string ClientId { get; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("ver")]
        public int Version { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("claims")]
        public List<KeyValuePair<string, string>> Claims { get; set; }
    }
}
