using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumOffchainWithdrawalClient
    {
        WebCallResult<OffchainResponse> OffchainWithdrawal_Broadcast(string currency, string txData, string withdrawalId, string signatureId, CancellationToken ct = default);
        Task<WebCallResult<OffchainResponse>> OffchainWithdrawal_Broadcast_Async(string currency, string txData, string withdrawalId, string signatureId, CancellationToken ct = default);
        WebCallResult<bool> OffchainWithdrawal_Cancel(string id, bool revert = true, CancellationToken ct = default);
        Task<WebCallResult<bool>> OffchainWithdrawal_Cancel_Async(string id, bool revert = true, CancellationToken ct = default);
        WebCallResult<bool> OffchainWithdrawal_CompleteRequest(string id, string txId, CancellationToken ct = default);
        Task<WebCallResult<bool>> OffchainWithdrawal_CompleteRequest_Async(string id, string txId, CancellationToken ct = default);
        WebCallResult<OffchainWithdrawalResponse> OffchainWithdrawal_Request(string senderAccountId, string address, decimal amount, string attr = null, bool? compliant = null, decimal? fee = null, IEnumerable<string> multipleAmounts = null, string paymentId = null, string senderNote = null, CancellationToken ct = default);
        Task<WebCallResult<OffchainWithdrawalResponse>> OffchainWithdrawal_Request_Async(string senderAccountId, string address, decimal amount, string attr = null, bool? compliant = null, decimal? fee = null, IEnumerable<string> multipleAmounts = null, string paymentId = null, string senderNote = null, CancellationToken ct = default);
    }
}
