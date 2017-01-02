using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer3.Contrib.RedisStores.Models
{
    /// <summary>
    /// ITokenModel interface.
    /// Represents a model for storage of an ITokenMetadata
    /// </summary>
    /// <typeparam name="T">The ITokenMetadata type</typeparam>
    public interface ITokenModel<T>
        where T : class, ITokenMetadata
    {
    }
}
