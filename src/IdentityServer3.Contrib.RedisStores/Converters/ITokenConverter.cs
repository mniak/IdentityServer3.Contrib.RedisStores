using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer3.Contrib.RedisStores.Converters
{
    interface ITokenConverter<TToken, TModel>
    {
        Task<TToken> GetTokenAsync(TModel model);
        Task<TModel> GetModelAsync(TToken token);
    }
}
