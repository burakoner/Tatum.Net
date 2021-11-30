using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumBlockchainScryptaClient
    {
        WebCallResult<BlockchainResponse> Broadcast(string txData, string signatureId, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> BroadcastAsync(string txData, string signatureId, CancellationToken ct = default);
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
        WebCallResult<ScryptaBlock> GetBlock(string hash_height, CancellationToken ct = default);
        WebCallResult<ScryptaChainInfo> GetBlockchainInformation(CancellationToken ct = default);
        Task<WebCallResult<ScryptaChainInfo>> GetBlockchainInformationAsync(CancellationToken ct = default);
        WebCallResult<string> GetBlockHash(long block_id, CancellationToken ct = default);
        Task<WebCallResult<string>> GetBlockHashAsync(long block_id, CancellationToken ct = default);
        Task<WebCallResult<ScryptaBlock>> GetBlockAsync(string hash_height, CancellationToken ct = default);
        WebCallResult<IEnumerable<ScryptaUTXO>> GetSpendableUTXO(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<ScryptaUTXO>>> GetSpendableUTXOAsync(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        WebCallResult<ScryptaTransaction> GetTransactionByHash(string hash, CancellationToken ct = default);
        Task<WebCallResult<ScryptaTransaction>> GetTransactionByHashAsync(string hash, CancellationToken ct = default);
        WebCallResult<IEnumerable<ScryptaTransaction>> GetTransactionsByAddress(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<ScryptaTransaction>>> GetTransactionsByAddressAsync(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        WebCallResult<ScryptaUTXO> GetTransactionUTXO(string txhash, long index, CancellationToken ct = default);
        Task<WebCallResult<ScryptaUTXO>> GetTransactionUTXOAsync(string txhash, long index, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Send(IEnumerable<ScryptaSendOrderFromAddress> fromAddress, IEnumerable<ScryptaSendOrderFromUTXO> fromUTXO, IEnumerable<ScryptaSendOrderTo> to, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> SendAsync(IEnumerable<ScryptaSendOrderFromAddress> fromAddress, IEnumerable<ScryptaSendOrderFromUTXO> fromUTXO, IEnumerable<ScryptaSendOrderTo> to, CancellationToken ct = default);

    }
}
