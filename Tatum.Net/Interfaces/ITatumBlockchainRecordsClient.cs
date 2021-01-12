using CryptoExchange.Net.Objects;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.Enums;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumBlockchainRecordsClient
    {
        WebCallResult<TatumData> Records_GetData(BlockchainType chain, string id, CancellationToken ct = default);
        Task<WebCallResult<TatumData>> Records_GetData_Async(BlockchainType chain, string id, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Records_SetData(BlockchainType chain, string data, string fromPrivateKey = null, string to = null, long? nonce = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Records_SetData_Async(BlockchainType chain, string data, string fromPrivateKey = null, string to = null, long? nonce = null, CancellationToken ct = default);
    }
}
