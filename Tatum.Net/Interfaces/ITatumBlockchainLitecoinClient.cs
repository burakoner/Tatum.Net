using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumBlockchainLitecoinClient
    {
        WebCallResult<BlockchainResponse> Litecoin_Broadcast(string txData, string signatureId, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Litecoin_Broadcast_Async(string txData, string signatureId, CancellationToken ct = default);
        WebCallResult<TatumAddress> Litecoin_GenerateDepositAddress(string xpub, int index, CancellationToken ct = default);
        Task<WebCallResult<TatumAddress>> Litecoin_GenerateDepositAddress_Async(string xpub, int index, CancellationToken ct = default);
        WebCallResult<TatumKey> Litecoin_GeneratePrivateKey(IEnumerable<string> mnemonics, int index, CancellationToken ct = default);
        WebCallResult<TatumKey> Litecoin_GeneratePrivateKey(string mnemonics, int index, CancellationToken ct = default);
        Task<WebCallResult<TatumKey>> Litecoin_GeneratePrivateKey_Async(IEnumerable<string> mnemonics, int index, CancellationToken ct = default);
        Task<WebCallResult<TatumKey>> Litecoin_GeneratePrivateKey_Async(string mnemonics, int index, CancellationToken ct = default);
        WebCallResult<BlockchainWallet> Litecoin_GenerateWallet(IEnumerable<string> mnemonics = null, CancellationToken ct = default);
        WebCallResult<BlockchainWallet> Litecoin_GenerateWallet(string mnemonics, CancellationToken ct = default);
        Task<WebCallResult<BlockchainWallet>> Litecoin_GenerateWallet_Async(IEnumerable<string> mnemonics = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainWallet>> Litecoin_GenerateWallet_Async(string mnemonics, CancellationToken ct = default);
        WebCallResult<LitecoinBalance> Litecoin_GetBalance(string address, CancellationToken ct = default);
        Task<WebCallResult<LitecoinBalance>> Litecoin_GetBalance_Async(string address, CancellationToken ct = default);
        WebCallResult<LitecoinBlock> Litecoin_GetBlock(string hash_height, CancellationToken ct = default);
        WebCallResult<LitecoinChainInfo> Litecoin_GetBlockchainInformation(CancellationToken ct = default);
        Task<WebCallResult<LitecoinChainInfo>> Litecoin_GetBlockchainInformation_Async(CancellationToken ct = default);
        WebCallResult<TatumHash> Litecoin_GetBlockHash(long block_id, CancellationToken ct = default);
        Task<WebCallResult<TatumHash>> Litecoin_GetBlockHash_Async(long block_id, CancellationToken ct = default);
        Task<WebCallResult<LitecoinBlock>> Litecoin_GetBlock_Async(string hash_height, CancellationToken ct = default);
        WebCallResult<LitecoinTransaction> Litecoin_GetTransactionByHash(string hash, CancellationToken ct = default);
        Task<WebCallResult<LitecoinTransaction>> Litecoin_GetTransactionByHash_Async(string hash, CancellationToken ct = default);
        WebCallResult<IEnumerable<LitecoinTransaction>> Litecoin_GetTransactionsByAddress(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<LitecoinTransaction>>> Litecoin_GetTransactionsByAddress_Async(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        WebCallResult<LitecoinUTXO> Litecoin_GetTransactionUTXO(string txhash, long index, CancellationToken ct = default);
        Task<WebCallResult<LitecoinUTXO>> Litecoin_GetTransactionUTXO_Async(string txhash, long index, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Litecoin_Send(IEnumerable<LitecoinSendOrderFromAddress> fromAddress, IEnumerable<LitecoinSendOrderFromUTXO> fromUTXO, IEnumerable<LitecoinSendOrderTo> to, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Litecoin_Send_Async(IEnumerable<LitecoinSendOrderFromAddress> fromAddress, IEnumerable<LitecoinSendOrderFromUTXO> fromUTXO, IEnumerable<LitecoinSendOrderTo> to, CancellationToken ct = default);

    }
}
