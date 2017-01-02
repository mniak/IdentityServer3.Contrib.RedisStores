//using IdentityServer3.Core.Models;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace IdentityServer3.Contrib.RedisStores.Models
//{
//    /// <summary>
//    /// Represents a Refresh Token
//    /// </summary>
//    public class RefreshTokenModel : ITokenModel<RefreshToken>
//    {
//        internal RefreshTokenModel()
//        {
//            Scopes = new List<string>();
//            Claims = new List<ClaimModel>();
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        [JsonProperty("token")]
//        public TokenModel AccessToken { get; set; }
//        /// <summary>
//        /// 
//        /// </summary>
//        [JsonProperty("client_id")]
//        public string ClientId { get; }
//        /// <summary>
//        /// 
//        /// </summary>
//        [JsonProperty("created_at")]
//        public DateTimeOffset CreationTime { get; set; }
//        /// <summary>
//        /// 
//        /// </summary>
//        [JsonProperty("ttl")]
//        public int LifeTime { get; set; }
//        /// <summary>
//        /// 
//        /// </summary>
//        [JsonProperty("scopes")]
//        public List<string> Scopes { get; set; }

//        /// <summary>
//        /// 
//        /// </summary>
//        [JsonProperty("sub")]
//        public string SubjectId { get; }
//        /// <summary>
//        /// 
//        /// </summary>
//        [JsonProperty("ver")]
//        public int Version { get; set; }

//        /// <summary>
//        /// 
//        /// </summary>
//        [JsonProperty("claims")]
//        public List<ClaimModel> Claims { get; set; }
//        /// <summary>
//        /// 
//        /// </summary>
//        [JsonProperty("auth_type")]
//        public string AuthenticationType { get; set; }
//    }
//}
