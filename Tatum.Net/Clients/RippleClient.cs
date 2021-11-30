using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.Interfaces;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Clients
{
    public class RippleClient : ITatumBlockchainRippleClient
    {
        public TatumClient Tatum { get; protected set; }

        #region API Endpoints

        #region Blockchain - Ripple
        protected const string Endpoints_GenerateAccount = "xrp/account";
        protected const string Endpoints_BlockchainInformation = "xrp/info";
        protected const string Endpoints_BlockchainFee = "xrp/fee";
        protected const string Endpoints_GetTransactionsByAccount = "xrp/account/tx/{0}";
        protected const string Endpoints_GetLedger = "xrp/ledger/{0}";
        protected const string Endpoints_GetTransactionByHash = "xrp/transaction/{0}";
        protected const string Endpoints_AccountInfo = "xrp/account/{0}";
        protected const string Endpoints_GetBalance = "xrp/account/{0}/balance";
        protected const string Endpoints_Send = "xrp/transaction";
        protected const string Endpoints_Trust = "xrp/trust";
        protected const string Endpoints_AccountSettings = "xrp/account/settings";
        protected const string Endpoints_Broadcast = "xrp/broadcast";
        #endregion

        #endregion

        public RippleClient(TatumClient tatumClient)
        {
            Tatum = tatumClient;
        }


        #region Blockchain / Ripple
        /// <summary>
        /// <b>Title:</b> Generate XRP account<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate XRP account. Tatum does not support HD wallet for XRP, only specific address and private key can be generated.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<RippleAddressSecret> GenerateAccount(CancellationToken ct = default) => GenerateAccountAsync(ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate XRP account<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate XRP account. Tatum does not support HD wallet for XRP, only specific address and private key can be generated.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<RippleAddressSecret>> GenerateAccountAsync(CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_GenerateAccount));
            return await Tatum.SendTatumRequest<RippleAddressSecret>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get XRP Blockchain Information<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XRP Blockchain last closed ledger index and hash.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<RippleChainInfo> GetBlockchainInformation(CancellationToken ct = default) => GetBlockchainInformationAsync(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get XRP Blockchain Information<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XRP Blockchain last closed ledger index and hash.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<RippleChainInfo>> GetBlockchainInformationAsync(CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_BlockchainInformation));
            return await Tatum.SendTatumRequest<RippleChainInfo>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get actual Blockchain fee<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XRP Blockchain fee. Standard fee for the transaction is available in the drops.base_fee section and is 10 XRP drops by default. When there is a heavy traffic on the blockchain, fees are increasing according to current traffic.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<RippleChainFee> GetBlockchainFee(CancellationToken ct = default) => GetBlockchainFeeAsync(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get actual Blockchain fee<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XRP Blockchain fee. Standard fee for the transaction is available in the drops.base_fee section and is 10 XRP drops by default. When there is a heavy traffic on the blockchain, fees are increasing according to current traffic.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<RippleChainFee>> GetBlockchainFeeAsync(CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_BlockchainFee));
            return await Tatum.SendTatumRequest<RippleChainFee>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Account transactions<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// List all Account transactions.
        /// </summary>
        /// <param name="account">Address of XRP account.</param>
        /// <param name="min">Ledger version to start scanning for transactions from.</param>
        /// <param name="marker">Marker from the last paginated request. It is stringified JSON from previous response.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<RippleAccountTransactions> GetTransactionsByAccount(string account, int? min = null, RippleMarker marker = null, CancellationToken ct = default) => GetTransactionsByAccountAsync(account, min, marker, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Account transactions<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// List all Account transactions.
        /// </summary>
        /// <param name="account">Address of XRP account.</param>
        /// <param name="min">Ledger version to start scanning for transactions from.</param>
        /// <param name="marker">Marker from the last paginated request. It is stringified JSON from previous response.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<RippleAccountTransactions>> GetTransactionsByAccountAsync(string account, int? min = null, RippleMarker marker = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("min", min);
            parameters.AddOptionalParameter("marker", marker);

            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetTransactionsByAccount, account));
            return await Tatum.SendTatumRequest<RippleAccountTransactions>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Ledger<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get ledger by sequence.
        /// </summary>
        /// <param name="index">Sequence of XRP ledger.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<RippleLedger> GetLedger(long index, CancellationToken ct = default) => GetLedgerAsync(index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Ledger<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get ledger by sequence.
        /// </summary>
        /// <param name="index">Sequence of XRP ledger.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<RippleLedger>> GetLedgerAsync(long index, CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetLedger, index));
            return await Tatum.SendTatumRequest<RippleLedger>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get XRP Transaction by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XRP Transaction by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<RippleTransactionData> GetTransactionByHash(string hash, CancellationToken ct = default) => GetTransactionByHashAsync(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get XRP Transaction by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XRP Transaction by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<RippleTransactionData>> GetTransactionByHashAsync(string hash, CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetTransactionByHash, hash));
            return await Tatum.SendTatumRequest<RippleTransactionData>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Account info<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XRP Account info.
        /// </summary>
        /// <param name="account">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<RippleAccount> GetAccountInfo(string account, CancellationToken ct = default) => GetAccountInfoAsync(account, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Account info<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XRP Account info.
        /// </summary>
        /// <param name="account">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<RippleAccount>> GetAccountInfoAsync(string account, CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_AccountInfo, account));
            return await Tatum.SendTatumRequest<RippleAccount>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Account Balance<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XRP Account Balance. Obtain balance of the XRP and other assets on the account.
        /// </summary>
        /// <param name="account">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<RippleBalance> GetBalance(string account, CancellationToken ct = default) => GetBalanceAsync(account, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Account Balance<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XRP Account Balance. Obtain balance of the XRP and other assets on the account.
        /// </summary>
        /// <param name="account">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<RippleBalance>> GetBalanceAsync(string account, CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetBalance, account));
            return await Tatum.SendTatumRequest<RippleBalance>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Send XRP from address to address<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send XRP from account to account.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="fromAccount">XRP account address. Must be the one used for generating deposit tags.</param>
        /// <param name="to">Blockchain address to send assets</param>
        /// <param name="amount">Amount to be sent, in XRP.</param>
        /// <param name="fromSecret">Secret for account. Secret, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the secret associated in signing application. Secret, or signature Id must be present.</param>
        /// <param name="fee">Fee to be paid, in XRP. If omitted, current fee will be calculated.</param>
        /// <param name="sourceTag">Source tag of sender account, if any.</param>
        /// <param name="destinationTag">Destination tag of recipient account, if any.</param>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets to create trust line for.</param>
        /// <param name="token">Asset name. Must be 160bit HEX string, e.g. SHA1.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> Send(
            string fromAccount,
            string to,
            string amount,
            string fromSecret = null,
            string signatureId = null,
            string fee = null,
            string sourceTag = null,
            string destinationTag = null,
            string issuerAccount = null,
            string token = null,
            CancellationToken ct = default) => SendAsync(fromAccount, to, amount, fromSecret, signatureId, fee, sourceTag, destinationTag, issuerAccount, token, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send XRP from address to address<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send XRP from account to account.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="fromAccount">XRP account address. Must be the one used for generating deposit tags.</param>
        /// <param name="to">Blockchain address to send assets</param>
        /// <param name="amount">Amount to be sent, in XRP.</param>
        /// <param name="fromSecret">Secret for account. Secret, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the secret associated in signing application. Secret, or signature Id must be present.</param>
        /// <param name="fee">Fee to be paid, in XRP. If omitted, current fee will be calculated.</param>
        /// <param name="sourceTag">Source tag of sender account, if any.</param>
        /// <param name="destinationTag">Destination tag of recipient account, if any.</param>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets to create trust line for.</param>
        /// <param name="token">Asset name. Must be 160bit HEX string, e.g. SHA1.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> SendAsync(
            string fromAccount,
            string to,
            string amount,
            string fromSecret = null,
            string signatureId = null,
            string fee = null,
            string sourceTag = null,
            string destinationTag = null,
            string issuerAccount = null,
            string token = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "fromAccount", fromAccount },
                { "to", to },
                { "amount", amount },
            };
            parameters.AddOptionalParameter("fromSecret", fromSecret);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("fee", fee);
            parameters.AddOptionalParameter("sourceTag", sourceTag);
            parameters.AddOptionalParameter("destinationTag", destinationTag);
            parameters.AddOptionalParameter("issuerAccount", issuerAccount);
            parameters.AddOptionalParameter("token", token);

            var credits = 10;
            var url = Tatum.GetUrl(string.Format(Endpoints_Send));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Create / Update / Delete XRP trust line<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Create / Update / Delete XRP trust line between accounts to transfer private assets. By creating trustline for the first time, the asset is created automatically and can be used in the transactions.
        /// Account setting rippling must be enabled on the issuer account before the trust line creation to asset work correctly.Creating a trust line will cause an additional 5 XRP to be blocked on the account.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="fromAccount">XRP account address. Must be the one used for generating deposit tags.</param>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets to create trust line for.</param>
        /// <param name="limit">Amount of the assets to be permitted to send over this trust line. 0 means deletion of the trust line.</param>
        /// <param name="token">Asset name. Must be 160bit HEX string, e.g. SHA1.</param>
        /// <param name="fromSecret">Secret for account. Secret, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the secret associated in signing application. Secret, or signature Id must be present.</param>
        /// <param name="fee">Fee to be paid, in XRP. If omitted, current fee will be calculated.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> TrustLine(
            string fromAccount,
            string issuerAccount,
            string limit,
            string token,
            string fromSecret = null,
            string signatureId = null,
            string fee = null,
            CancellationToken ct = default) => TrustLineAsync(fromAccount, issuerAccount, limit, token, fromSecret, signatureId, fee, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create / Update / Delete XRP trust line<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Create / Update / Delete XRP trust line between accounts to transfer private assets. By creating trustline for the first time, the asset is created automatically and can be used in the transactions.
        /// Account setting rippling must be enabled on the issuer account before the trust line creation to asset work correctly.Creating a trust line will cause an additional 5 XRP to be blocked on the account.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="fromAccount">XRP account address. Must be the one used for generating deposit tags.</param>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets to create trust line for.</param>
        /// <param name="limit">Amount of the assets to be permitted to send over this trust line. 0 means deletion of the trust line.</param>
        /// <param name="token">Asset name. Must be 160bit HEX string, e.g. SHA1.</param>
        /// <param name="fromSecret">Secret for account. Secret, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the secret associated in signing application. Secret, or signature Id must be present.</param>
        /// <param name="fee">Fee to be paid, in XRP. If omitted, current fee will be calculated.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> TrustLineAsync(
            string fromAccount,
            string issuerAccount,
            string limit,
            string token,
            string fromSecret = null,
            string signatureId = null,
            string fee = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "fromAccount", fromAccount },
                { "issuerAccount", issuerAccount },
                { "limit", limit },
                { "token", token },
            };
            parameters.AddOptionalParameter("fromSecret", fromSecret);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("fee", fee);

            var credits = 10;
            var url = Tatum.GetUrl(string.Format(Endpoints_Trust));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Modify XRP account<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Modify XRP account settings. If an XRP account should be an issuer of the custom asset, this accounts should have rippling enabled to true. In order to support off-chain processing, required destination tag should be set on the account.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="fromAccount">XRP account address. Must be the one used for generating deposit tags.</param>
        /// <param name="fromSecret">Secret for account. Secret, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Secret or signature Id must be present.</param>
        /// <param name="fee">Fee to be paid, in XRP. If omitted, current fee will be calculated.</param>
        /// <param name="rippling">Should be true, if an account is the issuer of assets.</param>
        /// <param name="requireDestinationTag">Should be true, if an account should support off-chain processing.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> ModifyAccountSettings(
            string fromAccount,
            string fromSecret = null,
            string signatureId = null,
            string fee = null,
            bool rippling = true,
            bool requireDestinationTag = true,
            CancellationToken ct = default) => ModifyAccountSettingsAsync(fromAccount, fromSecret, signatureId, fee, rippling, requireDestinationTag, ct).Result;
        /// <summary>
        /// <b>Title:</b> Modify XRP account<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Modify XRP account settings. If an XRP account should be an issuer of the custom asset, this accounts should have rippling enabled to true. In order to support off-chain processing, required destination tag should be set on the account.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="fromAccount">XRP account address. Must be the one used for generating deposit tags.</param>
        /// <param name="fromSecret">Secret for account. Secret, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Secret or signature Id must be present.</param>
        /// <param name="fee">Fee to be paid, in XRP. If omitted, current fee will be calculated.</param>
        /// <param name="rippling">Should be true, if an account is the issuer of assets.</param>
        /// <param name="requireDestinationTag">Should be true, if an account should support off-chain processing.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> ModifyAccountSettingsAsync(
            string fromAccount,
            string fromSecret = null,
            string signatureId = null,
            string fee = null,
            bool rippling = true,
            bool requireDestinationTag = true,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "fromAccount", fromAccount },
                { "rippling", rippling },
                { "requireDestinationTag", requireDestinationTag },
            };
            parameters.AddOptionalParameter("fromSecret", fromSecret);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("fee", fee);

            var credits = 10;
            var url = Tatum.GetUrl(string.Format(Endpoints_AccountSettings));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Broadcast signed XRP transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed transaction to XRP blockchain. This method is used internally from Tatum KMS, Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="signatureId">ID of prepared payment template to sign. Required only, when broadcasting transaction signed by Tatum KMS.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> Broadcast(string txData, string signatureId = null, CancellationToken ct = default) => BroadcastAsync(txData, signatureId, ct).Result;
        /// <summary>
        /// <b>Title:</b> Broadcast signed XRP transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed transaction to XRP blockchain. This method is used internally from Tatum KMS, Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
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

        #endregion


    }
}
