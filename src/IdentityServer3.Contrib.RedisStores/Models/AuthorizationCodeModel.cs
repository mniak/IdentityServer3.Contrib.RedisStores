using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace IdentityServer3.Contrib.RedisStores.Models
{
    public class AuthorizationCodeModel
    {
        [JsonProperty("challenge")]
        public string CodeChallenge { get; set; }
        [JsonProperty("challenge_method")]
        public string CodeChallengeMethod { get; set; }
        [JsonProperty("creation_time")]
        public DateTimeOffset CreationTime { get; set; }
        [JsonProperty("is_openid")]
        public bool IsOpenId { get; set; }
        [JsonProperty("nonce")]
        public string Nonce { get; set; }
        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }
        [JsonProperty("session_id")]
        public string SessionId { get; set; }
        [JsonProperty("consent_shown")]
        public bool WasConsentShown { get; set; }

        [JsonProperty("subject")]
        public ClaimsPrincipal Subject { get; set; }
        public string ClientId
        {
            get
            {
                var result = Subject?.Claims?.SingleOrDefault(x => x.Type == Constants.ClaimTypes.ClientId)?.Value;
                return result;
            }
        }

        public IEnumerable<Scope> RequestedScopes { get; set; }
    }
}
