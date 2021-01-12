using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumBlockchainVeChainClient
    {
        WebCallResult<BlockchainResponse> VeChain_Broadcast(string txData, string signatureId, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> VeChain_Broadcast_Async(string txData, string signatureId, CancellationToken ct = default);
        WebCallResult<decimal> VeChain_EstimateGasForTransaction(string from, string to, decimal value, string data = null, long? nonce = null, CancellationToken ct = default);
        Task<WebCallResult<decimal>> VeChain_EstimateGasForTransaction_Async(string from, string to, decimal value, string data = null, long? nonce = null, CancellationToken ct = default);
        WebCallResult<TatumAddress> VeChain_GenerateDepositAddress(string xpub, int index, CancellationToken ct = default);
        Task<WebCallResult<TatumAddress>> VeChain_GenerateDepositAddress_Async(string xpub, int index, CancellationToken ct = default);
        WebCallResult<TatumKey> VeChain_GeneratePrivateKey(IEnumerable<string> mnemonics, int index, CancellationToken ct = default);
        WebCallResult<TatumKey> VeChain_GeneratePrivateKey(string mnemonics, int index, CancellationToken ct = default);
        Task<WebCallResult<TatumKey>> VeChain_GeneratePrivateKey_Async(IEnumerable<string> mnemonics, int index, CancellationToken ct = default);
        Task<WebCallResult<TatumKey>> VeChain_GeneratePrivateKey_Async(string mnemonics, int index, CancellationToken ct = default);
        WebCallResult<BlockchainWallet> VeChain_GenerateWallet(IEnumerable<string> mnemonics = null, CancellationToken ct = default);
        WebCallResult<BlockchainWallet> VeChain_GenerateWallet(string mnemonics, CancellationToken ct = default);
        Task<WebCallResult<BlockchainWallet>> VeChain_GenerateWallet_Async(IEnumerable<string> mnemonics = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainWallet>> VeChain_GenerateWallet_Async(string mnemonics, CancellationToken ct = default);
        WebCallResult<VeChainBalance> VeChain_GetBalance(string address, CancellationToken ct = default);
        Task<WebCallResult<VeChainBalance>> VeChain_GetBalance_Async(string address, CancellationToken ct = default);
        WebCallResult<VeChainBlock> VeChain_GetBlock(string hash_height, CancellationToken ct = default);
        Task<WebCallResult<VeChainBlock>> VeChain_GetBlock_Async(string hash_height, CancellationToken ct = default);
        WebCallResult<long> VeChain_GetCurrentBlock(CancellationToken ct = default);
        Task<WebCallResult<long>> VeChain_GetCurrentBlock_Async(CancellationToken ct = default);
        WebCallResult<VeChainEnergy> VeChain_GetEnergy(string address, CancellationToken ct = default);
        Task<WebCallResult<VeChainEnergy>> VeChain_GetEnergy_Async(string address, CancellationToken ct = default);
        WebCallResult<VeChainTransaction> VeChain_GetTransactionByHash(string hash, CancellationToken ct = default);
        Task<WebCallResult<VeChainTransaction>> VeChain_GetTransactionByHash_Async(string hash, CancellationToken ct = default);
        WebCallResult<VeChainTransactionReceipt> VeChain_GetTransactionReceipt(string hash, CancellationToken ct = default);
        Task<WebCallResult<VeChainTransactionReceipt>> VeChain_GetTransactionReceipt_Async(string hash, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> VeChain_Send(string to, decimal amount, string fromPrivateKey = null, string signatureId = null, string data = null, VeChainFee fee = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> VeChain_Send_Async(string to, decimal amount, string fromPrivateKey = null, string signatureId = null, string data = null, VeChainFee fee = null, CancellationToken ct = default);
    }
}
