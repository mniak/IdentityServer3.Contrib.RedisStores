using IdentityServer3.Contrib.RedisStores.Models;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using Newtonsoft.Json;
using System;

namespace IdentityServer3.Contrib.RedisStores.Serialization
{
    class AuthorizationCodeConverter : JsonConverter
    {
        private readonly IClientStore clientStore;

        public AuthorizationCodeConverter(IClientStore clientStore)
        {
            this.clientStore = clientStore;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(AuthorizationCode);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var model = serializer.Deserialize<AuthorizationCodeModel>(reader);
            var clientTask = clientStore.FindClientByIdAsync(model.ClientId);
            var authCode = new AuthorizationCode()
            {
                CodeChallenge = model.CodeChallenge,
                CodeChallengeMethod = model.CodeChallengeMethod,
                CreationTime = model.CreationTime,
                IsOpenId = model.IsOpenId,
                Nonce = model.Nonce,
                RedirectUri = model.RedirectUri,
                RequestedScopes = model.RequestedScopes,
                SessionId = model.SessionId,
                Subject = model.Subject,
                WasConsentShown = model.WasConsentShown
            };
            authCode.Client = clientTask.Result;
            return authCode;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var authCode = value as AuthorizationCode;
            var model = new AuthorizationCodeModel()
            {
                CodeChallenge = authCode.CodeChallenge,
                CodeChallengeMethod = authCode.CodeChallengeMethod,
                CreationTime = authCode.CreationTime,
                IsOpenId = authCode.IsOpenId,
                Nonce = authCode.Nonce,
                RedirectUri = authCode.RedirectUri,
                SessionId = authCode.SessionId,
                Subject = authCode.Subject,
                WasConsentShown = authCode.WasConsentShown
            };
            serializer.Serialize(writer, model);
        }
    }
}
