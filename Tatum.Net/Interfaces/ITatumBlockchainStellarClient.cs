using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumBlockchainStellarClient
    {
        WebCallResult<BlockchainResponse> Stellar_Broadcast(string txData, string signatureId, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Stellar_Broadcast_Async(string txData, string signatureId, CancellationToken ct = default);
        WebCallResult<StellarAddressSecret> Stellar_GenerateAccount(CancellationToken ct = default);
        Task<WebCallResult<StellarAddressSecret>> Stellar_GenerateAccount_Async(CancellationToken ct = default);
        WebCallResult<StellarAccountInfo> Stellar_GetAccountInfo(string account, CancellationToken ct = default);
        Task<WebCallResult<StellarAccountInfo>> Stellar_GetAccountInfo_Async(string account, CancellationToken ct = default);
        WebCallResult<decimal> Stellar_GetBlockchainFee(CancellationToken ct = default);
        Task<WebCallResult<decimal>> Stellar_GetBlockchainFee_Async(CancellationToken ct = default);
        WebCallResult<StellarChainInfo> Stellar_GetBlockchainInformation(CancellationToken ct = default);
        Task<WebCallResult<StellarChainInfo>> Stellar_GetBlockchainInformation_Async(CancellationToken ct = default);
        WebCallResult<StellarChainInfo> Stellar_GetLedger(long sequence, CancellationToken ct = default);
        Task<WebCallResult<StellarChainInfo>> Stellar_GetLedger_Async(long sequence, CancellationToken ct = default);
        WebCallResult<StellarTransaction> Stellar_GetTransactionByHash(string hash, CancellationToken ct = default);
        Task<WebCallResult<StellarTransaction>> Stellar_GetTransactionByHash_Async(string hash, CancellationToken ct = default);
        WebCallResult<IEnumerable<StellarTransaction>> Stellar_GetTransactionsByAccount(string account, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<StellarTransaction>>> Stellar_GetTransactionsByAccount_Async(string account, CancellationToken ct = default);
        WebCallResult<IEnumerable<StellarTransaction>> Stellar_GetTransactionsInLedger(long sequence, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<StellarTransaction>>> Stellar_GetTransactionsInLedger_Async(long sequence, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Stellar_Send(string fromAccount, string to, string amount, string fromSecret = null, string signatureId = null, string token = null, string issuerAccount = null, string message = null, bool initialize = false, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Stellar_Send_Async(string to, string currency, string amount, string fromPrivateKey = null, string signatureId = null, string message = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Stellar_Send_Async(string fromAccount, string to, string amount, string fromSecret = null, string signatureId = null, string token = null, string issuerAccount = null, string message = null, bool initialize = false, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Stellar_TrustLine(string fromAccount, string issuerAccount, string token, string fromSecret = null, string signatureId = null, string limit = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Stellar_TrustLine_Async(string fromAccount, string issuerAccount, string token, string fromSecret = null, string signatureId = null, string limit = null, CancellationToken ct = default);

    }
}
