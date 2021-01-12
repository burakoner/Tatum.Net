using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.Enums;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumSecurityKMSClient
    {
        WebCallResult<bool> KMS_CompletePendingTransaction(string id, string txId, CancellationToken ct = default);
        Task<WebCallResult<bool>> KMS_CompletePendingTransaction_Async(string id, string txId, CancellationToken ct = default);
        WebCallResult<bool> KMS_DeleteTransaction(string id, bool revert = true, CancellationToken ct = default);
        Task<WebCallResult<bool>> KMS_DeleteTransaction_Async(string id, bool revert = true, CancellationToken ct = default);
        WebCallResult<IEnumerable<KMSPendingTransaction>> KMS_GetPendingTransactions(BlockchainType chain, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<KMSPendingTransaction>>> KMS_GetPendingTransactions_Async(BlockchainType chain, CancellationToken ct = default);
        WebCallResult<KMSPendingTransaction> KMS_GetTransaction(string id, CancellationToken ct = default);
        Task<WebCallResult<KMSPendingTransaction>> KMS_GetTransaction_Async(string id, CancellationToken ct = default);
    }
}
