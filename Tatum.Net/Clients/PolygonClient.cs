using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.Converters;
using Tatum.Net.Enums;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Clients
{
    public class PolygonClient
    {
        public TatumClient Tatum { get; protected set; }
        protected AuthenticationProvider AuthProvider { get; set; }

        public PolygonClient(TatumClient tatumClient, AuthenticationProvider authProvider)
        {
            Tatum = tatumClient;
            AuthProvider = authProvider;
        }

        // TODO: All Endpoints
    }
}
