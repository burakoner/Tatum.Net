using CryptoExchange.Net.Objects;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumBlockchainBinanceClient
    {
        WebCallResult<BlockchainResponse> Broadcast(string txData, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> BroadcastAsync(string txData, CancellationToken ct = default);
        WebCallResult<BinanceAddress> GenerateAccount(CancellationToken ct = default);
        Task<WebCallResult<BinanceAddress>> GenerateAccountAsync(CancellationToken ct = default);
        WebCallResult<BinanceAccount> GetAccountInfo(string account, CancellationToken ct = default);
        Task<WebCallResult<BinanceAccount>> GetAccountInfoAsync(string account, CancellationToken ct = default);
        WebCallResult<long> GetCurrentBlock(CancellationToken ct = default);
        Task<WebCallResult<long>> GetCurrentBlockAsync(CancellationToken ct = default);
        WebCallResult<BinanceTransaction> GetTransaction(string hash, CancellationToken ct = default);
        WebCallResult<BinanceBlockTransactions> GetTransactionsInBlock(long height, CancellationToken ct = default);
        Task<WebCallResult<BinanceBlockTransactions>> GetTransactionsInBlockAsync(long height, CancellationToken ct = default);
        Task<WebCallResult<BinanceTransaction>> GetTransactionAsync(string hash, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Send(string to, string currency, string amount, string fromPrivateKey = null, string signatureId = null, string message = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> SendAsync(string to, string currency, string amount, string fromPrivateKey = null, string signatureId = null, string message = null, CancellationToken ct = default);

    }
}
