using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.Converters;
using Tatum.Net.Enums;
using Tatum.Net.Helpers;
using Tatum.Net.Interfaces;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Clients
{
    public class EthereumClient : ITatumBlockchainEthereumClient
    {
        public TatumClient Tatum { get; protected set; }
        protected AuthenticationProvider AuthProvider { get; set; }

        #region API Endpoints

        #region Blockchain - Ethereum
        protected const string Endpoints_Web3HttpDriver = "ethereum/web3/{0}";
        protected const string Endpoints_CurrentBlockNumber = "ethereum/block/current";
        protected const string Endpoints_GetBlockByHash = "ethereum/block/{0}";
        protected const string Endpoints_GetTransactionByHash = "ethereum/transaction/{0}";
        protected const string Endpoints_GetETHBalance = "ethereum/account/balance/{0}";
        protected const string Endpoints_GetOutgoingTransactionsCount = "ethereum/transaction/count/{0}";
        protected const string Endpoints_GetTransactionsByAddress = "ethereum/account/transaction/{0}";
        protected const string Endpoints_Send = "ethereum/transaction";
        protected const string Endpoints_Broadcast = "ethereum/broadcast";
        protected const string Endpoints_SmartContract = "ethereum/smartcontract";
        protected const string Endpoints_ERC20Balance = "ethereum/account/balance/erc20/{0}";
        protected const string Endpoints_ERC20DeploySmartContract = "ethereum/erc20/deploy";
        protected const string Endpoints_ERC20Transfer = "ethereum/erc20/transaction";
        protected const string Endpoints_ERC721Balance = "ethereum/erc721/balance/{0}/{1}";
        protected const string Endpoints_ERC721DeploySmartContract = "ethereum/erc721/deploy";
        protected const string Endpoints_ERC721Mint = "ethereum/erc721/mint";
        protected const string Endpoints_ERC721MintMultiple = "ethereum/erc721/mint/batch";
        protected const string Endpoints_ERC721Transfer = "ethereum/erc721/transaction";
        protected const string Endpoints_ERC721Burn = "ethereum/erc721/burn";
        protected const string Endpoints_ERC721Token = "ethereum/erc721/token/{0}/{1}/{2}";
        protected const string Endpoints_ERC721TokenMetadata = "ethereum/erc721/metadata/{0}/{1}";
        protected const string Endpoints_ERC721TokenOwner = "ethereum/erc721/owner/{0}/{1}";
        #endregion

        #endregion

        public EthereumClient(TatumClient tatumClient, AuthenticationProvider authProvider)
        {
            Tatum = tatumClient;
            AuthProvider = authProvider;
        }


        #region Blockchain / Ethereum
        /// <summary>
        /// <b>Title:</b> Generate Ethereum wallet<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. 
        /// It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. 
        /// Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// Tatum follows BIP44 specification and generates for Ethereum wallet with derivation path m'/44'/60'/0'/0. 
        /// More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. 
        /// Generate BIP44 compatible Ethereum wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainWallet> GenerateWallet(string mnemonics, CancellationToken ct = default) => Tatum.Blockchain_GenerateWalletAsync(BlockchainType.Ethereum, new List<string> { mnemonics }, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Ethereum wallet<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. 
        /// It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. 
        /// Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// Tatum follows BIP44 specification and generates for Ethereum wallet with derivation path m'/44'/60'/0'/0. 
        /// More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. 
        /// Generate BIP44 compatible Ethereum wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainWallet> GenerateWallet(IEnumerable<string> mnemonics = null, CancellationToken ct = default) => Tatum.Blockchain_GenerateWalletAsync(BlockchainType.Ethereum, mnemonics, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Ethereum wallet<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. 
        /// It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. 
        /// Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// Tatum follows BIP44 specification and generates for Ethereum wallet with derivation path m'/44'/60'/0'/0. 
        /// More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. 
        /// Generate BIP44 compatible Ethereum wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainWallet>> GenerateWalletAsync(string mnemonics, CancellationToken ct = default) => await Tatum.Blockchain_GenerateWalletAsync(BlockchainType.Ethereum, new List<string> { mnemonics }, ct);
        /// <summary>
        /// <b>Title:</b> Generate Ethereum wallet<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. 
        /// It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. 
        /// Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// Tatum follows BIP44 specification and generates for Ethereum wallet with derivation path m'/44'/60'/0'/0. 
        /// More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. 
        /// Generate BIP44 compatible Ethereum wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainWallet>> GenerateWalletAsync(IEnumerable<string> mnemonics = null, CancellationToken ct = default) => await Tatum.Blockchain_GenerateWalletAsync(BlockchainType.Ethereum, mnemonics, ct);

        /// <summary>
        /// <b>Title:</b> Generate Ethereum account address from Extended public key<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Generate Ethereum account deposit address from Extended public key. 
        /// Deposit address is generated for the specific index - each extended public key can generate up to 2^32 addresses starting from index 0 until 2^31.
        /// </summary>
        /// <param name="xpub">Extended public key of wallet.</param>
        /// <param name="index">Derivation index of desired address to be generated.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TatumAddress> GenerateDepositAddress(string xpub, int index, CancellationToken ct = default) => Tatum.Blockchain_GenerateDepositAddressAsync(BlockchainType.Ethereum, xpub, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Ethereum account address from Extended public key<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Generate Ethereum account deposit address from Extended public key. 
        /// Deposit address is generated for the specific index - each extended public key can generate up to 2^32 addresses starting from index 0 until 2^31.
        /// </summary>
        /// <param name="xpub">Extended public key of wallet.</param>
        /// <param name="index">Derivation index of desired address to be generated.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TatumAddress>> GenerateDepositAddressAsync(string xpub, int index, CancellationToken ct = default) => await Tatum.Blockchain_GenerateDepositAddressAsync(BlockchainType.Ethereum, xpub, index, ct);

        /// <summary>
        /// <b>Title:</b> Generate Ethereum private key<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Generate private key of address from mnemonic for given derivation path index. 
        /// Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to generate private key from.</param>
        /// <param name="index">Derivation index of private key to generate.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TatumKey> GeneratePrivateKey(string mnemonics, int index, CancellationToken ct = default) => Tatum.Blockchain_GeneratePrivateKeyAsync(BlockchainType.Ethereum, new List<string> { mnemonics }, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Ethereum private key<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Generate private key of address from mnemonic for given derivation path index. 
        /// Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to generate private key from.</param>
        /// <param name="index">Derivation index of private key to generate.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TatumKey> GeneratePrivateKey(IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => Tatum.Blockchain_GeneratePrivateKeyAsync(BlockchainType.Ethereum, mnemonics, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Ethereum private key<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Generate private key of address from mnemonic for given derivation path index. 
        /// Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to generate private key from.</param>
        /// <param name="index">Derivation index of private key to generate.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TatumKey>> GeneratePrivateKeyAsync(string mnemonics, int index, CancellationToken ct = default) => await Tatum.Blockchain_GeneratePrivateKeyAsync(BlockchainType.Ethereum, new List<string> { mnemonics }, index, default);
        /// <summary>
        /// <b>Title:</b> Generate Ethereum private key<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Generate private key of address from mnemonic for given derivation path index. 
        /// Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to generate private key from.</param>
        /// <param name="index">Derivation index of private key to generate.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TatumKey>> GeneratePrivateKeyAsync(IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => await Tatum.Blockchain_GeneratePrivateKeyAsync(BlockchainType.Ethereum, mnemonics, index, default);

        /// <summary>
        /// <b>Title:</b> Web3 HTTP driver<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Use this endpoint URL as a http-based web3 driver to connect directly to the Ethereum node provided by Tatum. 
        /// To learn more about Ethereum Web3, please visit Ethereum developer's guide.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<EthereumDriver> Web3HttpDriver(CancellationToken ct = default) => Web3HttpDriverAsync(ct).Result;
        /// <summary>
        /// <b>Title:</b> Web3 HTTP driver<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Use this endpoint URL as a http-based web3 driver to connect directly to the Ethereum node provided by Tatum. 
        /// To learn more about Ethereum Web3, please visit Ethereum developer's guide.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<EthereumDriver>> Web3HttpDriverAsync(CancellationToken ct = default)
        {
            if (AuthProvider == null || AuthProvider.Credentials == null || AuthProvider.Credentials.Key == null)
                throw new ArgumentException("No valid API credentials provided. Api Key is needed.");

            var parameters = new Dictionary<string, object> {
                { "jsonrpc", "2.0" },
                { "method", "web3_clientVersion" },
                { "params", new List<string>() },
                { "id", 2 },
            };

            var credits = 2;
            var url = Tatum.GetUrl(string.Format(Endpoints_Web3HttpDriver, AuthProvider.Credentials.Key.GetString()));
            return await Tatum.SendTatumRequest<EthereumDriver>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get current block number<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum current block number. This is the number of the latest block in the blockchain.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<long> GetCurrentBlockNumber(CancellationToken ct = default) => GetCurrentBlockNumberAsync(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get current block number<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum current block number. This is the number of the latest block in the blockchain.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<long>> GetCurrentBlockNumberAsync(CancellationToken ct = default)
        {
            var credits = 1;
            var url = Tatum.GetUrl(string.Format(Endpoints_CurrentBlockNumber));
            var result = await Tatum.SendTatumRequest<string>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<long>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<long>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.ToLong(), null);
        }

        /// <summary>
        /// <b>Title:</b> Get Ethereum block by hash<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum block by block hash or block number.
        /// </summary>
        /// <param name="hash_height">Block hash or block number</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<EthereumBlock> GetBlock(string hash_height, CancellationToken ct = default) => GetBlockAsync(hash_height, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Ethereum block by hash<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum block by block hash or block number.
        /// </summary>
        /// <param name="hash_height">Block hash or block number</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<EthereumBlock>> GetBlockAsync(string hash_height, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetBlockByHash, hash_height));
            return await Tatum.SendTatumRequest<EthereumBlock>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Ethereum Account balance<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum account balance in ETH. This method does not prints any balance of the ERC20 or ERC721 tokens on the account.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<decimal> ETH_GetBalance(string address, CancellationToken ct = default) => ETH_GetBalanceAsync(address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Ethereum Account balance<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum account balance in ETH. This method does not prints any balance of the ERC20 or ERC721 tokens on the account.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<decimal>> ETH_GetBalanceAsync(string address, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetETHBalance, address));
            var result = await Tatum.SendTatumRequest<EthereumBalance>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<decimal>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            var balance = result.Data.Balance.ToDecimal();
            return new WebCallResult<decimal>(result.ResponseStatusCode, result.ResponseHeaders, balance, null);
        }

        /// <summary>
        /// <b>Title:</b> Get Ethereum ERC20 Account balance<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum ERC20 Account balance in the smallest ERC20 unit. 
        /// It is possible to obtain the balance of any ERC20 token, either supported by Tatum natively or via the address of the ERC20 token.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="contractAddress">ERC20 contract address to get balance of. Either contractAddress, or currency must be present.</param>
        /// <param name="decimals">Decimal places for display balance</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<decimal> ERC20_GetBalance(string address, string contractAddress, int decimals = 0, CancellationToken ct = default) => ERC20_GetBalanceAsync(address, contractAddress, decimals, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Ethereum ERC20 Account balance<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum ERC20 Account balance in the smallest ERC20 unit. 
        /// It is possible to obtain the balance of any ERC20 token, either supported by Tatum natively or via the address of the ERC20 token.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="contractAddress">ERC20 contract address to get balance of. Either contractAddress, or currency must be present.</param>
        /// <param name="decimals">Decimal places for display balance</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<decimal>> ERC20_GetBalanceAsync(string address, string contractAddress, int decimals = 0, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "contractAddress", contractAddress },
            };
            var credits = 1;
            var url = Tatum.GetUrl(string.Format(Endpoints_ERC20Balance, address));
            var result = await Tatum.SendTatumRequest<EthereumBalance>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<decimal>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            var balance = new BigDecimal(BigInteger.Parse(result.Data.Balance), -decimals).ToDecimal();
            return new WebCallResult<decimal>(result.ResponseStatusCode, result.ResponseHeaders, balance, null);
        }

        /// <summary>
        /// <b>Title:</b> Get Ethereum Transaction<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum transaction by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<EthereumTransaction> GetTransactionByHash(string hash, CancellationToken ct = default) => GetTransactionByHashAsync(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Ethereum Transaction<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum transaction by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<EthereumTransaction>> GetTransactionByHashAsync(string hash, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetTransactionByHash, hash));
            return await Tatum.SendTatumRequest<EthereumTransaction>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get count of outgoing Ethereum transactions<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get a number of outgoing Ethereum transactions for the address. 
        /// When a transaction is sent, there can be multiple outgoing transactions, which are not yet processed by the blockchain. 
        /// To distinguish between them, there is a counter called a nonce, which represents the order of the transaction in the list of outgoing transactions.
        /// </summary>
        /// <param name="address">address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<long> GetOutgoingTransactionsCount(string address, CancellationToken ct = default) => GetOutgoingTransactionsCountAsync(address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get count of outgoing Ethereum transactions<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get a number of outgoing Ethereum transactions for the address. 
        /// When a transaction is sent, there can be multiple outgoing transactions, which are not yet processed by the blockchain. 
        /// To distinguish between them, there is a counter called a nonce, which represents the order of the transaction in the list of outgoing transactions.
        /// </summary>
        /// <param name="address">address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<long>> GetOutgoingTransactionsCountAsync(string address, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetOutgoingTransactionsCount, address));
            var result = await Tatum.SendTatumRequest<string>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<long>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<long>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.ToLong(), null);
        }

        /// <summary>
        /// <b>Title:</b> Get Ethereum transactions by address<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum transactions by address. This includes incoming and outgoing transactions for the address.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<EthereumTransaction>> GetTransactionsByAddress(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default) => GetTransactionsByAddressAsync(address, pageSize, offset, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Ethereum transactions by address<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum transactions by address. This includes incoming and outgoing transactions for the address.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<EthereumTransaction>>> GetTransactionsByAddressAsync(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);
            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };

            var credits = 1;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetTransactionsByAddress, address));
            return await Tatum.SendTatumRequest<IEnumerable<EthereumTransaction>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Send Ethereum / ERC20 from account to account<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Ethereum or Tatum supported ERC20 token from account to account.
        /// This operation needs the private key of the blockchain address.
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key.
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds.
        /// In this method, it is possible to enter privateKey or signatureId.
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds.
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request.
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="currency">Currency to transfer from Ethereum Blockchain Account.</param>
        /// <param name="amount">Amount to be sent in Ether.</param>
        /// <param name="to">Blockchain address to send assets</param>
        /// <param name="data">Additinal data, that can be passed to blockchain transaction as data property. Only for ETH transactions.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> Send(
            EthereumPredefinedCurrency currency,
            string amount,
            string to,
            string data = null,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
            => SendAsync(currency, amount, to, data, signatureId, fromPrivateKey, nonce, fee, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send Ethereum / ERC20 from account to account<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Ethereum or Tatum supported ERC20 token from account to account.
        /// This operation needs the private key of the blockchain address.
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key.
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds.
        /// In this method, it is possible to enter privateKey or signatureId.
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds.
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request.
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="currency">Currency to transfer from Ethereum Blockchain Account.</param>
        /// <param name="amount">Amount to be sent in Ether.</param>
        /// <param name="to">Blockchain address to send assets</param>
        /// <param name="data">Additinal data, that can be passed to blockchain transaction as data property. Only for ETH transactions.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> SendAsync(
            EthereumPredefinedCurrency currency,
            string amount,
            string to,
            string data = null,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "currency", JsonConvert.SerializeObject(currency, new EthereumPredefinedCurrencyConverter(false)) },
                { "amount", amount },
                { "to", to },
            };
            parameters.AddOptionalParameter("nonce", nonce);
            parameters.AddOptionalParameter("data", data);
            parameters.AddOptionalParameter("fee", fee);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("fromPrivateKey", fromPrivateKey);

            var credits = 2;
            var url = Tatum.GetUrl(string.Format(Endpoints_Send));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Invoke Smart Contract method<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Invoke any method on an existing Smart Contract. 
        /// It is possibleto call read or write method on the Smsrt Contract defined via contractAddress. 
        /// For read operations, data is returned, for write operations, transaction Id of the associated transaction is returned.
        /// This operation needs the private key of the blockchain address.
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key.
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. 
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="contractAddress">Address of ERC20 token</param>
        /// <param name="methodName">Name of the method to invoke on smart contract.</param>
        /// <param name="methodABI">ABI of the method to invoke.</param>
        /// <param name="method_params">Parameters of the method to be invoked.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> InvokeSmartContractMethod(
            string contractAddress,
            string methodName,
            object methodABI,
            IEnumerable<object> method_params,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
            => InvokeSmartContractMethodAsync(contractAddress, methodName, methodABI, method_params, signatureId, fromPrivateKey, nonce, fee, ct).Result;
        /// <summary>
        /// <b>Title:</b> Invoke Smart Contract method<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Invoke any method on an existing Smart Contract. 
        /// It is possibleto call read or write method on the Smsrt Contract defined via contractAddress. 
        /// For read operations, data is returned, for write operations, transaction Id of the associated transaction is returned.
        /// This operation needs the private key of the blockchain address.
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key.
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. 
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="contractAddress">Address of ERC20 token</param>
        /// <param name="methodName">Name of the method to invoke on smart contract.</param>
        /// <param name="methodABI">ABI of the method to invoke.</param>
        /// <param name="method_params">Parameters of the method to be invoked.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> InvokeSmartContractMethodAsync(
            string contractAddress,
            string methodName,
            object methodABI,
            IEnumerable<object> method_params,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "contractAddress", contractAddress },
                { "methodName", methodName },
                { "methodABI", methodABI },
                { "params", method_params },
            };
            parameters.AddOptionalParameter("fee", fee);
            parameters.AddOptionalParameter("nonce", nonce);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("fromPrivateKey", fromPrivateKey);

            var credits = 2;
            var url = Tatum.GetUrl(string.Format(Endpoints_SmartContract));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Deploy Ethereum ERC20 Smart Contract.<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Deploy Ethereum ERC20 Smart Contract.
        /// This method creates new ERC20(Fungible Tokens) Smart Contract on the blockchain.Smart contract is standardized and audited.
        /// It is possible to mint and burn tokens.It is possible to see the code of the deployed contract here. 
        /// Whole predefined supply of the tokens will be transferred to the chosen address.
        /// This operation needs the private key of the blockchain address.
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key.
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. 
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="name">Name of the ERC20 token</param>
        /// <param name="symbol">Symbol of the ERC20 token</param>
        /// <param name="supply">Max supply of ERC20 token.</param>
        /// <param name="digits">Number of decimal points</param>
        /// <param name="address">Address on Ethereum blockchain, where all created ERC20 tokens will be transferred.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of Ethereum account address, from which the fee for the deployment of ERC20 will be paid. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> ERC20_DeploySmartContract(
            string name,
            string symbol,
            string supply,
            int digits,
            string address,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
            => ERC20_DeploySmartContractAsync(name, symbol, supply, digits, address, signatureId, fromPrivateKey, nonce, fee, ct).Result;
        /// <summary>
        /// <b>Title:</b> Deploy Ethereum ERC20 Smart Contract.<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Deploy Ethereum ERC20 Smart Contract.
        /// This method creates new ERC20(Fungible Tokens) Smart Contract on the blockchain.Smart contract is standardized and audited.
        /// It is possible to mint and burn tokens.It is possible to see the code of the deployed contract here. 
        /// Whole predefined supply of the tokens will be transferred to the chosen address.
        /// This operation needs the private key of the blockchain address.
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key.
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. 
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="name">Name of the ERC20 token</param>
        /// <param name="symbol">Symbol of the ERC20 token</param>
        /// <param name="supply">Max supply of ERC20 token.</param>
        /// <param name="digits">Number of decimal points</param>
        /// <param name="address">Address on Ethereum blockchain, where all created ERC20 tokens will be transferred.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of Ethereum account address, from which the fee for the deployment of ERC20 will be paid. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> ERC20_DeploySmartContractAsync(
            string name,
            string symbol,
            string supply,
            int digits,
            string address,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "name", name },
                { "symbol", symbol },
                { "supply", supply },
                { "digits", digits },
                { "address", address },
            };
            parameters.AddOptionalParameter("fee", fee);
            parameters.AddOptionalParameter("nonce", nonce);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("fromPrivateKey", fromPrivateKey);

            var credits = 2;
            var url = Tatum.GetUrl(string.Format(Endpoints_ERC20DeploySmartContract));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Transfer Ethereum ERC20<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Transfer Ethereum ERC20 Smart Contract Tokens from account to account. 
        /// Transfer any ERC20 tokens from smart contract defined in contractAddress. 
        /// This method invokes ERC20 method transfer() to transfer tokens.
        /// This operation needs the private key of the blockchain address.
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key.
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. 
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="contractAddress">Address of ERC20 token</param>
        /// <param name="to">Blockchain address to send ERC20 token to</param>
        /// <param name="amount">Amount to be sent.</param>
        /// <param name="digits">Number of decimal points that ERC20 token has.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> ERC20_Transfer(
            string contractAddress,
            string to,
            string amount,
            int digits,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
            => ERC20_TransferAsync(contractAddress, to, amount, digits, signatureId, fromPrivateKey, nonce, fee, ct).Result;
        /// <summary>
        /// <b>Title:</b> Transfer Ethereum ERC20<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Transfer Ethereum ERC20 Smart Contract Tokens from account to account. 
        /// Transfer any ERC20 tokens from smart contract defined in contractAddress. 
        /// This method invokes ERC20 method transfer() to transfer tokens.
        /// This operation needs the private key of the blockchain address.
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key.
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. 
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="contractAddress">Address of ERC20 token</param>
        /// <param name="to">Blockchain address to send ERC20 token to</param>
        /// <param name="amount">Amount to be sent.</param>
        /// <param name="digits">Number of decimal points that ERC20 token has.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> ERC20_TransferAsync(
            string contractAddress,
            string to,
            string amount,
            int digits,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "contractAddress", contractAddress },
                { "to", to },
                { "amount", amount },
                { "digits", digits },
            };
            parameters.AddOptionalParameter("fee", fee);
            parameters.AddOptionalParameter("nonce", nonce);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("fromPrivateKey", fromPrivateKey);

            var credits = 2;
            var url = Tatum.GetUrl(string.Format(Endpoints_ERC20Transfer));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Deploy Ethereum ERC721 Smart Contract.<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Deploy Ethereum ERC721 Smart Contract. 
        /// This method creates new ERC721 Smart Contract (Non-Fungible Tokens) on the blockchain. 
        /// Smart contract is standardized and audited. It is possible to mint, burn and transfer tokens. 
        /// It is also possible to mint multiple tokens at once. It is possible to see the code of the deployed contract here.
        /// This operation needs the private key of the blockchain address.
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key.
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. 
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="name">Name of the ERC721 token</param>
        /// <param name="symbol">Symbol of the ERC721 token</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of Ethereum account address, from which gas for deployment of ERC721 will be paid. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> ERC721_DeploySmartContract(
            string name,
            string symbol,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
            => ERC721_DeploySmartContractAsync(name, symbol, signatureId, fromPrivateKey, nonce, fee, ct).Result;
        /// <summary>
        /// <b>Title:</b> Deploy Ethereum ERC721 Smart Contract.<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Deploy Ethereum ERC721 Smart Contract. 
        /// This method creates new ERC721 Smart Contract (Non-Fungible Tokens) on the blockchain. 
        /// Smart contract is standardized and audited. It is possible to mint, burn and transfer tokens. 
        /// It is also possible to mint multiple tokens at once. It is possible to see the code of the deployed contract here.
        /// This operation needs the private key of the blockchain address.
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key.
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. 
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="name">Name of the ERC721 token</param>
        /// <param name="symbol">Symbol of the ERC721 token</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of Ethereum account address, from which gas for deployment of ERC721 will be paid. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> ERC721_DeploySmartContractAsync(
            string name,
            string symbol,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "name", name },
                { "symbol", symbol },
            };
            parameters.AddOptionalParameter("fee", fee);
            parameters.AddOptionalParameter("nonce", nonce);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("fromPrivateKey", fromPrivateKey);

            var credits = 2;
            var url = Tatum.GetUrl(string.Format(Endpoints_ERC721DeploySmartContract));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Mint Ethereum ERC721<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create one Ethereum ERC721 Smart Contract Token and transfer it to destination account. 
        /// Create and transfer any ERC721 token from smart contract defined in contractAddress. 
        /// It is possible to add URL to the created token with a more detailed information about it.
        /// This operation needs the private key of the blockchain address.
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key.
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds.
        /// In this method, it is possible to enter privateKey or signatureId.
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds.
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request.
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="contractAddress">Address of ERC721 token</param>
        /// <param name="tokenId">ID of token to be created.</param>
        /// <param name="to">Blockchain address to send ERC721 token to</param>
        /// <param name="url">Metadata of the token, usually as URL.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> ERC721_Mint(
            string contractAddress,
            string tokenId,
            string to,
            string url,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
            => ERC721_MintAsync(contractAddress, tokenId, to, url, signatureId, fromPrivateKey, nonce, fee, ct).Result;
        /// <summary>
        /// <b>Title:</b> Mint Ethereum ERC721<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create one Ethereum ERC721 Smart Contract Token and transfer it to destination account. 
        /// Create and transfer any ERC721 token from smart contract defined in contractAddress. 
        /// It is possible to add URL to the created token with a more detailed information about it.
        /// This operation needs the private key of the blockchain address.
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key.
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds.
        /// In this method, it is possible to enter privateKey or signatureId.
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds.
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request.
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="contractAddress">Address of ERC721 token</param>
        /// <param name="tokenId">ID of token to be created.</param>
        /// <param name="to">Blockchain address to send ERC721 token to</param>
        /// <param name="url">Metadata of the token, usually as URL.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> ERC721_MintAsync(
            string contractAddress,
            string tokenId,
            string to,
            string url,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "contractAddress", contractAddress },
                { "tokenId", tokenId },
                { "to", to },
                { "url", url },
            };
            parameters.AddOptionalParameter("fee", fee);
            parameters.AddOptionalParameter("nonce", nonce);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("fromPrivateKey", fromPrivateKey);

            var credits = 2;
            var url_ = Tatum.GetUrl(string.Format(Endpoints_ERC721Mint));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url_, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Transfer Ethereum ERC721 Token<br />
        /// <b>Credits:</b> 2 credits per API call<br />
        /// <b>Description:</b>
        /// Transfer Ethereum ERC721 Smart Contract Tokens from account to account. 
        /// Transfer any ERC721 token from smart contract defined in contractAddress. 
        /// Only 1 specific token with specified tokenId can be transfered. This method invokes ERC721 method safeTransfer() to transfer the token.
        /// This operation needs the private key of the blockchain address. 
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key. 
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. 
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="contractAddress">Address of ERC721 token</param>
        /// <param name="tokenId">ID of token.</param>
        /// <param name="to">Blockchain address to send ERC721 token to</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> ERC721_Transfer(
            string contractAddress,
            string tokenId,
            string to,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
            => ERC721_TransferAsync(contractAddress, tokenId, to, signatureId, fromPrivateKey, nonce, fee, ct).Result;
        /// <summary>
        /// <b>Title:</b> Transfer Ethereum ERC721 Token<br />
        /// <b>Credits:</b> 2 credits per API call<br />
        /// <b>Description:</b>
        /// Transfer Ethereum ERC721 Smart Contract Tokens from account to account. 
        /// Transfer any ERC721 token from smart contract defined in contractAddress. 
        /// Only 1 specific token with specified tokenId can be transfered. This method invokes ERC721 method safeTransfer() to transfer the token.
        /// This operation needs the private key of the blockchain address. 
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key. 
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. 
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="contractAddress">Address of ERC721 token</param>
        /// <param name="tokenId">ID of token.</param>
        /// <param name="to">Blockchain address to send ERC721 token to</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> ERC721_TransferAsync(
            string contractAddress,
            string tokenId,
            string to,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "contractAddress", contractAddress },
                { "tokenId", tokenId },
                { "to", to },
            };
            parameters.AddOptionalParameter("fee", fee);
            parameters.AddOptionalParameter("nonce", nonce);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("fromPrivateKey", fromPrivateKey);

            var credits = 2;
            var url = Tatum.GetUrl(string.Format(Endpoints_ERC721Transfer));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Mint Ethereum ERC721 Multiple Tokens<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create multiple Ethereum ERC721 Smart Contract Tokens and transfer them to destination account. 
        /// Create and transfer any ERC721 tokens from smart contract defined in contractAddress.
        /// This operation needs the private key of the blockchain address. 
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key. 
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. 
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="contractAddress">Address of ERC721 token</param>
        /// <param name="tokenId">ID of token to be created.</param>
        /// <param name="to">Blockchain address to send ERC721 token to.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> ERC721_MintMultiple(
            string contractAddress,
            IEnumerable<string> tokenId,
            IEnumerable<string> to,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
            => ERC721_MintMultipleAsync(contractAddress, tokenId, to, signatureId, fromPrivateKey, nonce, fee, ct).Result;
        /// <summary>
        /// <b>Title:</b> Mint Ethereum ERC721 Multiple Tokens<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create multiple Ethereum ERC721 Smart Contract Tokens and transfer them to destination account. 
        /// Create and transfer any ERC721 tokens from smart contract defined in contractAddress.
        /// This operation needs the private key of the blockchain address. 
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key. 
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. 
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="contractAddress">Address of ERC721 token</param>
        /// <param name="tokenId">ID of token to be created.</param>
        /// <param name="to">Blockchain address to send ERC721 token to.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> ERC721_MintMultipleAsync(
            string contractAddress,
            IEnumerable<string> tokenId,
            IEnumerable<string> to,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "contractAddress", contractAddress },
                { "tokenId", tokenId },
                { "to", to },
            };
            parameters.AddOptionalParameter("fee", fee);
            parameters.AddOptionalParameter("nonce", nonce);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("fromPrivateKey", fromPrivateKey);

            var credits = 2;
            var url = Tatum.GetUrl(string.Format(Endpoints_ERC721MintMultiple));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Burn Ethereum ERC721<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Burn one Ethereum ERC721 Smart Contract Token. This method destroys any ERC721 token from smart contract defined in contractAddress.
        /// This operation needs the private key of the blockchain address. 
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key. 
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. 
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="contractAddress">Address of ERC721 token</param>
        /// <param name="tokenId">ID of token to be destroyed.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> ERC721_Burn(
            string contractAddress,
            string tokenId,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
            => ERC721_BurnAsync(contractAddress, tokenId, signatureId, fromPrivateKey, nonce, fee, ct).Result;
        /// <summary>
        /// <b>Title:</b> Burn Ethereum ERC721<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Burn one Ethereum ERC721 Smart Contract Token. This method destroys any ERC721 token from smart contract defined in contractAddress.
        /// This operation needs the private key of the blockchain address. 
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key. 
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. 
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="contractAddress">Address of ERC721 token</param>
        /// <param name="tokenId">ID of token to be destroyed.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> ERC721_BurnAsync(
            string contractAddress,
            string tokenId,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "contractAddress", contractAddress },
                { "tokenId", tokenId },
            };
            parameters.AddOptionalParameter("fee", fee);
            parameters.AddOptionalParameter("nonce", nonce);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("fromPrivateKey", fromPrivateKey);

            var credits = 2;
            var url = Tatum.GetUrl(string.Format(Endpoints_ERC721Burn));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Get Ethereum ERC721 Account balance<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum ERC721 Account balance. Returns number of tokens Account holds.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="contractAddress">ERC721 contract address</param>
        /// <param name="decimals">Decimal places for display balance</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<decimal> ERC721_GetBalance(string address, string contractAddress, int decimals = 0, CancellationToken ct = default) => GetERC721BalanceAsync(address, contractAddress, decimals, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Ethereum ERC721 Account balance<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum ERC721 Account balance. Returns number of tokens Account holds.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="contractAddress">ERC721 contract address</param>
        /// <param name="decimals">Decimal places for display balance</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<decimal>> GetERC721BalanceAsync(string address, string contractAddress, int decimals = 0, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Tatum.GetUrl(string.Format(Endpoints_ERC721Balance, address, contractAddress));
            var result = await Tatum.SendTatumRequest<EthereumData>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<decimal>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            var balance = new BigDecimal(BigInteger.Parse(result.Data.Data), -decimals).ToDecimal();
            return new WebCallResult<decimal>(result.ResponseStatusCode, result.ResponseHeaders, balance, null);
        }

        /// <summary>
        /// <b>Title:</b> Get Ethereum ERC721 Token<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum ERC721 token at given index of Account. Total number of tokens can be obtained from Get Balance operation.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="index">Token index</param>
        /// <param name="contractAddress">ERC721 contract address</param>
        /// <param name="decimals">Decimal places for display balance</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<decimal> ERC721_GetToken(string address, int index, string contractAddress, int decimals = 0, CancellationToken ct = default) => ERC721_GetTokenAsync(address, index, contractAddress, decimals, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Ethereum ERC721 Token<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum ERC721 token at given index of Account. Total number of tokens can be obtained from Get Balance operation.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="index">Token index</param>
        /// <param name="contractAddress">ERC721 contract address</param>
        /// <param name="decimals">Decimal places for display balance</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<decimal>> ERC721_GetTokenAsync(string address, int index, string contractAddress, int decimals = 0, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Tatum.GetUrl(string.Format(Endpoints_ERC721Token, address, index, contractAddress));
            var result = await Tatum.SendTatumRequest<EthereumData>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<decimal>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            var balance = new BigDecimal(BigInteger.Parse(result.Data.Data), -decimals).ToDecimal();
            return new WebCallResult<decimal>(result.ResponseStatusCode, result.ResponseHeaders, balance, null);
        }

        /// <summary>
        /// <b>Title:</b> Get Ethereum ERC721 Token Metadata<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum ERC721 token metadata.
        /// </summary>
        /// <param name="token">Token ID</param>
        /// <param name="contractAddress">ERC721 contract address</param>
        /// <param name="divider">Divider for result to get display balance</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<EthereumData> ERC721_GetTokenMetadata(string token, string contractAddress, decimal divider = 1, CancellationToken ct = default) => ERC721_GetTokenMetadataAsync(token, contractAddress, divider, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Ethereum ERC721 Token Metadata<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum ERC721 token metadata.
        /// </summary>
        /// <param name="token">Token ID</param>
        /// <param name="contractAddress">ERC721 contract address</param>
        /// <param name="divider">Divider for result to get display balance</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<EthereumData>> ERC721_GetTokenMetadataAsync(string token, string contractAddress, decimal divider = 1, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Tatum.GetUrl(string.Format(Endpoints_ERC721TokenMetadata, token, contractAddress));
            return await Tatum.SendTatumRequest<EthereumData>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Ethereum ERC721 Token owner<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum ERC721 token owner.
        /// </summary>
        /// <param name="token">Token ID</param>
        /// <param name="contractAddress">ERC721 contract address</param>
        /// <param name="divider">Divider for result to get display balance</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<EthereumData> ERC721_GetTokenOwner(string token, string contractAddress, decimal divider = 1, CancellationToken ct = default) => ERC721_GetTokenOwnerAsync(token, contractAddress, divider, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Ethereum ERC721 Token owner<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum ERC721 token owner.
        /// </summary>
        /// <param name="token">Token ID</param>
        /// <param name="contractAddress">ERC721 contract address</param>
        /// <param name="divider">Divider for result to get display balance</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<EthereumData>> ERC721_GetTokenOwnerAsync(string token, string contractAddress, decimal divider = 1, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Tatum.GetUrl(string.Format(Endpoints_ERC721TokenOwner, token, contractAddress));
            return await Tatum.SendTatumRequest<EthereumData>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Broadcast signed Ethereum transaction<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed transaction to Ethereum blockchain. 
        /// This method is used internally from Tatum KMS, Tatum Middleware or Tatum client libraries. 
        /// It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="signatureId">ID of prepared payment template to sign. Required only, when broadcasting transaction signed by Tatum KMS.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> Broadcast(string txData, string signatureId = null, CancellationToken ct = default) => BroadcastAsync(txData, signatureId, ct).Result;
        /// <summary>
        /// <b>Title:</b> Broadcast signed Ethereum transaction<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed transaction to Ethereum blockchain. 
        /// This method is used internally from Tatum KMS, Tatum Middleware or Tatum client libraries. 
        /// It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="signatureId">ID of prepared payment template to sign. Required only, when broadcasting transaction signed by Tatum KMS.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> BroadcastAsync(string txData, string signatureId = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "txData", txData },
            };
            parameters.AddOptionalParameter("signatureId", signatureId);

            var credits = 2;
            var url = Tatum.GetUrl(string.Format(Endpoints_Broadcast));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }
        #endregion


    }
}
