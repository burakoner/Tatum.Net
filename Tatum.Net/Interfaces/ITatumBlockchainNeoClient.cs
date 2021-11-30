using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumBlockchainNeoClient
    {
        WebCallResult<BlockchainResponse> Broadcast(string txData, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> BroadcastAsync(string txData, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> ClaimGAS(string privateKey, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> ClaimGASAsync(string privateKey, CancellationToken ct = default);
        WebCallResult<NeoAccount> GenerateAccount(CancellationToken ct = default);
        Task<WebCallResult<NeoAccount>> GenerateAccountAsync(CancellationToken ct = default);
        WebCallResult<NeoAsset> GetAssetInfo(string asset, CancellationToken ct = default);
        Task<WebCallResult<NeoAsset>> GetAssetInfoAsync(string asset, CancellationToken ct = default);
        WebCallResult<NeoBalance> GetBalance(string address, CancellationToken ct = default);
        Task<WebCallResult<NeoBalance>> GetBalanceAsync(string address, CancellationToken ct = default);
        WebCallResult<NeoBlock> GetBlock(string hash_height, CancellationToken ct = default);
        Task<WebCallResult<NeoBlock>> GetBlockAsync(string hash_height, CancellationToken ct = default);
        WebCallResult<NeoContract> GetContractInfo(string scriptHash, CancellationToken ct = default);
        Task<WebCallResult<NeoContract>> GetContractInfoAsync(string scriptHash, CancellationToken ct = default);
        WebCallResult<long> GetCurrentBlock(CancellationToken ct = default);
        Task<WebCallResult<long>> GetCurrentBlockAsync(CancellationToken ct = default);
        WebCallResult<NeoTransaction> GetTransactionByHash(string hash, CancellationToken ct = default);
        Task<WebCallResult<NeoTransaction>> GetTransactionByHashAsync(string hash, CancellationToken ct = default);
        WebCallResult<IEnumerable<NeoAccountTransaction>> GetTransactionsByAccount(string address, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<NeoAccountTransaction>>> GetTransactionsByAccountAsync(string address, CancellationToken ct = default);
        WebCallResult<NeoTransactionOutput> GetUnspentTransactionOutputs(string txId, long index, CancellationToken ct = default);
        Task<WebCallResult<NeoTransactionOutput>> GetUnspentTransactionOutputsAsync(string txId, long index, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Send(string to, decimal Amount, decimal GAS_Amount, string fromPrivateKey, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> SendToken(string scriptHash, decimal amount, int numOfDecimals, string fromPrivateKey, string to, decimal additionalInvocationGas = 0, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> SendTokenAsync(string scriptHash, decimal amount, int numOfDecimals, string fromPrivateKey, string to, decimal additionalInvocationGas = 0, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> SendAsync(string to, decimal Amount, decimal GAS_Amount, string fromPrivateKey, CancellationToken ct = default);

    }
}
