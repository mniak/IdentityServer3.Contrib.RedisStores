using IdentityServer3.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer3.Contrib.RedisStores.Models
{
    public class RefreshTokenModel : ITokenModel<RefreshToken>
    {
        internal RefreshTokenModel()
        {
            Scopes = new List<string>();
        }

        public RefreshToken GetToken()
        {
            var token = new RefreshToken();
            token.AccessToken = token
            return token;
        }

        public void ImportData(RefreshToken token)
        {
            throw new NotImplementedException();
        }

        [JsonProperty("")]
        public Token AccessToken { get; set; }
        [JsonProperty("")]
        public string ClientId { get; }
        [JsonProperty("")]
        public DateTimeOffset CreationTime { get; set; }
        [JsonProperty("")]
        public int LifeTime { get; set; }
        [JsonProperty("")]
        public List<string> Scopes { get; set; }

        [JsonProperty("")]
        public string SubjectId { get; }
        [JsonProperty("")]
        public int Version { get; set; }

        [JsonProperty("claims")]
        public Dictionary<string, string> Claims { get; set; }
        [JsonProperty("auth_type")]
        public string AuthenticationType { get; set; }
    }
}
