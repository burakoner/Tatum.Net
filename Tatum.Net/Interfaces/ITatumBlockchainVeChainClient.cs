using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumBlockchainVeChainClient
    {
        WebCallResult<BlockchainResponse> Broadcast(string txData, string signatureId, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> BroadcastAsync(string txData, string signatureId, CancellationToken ct = default);
        WebCallResult<decimal> EstimateGasForTransaction(string from, string to, decimal value, string data = null, long? nonce = null, CancellationToken ct = default);
        Task<WebCallResult<decimal>> EstimateGasForTransactionAsync(string from, string to, decimal value, string data = null, long? nonce = null, CancellationToken ct = default);
        WebCallResult<TatumAddress> GenerateDepositAddress(string xpub, int index, CancellationToken ct = default);
        Task<WebCallResult<TatumAddress>> GenerateDepositAddressAsync(string xpub, int index, CancellationToken ct = default);
        WebCallResult<TatumKey> GeneratePrivateKey(IEnumerable<string> mnemonics, int index, CancellationToken ct = default);
        WebCallResult<TatumKey> GeneratePrivateKey(string mnemonics, int index, CancellationToken ct = default);
        Task<WebCallResult<TatumKey>> GeneratePrivateKeyAsync(IEnumerable<string> mnemonics, int index, CancellationToken ct = default);
        Task<WebCallResult<TatumKey>> GeneratePrivateKeyAsync(string mnemonics, int index, CancellationToken ct = default);
        WebCallResult<BlockchainWallet> GenerateWallet(IEnumerable<string> mnemonics = null, CancellationToken ct = default);
        WebCallResult<BlockchainWallet> GenerateWallet(string mnemonics, CancellationToken ct = default);
        Task<WebCallResult<BlockchainWallet>> GenerateWalletAsync(IEnumerable<string> mnemonics = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainWallet>> GenerateWalletAsync(string mnemonics, CancellationToken ct = default);
        WebCallResult<VeChainBalance> GetBalance(string address, CancellationToken ct = default);
        Task<WebCallResult<VeChainBalance>> GetBalanceAsync(string address, CancellationToken ct = default);
        WebCallResult<VeChainBlock> GetBlock(string hash_height, CancellationToken ct = default);
        Task<WebCallResult<VeChainBlock>> GetBlockAsync(string hash_height, CancellationToken ct = default);
        WebCallResult<long> GetCurrentBlock(CancellationToken ct = default);
        Task<WebCallResult<long>> GetCurrentBlockAsync(CancellationToken ct = default);
        WebCallResult<VeChainEnergy> GetEnergy(string address, CancellationToken ct = default);
        Task<WebCallResult<VeChainEnergy>> GetEnergyAsync(string address, CancellationToken ct = default);
        WebCallResult<VeChainTransaction> GetTransactionByHash(string hash, CancellationToken ct = default);
        Task<WebCallResult<VeChainTransaction>> GetTransactionByHashAsync(string hash, CancellationToken ct = default);
        WebCallResult<VeChainTransactionReceipt> GetTransactionReceipt(string hash, CancellationToken ct = default);
        Task<WebCallResult<VeChainTransactionReceipt>> GetTransactionReceiptAsync(string hash, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Send(string to, decimal amount, string fromPrivateKey = null, string signatureId = null, string data = null, VeChainFee fee = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> SendAsync(string to, decimal amount, string fromPrivateKey = null, string signatureId = null, string data = null, VeChainFee fee = null, CancellationToken ct = default);
    }
}
