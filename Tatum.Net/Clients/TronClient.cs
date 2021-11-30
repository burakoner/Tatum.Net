using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.Interfaces;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Clients
{
    public class TronClient : ITatumBlockchainTronClient
    {
        public TatumClient Tatum { get; protected set; }

        #region API Endpoints

        #region Blockchain - TRON
        protected const string Endpoints_GenerateAccount = "tron/account";
        protected const string Endpoints_CurrentBlock = "tron/info";
        protected const string Endpoints_GetBlock = "tron/block/{0}";
        protected const string Endpoints_GetTransactionsByAccount = "tron/transaction/account/{0}";
        protected const string Endpoints_GetTransactionByHash = "tron/transaction/{0}";
        protected const string Endpoints_Send = "tron/transaction";
        protected const string Endpoints_Broadcast = "tron/broadcast";

        protected const string Endpoints_Freeze = "tron/freezeBalance";
        protected const string Endpoints_TRC10GetToken = "tron/trc10/detail/{0}";
        protected const string Endpoints_TRC10CreateToken = "tron/trc10/deploy";
        protected const string Endpoints_TRC10Send = "tron/trc10/transaction";
        protected const string Endpoints_TRC20CreateToken = "tron/trc20/deploy";
        protected const string Endpoints_TRC20Send = "tron/trc20/transaction";
        #endregion

        #endregion

        public TronClient(TatumClient tatumClient)
        {
            Tatum = tatumClient;
        }



        #region Blockchain / TRON
        /// <summary>
        /// <b>Title:</b> Generate Tron wallet<br />
        /// <b>Credits:</b> 5 credit per API call.<br />
        /// <b>Description:</b>
        /// Generate TRON address and private key.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TronWallet> GenerateAccount(CancellationToken ct = default) => GenerateAccountAsync(ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Tron wallet<br />
        /// <b>Credits:</b> 5 credit per API call.<br />
        /// <b>Description:</b>
        /// Generate TRON address and private key.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TronWallet>> GenerateAccountAsync(CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_GenerateAccount));
            return await Tatum.SendTatumRequest<TronWallet>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> XXXXXXXXXXXX<br />
        /// <b>Credits:</b> XXXXXXXXXXXX<br />
        /// <b>Description:</b>
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TronCurrentBlock> GetCurrentBlock(CancellationToken ct = default) => GetCurrentBlockAsync(ct).Result;
        /// <summary>
        /// <b>Title:</b> XXXXXXXXXXXX<br />
        /// <b>Credits:</b> XXXXXXXXXXXX<br />
        /// <b>Description:</b>
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TronCurrentBlock>> GetCurrentBlockAsync(CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_CurrentBlock));
            return await Tatum.SendTatumRequest<TronCurrentBlock>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get current Tron block<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Tron block by hash or height.
        /// </summary>
        /// <param name="hash_height">Block hash or height.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TronBlock> GetBlock(string hash_height, CancellationToken ct = default) => GetBlockAsync(hash_height, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get current Tron block<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Tron block by hash or height.
        /// </summary>
        /// <param name="hash_height">Block hash or height.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TronBlock>> GetBlockAsync(string hash_height, CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetBlock, hash_height));
            return await Tatum.SendTatumRequest<TronBlock>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Tron Account transactions<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Tron Account transactions. Default page size is 200 transactions per request.
        /// </summary>
        /// <param name="address">Address to get transactions for.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TronAccountTransactions> GetTransactionsByAccount(string address, CancellationToken ct = default) => GetTransactionsByAccountAsync(address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Tron Account transactions<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Tron Account transactions. Default page size is 200 transactions per request.
        /// </summary>
        /// <param name="address">Address to get transactions for.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TronAccountTransactions>> GetTransactionsByAccountAsync(string address, CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetTransactionsByAccount, address));
            return await Tatum.SendTatumRequest<TronAccountTransactions>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Tron transaction by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Tron transaction by hash.
        /// </summary>
        /// <param name="hash">Transaction hash.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TronTransaction> GetTransactionByHash(string hash, CancellationToken ct = default) => GetTransactionByHashAsync(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Tron transaction by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Tron transaction by hash.
        /// </summary>
        /// <param name="hash">Transaction hash.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TronTransaction>> GetTransactionByHashAsync(string hash, CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetTransactionByHash, hash));
            return await Tatum.SendTatumRequest<TronTransaction>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Send Tron transaction<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Tron transaction from address to address.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="fromPrivateKey">Private key of the address, from which the TRX will be sent.</param>
        /// <param name="to">Recipient address of TRON account in Base58 format.</param>
        /// <param name="amount">Amount to be sent in TRX.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> Send(string fromPrivateKey, string to, decimal amount, CancellationToken ct = default) => SendAsync(fromPrivateKey, to, amount, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send Tron transaction<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Tron transaction from address to address.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="fromPrivateKey">Private key of the address, from which the TRX will be sent.</param>
        /// <param name="to">Recipient address of TRON account in Base58 format.</param>
        /// <param name="amount">Amount to be sent in TRX.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> SendAsync(string fromPrivateKey, string to, decimal amount, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "fromPrivateKey", fromPrivateKey },
                { "to", to },
                { "amount", amount.ToString(CultureInfo.InvariantCulture) },
            };

            var credits = 10;
            var url = Tatum.GetUrl(string.Format(Endpoints_Send));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Broadcast Tron transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast Tron transaction. This method is used internally from Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> Broadcast(string txData, CancellationToken ct = default) => BroadcastAsync(txData, ct).Result;
        /// <summary>
        /// <b>Title:</b> Broadcast Tron transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast Tron transaction. This method is used internally from Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> BroadcastAsync(string txData, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "txData", txData },
            };

            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_Broadcast));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Freeze Tron balance<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Freeze Tron assets on the address. By freezing assets, you can obtain energy or bandwith to perform transactions.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="fromPrivateKey">Private key of the address, from which the TRX will be sent.</param>
        /// <param name="receiver">Recipient address of frozen BANDWIDTH or ENERGY.</param>
        /// <param name="duration">Duration of frozen funds, in days.</param>
        /// <param name="resource">Resource to obtain, BANDWIDTH or ENERGY.</param>
        /// <param name="amount">Amount to be frozen in TRX.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> FreezeBalance(string fromPrivateKey, string receiver, int duration, string resource, decimal amount, CancellationToken ct = default) => FreezeBalanceAsync(fromPrivateKey, receiver, duration, resource, amount, ct).Result;
        /// <summary>
        /// <b>Title:</b> Freeze Tron balance<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Freeze Tron assets on the address. By freezing assets, you can obtain energy or bandwith to perform transactions.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="fromPrivateKey">Private key of the address, from which the TRX will be sent.</param>
        /// <param name="receiver">Recipient address of frozen BANDWIDTH or ENERGY.</param>
        /// <param name="duration">Duration of frozen funds, in days.</param>
        /// <param name="resource">Resource to obtain, BANDWIDTH or ENERGY.</param>
        /// <param name="amount">Amount to be frozen in TRX.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> FreezeBalanceAsync(string fromPrivateKey, string receiver, int duration, string resource, decimal amount, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "fromPrivateKey", fromPrivateKey },
                { "receiver", receiver },
                { "duration", duration },
                { "resource", resource },
                { "amount", amount.ToString(CultureInfo.InvariantCulture) },
            };

            var credits = 10;
            var url = Tatum.GetUrl(string.Format(Endpoints_Freeze));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Get Tron TRC10 token detail<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Tron TRC10 token details.
        /// </summary>
        /// <param name="id">TRC10 token ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TronTRC10Token> TRC10GetTokenDetails(long id, CancellationToken ct = default) => TRC10GetTokenDetailsAsync(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Tron TRC10 token detail<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Tron TRC10 token details.
        /// </summary>
        /// <param name="id">TRC10 token ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TronTRC10Token>> TRC10GetTokenDetailsAsync(long id, CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_TRC10GetToken, id));
            return await Tatum.SendTatumRequest<TronTRC10Token>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Create Tron TRC10 token<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Create Tron TRC10 token. 1 account can create only 1 token. All supply of the tokens are transfered to the issuer account 100 seconds after the creation.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="fromPrivateKey">Private key of the address, from which the TRX will be sent.</param>
        /// <param name="recipient">Recipient address of created TRC20 tokens.</param>
        /// <param name="name">Name of the token.</param>
        /// <param name="abbreviation">Abbreviation of the token.</param>
        /// <param name="description">Description of the token.</param>
        /// <param name="url">URL of the token.</param>
        /// <param name="totalSupply">Total supply of the tokens.</param>
        /// <param name="decimals">Number of decimal places of the token.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> TRC10CreateToken(string fromPrivateKey, string recipient, string name, string abbreviation, string description, string url, long totalSupply, int decimals, CancellationToken ct = default) => TRC10CreateTokenAsync(fromPrivateKey, recipient, name, abbreviation, description, url, totalSupply, decimals, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create Tron TRC10 token<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Create Tron TRC10 token. 1 account can create only 1 token. All supply of the tokens are transfered to the issuer account 100 seconds after the creation.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="fromPrivateKey">Private key of the address, from which the TRX will be sent.</param>
        /// <param name="recipient">Recipient address of created TRC20 tokens.</param>
        /// <param name="name">Name of the token.</param>
        /// <param name="abbreviation">Abbreviation of the token.</param>
        /// <param name="description">Description of the token.</param>
        /// <param name="url">URL of the token.</param>
        /// <param name="totalSupply">Total supply of the tokens.</param>
        /// <param name="decimals">Number of decimal places of the token.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> TRC10CreateTokenAsync(string fromPrivateKey, string recipient, string name, string abbreviation, string description, string url, long totalSupply, int decimals, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "fromPrivateKey", fromPrivateKey },
                { "recipient", recipient },
                { "name", name },
                { "abbreviation", abbreviation },
                { "description", description },
                { "url", url },
                { "totalSupply", totalSupply },
                { "decimals", decimals },
            };

            var credits = 10;
            var url_ = Tatum.GetUrl(string.Format(Endpoints_TRC10CreateToken));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url_, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Send Tron TRC10 transaction<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Tron TRC10 transaction from address to address.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="fromPrivateKey">Private key of the address, from which the TRX will be sent.</param>
        /// <param name="to">Recipient address of TRON account in Base58 format.</param>
        /// <param name="tokenId">ID of the token to transfer.</param>
        /// <param name="amount">Amount to be sent in TRX.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> TRC10Send(string fromPrivateKey, string to, long tokenId, decimal amount, CancellationToken ct = default) => TRC10SendAsync(fromPrivateKey, to, tokenId, amount, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send Tron TRC10 transaction<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Tron TRC10 transaction from address to address.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="fromPrivateKey">Private key of the address, from which the TRX will be sent.</param>
        /// <param name="to">Recipient address of TRON account in Base58 format.</param>
        /// <param name="tokenId">ID of the token to transfer.</param>
        /// <param name="amount">Amount to be sent in TRX.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> TRC10SendAsync(string fromPrivateKey, string to, long tokenId, decimal amount, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "fromPrivateKey", fromPrivateKey },
                { "to", to },
                { "tokenId", tokenId.ToString(CultureInfo.InvariantCulture) },
                { "amount", amount.ToString(CultureInfo.InvariantCulture) },
            };

            var credits = 10;
            var url_ = Tatum.GetUrl(string.Format(Endpoints_TRC10Send));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url_, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Create Tron TRC20 token<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Create Tron TRC20 token. 1 account can create only 1 token. All supply of the tokens are transfered to the issuer account 100 seconds after the creation.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="fromPrivateKey">Private key of the address, from which the TRX will be sent.</param>
        /// <param name="recipient">Recipient address of created TRC20 tokens.</param>
        /// <param name="name">Name of the token.</param>
        /// <param name="symbol">Symbol of the token.</param>
        /// <param name="totalSupply">Total supply of the tokens.</param>
        /// <param name="decimals">Number of decimal places of the token.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> TRC20CreateToken(string fromPrivateKey, string recipient, string name, string symbol, long totalSupply, int decimals, CancellationToken ct = default) => TRC20CreateTokenAsync(fromPrivateKey, recipient, name, symbol, totalSupply, decimals, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create Tron TRC20 token<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Create Tron TRC20 token. 1 account can create only 1 token. All supply of the tokens are transfered to the issuer account 100 seconds after the creation.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="fromPrivateKey">Private key of the address, from which the TRX will be sent.</param>
        /// <param name="recipient">Recipient address of created TRC20 tokens.</param>
        /// <param name="name">Name of the token.</param>
        /// <param name="symbol">Symbol of the token.</param>
        /// <param name="totalSupply">Total supply of the tokens.</param>
        /// <param name="decimals">Number of decimal places of the token.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> TRC20CreateTokenAsync(string fromPrivateKey, string recipient, string name, string symbol, long totalSupply, int decimals, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "fromPrivateKey", fromPrivateKey },
                { "recipient", recipient },
                { "name", name },
                { "symbol", symbol },
                { "totalSupply", totalSupply },
                { "decimals", decimals },
            };

            var credits = 10;
            var url_ = Tatum.GetUrl(string.Format(Endpoints_TRC20CreateToken));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url_, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Send Tron TRC20 transaction<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Tron TRC20 transaction from address to address.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="fromPrivateKey">Private key of the address, from which the TRX will be sent.</param>
        /// <param name="to">Recipient address of TRON account in Base58 format.</param>
        /// <param name="tokenAddress">Address of the TRC20 token to transfer.</param>
        /// <param name="amount">Fee in TRX to be paid.</param>
        /// <param name="feeLimit">Amount to be sent in TRX.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> TRC20Send(string fromPrivateKey, string to, string tokenAddress, decimal amount, decimal feeLimit, CancellationToken ct = default) => TRC20SendAsync(fromPrivateKey, to, tokenAddress, amount, feeLimit, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send Tron TRC20 transaction<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Tron TRC20 transaction from address to address.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="fromPrivateKey">Private key of the address, from which the TRX will be sent.</param>
        /// <param name="to">Recipient address of TRON account in Base58 format.</param>
        /// <param name="tokenAddress">Address of the TRC20 token to transfer.</param>
        /// <param name="amount">Fee in TRX to be paid.</param>
        /// <param name="feeLimit">Amount to be sent in TRX.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> TRC20SendAsync(string fromPrivateKey, string to, string tokenAddress, decimal amount, decimal feeLimit, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "fromPrivateKey", fromPrivateKey },
                { "to", to },
                { "tokenAddress", tokenAddress },
                { "feeLimit", feeLimit },
                { "amount", amount.ToString(CultureInfo.InvariantCulture) },
            };

            var credits = 10;
            var url_ = Tatum.GetUrl(string.Format(Endpoints_TRC20Send));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url_, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }
        #endregion

    }
}
