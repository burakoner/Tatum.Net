using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumBlockchainNeoClient
    {
        WebCallResult<BlockchainResponse> Neo_Broadcast(string txData, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Neo_Broadcast_Async(string txData, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Neo_ClaimGAS(string privateKey, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Neo_ClaimGAS_Async(string privateKey, CancellationToken ct = default);
        WebCallResult<NeoAccount> Neo_GenerateAccount(CancellationToken ct = default);
        Task<WebCallResult<NeoAccount>> Neo_GenerateAccount_Async(CancellationToken ct = default);
        WebCallResult<NeoAsset> Neo_GetAssetInfo(string asset, CancellationToken ct = default);
        Task<WebCallResult<NeoAsset>> Neo_GetAssetInfo_Async(string asset, CancellationToken ct = default);
        WebCallResult<NeoBalance> Neo_GetBalance(string address, CancellationToken ct = default);
        Task<WebCallResult<NeoBalance>> Neo_GetBalance_Async(string address, CancellationToken ct = default);
        WebCallResult<NeoBlock> Neo_GetBlock(string hash_height, CancellationToken ct = default);
        Task<WebCallResult<NeoBlock>> Neo_GetBlock_Async(string hash_height, CancellationToken ct = default);
        WebCallResult<NeoContract> Neo_GetContractInfo(string scriptHash, CancellationToken ct = default);
        Task<WebCallResult<NeoContract>> Neo_GetContractInfo_Async(string scriptHash, CancellationToken ct = default);
        WebCallResult<long> Neo_GetCurrentBlock(CancellationToken ct = default);
        Task<WebCallResult<long>> Neo_GetCurrentBlock_Async(CancellationToken ct = default);
        WebCallResult<NeoTransaction> Neo_GetTransactionByHash(string hash, CancellationToken ct = default);
        Task<WebCallResult<NeoTransaction>> Neo_GetTransactionByHash_Async(string hash, CancellationToken ct = default);
        WebCallResult<IEnumerable<NeoAccountTransaction>> Neo_GetTransactionsByAccount(string address, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<NeoAccountTransaction>>> Neo_GetTransactionsByAccount_Async(string address, CancellationToken ct = default);
        WebCallResult<NeoTransactionOutput> Neo_GetUnspentTransactionOutputs(string txId, long index, CancellationToken ct = default);
        Task<WebCallResult<NeoTransactionOutput>> Neo_GetUnspentTransactionOutputs_Async(string txId, long index, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Neo_Send(string to, decimal NEO_Amount, decimal GAS_Amount, string fromPrivateKey, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Neo_SendToken(string scriptHash, decimal amount, int numOfDecimals, string fromPrivateKey, string to, decimal additionalInvocationGas = 0, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Neo_SendToken_Async(string scriptHash, decimal amount, int numOfDecimals, string fromPrivateKey, string to, decimal additionalInvocationGas = 0, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Neo_Send_Async(string to, decimal NEO_Amount, decimal GAS_Amount, string fromPrivateKey, CancellationToken ct = default);

    }
}
