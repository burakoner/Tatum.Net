using CryptoExchange.Net.Objects;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumBlockchainBinanceClient
    {
        WebCallResult<BlockchainResponse> Binance_Broadcast(string txData, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Binance_Broadcast_Async(string txData, CancellationToken ct = default);
        WebCallResult<BinanceAddress> Binance_GenerateAccount(CancellationToken ct = default);
        Task<WebCallResult<BinanceAddress>> Binance_GenerateAccount_Async(CancellationToken ct = default);
        WebCallResult<BinanceAccount> Binance_GetAccountInfo(string account, CancellationToken ct = default);
        Task<WebCallResult<BinanceAccount>> Binance_GetAccountInfo_Async(string account, CancellationToken ct = default);
        WebCallResult<long> Binance_GetCurrentBlock(CancellationToken ct = default);
        Task<WebCallResult<long>> Binance_GetCurrentBlock_Async(CancellationToken ct = default);
        WebCallResult<BinanceTransaction> Binance_GetTransaction(string hash, CancellationToken ct = default);
        WebCallResult<BinanceBlockTransactions> Binance_GetTransactionsInBlock(long height, CancellationToken ct = default);
        Task<WebCallResult<BinanceBlockTransactions>> Binance_GetTransactionsInBlock_Async(long height, CancellationToken ct = default);
        Task<WebCallResult<BinanceTransaction>> Binance_GetTransaction_Async(string hash, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Binance_Send(string to, string currency, string amount, string fromPrivateKey = null, string signatureId = null, string message = null, CancellationToken ct = default);
    }
}
