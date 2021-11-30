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
    public class LedgerCustomerClient : ITatumLedgerCustomerClient
    {
        public LedgerClient Ledger { get; protected set; }

        #region API Endpoints

        #region Ledger Customer
        protected const string Endpoints_List = "ledger/customer";
        protected const string Endpoints_Get = "ledger/customer/{0}";
        protected const string Endpoints_Update = "ledger/customer/{0}";
        protected const string Endpoints_Activate = "ledger/customer/{0}/activate";
        protected const string Endpoints_Deactivate = "ledger/customer/{0}/deactivate";
        protected const string Endpoints_Enable = "ledger/customer/{0}/enable";
        protected const string Endpoints_Disable = "ledger/customer/{0}/disable";
        #endregion

        #endregion

        public LedgerCustomerClient(LedgerClient ledgerClient)
        {
            Ledger = ledgerClient;
        }

        #region Ledger / Customer
        /// <summary>
        /// <b>Title:</b> List all customers<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// List of all customers. Also inactive an disabled customers are present.
        /// </summary>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<LedgerCustomer>> ListAll(int pageSize = 50, int offset = 0, CancellationToken ct = default) => ListAllAsync(pageSize, offset, ct).Result;
        /// <summary>
        /// <b>Title:</b> List all customers<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// List of all customers. Also inactive an disabled customers are present.
        /// </summary>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<LedgerCustomer>>> ListAllAsync(int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);
            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };

            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_List));
            return await Ledger.Tatum.SendTatumRequest<IEnumerable<LedgerCustomer>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get customer details<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Using anonymized external ID or internal customer ID you can access customer detail information. Internal ID is needed to call other customer related methods.
        /// </summary>
        /// <param name="id">Customer external or internal ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<LedgerCustomer> Get(string id, CancellationToken ct = default) => GetAsync(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get customer details<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Using anonymized external ID or internal customer ID you can access customer detail information. Internal ID is needed to call other customer related methods.
        /// </summary>
        /// <param name="id">Customer external or internal ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<LedgerCustomer>> GetAsync(string id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Get, id));
            return await Ledger.Tatum.SendTatumRequest<LedgerCustomer>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Update customer<br />
        /// <b>Credits:</b> 2 credit per API call.<br />
        /// <b>Description:</b>
        /// This method is helpful in case your primary system will change ID's or customer will change the country he/she is supposed to be in compliance with.
        /// </summary>
        /// <param name="id">Customer internal ID</param>
        /// <param name="externalId">External customer ID. If not set, it will not be updated.</param>
        /// <param name="accountingCurrency">All transaction will be accounted in this currency for all accounts. Currency can be overridden per account level. If not set, it will not be updated. ISO-4217</param>
        /// <param name="customerCountry">Country customer has to be compliant with. If not set, it will not be updated. ISO-3166-1.</param>
        /// <param name="providerCountry">Country service provider has to be compliant with. If not set, it will not be updated. ISO-3166-1</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<LedgerCustomer> Update(string id, string externalId, string accountingCurrency = null, string customerCountry = null, string providerCountry = null, CancellationToken ct = default) => UpdateAsync(id, externalId, accountingCurrency, customerCountry, providerCountry, ct).Result;
        /// <summary>
        /// <b>Title:</b> Update customer<br />
        /// <b>Credits:</b> 2 credit per API call.<br />
        /// <b>Description:</b>
        /// This method is helpful in case your primary system will change ID's or customer will change the country he/she is supposed to be in compliance with.
        /// </summary>
        /// <param name="id">Customer internal ID</param>
        /// <param name="externalId">External customer ID. If not set, it will not be updated.</param>
        /// <param name="accountingCurrency">All transaction will be accounted in this currency for all accounts. Currency can be overridden per account level. If not set, it will not be updated. ISO-4217</param>
        /// <param name="customerCountry">Country customer has to be compliant with. If not set, it will not be updated. ISO-3166-1.</param>
        /// <param name="providerCountry">Country service provider has to be compliant with. If not set, it will not be updated. ISO-3166-1</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<LedgerCustomer>> UpdateAsync(string id, string externalId, string accountingCurrency = null, string customerCountry = null, string providerCountry = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "externalId", externalId },
            };
            parameters.AddOptionalParameter("accountingCurrency", accountingCurrency);
            parameters.AddOptionalParameter("customerCountry", customerCountry);
            parameters.AddOptionalParameter("providerCountry", providerCountry);

            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Update, id));
            return await Ledger.Tatum.SendTatumRequest<LedgerCustomer>(url, HttpMethod.Put, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Activate customer<br />
        /// <b>Credits:</b> 2 credit per API call.<br />
        /// <b>Description:</b>
        /// Activated customer is able to do any operation.
        /// </summary>
        /// <param name="id">Customer internal ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> Activate(string id, CancellationToken ct = default) => ActivateAsync(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Activate customer<br />
        /// <b>Credits:</b> 2 credit per API call.<br />
        /// <b>Description:</b>
        /// Activated customer is able to do any operation.
        /// </summary>
        /// <param name="id">Customer internal ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> ActivateAsync(string id, CancellationToken ct = default)
        {
            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Activate, id));
            var result = await Ledger.Tatum.SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Deactivate customer<br />
        /// <b>Credits:</b> 2 credit per API call.<br />
        /// <b>Description:</b>
        /// Deactivate customer is not able to do any operation. Customer can be deactivated only when all their accounts are already deactivated.
        /// </summary>
        /// <param name="id">Customer internal ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> Deactivate(string id, CancellationToken ct = default) => DeactivateAsync(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Deactivate customer<br />
        /// <b>Credits:</b> 2 credit per API call.<br />
        /// <b>Description:</b>
        /// Deactivate customer is not able to do any operation. Customer can be deactivated only when all their accounts are already deactivated.
        /// </summary>
        /// <param name="id">Customer internal ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> DeactivateAsync(string id, CancellationToken ct = default)
        {
            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Deactivate, id));
            var result = await Ledger.Tatum.SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Enable customer<br />
        /// <b>Credits:</b> 2 credit per API call.<br />
        /// <b>Description:</b>
        /// Enabled customer can perform all operations. By default all customers are enabled. All previously blocked account balances will be unblocked.
        /// </summary>
        /// <param name="id">Customer internal ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> Enable(string id, CancellationToken ct = default) => EnableAsync(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Enable customer<br />
        /// <b>Credits:</b> 2 credit per API call.<br />
        /// <b>Description:</b>
        /// Enabled customer can perform all operations. By default all customers are enabled. All previously blocked account balances will be unblocked.
        /// </summary>
        /// <param name="id">Customer internal ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> EnableAsync(string id, CancellationToken ct = default)
        {
            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Enable, id));
            var result = await Ledger.Tatum.SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Disable customer<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Disabled customer cannot perform end-user operations, such as create new accounts or send transactions. Available balance on all accounts is set to 0. Account balance will stay untouched.
        /// </summary>
        /// <param name="id">Customer internal ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> Disable(string id, CancellationToken ct = default) => DisableAsync(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Disable customer<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Disabled customer cannot perform end-user operations, such as create new accounts or send transactions. Available balance on all accounts is set to 0. Account balance will stay untouched.
        /// </summary>
        /// <param name="id">Customer internal ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> DisableAsync(string id, CancellationToken ct = default)
        {
            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Disable, id));
            var result = await Ledger.Tatum.SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }
        #endregion

    }
}
