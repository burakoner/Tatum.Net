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
        WebCallResult<bool> CompletePendingTransaction(string id, string txId, CancellationToken ct = default);
        Task<WebCallResult<bool>> CompletePendingTransactionAsync(string id, string txId, CancellationToken ct = default);
        WebCallResult<bool> DeleteTransaction(string id, bool revert = true, CancellationToken ct = default);
        Task<WebCallResult<bool>> DeleteTransactionAsync(string id, bool revert = true, CancellationToken ct = default);
        WebCallResult<IEnumerable<KMSPendingTransaction>> GetPendingTransactions(BlockchainType chain, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<KMSPendingTransaction>>> GetPendingTransactionsAsync(BlockchainType chain, CancellationToken ct = default);
        WebCallResult<KMSPendingTransaction> GetTransaction(string id, CancellationToken ct = default);
        Task<WebCallResult<KMSPendingTransaction>> GetTransactionAsync(string id, CancellationToken ct = default);
    }
}
