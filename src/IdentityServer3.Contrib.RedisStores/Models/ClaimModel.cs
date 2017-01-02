using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer3.Contrib.RedisStores.Models
{
    /// <summary>
    /// Represents a claim
    /// </summary>
    public class ClaimModel
    {
        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ClaimModel()
        {

        }
        /// <summary>
        /// Creates a new ClaimModel by the the type and value
        /// </summary>
        /// <param name="type">The type of the claim</param>
        /// <param name="value">The value of the claim</param>
        public ClaimModel(string type, string value)
        {
            this.Type = type;
            this.Value = value;
        }
        /// <summary>
        /// Creates a new ClaimModel extract the values from a Claim
        /// </summary>
        /// <param name="claim">The claim from which extract the values</param>
        public ClaimModel(Claim claim)
        {
            this.Type = claim.Type;
            this.Value = claim.Value;
        }

        /// <summary>
        /// The type of the claim model
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// The value of the claim model
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
