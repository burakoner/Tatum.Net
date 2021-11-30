using CryptoExchange.Net.Objects;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.Enums;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumBlockchainRecordsClient
    {
        WebCallResult<TatumData> GetData(BlockchainType chain, string id, CancellationToken ct = default);
        Task<WebCallResult<TatumData>> GetDataAsync(BlockchainType chain, string id, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> SetData(BlockchainType chain, string data, string fromPrivateKey = null, string to = null, long? nonce = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> SetDataAsync(BlockchainType chain, string data, string fromPrivateKey = null, string to = null, long? nonce = null, CancellationToken ct = default);
    }
}
