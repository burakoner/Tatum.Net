using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumBlockchainBitcoinCashClient
    {
        WebCallResult<BlockchainResponse> BitcoinCash_Broadcast(string txData, string signatureId, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> BitcoinCash_Broadcast_Async(string txData, string signatureId, CancellationToken ct = default);
        WebCallResult<TatumAddress> BitcoinCash_GenerateDepositAddress(string xpub, int index, CancellationToken ct = default);
        Task<WebCallResult<TatumAddress>> BitcoinCash_GenerateDepositAddress_Async(string xpub, int index, CancellationToken ct = default);
        WebCallResult<TatumKey> BitcoinCash_GeneratePrivateKey(IEnumerable<string> mnemonics, int index, CancellationToken ct = default);
        WebCallResult<TatumKey> BitcoinCash_GeneratePrivateKey(string mnemonics, int index, CancellationToken ct = default);
        Task<WebCallResult<TatumKey>> BitcoinCash_GeneratePrivateKey_Async(IEnumerable<string> mnemonics, int index, CancellationToken ct = default);
        Task<WebCallResult<TatumKey>> BitcoinCash_GeneratePrivateKey_Async(string mnemonics, int index, CancellationToken ct = default);
        WebCallResult<BlockchainWallet> BitcoinCash_GenerateWallet(IEnumerable<string> mnemonics = null, CancellationToken ct = default);
        WebCallResult<BlockchainWallet> BitcoinCash_GenerateWallet(string mnemonics, CancellationToken ct = default);
        Task<WebCallResult<BlockchainWallet>> BitcoinCash_GenerateWallet_Async(IEnumerable<string> mnemonics = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainWallet>> BitcoinCash_GenerateWallet_Async(string mnemonics, CancellationToken ct = default);
        WebCallResult<BitcoinCashBlock> BitcoinCash_GetBlock(string hash_height, CancellationToken ct = default);
        WebCallResult<BitcoinCashChainInfo> BitcoinCash_GetBlockchainInformation(CancellationToken ct = default);
        Task<WebCallResult<BitcoinCashChainInfo>> BitcoinCash_GetBlockchainInformation_Async(CancellationToken ct = default);
        WebCallResult<TatumHash> BitcoinCash_GetBlockHash(long block_id, CancellationToken ct = default);
        Task<WebCallResult<TatumHash>> BitcoinCash_GetBlockHash_Async(long block_id, CancellationToken ct = default);
        Task<WebCallResult<BitcoinCashBlock>> BitcoinCash_GetBlock_Async(string hash_height, CancellationToken ct = default);
        WebCallResult<BitcoinCashTransaction> BitcoinCash_GetTransactionByHash(string hash, CancellationToken ct = default);
        Task<WebCallResult<BitcoinCashTransaction>> BitcoinCash_GetTransactionByHash_Async(string hash, CancellationToken ct = default);
        WebCallResult<IEnumerable<BitcoinCashTransaction>> BitcoinCash_GetTransactionsByAddress(string address, int skip = 0, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<BitcoinCashTransaction>>> BitcoinCash_GetTransactionsByAddress_Async(string address, int skip = 0, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> BitcoinCash_Send(IEnumerable<BitcoinCashSendOrderFromUTXO> fromUTXO, IEnumerable<BitcoinCashSendOrderTo> to, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> BitcoinCash_Send_Async(IEnumerable<BitcoinCashSendOrderFromUTXO> fromUTXO, IEnumerable<BitcoinCashSendOrderTo> to, CancellationToken ct = default);
    }
}
