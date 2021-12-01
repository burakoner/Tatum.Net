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
    public class IpfsClient
    {
        public TatumClient Tatum { get; protected set; }

        protected const string Endpoints_Store = "ipfs";
        protected const string Endpoints_Get = "ipfs";

        public IpfsClient(TatumClient tatumClient)
        {
            Tatum = tatumClient;
        }

        // TODO
        public virtual WebCallResult<BlockchainResponse> StoreData(byte[] contents, CancellationToken ct = default) 
            => StoreDataAsync(contents, ct).Result;
        public virtual async Task<WebCallResult<BlockchainResponse>> StoreDataAsync(byte[] contents, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        // TODO
        public virtual WebCallResult<TatumData> GetData(string id, CancellationToken ct = default) 
            => GetDataAsync(id, ct).Result;
        public virtual async Task<WebCallResult<TatumData>> GetDataAsync(string id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
