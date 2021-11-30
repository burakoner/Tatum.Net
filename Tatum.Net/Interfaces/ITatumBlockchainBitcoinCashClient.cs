using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumBlockchainBitcoinCashClient
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
        WebCallResult<BitcoinCashBlock> GetBlock(string hash_height, CancellationToken ct = default);
        WebCallResult<BitcoinCashChainInfo> GetBlockchainInformation(CancellationToken ct = default);
        Task<WebCallResult<BitcoinCashChainInfo>> GetBlockchainInformationAsync(CancellationToken ct = default);
        WebCallResult<TatumHash> GetBlockHash(long block_id, CancellationToken ct = default);
        Task<WebCallResult<TatumHash>> GetBlockHashAsync(long block_id, CancellationToken ct = default);
        Task<WebCallResult<BitcoinCashBlock>> GetBlockAsync(string hash_height, CancellationToken ct = default);
        WebCallResult<BitcoinCashTransaction> GetTransactionByHash(string hash, CancellationToken ct = default);
        Task<WebCallResult<BitcoinCashTransaction>> GetTransactionByHashAsync(string hash, CancellationToken ct = default);
        WebCallResult<IEnumerable<BitcoinCashTransaction>> GetTransactionsByAddress(string address, int skip = 0, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<BitcoinCashTransaction>>> GetTransactionsByAddressAsync(string address, int skip = 0, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Send(IEnumerable<BitcoinCashSendOrderFromUTXO> fromUTXO, IEnumerable<BitcoinCashSendOrderTo> to, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> SendAsync(IEnumerable<BitcoinCashSendOrderFromUTXO> fromUTXO, IEnumerable<BitcoinCashSendOrderTo> to, CancellationToken ct = default);
    }
}
