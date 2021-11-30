using CryptoExchange.Net.Objects;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumBlockchainTronClient
    {
        WebCallResult<BlockchainResponse> Broadcast(string txData, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> BroadcastAsync(string txData, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> FreezeBalance(string fromPrivateKey, string receiver, int duration, string resource, decimal amount, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> FreezeBalanceAsync(string fromPrivateKey, string receiver, int duration, string resource, decimal amount, CancellationToken ct = default);
        WebCallResult<TronWallet> GenerateAccount(CancellationToken ct = default);
        Task<WebCallResult<TronWallet>> GenerateAccountAsync(CancellationToken ct = default);
        WebCallResult<TronBlock> GetBlock(string hash_height, CancellationToken ct = default);
        Task<WebCallResult<TronBlock>> GetBlockAsync(string hash_height, CancellationToken ct = default);
        WebCallResult<TronCurrentBlock> GetCurrentBlock(CancellationToken ct = default);
        Task<WebCallResult<TronCurrentBlock>> GetCurrentBlockAsync(CancellationToken ct = default);
        WebCallResult<TronTransaction> GetTransactionByHash(string hash, CancellationToken ct = default);
        Task<WebCallResult<TronTransaction>> GetTransactionByHashAsync(string hash, CancellationToken ct = default);
        WebCallResult<TronAccountTransactions> GetTransactionsByAccount(string address, CancellationToken ct = default);
        Task<WebCallResult<TronAccountTransactions>> GetTransactionsByAccountAsync(string address, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Send(string fromPrivateKey, string to, decimal amount, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> SendAsync(string fromPrivateKey, string to, decimal amount, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> TRC10CreateToken(string fromPrivateKey, string recipient, string name, string abbreviation, string description, string url, long totalSupply, int decimals, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> TRC10CreateTokenAsync(string fromPrivateKey, string recipient, string name, string abbreviation, string description, string url, long totalSupply, int decimals, CancellationToken ct = default);
        WebCallResult<TronTRC10Token> TRC10GetTokenDetails(long id, CancellationToken ct = default);
        Task<WebCallResult<TronTRC10Token>> TRC10GetTokenDetailsAsync(long id, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> TRC10Send(string fromPrivateKey, string to, long tokenId, decimal amount, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> TRC10SendAsync(string fromPrivateKey, string to, long tokenId, decimal amount, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> TRC20CreateToken(string fromPrivateKey, string recipient, string name, string symbol, long totalSupply, int decimals, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> TRC20CreateTokenAsync(string fromPrivateKey, string recipient, string name, string symbol, long totalSupply, int decimals, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> TRC20Send(string fromPrivateKey, string to, string tokenAddress, decimal amount, decimal feeLimit, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> TRC20SendAsync(string fromPrivateKey, string to, string tokenAddress, decimal amount, decimal feeLimit, CancellationToken ct = default);
    }
}
