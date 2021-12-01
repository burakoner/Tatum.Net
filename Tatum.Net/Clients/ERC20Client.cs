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
    public class ERC20Client
    {
        public TatumClient Tatum { get; protected set; }

        protected const string Endpoints_SmartContract = "ethereum/smartcontract";
        protected const string Endpoints_ERC20Balance = "ethereum/account/balance/erc20/{0}";
        protected const string Endpoints_ERC20DeploySmartContract = "ethereum/erc20/deploy";
        protected const string Endpoints_ERC20Transfer = "ethereum/erc20/transaction";

        public ERC20Client(TatumClient tatumClient)
        {
            Tatum = tatumClient;
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
        public virtual WebCallResult<BlockchainResponse> DeploySmartContract(
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
            => DeploySmartContractAsync(name, symbol, supply, digits, address, signatureId, fromPrivateKey, nonce, fee, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainResponse>> DeploySmartContractAsync(
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

        // TODO: Mint ERC20
        // TODO: Burn ERC20
        // TODO: Approve spending of ERC20

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
        public virtual WebCallResult<BlockchainResponse> Transfer(
            string contractAddress,
            string to,
            string amount,
            int digits,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
            => TransferAsync(contractAddress, to, amount, digits, signatureId, fromPrivateKey, nonce, fee, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainResponse>> TransferAsync(
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



        // TODO: Get ERC20 transactions by address





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
        public virtual WebCallResult<decimal> GetBalance(string address, string contractAddress, int decimals = 0, CancellationToken ct = default) => GetBalanceAsync(address, contractAddress, decimals, ct).Result;
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
        public virtual async Task<WebCallResult<decimal>> GetBalanceAsync(string address, string contractAddress, int decimals = 0, CancellationToken ct = default)
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
    }
}
