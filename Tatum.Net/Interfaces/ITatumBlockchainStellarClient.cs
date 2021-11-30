using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumBlockchainStellarClient
    {
        WebCallResult<BlockchainResponse> Broadcast(string txData, string signatureId, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> BroadcastAsync(string txData, string signatureId, CancellationToken ct = default);
        WebCallResult<StellarAddressSecret> GenerateAccount(CancellationToken ct = default);
        Task<WebCallResult<StellarAddressSecret>> GenerateAccountAsync(CancellationToken ct = default);
        WebCallResult<StellarAccountInfo> GetAccountInfo(string account, CancellationToken ct = default);
        Task<WebCallResult<StellarAccountInfo>> GetAccountInfoAsync(string account, CancellationToken ct = default);
        WebCallResult<decimal> GetBlockchainFee(CancellationToken ct = default);
        Task<WebCallResult<decimal>> GetBlockchainFeeAsync(CancellationToken ct = default);
        WebCallResult<StellarChainInfo> GetBlockchainInformation(CancellationToken ct = default);
        Task<WebCallResult<StellarChainInfo>> GetBlockchainInformationAsync(CancellationToken ct = default);
        WebCallResult<StellarChainInfo> GetLedger(long sequence, CancellationToken ct = default);
        Task<WebCallResult<StellarChainInfo>> GetLedgerAsync(long sequence, CancellationToken ct = default);
        WebCallResult<StellarTransaction> GetTransactionByHash(string hash, CancellationToken ct = default);
        Task<WebCallResult<StellarTransaction>> GetTransactionByHashAsync(string hash, CancellationToken ct = default);
        WebCallResult<IEnumerable<StellarTransaction>> GetTransactionsByAccount(string account, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<StellarTransaction>>> GetTransactionsByAccountAsync(string account, CancellationToken ct = default);
        WebCallResult<IEnumerable<StellarTransaction>> GetTransactionsInLedger(long sequence, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<StellarTransaction>>> GetTransactionsInLedgerAsync(long sequence, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Send(string fromAccount, string to, string amount, string fromSecret = null, string signatureId = null, string token = null, string issuerAccount = null, string message = null, bool initialize = false, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> SendAsync(string fromAccount, string to, string amount, string fromSecret = null, string signatureId = null, string token = null, string issuerAccount = null, string message = null, bool initialize = false, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> TrustLine(string fromAccount, string issuerAccount, string token, string fromSecret = null, string signatureId = null, string limit = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> TrustLineAsync(string fromAccount, string issuerAccount, string token, string fromSecret = null, string signatureId = null, string limit = null, CancellationToken ct = default);

    }
}