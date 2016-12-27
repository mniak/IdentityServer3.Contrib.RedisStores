using IdentityServer3.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using IdentityServer3.Contrib.RedisStores.Models;

namespace IdentityServer3.Contrib.RedisStores.Stores
{
    public class RefreshTokenStore: RedisTransientStore<RefreshToken, RefreshTokenModel>
    {

    }
}
