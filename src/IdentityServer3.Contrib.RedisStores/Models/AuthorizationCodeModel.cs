using IdentityServer3.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer3.Contrib.RedisStores.Models
{
    /// <summary>
    /// Model of the AuthorizationCode
    /// </summary>
    public class AuthorizationCodeModel : ITokenModel<AuthorizationCode>
    {
        internal AuthorizationCodeModel()
        {
            this.Claims = new List<ClaimModel>();
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("client_id")]
        public string ClientId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("challenge")]
        public string CodeChallenge { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("challenge_method")]
        public string CodeChallengeMethod { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("creation_time")]
        public DateTimeOffset CreationTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("is_openid")]
        public bool IsOpenId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("nonce")]
        public string Nonce { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("scopes")]
        public List<string> Scopes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("session_id")]
        public string SessionId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("consent_shown")]
        public bool WasConsentShown { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("claims")]
        public List<ClaimModel> Claims { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("auth_type")]
        public string AuthenticationType { get; set; }
    }
}
