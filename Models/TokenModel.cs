using IdentityModel;
using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace IdentityServer3.Contrib.RedisStores.Models
{
    class TokenModel
    {
        public TokenModel()
        {
            Claims = new Dictionary<string, string>();
        }
        public TokenModel(Token token) : this()
        {
            this.Audience = token.Audience;
            this.Issuer = token.Issuer;
            this.IssuedAt = token.CreationTime.ToEpochTime();
            this.Lifetime = token.Lifetime;
            this.Type = token.Type;
            this.Version = token.Version;
            this.Claims = token.Claims.ToDictionary(x => x.Type, x => x.Value);
        }
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
        public ITokenMetadata GetTokenMetadata()
        {
            return new TokenMetadata()
            {
                ClientId = ClientId,
                Scopes = Scopes,
                SubjectId = SubjectId,
            };
        }
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
        [JsonProperty("cli")]
        public string ClientId { get; }
        [JsonProperty("ver")]
        public int Version { get; set; }

        [JsonProperty("claims")]
        public Dictionary<string, string> Claims { get; set; }

        [JsonIgnore]
        public IEnumerable<string> Scopes
        {
            get
            {
                return Claims
                    .Where(x => x.Key == Constants.ClaimTypes.Scope)
                    .Select(x => x.Value);
            }
        }
        [JsonIgnore]
        public string SubjectId
        {
            get
            {
                return Claims.ContainsKey(Constants.ClaimTypes.Subject)
                     ? Claims.SingleOrDefault(x => x.Key == Constants.ClaimTypes.Subject).Value
                     : null;
            }
        }
    }
}
