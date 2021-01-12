using CryptoExchange.Net.Objects;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumBlockchainRippleClient
    {
        WebCallResult<BlockchainResponse> Ripple_Broadcast(string txData, string signatureId, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Ripple_Broadcast_Async(string txData, string signatureId, CancellationToken ct = default);
        WebCallResult<RippleAddressSecret> Ripple_GenerateAccount(CancellationToken ct = default);
        Task<WebCallResult<RippleAddressSecret>> Ripple_GenerateAccount_Async(CancellationToken ct = default);
        WebCallResult<RippleAccount> Ripple_GetAccountInfo(string account, CancellationToken ct = default);
        Task<WebCallResult<RippleAccount>> Ripple_GetAccountInfo_Async(string account, CancellationToken ct = default);
        WebCallResult<RippleBalance> Ripple_GetBalance(string account, CancellationToken ct = default);
        Task<WebCallResult<RippleBalance>> Ripple_GetBalance_Async(string account, CancellationToken ct = default);
        WebCallResult<RippleChainFee> Ripple_GetBlockchainFee(CancellationToken ct = default);
        Task<WebCallResult<RippleChainFee>> Ripple_GetBlockchainFee_Async(CancellationToken ct = default);
        WebCallResult<RippleChainInfo> Ripple_GetBlockchainInformation(CancellationToken ct = default);
        Task<WebCallResult<RippleChainInfo>> Ripple_GetBlockchainInformation_Async(CancellationToken ct = default);
        WebCallResult<RippleLedger> Ripple_GetLedger(long index, CancellationToken ct = default);
        Task<WebCallResult<RippleLedger>> Ripple_GetLedger_Async(long index, CancellationToken ct = default);
        WebCallResult<RippleTransactionData> Ripple_GetTransactionByHash(string hash, CancellationToken ct = default);
        Task<WebCallResult<RippleTransactionData>> Ripple_GetTransactionByHash_Async(string hash, CancellationToken ct = default);
        WebCallResult<RippleAccountTransactions> Ripple_GetTransactionsByAccount(string account, int? min = null, RippleMarker marker = null, CancellationToken ct = default);
        Task<WebCallResult<RippleAccountTransactions>> Ripple_GetTransactionsByAccount_Async(string account, int? min = null, RippleMarker marker = null, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Ripple_ModifyAccountSettings(string fromAccount, string fromSecret = null, string signatureId = null, string fee = null, bool rippling = true, bool requireDestinationTag = true, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Ripple_ModifyAccountSettings_Async(string fromAccount, string fromSecret = null, string signatureId = null, string fee = null, bool rippling = true, bool requireDestinationTag = true, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Ripple_Send(string fromAccount, string to, string amount, string fromSecret = null, string signatureId = null, string fee = null, string sourceTag = null, string destinationTag = null, string issuerAccount = null, string token = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Ripple_Send_Async(string fromAccount, string to, string amount, string fromSecret = null, string signatureId = null, string fee = null, string sourceTag = null, string destinationTag = null, string issuerAccount = null, string token = null, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Ripple_TrustLine(string fromAccount, string issuerAccount, string limit, string token, string fromSecret = null, string signatureId = null, string fee = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Ripple_TrustLine_Async(string fromAccount, string issuerAccount, string limit, string token, string fromSecret = null, string signatureId = null, string fee = null, CancellationToken ct = default);

    }
}
