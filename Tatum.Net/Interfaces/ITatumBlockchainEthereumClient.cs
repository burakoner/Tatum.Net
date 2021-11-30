using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.Enums;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumBlockchainEthereumClient
    {
        WebCallResult<BlockchainResponse> Broadcast(string txData, string signatureId, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> BroadcastAsync(string txData, string signatureId, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> ERC20_DeploySmartContract(string name, string symbol, string supply, int digits, string address, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> ERC20_DeploySmartContractAsync(string name, string symbol, string supply, int digits, string address, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        WebCallResult<decimal> ERC20_GetBalance(string address, string contractAddress, int decimals = 0, CancellationToken ct = default);
        Task<WebCallResult<decimal>> ERC20_GetBalanceAsync(string address, string contractAddress, int decimals = 0, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> ERC20_Transfer(string contractAddress, string to, string amount, int digits, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> ERC20_TransferAsync(string contractAddress, string to, string amount, int digits, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> ERC721_Burn(string contractAddress, string tokenId, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> ERC721_BurnAsync(string contractAddress, string tokenId, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> ERC721_DeploySmartContract(string name, string symbol, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> ERC721_DeploySmartContractAsync(string name, string symbol, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        WebCallResult<decimal> ERC721_GetBalance(string address, string contractAddress, int decimals = 0, CancellationToken ct = default);
        WebCallResult<decimal> ERC721_GetToken(string address, int index, string contractAddress, int decimals = 0, CancellationToken ct = default);
        WebCallResult<EthereumData> ERC721_GetTokenMetadata(string token, string contractAddress, decimal divider = 1, CancellationToken ct = default);
        Task<WebCallResult<EthereumData>> ERC721_GetTokenMetadataAsync(string token, string contractAddress, decimal divider = 1, CancellationToken ct = default);
        WebCallResult<EthereumData> ERC721_GetTokenOwner(string token, string contractAddress, decimal divider = 1, CancellationToken ct = default);
        Task<WebCallResult<EthereumData>> ERC721_GetTokenOwnerAsync(string token, string contractAddress, decimal divider = 1, CancellationToken ct = default);
        Task<WebCallResult<decimal>> ERC721_GetTokenAsync(string address, int index, string contractAddress, int decimals = 0, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> ERC721_Mint(string contractAddress, string tokenId, string to, string url, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> ERC721_MintMultiple(string contractAddress, IEnumerable<string> tokenId, IEnumerable<string> to, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> ERC721_MintMultipleAsync(string contractAddress, IEnumerable<string> tokenId, IEnumerable<string> to, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> ERC721_MintAsync(string contractAddress, string tokenId, string to, string url, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> ERC721_Transfer(string contractAddress, string tokenId, string to, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> ERC721_TransferAsync(string contractAddress, string tokenId, string to, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        WebCallResult<decimal> ETH_GetBalance(string address, CancellationToken ct = default);
        Task<WebCallResult<decimal>> ETH_GetBalanceAsync(string address, CancellationToken ct = default);
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
        WebCallResult<EthereumBlock> GetBlock(string hash_height, CancellationToken ct = default);
        Task<WebCallResult<EthereumBlock>> GetBlockAsync(string hash_height, CancellationToken ct = default);
        WebCallResult<long> GetCurrentBlockNumber(CancellationToken ct = default);
        Task<WebCallResult<long>> GetCurrentBlockNumberAsync(CancellationToken ct = default);
        Task<WebCallResult<decimal>> GetERC721BalanceAsync(string address, string contractAddress, int decimals = 0, CancellationToken ct = default);
        WebCallResult<long> GetOutgoingTransactionsCount(string address, CancellationToken ct = default);
        Task<WebCallResult<long>> GetOutgoingTransactionsCountAsync(string address, CancellationToken ct = default);
        WebCallResult<EthereumTransaction> GetTransactionByHash(string hash, CancellationToken ct = default);
        Task<WebCallResult<EthereumTransaction>> GetTransactionByHashAsync(string hash, CancellationToken ct = default);
        WebCallResult<IEnumerable<EthereumTransaction>> GetTransactionsByAddress(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<EthereumTransaction>>> GetTransactionsByAddressAsync(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> InvokeSmartContractMethod(string contractAddress, string methodName, object methodABI, IEnumerable<object> method_params, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> InvokeSmartContractMethodAsync(string contractAddress, string methodName, object methodABI, IEnumerable<object> method_params, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Send(EthereumPredefinedCurrency currency, string amount, string to, string data = null, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> SendAsync(EthereumPredefinedCurrency currency, string amount, string to, string data = null, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        WebCallResult<EthereumDriver> Web3HttpDriver(CancellationToken ct = default);
        Task<WebCallResult<EthereumDriver>> Web3HttpDriverAsync(CancellationToken ct = default);
    }
}
