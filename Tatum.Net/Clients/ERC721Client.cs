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
using Tatum.Net.RestObjects;

namespace Tatum.Net.Clients
{
    public class ERC721Client
    {
        public TatumClient Tatum { get; protected set; }

        protected const string Endpoints_ERC721Balance = "ethereum/erc721/balance/{0}/{1}";
        protected const string Endpoints_ERC721DeploySmartContract = "ethereum/erc721/deploy";
        protected const string Endpoints_ERC721Mint = "ethereum/erc721/mint";
        protected const string Endpoints_ERC721MintMultiple = "ethereum/erc721/mint/batch";
        protected const string Endpoints_ERC721Transfer = "ethereum/erc721/transaction";
        protected const string Endpoints_ERC721Burn = "ethereum/erc721/burn";
        protected const string Endpoints_ERC721Token = "ethereum/erc721/token/{0}/{1}/{2}";
        protected const string Endpoints_ERC721TokenMetadata = "ethereum/erc721/metadata/{0}/{1}";
        protected const string Endpoints_ERC721TokenOwner = "ethereum/erc721/owner/{0}/{1}";

        public ERC721Client(TatumClient tatumClient)
        {
            Tatum = tatumClient;
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
        public virtual WebCallResult<BlockchainResponse> DeploySmartContract(
            string name,
            string symbol,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
            => DeploySmartContractAsync(name, symbol, signatureId, fromPrivateKey, nonce, fee, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainResponse>> DeploySmartContractAsync(
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
        public virtual WebCallResult<BlockchainResponse> Mint(
            string contractAddress,
            string tokenId,
            string to,
            string url,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
            => MintAsync(contractAddress, tokenId, to, url, signatureId, fromPrivateKey, nonce, fee, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainResponse>> MintAsync(
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
        public virtual WebCallResult<BlockchainResponse> Transfer(
            string contractAddress,
            string tokenId,
            string to,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
            => TransferAsync(contractAddress, tokenId, to, signatureId, fromPrivateKey, nonce, fee, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainResponse>> TransferAsync(
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
        public virtual WebCallResult<BlockchainResponse> MintMultiple(
            string contractAddress,
            IEnumerable<string> tokenId,
            IEnumerable<string> to,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
            => MintMultipleAsync(contractAddress, tokenId, to, signatureId, fromPrivateKey, nonce, fee, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainResponse>> MintMultipleAsync(
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
        public virtual WebCallResult<BlockchainResponse> Burn(
            string contractAddress,
            string tokenId,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
            => BurnAsync(contractAddress, tokenId, signatureId, fromPrivateKey, nonce, fee, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainResponse>> BurnAsync(
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
        public virtual WebCallResult<decimal> GetBalance(string address, string contractAddress, int decimals = 0, CancellationToken ct = default) => GetERC721BalanceAsync(address, contractAddress, decimals, ct).Result;
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
        public virtual WebCallResult<decimal> GetToken(string address, int index, string contractAddress, int decimals = 0, CancellationToken ct = default) => GetTokenAsync(address, index, contractAddress, decimals, ct).Result;
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
        public virtual async Task<WebCallResult<decimal>> GetTokenAsync(string address, int index, string contractAddress, int decimals = 0, CancellationToken ct = default)
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
        public virtual WebCallResult<EthereumData> GetTokenMetadata(string token, string contractAddress, decimal divider = 1, CancellationToken ct = default) => GetTokenMetadataAsync(token, contractAddress, divider, ct).Result;
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
        public virtual async Task<WebCallResult<EthereumData>> GetTokenMetadataAsync(string token, string contractAddress, decimal divider = 1, CancellationToken ct = default)
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
        public virtual WebCallResult<EthereumData> GetTokenOwner(string token, string contractAddress, decimal divider = 1, CancellationToken ct = default) => GetTokenOwnerAsync(token, contractAddress, divider, ct).Result;
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
        public virtual async Task<WebCallResult<EthereumData>> GetTokenOwnerAsync(string token, string contractAddress, decimal divider = 1, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Tatum.GetUrl(string.Format(Endpoints_ERC721TokenOwner, token, contractAddress));
            return await Tatum.SendTatumRequest<EthereumData>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }
    }
}
