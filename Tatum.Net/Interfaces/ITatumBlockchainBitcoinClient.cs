using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumBlockchainBitcoinClient
    {
        WebCallResult<BlockchainResponse> Bitcoin_Broadcast(string txData, string signatureId, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Bitcoin_Broadcast_Async(string txData, string signatureId, CancellationToken ct = default);
        WebCallResult<TatumAddress> Bitcoin_GenerateDepositAddress(string xpub, int index, CancellationToken ct = default);
        Task<WebCallResult<TatumAddress>> Bitcoin_GenerateDepositAddress_Async(string xpub, int index, CancellationToken ct = default);
        WebCallResult<TatumKey> Bitcoin_GeneratePrivateKey(IEnumerable<string> mnemonics, int index, CancellationToken ct = default);
        WebCallResult<TatumKey> Bitcoin_GeneratePrivateKey(string mnemonics, int index, CancellationToken ct = default);
        Task<WebCallResult<TatumKey>> Bitcoin_GeneratePrivateKey_Async(IEnumerable<string> mnemonics, int index, CancellationToken ct = default);
        Task<WebCallResult<TatumKey>> Bitcoin_GeneratePrivateKey_Async(string mnemonics, int index, CancellationToken ct = default);
        WebCallResult<BlockchainWallet> Bitcoin_GenerateWallet(IEnumerable<string> mnemonics = null, CancellationToken ct = default);
        WebCallResult<BlockchainWallet> Bitcoin_GenerateWallet(string mnemonics, CancellationToken ct = default);
        Task<WebCallResult<BlockchainWallet>> Bitcoin_GenerateWallet_Async(IEnumerable<string> mnemonics = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainWallet>> Bitcoin_GenerateWallet_Async(string mnemonics, CancellationToken ct = default);
        WebCallResult<BitcoinBalance> Bitcoin_GetBalance(string address, CancellationToken ct = default);
        Task<WebCallResult<BitcoinBalance>> Bitcoin_GetBalance_Async(string address, CancellationToken ct = default);
        WebCallResult<BitcoinBlock> Bitcoin_GetBlock(string hash_height, CancellationToken ct = default);
        WebCallResult<BitcoinChainInfo> Bitcoin_GetBlockchainInformation(CancellationToken ct = default);
        Task<WebCallResult<BitcoinChainInfo>> Bitcoin_GetBlockchainInformation_Async(CancellationToken ct = default);
        WebCallResult<TatumHash> Bitcoin_GetBlockHash(long block_id, CancellationToken ct = default);
        Task<WebCallResult<TatumHash>> Bitcoin_GetBlockHash_Async(long block_id, CancellationToken ct = default);
        Task<WebCallResult<BitcoinBlock>> Bitcoin_GetBlock_Async(string hash_height, CancellationToken ct = default);
        WebCallResult<BitcoinTransaction> Bitcoin_GetTransactionByHash(string hash, CancellationToken ct = default);
        Task<WebCallResult<BitcoinTransaction>> Bitcoin_GetTransactionByHash_Async(string hash, CancellationToken ct = default);
        WebCallResult<IEnumerable<BitcoinTransaction>> Bitcoin_GetTransactionsByAddress(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<BitcoinTransaction>>> Bitcoin_GetTransactionsByAddress_Async(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        WebCallResult<BitcoinUTXO> Bitcoin_GetTransactionUTXO(string txhash, long index, CancellationToken ct = default);
        Task<WebCallResult<BitcoinUTXO>> Bitcoin_GetTransactionUTXO_Async(string txhash, long index, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Bitcoin_Send(IEnumerable<BitcoinSendOrderFromAddress> fromAddress, IEnumerable<BitcoinSendOrderFromUTXO> fromUTXO, IEnumerable<BitcoinSendOrderTo> to, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Bitcoin_Send_Async(IEnumerable<BitcoinSendOrderFromAddress> fromAddress, IEnumerable<BitcoinSendOrderFromUTXO> fromUTXO, IEnumerable<BitcoinSendOrderTo> to, CancellationToken ct = default);
    }
}
