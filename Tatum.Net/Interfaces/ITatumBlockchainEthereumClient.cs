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
        WebCallResult<BlockchainResponse> Ethereum_Broadcast(string txData, string signatureId, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Ethereum_Broadcast_Async(string txData, string signatureId, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Ethereum_ERC20_DeploySmartContract(string name, string symbol, string supply, int digits, string address, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Ethereum_ERC20_DeploySmartContract_Async(string name, string symbol, string supply, int digits, string address, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        WebCallResult<decimal> Ethereum_ERC20_GetBalance(string address, string contractAddress, int decimals = 0, CancellationToken ct = default);
        Task<WebCallResult<decimal>> Ethereum_ERC20_GetBalance_Async(string address, string contractAddress, int decimals = 0, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Ethereum_ERC20_Transfer(string contractAddress, string to, string amount, int digits, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Ethereum_ERC20_Transfer_Async(string contractAddress, string to, string amount, int digits, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Ethereum_ERC721_Burn(string contractAddress, string tokenId, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Ethereum_ERC721_Burn_Async(string contractAddress, string tokenId, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Ethereum_ERC721_DeploySmartContract(string name, string symbol, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Ethereum_ERC721_DeploySmartContract_Async(string name, string symbol, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        WebCallResult<decimal> Ethereum_ERC721_GetBalance(string address, string contractAddress, int decimals = 0, CancellationToken ct = default);
        WebCallResult<decimal> Ethereum_ERC721_GetToken(string address, int index, string contractAddress, int decimals = 0, CancellationToken ct = default);
        WebCallResult<EthereumData> Ethereum_ERC721_GetTokenMetadata(string token, string contractAddress, decimal divider = 1, CancellationToken ct = default);
        Task<WebCallResult<EthereumData>> Ethereum_ERC721_GetTokenMetadata_Async(string token, string contractAddress, decimal divider = 1, CancellationToken ct = default);
        WebCallResult<EthereumData> Ethereum_ERC721_GetTokenOwner(string token, string contractAddress, decimal divider = 1, CancellationToken ct = default);
        Task<WebCallResult<EthereumData>> Ethereum_ERC721_GetTokenOwner_Async(string token, string contractAddress, decimal divider = 1, CancellationToken ct = default);
        Task<WebCallResult<decimal>> Ethereum_ERC721_GetToken_Async(string address, int index, string contractAddress, int decimals = 0, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Ethereum_ERC721_Mint(string contractAddress, string tokenId, string to, string url, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Ethereum_ERC721_MintMultiple(string contractAddress, IEnumerable<string> tokenId, IEnumerable<string> to, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Ethereum_ERC721_MintMultiple_Async(string contractAddress, IEnumerable<string> tokenId, IEnumerable<string> to, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Ethereum_ERC721_Mint_Async(string contractAddress, string tokenId, string to, string url, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Ethereum_ERC721_Transfer(string contractAddress, string tokenId, string to, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Ethereum_ERC721_Transfer_Async(string contractAddress, string tokenId, string to, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        WebCallResult<decimal> Ethereum_ETH_GetBalance(string address, CancellationToken ct = default);
        Task<WebCallResult<decimal>> Ethereum_ETH_GetBalance_Async(string address, CancellationToken ct = default);
        WebCallResult<TatumAddress> Ethereum_GenerateDepositAddress(string xpub, int index, CancellationToken ct = default);
        Task<WebCallResult<TatumAddress>> Ethereum_GenerateDepositAddress_Async(string xpub, int index, CancellationToken ct = default);
        WebCallResult<TatumKey> Ethereum_GeneratePrivateKey(IEnumerable<string> mnemonics, int index, CancellationToken ct = default);
        WebCallResult<TatumKey> Ethereum_GeneratePrivateKey(string mnemonics, int index, CancellationToken ct = default);
        Task<WebCallResult<TatumKey>> Ethereum_GeneratePrivateKey_Async(IEnumerable<string> mnemonics, int index, CancellationToken ct = default);
        Task<WebCallResult<TatumKey>> Ethereum_GeneratePrivateKey_Async(string mnemonics, int index, CancellationToken ct = default);
        WebCallResult<BlockchainWallet> Ethereum_GenerateWallet(IEnumerable<string> mnemonics = null, CancellationToken ct = default);
        WebCallResult<BlockchainWallet> Ethereum_GenerateWallet(string mnemonics, CancellationToken ct = default);
        Task<WebCallResult<BlockchainWallet>> Ethereum_GenerateWallet_Async(IEnumerable<string> mnemonics = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainWallet>> Ethereum_GenerateWallet_Async(string mnemonics, CancellationToken ct = default);
        WebCallResult<EthereumBlock> Ethereum_GetBlock(string hash_height, CancellationToken ct = default);
        Task<WebCallResult<EthereumBlock>> Ethereum_GetBlock_Async(string hash_height, CancellationToken ct = default);
        WebCallResult<long> Ethereum_GetCurrentBlockNumber(CancellationToken ct = default);
        Task<WebCallResult<long>> Ethereum_GetCurrentBlockNumber_Async(CancellationToken ct = default);
        Task<WebCallResult<decimal>> Ethereum_GetERC721Balance_Async(string address, string contractAddress, int decimals = 0, CancellationToken ct = default);
        WebCallResult<long> Ethereum_GetOutgoingTransactionsCount(string address, CancellationToken ct = default);
        Task<WebCallResult<long>> Ethereum_GetOutgoingTransactionsCount_Async(string address, CancellationToken ct = default);
        WebCallResult<EthereumTransaction> Ethereum_GetTransactionByHash(string hash, CancellationToken ct = default);
        Task<WebCallResult<EthereumTransaction>> Ethereum_GetTransactionByHash_Async(string hash, CancellationToken ct = default);
        WebCallResult<IEnumerable<EthereumTransaction>> Ethereum_GetTransactionsByAddress(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<EthereumTransaction>>> Ethereum_GetTransactionsByAddress_Async(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Ethereum_InvokeSmartContractMethod(string contractAddress, string methodName, object methodABI, IEnumerable<object> method_params, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Ethereum_InvokeSmartContractMethod_Async(string contractAddress, string methodName, object methodABI, IEnumerable<object> method_params, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        WebCallResult<BlockchainResponse> Ethereum_Send(EthereumPredefinedCurrency currency, string amount, string to, string data = null, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        Task<WebCallResult<BlockchainResponse>> Ethereum_Send_Async(EthereumPredefinedCurrency currency, string amount, string to, string data = null, string signatureId = null, string fromPrivateKey = null, long? nonce = null, EthereumFee fee = null, CancellationToken ct = default);
        WebCallResult<EthereumDriver> Ethereum_Web3HttpDriver(CancellationToken ct = default);
        Task<WebCallResult<EthereumDriver>> Ethereum_Web3HttpDriver_Async(CancellationToken ct = default);
    }
}
