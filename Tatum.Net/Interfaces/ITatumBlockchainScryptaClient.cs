using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumBlockchainScryptaClient
    {
        WebCallResult<BlockchainResponse> Scrypta_Broadcast(string txData, string signatureId, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Scrypta_Broadcast_Async(string txData, string signatureId, CancellationToken ct = default);
        WebCallResult<TatumAddress> Scrypta_GenerateDepositAddress(string xpub, int index, CancellationToken ct = default);
        Task<WebCallResult<TatumAddress>> Scrypta_GenerateDepositAddress_Async(string xpub, int index, CancellationToken ct = default);
        WebCallResult<TatumKey> Scrypta_GeneratePrivateKey(IEnumerable<string> mnemonics, int index, CancellationToken ct = default);
        WebCallResult<TatumKey> Scrypta_GeneratePrivateKey(string mnemonics, int index, CancellationToken ct = default);
        Task<WebCallResult<TatumKey>> Scrypta_GeneratePrivateKey_Async(IEnumerable<string> mnemonics, int index, CancellationToken ct = default);
        Task<WebCallResult<TatumKey>> Scrypta_GeneratePrivateKey_Async(string mnemonics, int index, CancellationToken ct = default);
        WebCallResult<BlockchainWallet> Scrypta_GenerateWallet(IEnumerable<string> mnemonics = null, CancellationToken ct = default);
        WebCallResult<BlockchainWallet> Scrypta_GenerateWallet(string mnemonics, CancellationToken ct = default);
        Task<WebCallResult<BlockchainWallet>> Scrypta_GenerateWallet_Async(IEnumerable<string> mnemonics = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainWallet>> Scrypta_GenerateWallet_Async(string mnemonics, CancellationToken ct = default);
        WebCallResult<ScryptaBlock> Scrypta_GetBlock(string hash_height, CancellationToken ct = default);
        WebCallResult<ScryptaChainInfo> Scrypta_GetBlockchainInformation(CancellationToken ct = default);
        Task<WebCallResult<ScryptaChainInfo>> Scrypta_GetBlockchainInformation_Async(CancellationToken ct = default);
        WebCallResult<string> Scrypta_GetBlockHash(long block_id, CancellationToken ct = default);
        Task<WebCallResult<string>> Scrypta_GetBlockHash_Async(long block_id, CancellationToken ct = default);
        Task<WebCallResult<ScryptaBlock>> Scrypta_GetBlock_Async(string hash_height, CancellationToken ct = default);
        WebCallResult<IEnumerable<ScryptaUTXO>> Scrypta_GetSpendableUTXO(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<ScryptaUTXO>>> Scrypta_GetSpendableUTXO_Async(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        WebCallResult<ScryptaTransaction> Scrypta_GetTransactionByHash(string hash, CancellationToken ct = default);
        Task<WebCallResult<ScryptaTransaction>> Scrypta_GetTransactionByHash_Async(string hash, CancellationToken ct = default);
        WebCallResult<IEnumerable<ScryptaTransaction>> Scrypta_GetTransactionsByAddress(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<ScryptaTransaction>>> Scrypta_GetTransactionsByAddress_Async(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        WebCallResult<ScryptaUTXO> Scrypta_GetTransactionUTXO(string txhash, long index, CancellationToken ct = default);
        Task<WebCallResult<ScryptaUTXO>> Scrypta_GetTransactionUTXO_Async(string txhash, long index, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Scrypta_Send(IEnumerable<ScryptaSendOrderFromAddress> fromAddress, IEnumerable<ScryptaSendOrderFromUTXO> fromUTXO, IEnumerable<ScryptaSendOrderTo> to, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Scrypta_Send_Async(IEnumerable<ScryptaSendOrderFromAddress> fromAddress, IEnumerable<ScryptaSendOrderFromUTXO> fromUTXO, IEnumerable<ScryptaSendOrderTo> to, CancellationToken ct = default);

    }
}
