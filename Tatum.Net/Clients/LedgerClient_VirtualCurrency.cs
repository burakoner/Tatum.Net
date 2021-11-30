using CryptoExchange.Net;
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
    public class LedgerVirtualCurrencyClient : ITatumLedgerVirtualCurrencyClient
    {
        public LedgerClient Ledger { get; protected set; }

        #region API Endpoints

        #region Ledger Virtual Currency
        protected const string Endpoints_Create = "ledger/virtualCurrency";
        protected const string Endpoints_Update = "ledger/virtualCurrency";
        protected const string Endpoints_Get = "ledger/virtualCurrency/{0}";
        protected const string Endpoints_Mint = "ledger/virtualCurrency/mint";
        protected const string Endpoints_Destroy = "ledger/virtualCurrency/revoke";
        #endregion

        #endregion

        public LedgerVirtualCurrencyClient(LedgerClient ledgerClient)
        {
            Ledger = ledgerClient;
        }


        #region Ledger / Virtual Currency
        /// <summary>
        /// <b>Title:</b> Create new virtual currency<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create new virtual currency with given supply stored in account. This will create Tatum internal virtual currency. Every virtual currency must be prefixed with VC_.
        /// Every virtual currency must be pegged to existing FIAT or supported cryptocurrency. 1 unit of virtual currency has then the same amount as 1 unit of the base currency it is pegged to.It is possible to set a custom base rate for the virtual currency. (baseRate = 2 => 1 VC unit = 2 basePair units)
        /// Tatum virtual currency acts as any other asset within Tatum.For creation of ERC20 token, see ERC20.
        /// This operation returns the newly created Tatum Ledger account with an initial balance set to the virtual currency's total supply. Total supply can be changed in the future.
        /// </summary>
        /// <param name="name">Virtual currency name. Must be prefixed with 'VC_'.</param>
        /// <param name="supply">Supply of virtual currency.</param>
        /// <param name="basePair">Base pair for virtual currency. Transaction value will be calculated according to this base pair. e.g. 1 VC_VIRTUAL is equal to 1 BTC, if basePair is set to BTC.</param>
        /// <param name="baseRate">Exchange rate of the base pair. Each unit of the created curency will represent value of baseRate*1 basePair.</param>
        /// <param name="customer">If customer is filled then is created or updated.</param>
        /// <param name="description">Used as a description within Tatum system.</param>
        /// <param name="accountCode">For bookkeeping to distinct account purpose.</param>
        /// <param name="accountNumber">Account number from external system.</param>
        /// <param name="accountingCurrency">All transaction will be billed in this currency for created account associated with this currency. If not set, EUR is used. ISO-4217</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<LedgerReport> Create(
            string name,
            string supply,
            string basePair,
            decimal baseRate = 1.0m,
            LedgerCustomerOptions customer = null,
            string description = null,
            string accountCode = null,
            string accountNumber = null,
            string accountingCurrency = null,
            CancellationToken ct = default)
            => CreateAsync(name, supply, basePair, baseRate, customer, description, accountCode, accountNumber, accountingCurrency, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create new virtual currency<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create new virtual currency with given supply stored in account. This will create Tatum internal virtual currency. Every virtual currency must be prefixed with VC_.
        /// Every virtual currency must be pegged to existing FIAT or supported cryptocurrency. 1 unit of virtual currency has then the same amount as 1 unit of the base currency it is pegged to.It is possible to set a custom base rate for the virtual currency. (baseRate = 2 => 1 VC unit = 2 basePair units)
        /// Tatum virtual currency acts as any other asset within Tatum.For creation of ERC20 token, see ERC20.
        /// This operation returns the newly created Tatum Ledger account with an initial balance set to the virtual currency's total supply. Total supply can be changed in the future.
        /// </summary>
        /// <param name="name">Virtual currency name. Must be prefixed with 'VC_'.</param>
        /// <param name="supply">Supply of virtual currency.</param>
        /// <param name="basePair">Base pair for virtual currency. Transaction value will be calculated according to this base pair. e.g. 1 VC_VIRTUAL is equal to 1 BTC, if basePair is set to BTC.</param>
        /// <param name="baseRate">Exchange rate of the base pair. Each unit of the created curency will represent value of baseRate*1 basePair.</param>
        /// <param name="customer">If customer is filled then is created or updated.</param>
        /// <param name="description">Used as a description within Tatum system.</param>
        /// <param name="accountCode">For bookkeeping to distinct account purpose.</param>
        /// <param name="accountNumber">Account number from external system.</param>
        /// <param name="accountingCurrency">All transaction will be billed in this currency for created account associated with this currency. If not set, EUR is used. ISO-4217</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<LedgerReport>> CreateAsync(
            string name,
            string supply,
            string basePair,
            decimal baseRate = 1.0m,
            LedgerCustomerOptions customer = null,
            string description = null,
            string accountCode = null,
            string accountNumber = null,
            string accountingCurrency = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "name", name },
                { "supply", supply },
                { "basePair", basePair },
                { "baseRate", baseRate },
            };
            parameters.AddOptionalParameter("customer", customer);
            parameters.AddOptionalParameter("description", description);
            parameters.AddOptionalParameter("accountCode", accountCode);
            parameters.AddOptionalParameter("accountNumber", accountNumber);
            parameters.AddOptionalParameter("accountingCurrency", accountingCurrency);

            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Create));
            return await Ledger.Tatum.SendTatumRequest<LedgerReport>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Update virtual currency<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Change base pair and/or base rate of existing virtual currency.
        /// </summary>
        /// <param name="name">Virtual currency name, which will be updated. It is not possible to update the name of the virtual currency.</param>
        /// <param name="basePair">Exchange rate of the base pair. Each unit of the created curency will represent value of baseRate*1 basePair.</param>
        /// <param name="baseRate">Base pair for virtual currency. Transaction value will be calculated according to this base pair. e.g. 1 VC_VIRTUAL is equal to 1 BTC, if basePair is set to BTC.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> Update(string name, string basePair = null, decimal? baseRate = null, CancellationToken ct = default) => UpdateAsync(name, basePair, baseRate, ct).Result;
        /// <summary>
        /// <b>Title:</b> Update virtual currency<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Change base pair and/or base rate of existing virtual currency.
        /// </summary>
        /// <param name="name">Virtual currency name, which will be updated. It is not possible to update the name of the virtual currency.</param>
        /// <param name="basePair">Exchange rate of the base pair. Each unit of the created curency will represent value of baseRate*1 basePair.</param>
        /// <param name="baseRate">Base pair for virtual currency. Transaction value will be calculated according to this base pair. e.g. 1 VC_VIRTUAL is equal to 1 BTC, if basePair is set to BTC.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> UpdateAsync(string name, string basePair = null, decimal? baseRate = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "name", name },
            };
            parameters.AddOptionalParameter("basePair", basePair);
            parameters.AddOptionalParameter("baseRate", baseRate);

            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Update));
            var result = await Ledger.Tatum.SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Get virtual currency<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get detail of virtual currency.
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<LedgerReport> Get(string name, CancellationToken ct = default) => GetAsync(name, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get virtual currency<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get detail of virtual currency.
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<LedgerReport>> GetAsync(string name, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Get, name));
            return await Ledger.Tatum.SendTatumRequest<LedgerReport>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Create new supply of virtual currency<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create new supply of virtual currency linked on the given accountId. Method increases the total supply of the currency.
        /// This method creates Ledger transaction with operationType MINT with undefined counterAccountId.
        /// </summary>
        /// <param name="accountId">Ledger account with currency of the virtual currency, on which the operation will be performed.</param>
        /// <param name="amount">Amount of virtual currency to operate within this operation.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="reference">Reference of the payment.</param>
        /// <param name="transactionCode">For bookkeeping to distinct transaction purpose.</param>
        /// <param name="recipientNote">Note visible to both, sender and recipient. Available for both Mint and Revoke operations</param>
        /// <param name="counterAccount">External account identifier. By default, there is no counterAccount present in the transaction.</param>
        /// <param name="senderNote">Note visible to sender. Available in Revoke operation.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TatumReference> Mint(
            string accountId,
            decimal amount,
            string paymentId = null,
            string reference = null,
            string transactionCode = null,
            string recipientNote = null,
            string counterAccount = null,
            string senderNote = null,
            CancellationToken ct = default)
            => MintAsync(accountId, amount, paymentId, reference, transactionCode, recipientNote, counterAccount, senderNote, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create new supply of virtual currency<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create new supply of virtual currency linked on the given accountId. Method increases the total supply of the currency.
        /// This method creates Ledger transaction with operationType MINT with undefined counterAccountId.
        /// </summary>
        /// <param name="accountId">Ledger account with currency of the virtual currency, on which the operation will be performed.</param>
        /// <param name="amount">Amount of virtual currency to operate within this operation.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="reference">Reference of the payment.</param>
        /// <param name="transactionCode">For bookkeeping to distinct transaction purpose.</param>
        /// <param name="recipientNote">Note visible to both, sender and recipient. Available for both Mint and Revoke operations</param>
        /// <param name="counterAccount">External account identifier. By default, there is no counterAccount present in the transaction.</param>
        /// <param name="senderNote">Note visible to sender. Available in Revoke operation.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TatumReference>> MintAsync(
            string accountId,
            decimal amount,
            string paymentId = null,
            string reference = null,
            string transactionCode = null,
            string recipientNote = null,
            string counterAccount = null,
            string senderNote = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "accountId", accountId },
                { "amount", amount.ToString(CultureInfo.InvariantCulture) },
            };
            parameters.AddOptionalParameter("paymentId", paymentId);
            parameters.AddOptionalParameter("reference", reference);
            parameters.AddOptionalParameter("transactionCode", transactionCode);
            parameters.AddOptionalParameter("recipientNote", recipientNote);
            parameters.AddOptionalParameter("counterAccount", counterAccount);
            parameters.AddOptionalParameter("senderNote", senderNote);

            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Mint));
            return await Ledger.Tatum.SendTatumRequest<TatumReference>(url, HttpMethod.Put, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Destroy supply of virtual currency<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Destroy supply of virtual currency linked on the given accountId. Method decreases the total supply of the currency.
        /// This method creates Ledger transaction with operationType REVOKE with undefined counterAccountId.
        /// </summary>
        /// <param name="accountId">Ledger account with currency of the virtual currency, on which the operation will be performed.</param>
        /// <param name="amount">Amount of virtual currency to operate within this operation.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="reference">Reference of the payment.</param>
        /// <param name="transactionCode">For bookkeeping to distinct transaction purpose.</param>
        /// <param name="recipientNote">Note visible to both, sender and recipient. Available for both Mint and Revoke operations</param>
        /// <param name="counterAccount">External account identifier. By default, there is no counterAccount present in the transaction.</param>
        /// <param name="senderNote">Note visible to sender. Available in Revoke operation.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TatumReference> Destroy(
            string accountId,
            decimal amount,
            string paymentId = null,
            string reference = null,
            string transactionCode = null,
            string recipientNote = null,
            string counterAccount = null,
            string senderNote = null,
            CancellationToken ct = default)
            => DestroyAsync(accountId, amount, paymentId, reference, transactionCode, recipientNote, counterAccount, senderNote, ct).Result;
        /// <summary>
        /// <b>Title:</b> Destroy supply of virtual currency<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Destroy supply of virtual currency linked on the given accountId. Method decreases the total supply of the currency.
        /// This method creates Ledger transaction with operationType REVOKE with undefined counterAccountId.
        /// </summary>
        /// <param name="accountId">Ledger account with currency of the virtual currency, on which the operation will be performed.</param>
        /// <param name="amount">Amount of virtual currency to operate within this operation.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="reference">Reference of the payment.</param>
        /// <param name="transactionCode">For bookkeeping to distinct transaction purpose.</param>
        /// <param name="recipientNote">Note visible to both, sender and recipient. Available for both Mint and Revoke operations</param>
        /// <param name="counterAccount">External account identifier. By default, there is no counterAccount present in the transaction.</param>
        /// <param name="senderNote">Note visible to sender. Available in Revoke operation.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TatumReference>> DestroyAsync(
            string accountId,
            decimal amount,
            string paymentId = null,
            string reference = null,
            string transactionCode = null,
            string recipientNote = null,
            string counterAccount = null,
            string senderNote = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "accountId", accountId },
                { "amount", amount.ToString(CultureInfo.InvariantCulture) },
            };
            parameters.AddOptionalParameter("paymentId", paymentId);
            parameters.AddOptionalParameter("reference", reference);
            parameters.AddOptionalParameter("transactionCode", transactionCode);
            parameters.AddOptionalParameter("recipientNote", recipientNote);
            parameters.AddOptionalParameter("counterAccount", counterAccount);
            parameters.AddOptionalParameter("senderNote", senderNote);

            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Destroy));
            return await Ledger.Tatum.SendTatumRequest<TatumReference>(url, HttpMethod.Put, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }
        #endregion


    }
}
