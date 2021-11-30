using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumBlockchainBitcoinClient
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
        WebCallResult<BitcoinBalance> GetBalance(string address, CancellationToken ct = default);
        Task<WebCallResult<BitcoinBalance>> GetBalanceAsync(string address, CancellationToken ct = default);
        WebCallResult<BitcoinBlock> GetBlock(string hash_height, CancellationToken ct = default);
        WebCallResult<BitcoinChainInfo> GetBlockchainInformation(CancellationToken ct = default);
        Task<WebCallResult<BitcoinChainInfo>> GetBlockchainInformationAsync(CancellationToken ct = default);
        WebCallResult<TatumHash> GetBlockHash(long block_id, CancellationToken ct = default);
        Task<WebCallResult<TatumHash>> GetBlockHashAsync(long block_id, CancellationToken ct = default);
        Task<WebCallResult<BitcoinBlock>> GetBlockAsync(string hash_height, CancellationToken ct = default);
        WebCallResult<BitcoinTransaction> GetTransactionByHash(string hash, CancellationToken ct = default);
        Task<WebCallResult<BitcoinTransaction>> GetTransactionByHashAsync(string hash, CancellationToken ct = default);
        WebCallResult<IEnumerable<BitcoinTransaction>> GetTransactionsByAddress(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<BitcoinTransaction>>> GetTransactionsByAddressAsync(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        WebCallResult<BitcoinUTXO> GetTransactionUTXO(string txhash, long index, CancellationToken ct = default);
        Task<WebCallResult<BitcoinUTXO>> GetTransactionUTXOAsync(string txhash, long index, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Send(IEnumerable<BitcoinSendOrderFromAddress> fromAddress, IEnumerable<BitcoinSendOrderFromUTXO> fromUTXO, IEnumerable<BitcoinSendOrderTo> to, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> SendAsync(IEnumerable<BitcoinSendOrderFromAddress> fromAddress, IEnumerable<BitcoinSendOrderFromUTXO> fromUTXO, IEnumerable<BitcoinSendOrderTo> to, CancellationToken ct = default);
    }
}
