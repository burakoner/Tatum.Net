using CryptoExchange.Net.Objects;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumBlockchainRippleClient
    {
        WebCallResult<BlockchainResponse> Broadcast(string txData, string signatureId, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> BroadcastAsync(string txData, string signatureId, CancellationToken ct = default);
        WebCallResult<RippleAddressSecret> GenerateAccount(CancellationToken ct = default);
        Task<WebCallResult<RippleAddressSecret>> GenerateAccountAsync(CancellationToken ct = default);
        WebCallResult<RippleAccount> GetAccountInfo(string account, CancellationToken ct = default);
        Task<WebCallResult<RippleAccount>> GetAccountInfoAsync(string account, CancellationToken ct = default);
        WebCallResult<RippleBalance> GetBalance(string account, CancellationToken ct = default);
        Task<WebCallResult<RippleBalance>> GetBalanceAsync(string account, CancellationToken ct = default);
        WebCallResult<RippleChainFee> GetBlockchainFee(CancellationToken ct = default);
        Task<WebCallResult<RippleChainFee>> GetBlockchainFeeAsync(CancellationToken ct = default);
        WebCallResult<RippleChainInfo> GetBlockchainInformation(CancellationToken ct = default);
        Task<WebCallResult<RippleChainInfo>> GetBlockchainInformationAsync(CancellationToken ct = default);
        WebCallResult<RippleLedger> GetLedger(long index, CancellationToken ct = default);
        Task<WebCallResult<RippleLedger>> GetLedgerAsync(long index, CancellationToken ct = default);
        WebCallResult<RippleTransactionData> GetTransactionByHash(string hash, CancellationToken ct = default);
        Task<WebCallResult<RippleTransactionData>> GetTransactionByHashAsync(string hash, CancellationToken ct = default);
        WebCallResult<RippleAccountTransactions> GetTransactionsByAccount(string account, int? min = null, RippleMarker marker = null, CancellationToken ct = default);
        Task<WebCallResult<RippleAccountTransactions>> GetTransactionsByAccountAsync(string account, int? min = null, RippleMarker marker = null, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> ModifyAccountSettings(string fromAccount, string fromSecret = null, string signatureId = null, string fee = null, bool rippling = true, bool requireDestinationTag = true, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> ModifyAccountSettingsAsync(string fromAccount, string fromSecret = null, string signatureId = null, string fee = null, bool rippling = true, bool requireDestinationTag = true, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Send(string fromAccount, string to, string amount, string fromSecret = null, string signatureId = null, string fee = null, string sourceTag = null, string destinationTag = null, string issuerAccount = null, string token = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> SendAsync(string fromAccount, string to, string amount, string fromSecret = null, string signatureId = null, string fee = null, string sourceTag = null, string destinationTag = null, string issuerAccount = null, string token = null, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> TrustLine(string fromAccount, string issuerAccount, string limit, string token, string fromSecret = null, string signatureId = null, string fee = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> TrustLineAsync(string fromAccount, string issuerAccount, string limit, string token, string fromSecret = null, string signatureId = null, string fee = null, CancellationToken ct = default);

    }
}
