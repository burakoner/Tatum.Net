using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumOffchainWithdrawalClient
    {
        WebCallResult<OffchainResponse> Broadcast(string currency, string txData, string withdrawalId, string signatureId, CancellationToken ct = default);
        Task<WebCallResult<OffchainResponse>> BroadcastAsync(string currency, string txData, string withdrawalId, string signatureId, CancellationToken ct = default);
        WebCallResult<bool> Cancel(string id, bool revert = true, CancellationToken ct = default);
        Task<WebCallResult<bool>> CancelAsync(string id, bool revert = true, CancellationToken ct = default);
        WebCallResult<bool> CompleteRequest(string id, string txId, CancellationToken ct = default);
        Task<WebCallResult<bool>> CompleteRequestAsync(string id, string txId, CancellationToken ct = default);
        WebCallResult<OffchainWithdrawalResponse> Request(string senderAccountId, string address, decimal amount, string attr = null, bool? compliant = null, decimal? fee = null, IEnumerable<string> multipleAmounts = null, string paymentId = null, string senderNote = null, CancellationToken ct = default);
        Task<WebCallResult<OffchainWithdrawalResponse>> RequestAsync(string senderAccountId, string address, decimal amount, string attr = null, bool? compliant = null, decimal? fee = null, IEnumerable<string> multipleAmounts = null, string paymentId = null, string senderNote = null, CancellationToken ct = default);
    }
}
