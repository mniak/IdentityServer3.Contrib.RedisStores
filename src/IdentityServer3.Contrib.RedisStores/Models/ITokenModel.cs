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
        where T : class, ITokenMetadata, new()
    {
        /// <summary>
        /// Converts the model to an ITokenMetadata
        /// </summary>
        /// <returns>The token</returns>
        T GetToken();

        /// <summary>
        /// Imports the data of the token to the model
        /// </summary>
        /// <param name="token">The token</param>
        void ImportData(T token);
    }
}
