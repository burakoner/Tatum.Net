using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.Helpers;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Clients
{
    public class StellarClient
    {
        public TatumClient Tatum { get; protected set; }

        protected const string Endpoints_GenerateAccount = "xlm/account";
        protected const string Endpoints_BlockchainInformation = "xlm/info";
        protected const string Endpoints_BlockchainFee = "xlm/fee";
        protected const string Endpoints_GetLedger = "xlm/ledger/{0}";
        protected const string Endpoints_GetTransactionsInLedger = "xlm/ledger/{0}/transaction";
        protected const string Endpoints_GetTransactionsByAccount = "xlm/account/tx/{0}";
        protected const string Endpoints_GetTransactionByHash = "xlm/transaction/{0}";
        protected const string Endpoints_AccountInfo = "xlm/account/{0}";
        protected const string Endpoints_Send = "xlm/transaction";
        protected const string Endpoints_Trust = "xlm/trust";
        protected const string Endpoints_Broadcast = "xlm/broadcast";

        public StellarClient(TatumClient tatumClient)
        {
            Tatum = tatumClient;
        }

        /// <summary>
        /// <b>Title:</b> Generate XLM account<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate XLM account. Tatum does not support HD wallet for XLM, only specific address and private key can be generated.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<StellarAddressSecret> GenerateAccount(CancellationToken ct = default) => GenerateAccountAsync(ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate XLM account<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate XLM account. Tatum does not support HD wallet for XLM, only specific address and private key can be generated.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<StellarAddressSecret>> GenerateAccountAsync(CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_GenerateAccount));
            return await Tatum.SendTatumRequest<StellarAddressSecret>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get XLM Blockchain Information<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Blockchain last closed ledger.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<StellarChainInfo> GetBlockchainInformation(CancellationToken ct = default) => GetBlockchainInformationAsync(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get XLM Blockchain Information<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Blockchain last closed ledger.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<StellarChainInfo>> GetBlockchainInformationAsync(CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_BlockchainInformation));
            return await Tatum.SendTatumRequest<StellarChainInfo>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get XLM Blockchain Ledger by sequence<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Blockchain ledger for ledger sequence.
        /// </summary>
        /// <param name="sequence">Sequence of the ledger.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<StellarChainInfo> GetLedger(long sequence, CancellationToken ct = default) => GetLedgerAsync(sequence, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get XLM Blockchain Ledger by sequence<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Blockchain ledger for ledger sequence.
        /// </summary>
        /// <param name="sequence">Sequence of the ledger.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<StellarChainInfo>> GetLedgerAsync(long sequence, CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetLedger, sequence));
            return await Tatum.SendTatumRequest<StellarChainInfo>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get XLM Blockchain Transactions in Ledger<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Blockchain transactions in the ledger.
        /// </summary>
        /// <param name="sequence">Sequence of the ledger.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<StellarTransaction>> GetTransactionsInLedger(long sequence, CancellationToken ct = default) => GetTransactionsInLedgerAsync(sequence, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get XLM Blockchain Transactions in Ledger<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Blockchain transactions in the ledger.
        /// </summary>
        /// <param name="sequence">Sequence of the ledger.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<StellarTransaction>>> GetTransactionsInLedgerAsync(long sequence, CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetTransactionsInLedger, sequence));
            return await Tatum.SendTatumRequest<IEnumerable<StellarTransaction>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get actual XLM fee<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Blockchain fee in 1/10000000 of XLM (stroop)
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<decimal> GetBlockchainFee(CancellationToken ct = default) => GetBlockchainFeeAsync(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get actual XLM fee<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Blockchain fee in 1/10000000 of XLM (stroop)
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<decimal>> GetBlockchainFeeAsync(CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_BlockchainFee));
            var result = await Tatum.SendTatumRequest<string>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<decimal>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<decimal>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.ToDecimal(), null);
        }

        /// <summary>
        /// <b>Title:</b> Get XLM Account transactions<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// List all XLM account transactions.
        /// </summary>
        /// <param name="account">Address of XLM account.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<StellarTransaction>> GetTransactionsByAccount(string account, CancellationToken ct = default) => GetTransactionsByAccountAsync(account, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get XLM Account transactions<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// List all XLM account transactions.
        /// </summary>
        /// <param name="account">Address of XLM account.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<StellarTransaction>>> GetTransactionsByAccountAsync(string account, CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetTransactionsByAccount, account));
            return await Tatum.SendTatumRequest<IEnumerable<StellarTransaction>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get XLM Transaction by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Transaction by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<StellarTransaction> GetTransactionByHash(string hash, CancellationToken ct = default) => GetTransactionByHashAsync(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get XLM Transaction by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Transaction by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<StellarTransaction>> GetTransactionByHashAsync(string hash, CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetTransactionByHash, hash));
            return await Tatum.SendTatumRequest<StellarTransaction>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get XLM Account info<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Account detail.
        /// </summary>
        /// <param name="account">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<StellarAccountInfo> GetAccountInfo(string account, CancellationToken ct = default) => GetAccountInfoAsync(account, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get XLM Account info<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Account detail.
        /// </summary>
        /// <param name="account">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<StellarAccountInfo>> GetAccountInfoAsync(string account, CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_AccountInfo, account));
            return await Tatum.SendTatumRequest<StellarAccountInfo>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Send XLM from address to address<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send XLM from account to account. It is possbile to send native XLM asset, or any other custom asset present on the network.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="fromAccount">Blockchain account to send assets from</param>
        /// <param name="to">Blockchain address to send assets</param>
        /// <param name="amount">Amount to be sent, in XLM.</param>
        /// <param name="fromSecret">Secret for account. Secret, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the secret associated in signing application. Secret, or signature Id must be present.</param>
        /// <param name="token">Asset name. If not defined, transaction is being sent in native XLM asset.</param>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets to send, if not native XLM asset.</param>
        /// <param name="message">Short message to recipient. It can be either 28 characters long ASCII text, 64 characters long HEX string or uint64 number.</param>
        /// <param name="initialize">Set to true, if the destination address is not yet initialized and should be funded for the first time.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> Send(
            string fromAccount,
            string to,
            string amount,
            string fromSecret = null,
            string signatureId = null,
            string token = null,
            string issuerAccount = null,
            string message = null,
            bool initialize = false,
            CancellationToken ct = default) => SendAsync(fromAccount, to, amount, fromSecret, signatureId, token, issuerAccount, message, initialize, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send XLM from address to address<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send XLM from account to account. It is possbile to send native XLM asset, or any other custom asset present on the network.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="fromAccount">Blockchain account to send assets from</param>
        /// <param name="to">Blockchain address to send assets</param>
        /// <param name="amount">Amount to be sent, in XLM.</param>
        /// <param name="fromSecret">Secret for account. Secret, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the secret associated in signing application. Secret, or signature Id must be present.</param>
        /// <param name="token">Asset name. If not defined, transaction is being sent in native XLM asset.</param>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets to send, if not native XLM asset.</param>
        /// <param name="message">Short message to recipient. It can be either 28 characters long ASCII text, 64 characters long HEX string or uint64 number.</param>
        /// <param name="initialize">Set to true, if the destination address is not yet initialized and should be funded for the first time.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> SendAsync(
            string fromAccount,
            string to,
            string amount,
            string fromSecret = null,
            string signatureId = null,
            string token = null,
            string issuerAccount = null,
            string message = null,
            bool initialize = false,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "fromAccount", fromAccount },
                { "to", to },
                { "amount", amount },
                { "initialize", initialize },
            };
            parameters.AddOptionalParameter("fromSecret", fromSecret);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("token", token);
            parameters.AddOptionalParameter("issuerAccount", issuerAccount);
            parameters.AddOptionalParameter("message", message);

            var credits = 10;
            var url = Tatum.GetUrl(string.Format(Endpoints_Send));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Create / Update / Delete XLM trust line<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Create / Update / Delete XLM trust line between accounts to transfer private assets. By creating trustline for the first time, the asset is created automatically and can be used in the transactions.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="fromAccount">XLM account address. Must be the one used for generating deposit tags.</param>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets to create trust line for.</param>
        /// <param name="token">Asset name.</param>
        /// <param name="fromSecret">Secret for account. Secret, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the secret associated in signing application. Secret, or signature Id must be present.</param>
        /// <param name="limit">Amount of the assets to be permitted to send over this trust line. 0 means deletion of the trust line. When no limit is specified, maximum amount available is permitted.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> TrustLine(
            string fromAccount,
            string issuerAccount,
            string token,
            string fromSecret = null,
            string signatureId = null,
            string limit = null,
            CancellationToken ct = default) => TrustLineAsync(fromAccount, issuerAccount, token, fromSecret, signatureId, limit, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create / Update / Delete XLM trust line<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Create / Update / Delete XLM trust line between accounts to transfer private assets. By creating trustline for the first time, the asset is created automatically and can be used in the transactions.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="fromAccount">XLM account address. Must be the one used for generating deposit tags.</param>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets to create trust line for.</param>
        /// <param name="token">Asset name.</param>
        /// <param name="fromSecret">Secret for account. Secret, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the secret associated in signing application. Secret, or signature Id must be present.</param>
        /// <param name="limit">Amount of the assets to be permitted to send over this trust line. 0 means deletion of the trust line. When no limit is specified, maximum amount available is permitted.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> TrustLineAsync(
            string fromAccount,
            string issuerAccount,
            string token,
            string fromSecret = null,
            string signatureId = null,
            string limit = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "fromAccount", fromAccount },
                { "issuerAccount", issuerAccount },
                { "token", token },
            };
            parameters.AddOptionalParameter("fromSecret", fromSecret);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("limit", limit);

            var credits = 10;
            var url = Tatum.GetUrl(string.Format(Endpoints_Trust));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Broadcast signed XLM transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed transaction to XLM blockchain. This method is used internally from Tatum KMS, Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="signatureId">ID of prepared payment template to sign. Required only, when broadcasting transaction signed by Tatum KMS.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> Broadcast(string txData, string signatureId = null, CancellationToken ct = default) => BroadcastAsync(txData, signatureId, ct).Result;
        /// <summary>
        /// <b>Title:</b> Broadcast signed XLM transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed transaction to XLM blockchain. This method is used internally from Tatum KMS, Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
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

            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_Broadcast));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }
    }
}
