using CryptoExchange.Net;
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
    public class NftClient
    {
        public TatumClient Tatum { get; protected set; }

        public NftClient(TatumClient tatumClient)
        {
            Tatum = tatumClient;
        }

        // TODO: All Endpoints
    }
}
