using IdentityModel;
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
    class TokenConverter : ITokenConverter<Token, TokenModel>
    {

        public Task<Token> GetTokenAsync(TokenModel model)
        {
            var result = new Token()
            {
                Audience = model.Audience,
                Issuer = model.Issuer,
                CreationTime = model.IssuedAt.ToDateTimeOffsetFromEpoch(),
                Lifetime = model.Lifetime,
                Type = model.Type,
                Client = new Client() { ClientId = model.ClientId },
                Claims = model.Claims.GetClaims(),
                Version = model.Version,
            };
            return Task.FromResult(result);
        }
        public Task<TokenModel> GetModelAsync(Token token)
        {
            var result = new TokenModel()
            {
                Audience = token.Audience,
                Issuer = token.Issuer,
                IssuedAt = token.CreationTime.ToEpochTime(),
                Lifetime = token.Lifetime,
                Type = token.Type,
                Version = token.Version
            };
            result.Claims.AddRange(token.Claims.GetClaimModels());
            return Task.FromResult(result);
        }
    }
}
