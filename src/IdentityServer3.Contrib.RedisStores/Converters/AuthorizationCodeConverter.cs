using IdentityServer3.Contrib.RedisStores.Models;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
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
        private readonly IClientStore clientStore;

        public AuthorizationCodeConverter(IClientStore clientStore)
        {
            this.clientStore = clientStore;
        }
        public Task<AuthorizationCodeModel> GetModelAsync(AuthorizationCode authCode)
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
            return Task.FromResult(result);
        }

        public async Task<AuthorizationCode> GetTokenAsync(AuthorizationCodeModel model)
        {
            var client = await clientStore.FindClientByIdAsync(model.ClientId);
            var result = new AuthorizationCode()
            {
                Client = client,
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
