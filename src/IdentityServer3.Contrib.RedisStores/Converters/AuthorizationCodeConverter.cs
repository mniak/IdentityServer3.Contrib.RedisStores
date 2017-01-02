using IdentityServer3.Contrib.RedisStores.Models;
using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer3.Contrib.RedisStores.Converters
{
    class AuthorizationCodeConverter : ITokenConverter<AuthorizationCode, AuthorizationCodeModel>
    {
        public AuthorizationCodeModel GetModel(AuthorizationCode authCode)
        {
            var result = new AuthorizationCodeModel()
            {
                ClientId = authCode.ClientId,
                CodeChallenge = authCode.CodeChallenge,
                CodeChallengeMethod = authCode.CodeChallengeMethod,
                CreationTime = authCode.CreationTime,
                IsOpenId = authCode.IsOpenId,
                Nonce = authCode.Nonce,
                RedirectUri = authCode.RedirectUri,
                Scopes = authCode.Scopes.ToList(),
                SessionId = authCode.SessionId,
                Claims = authCode.Subject.Claims.GetClaimModels(),
                AuthenticationType = authCode.Subject.Identity.AuthenticationType,
                WasConsentShown = authCode.WasConsentShown
            };
            return result;
        }

        public AuthorizationCode GetToken(AuthorizationCodeModel model)
        {
            var result = new AuthorizationCode()
            {
                Client = new Client() { ClientId = model.ClientId },
                CodeChallenge = model.CodeChallenge,
                CodeChallengeMethod = model.CodeChallengeMethod,
                CreationTime = model.CreationTime,
                IsOpenId = model.IsOpenId,
                Nonce = model.Nonce,
                RedirectUri = model.RedirectUri,
                RequestedScopes = model.Scopes.GetScopes(),
                SessionId = model.SessionId,
                Subject = new ClaimsPrincipal(new ClaimsIdentity(model.Claims.GetClaims(model.AuthenticationType))),
                WasConsentShown = model.WasConsentShown
            };
            return result;
        }
    }
}
