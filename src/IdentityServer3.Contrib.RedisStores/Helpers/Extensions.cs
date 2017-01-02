using IdentityServer3.Contrib.RedisStores.Models;
using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer3.Contrib.RedisStores
{
    internal static class Extensions
    {
        public static List<ClaimModel> GetClaimModels(this IEnumerable<Claim> claims)
        {
            var result = claims.Select(x => new ClaimModel(x.Type, x.Value));
            return result.ToList();
        }
        public static List<Claim> GetClaims(this IEnumerable<ClaimModel> claims, string authenticationType = null)
        {
            var result = claims.Select(x => string.IsNullOrEmpty(authenticationType)
                                                ? new Claim(x.Type, x.Value)
                                                : new Claim(x.Type, x.Value, authenticationType));
            return result.ToList();
        }
        public static IEnumerable<Scope> GetScopes(this IEnumerable<string> scopeNames)
        {
            var result = scopeNames.Select(x => new Scope() { Name = x });
            return result;
        }
    }
}
