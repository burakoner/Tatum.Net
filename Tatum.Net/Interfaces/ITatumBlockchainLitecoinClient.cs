using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumBlockchainLitecoinClient
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
        WebCallResult<LitecoinBalance> GetBalance(string address, CancellationToken ct = default);
        Task<WebCallResult<LitecoinBalance>> GetBalanceAsync(string address, CancellationToken ct = default);
        WebCallResult<LitecoinBlock> GetBlock(string hash_height, CancellationToken ct = default);
        WebCallResult<LitecoinChainInfo> GetBlockchainInformation(CancellationToken ct = default);
        Task<WebCallResult<LitecoinChainInfo>> GetBlockchainInformationAsync(CancellationToken ct = default);
        WebCallResult<TatumHash> GetBlockHash(long block_id, CancellationToken ct = default);
        Task<WebCallResult<TatumHash>> GetBlockHashAsync(long block_id, CancellationToken ct = default);
        Task<WebCallResult<LitecoinBlock>> GetBlockAsync(string hash_height, CancellationToken ct = default);
        WebCallResult<LitecoinTransaction> GetTransactionByHash(string hash, CancellationToken ct = default);
        Task<WebCallResult<LitecoinTransaction>> GetTransactionByHashAsync(string hash, CancellationToken ct = default);
        WebCallResult<IEnumerable<LitecoinTransaction>> GetTransactionsByAddress(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<LitecoinTransaction>>> GetTransactionsByAddressAsync(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        WebCallResult<LitecoinUTXO> GetTransactionUTXO(string txhash, long index, CancellationToken ct = default);
        Task<WebCallResult<LitecoinUTXO>> GetTransactionUTXOAsync(string txhash, long index, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Send(IEnumerable<LitecoinSendOrderFromAddress> fromAddress, IEnumerable<LitecoinSendOrderFromUTXO> fromUTXO, IEnumerable<LitecoinSendOrderTo> to, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> SendAsync(IEnumerable<LitecoinSendOrderFromAddress> fromAddress, IEnumerable<LitecoinSendOrderFromUTXO> fromUTXO, IEnumerable<LitecoinSendOrderTo> to, CancellationToken ct = default);

    }
}
