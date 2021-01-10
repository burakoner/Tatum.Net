using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Tatum.Net.CoreObjects
{
    public class TatumAuthenticationProvider : AuthenticationProvider
    {
        public TatumAuthenticationProvider(string apiKey) : base(new ApiCredentials(apiKey, apiKey))
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException("No valid API credentials provided. Api Key is mandatory.");
        }

        public override Dictionary<string, string> AddAuthenticationToHeaders(string uri, HttpMethod method, Dictionary<string, object> parameters, bool signed, PostParameters postParameterPosition, ArrayParametersSerialization arraySerialization)
        {
            if (Credentials.Key == null || Credentials.Secret == null)
                throw new ArgumentException("No valid API credentials provided. Api Key is mandatory.");

            if (uri.Contains("/v3/"))
            {
                return new Dictionary<string, string> {
                    { "x-api-key", Credentials.Key.GetString() },
                };
            }

            return new Dictionary<string, string>();
        }
    }
}