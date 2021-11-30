using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.Helpers;
using Tatum.Net.Interfaces;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Clients
{
    public class BinanceClient : ITatumBlockchainBinanceClient
    {
        public TatumClient Tatum { get; protected set; }

        #region API Endpoints

        #region Blockchain - Binance
        protected const string Endpoints_GenerateAccount = "bnb/account";
        protected const string Endpoints_CurrentBlock = "bnb/block/current";
        protected const string Endpoints_GetTransactionsInBlock = "bnb/block/{0}";
        protected const string Endpoints_AccountInfo = "bnb/account/{0}";
        protected const string Endpoints_GetTransaction = "bnb/transaction/{0}";
        protected const string Endpoints_Send = "bnb/transaction";
        protected const string Endpoints_Broadcast = "bnb/broadcast";
        #endregion

        #endregion

        public BinanceClient(TatumClient tatumClient)
        {
            Tatum = tatumClient;
        }



        #region Blockchain / Binance
        /// <summary>
        /// <b>Title:</b> Generate Binance wallet<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate BNB account. Tatum does not support HD wallet for BNB, only specific address and private key can be generated.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BinanceAddress> GenerateAccount(CancellationToken ct = default) => GenerateAccountAsync(ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Binance wallet<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate BNB account. Tatum does not support HD wallet for BNB, only specific address and private key can be generated.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BinanceAddress>> GenerateAccountAsync(CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_GenerateAccount));
            return await Tatum.SendTatumRequest<BinanceAddress>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Binance current block<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Binance current block number.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<long> GetCurrentBlock(CancellationToken ct = default) => GetCurrentBlockAsync(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Binance current block<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Binance current block number.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<long>> GetCurrentBlockAsync(CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_CurrentBlock));
            var result = await Tatum.SendTatumRequest<string>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<long>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<long>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.ToLong(), null);
        }

        /// <summary>
        /// <b>Title:</b> Get Binance Transactions in Block<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Transactions in block by block height.
        /// </summary>
        /// <param name="height">Block height</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BinanceBlockTransactions> GetTransactionsInBlock(long height, CancellationToken ct = default) => GetTransactionsInBlockAsync(height, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Binance Transactions in Block<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Transactions in block by block height.
        /// </summary>
        /// <param name="height">Block height</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BinanceBlockTransactions>> GetTransactionsInBlockAsync(long height, CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetTransactionsInBlock, height));
            return await Tatum.SendTatumRequest<BinanceBlockTransactions>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Binance Account<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Binance Account Detail by address.
        /// </summary>
        /// <param name="account">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BinanceAccount> GetAccountInfo(string account, CancellationToken ct = default) => GetAccountInfoAsync(account, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Binance Account<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Binance Account Detail by address.
        /// </summary>
        /// <param name="account">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BinanceAccount>> GetAccountInfoAsync(string account, CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_AccountInfo, account));
            return await Tatum.SendTatumRequest<BinanceAccount>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Binance Transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Binance Transaction by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BinanceTransaction> GetTransaction(string hash, CancellationToken ct = default) => GetTransactionAsync(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Binance Transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Binance Transaction by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BinanceTransaction>> GetTransactionAsync(string hash, CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetTransaction, hash));
            return await Tatum.SendTatumRequest<BinanceTransaction>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Send Binance / Binance Token from account to account<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Binance or Binance Token token from account to account.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="to">Blockchain address to send assets.</param>
        /// <param name="currency">Currency to transfer from Binance Blockchain Account.</param>
        /// <param name="amount">Amount to be sent in BNB.</param>
        /// <param name="fromPrivateKey">Private key of sender address.</param>
        /// <param name="signatureId">Signature hash of the mnemonic, which will be used to sign transactions locally. All signature Ids should be present, which might be used to sign transaction.</param>
        /// <param name="message">Message to recipient.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> Send(
            string to,
            string currency,
            string amount,
            string fromPrivateKey = null,
            string signatureId = null,
            string message = null,
            CancellationToken ct = default) => SendAsync(to, currency, amount, fromPrivateKey, signatureId, message, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send Binance / Binance Token from account to account<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Binance or Binance Token token from account to account.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="to">Blockchain address to send assets.</param>
        /// <param name="currency">Currency to transfer from Binance Blockchain Account.</param>
        /// <param name="amount">Amount to be sent in BNB.</param>
        /// <param name="fromPrivateKey">Private key of sender address.</param>
        /// <param name="signatureId">Signature hash of the mnemonic, which will be used to sign transactions locally. All signature Ids should be present, which might be used to sign transaction.</param>
        /// <param name="message">Message to recipient.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> SendAsync(
            string to,
            string currency,
            string amount,
            string fromPrivateKey = null,
            string signatureId = null,
            string message = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "to", to },
                { "amount", amount },
                { "currency", currency },
            };
            parameters.AddOptionalParameter("fromPrivateKey", fromPrivateKey);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("message", message);

            var credits = 10;
            var url = Tatum.GetUrl(string.Format(Endpoints_Send));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Broadcast signed BNB transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed transaction to Binance blockchain. This method is used internally from Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> Broadcast(string txData, CancellationToken ct = default) => BroadcastAsync(txData, ct).Result;
        /// <summary>
        /// <b>Title:</b> Broadcast signed BNB transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed transaction to Binance blockchain. This method is used internally from Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
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
        #endregion

    }
}
