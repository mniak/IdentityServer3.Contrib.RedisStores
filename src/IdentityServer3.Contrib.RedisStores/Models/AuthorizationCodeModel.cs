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
            this.Claims = new Dictionary<string, string>();
        }
        /// <summary>
        /// Converts the model into an AuthorizationCode and returns it
        /// </summary>
        /// <returns>The AuthorizationCode</returns>
        public AuthorizationCode GetToken()
        {
            var result = new AuthorizationCode();
            result.Client = new Client() { ClientId = this.ClientId };
            result.CodeChallenge = this.CodeChallenge;
            result.CodeChallengeMethod = this.CodeChallengeMethod;
            result.CreationTime = this.CreationTime;
            result.IsOpenId = this.IsOpenId;
            result.Nonce = this.Nonce;
            result.RedirectUri = this.RedirectUri;
            result.RequestedScopes = this.Scopes.Select(x => new Scope() { Name = x });
            result.SessionId = this.SessionId;
            result.Subject = new ClaimsPrincipal(new ClaimsIdentity(this.Claims.Select(x => new Claim(x.Key, x.Value)), this.AuthenticationType));
            result.WasConsentShown = this.WasConsentShown;
            return result;
        }

        /// <summary>
        /// Import an AuthorizationCode into the model
        /// </summary>
        /// <param name="authCode">The authorization code</param>
        public void ImportData(AuthorizationCode authCode)
        {
            this.ClientId = authCode.ClientId;
            this.CodeChallenge = authCode.CodeChallenge;
            this.CodeChallengeMethod = authCode.CodeChallengeMethod;
            this.CreationTime = authCode.CreationTime;
            this.IsOpenId = authCode.IsOpenId;
            this.Nonce = authCode.Nonce;
            this.RedirectUri = authCode.RedirectUri;
            this.Scopes = authCode.Scopes.ToList();
            this.SessionId = authCode.SessionId;
            this.Claims = authCode.Subject.Claims.ToDictionary(x => x.Type, x => x.Value);
            this.AuthenticationType = authCode.Subject.Identity.AuthenticationType;
            this.WasConsentShown = authCode.WasConsentShown;
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
        public Dictionary<string, string> Claims { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("auth_type")]
        public string AuthenticationType { get; set; }
    }
}
