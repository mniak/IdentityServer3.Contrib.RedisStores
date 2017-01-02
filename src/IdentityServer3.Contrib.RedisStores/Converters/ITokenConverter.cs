using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer3.Contrib.RedisStores.Converters
{
    interface ITokenConverter<TToken, TModel>
    {
        TToken GetToken(TModel model);
        TModel GetModel(TToken token);
    }
}
