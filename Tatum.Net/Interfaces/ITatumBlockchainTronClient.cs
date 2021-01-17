using CryptoExchange.Net.Objects;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumBlockchainTronClient
    {
        WebCallResult<BlockchainResponse> Tron_Broadcast(string txData, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Tron_Broadcast_Async(string txData, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Tron_FreezeBalance(string fromPrivateKey, string receiver, int duration, string resource, decimal amount, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Tron_FreezeBalance_Async(string fromPrivateKey, string receiver, int duration, string resource, decimal amount, CancellationToken ct = default);
        WebCallResult<TronWallet> Tron_GenerateAccount(CancellationToken ct = default);
        Task<WebCallResult<TronWallet>> Tron_GenerateAccount_Async(CancellationToken ct = default);
        WebCallResult<TronBlock> Tron_GetBlock(string hash_height, CancellationToken ct = default);
        Task<WebCallResult<TronBlock>> Tron_GetBlock_Async(string hash_height, CancellationToken ct = default);
        WebCallResult<TronCurrentBlock> Tron_GetCurrentBlock(CancellationToken ct = default);
        Task<WebCallResult<TronCurrentBlock>> Tron_GetCurrentBlock_Async(CancellationToken ct = default);
        WebCallResult<TronTransaction> Tron_GetTransactionByHash(string hash, CancellationToken ct = default);
        Task<WebCallResult<TronTransaction>> Tron_GetTransactionByHash_Async(string hash, CancellationToken ct = default);
        WebCallResult<TronAccountTransactions> Tron_GetTransactionsByAccount(string address, CancellationToken ct = default);
        Task<WebCallResult<TronAccountTransactions>> Tron_GetTransactionsByAccount_Async(string address, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Tron_Send(string fromPrivateKey, string to, decimal amount, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Tron_Send_Async(string fromPrivateKey, string to, decimal amount, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Tron_TRC10CreateToken(string fromPrivateKey, string recipient, string name, string abbreviation, string description, string url, long totalSupply, int decimals, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Tron_TRC10CreateToken_Async(string fromPrivateKey, string recipient, string name, string abbreviation, string description, string url, long totalSupply, int decimals, CancellationToken ct = default);
        WebCallResult<TronTRC10Token> Tron_TRC10GetTokenDetails(long id, CancellationToken ct = default);
        Task<WebCallResult<TronTRC10Token>> Tron_TRC10GetTokenDetails_Async(long id, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Tron_TRC10Send(string fromPrivateKey, string to, long tokenId, decimal amount, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Tron_TRC10Send_Async(string fromPrivateKey, string to, long tokenId, decimal amount, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Tron_TRC20CreateToken(string fromPrivateKey, string recipient, string name, string symbol, long totalSupply, int decimals, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Tron_TRC20CreateToken_Async(string fromPrivateKey, string recipient, string name, string symbol, long totalSupply, int decimals, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Tron_TRC20Send(string fromPrivateKey, string to, string tokenAddress, decimal amount, decimal feeLimit, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Tron_TRC20Send_Async(string fromPrivateKey, string to, string tokenAddress, decimal amount, decimal feeLimit, CancellationToken ct = default);
    }
}
