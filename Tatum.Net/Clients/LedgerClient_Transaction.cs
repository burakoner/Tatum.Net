using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.Converters;
using Tatum.Net.Enums;
using Tatum.Net.Helpers;
using Tatum.Net.Interfaces;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Clients
{
    public class LedgerTransactionClient : ITatumLedgerTransactionClient
    {
        public LedgerClient Ledger { get; protected set; }

        #region API Endpoints

        #region Ledger Transaction
        protected const string Endpoints_SendPayment = "ledger/transaction";
        protected const string Endpoints_GetTransactionsByAccount = "ledger/transaction/account";
        protected const string Endpoints_GetTransactionsByCustomer = "ledger/transaction/customer";
        protected const string Endpoints_GetTransactionsByLedger = "ledger/transaction/ledger";
        protected const string Endpoints_GetTransactionsByReference = "ledger/transaction/reference/{0}";
        #endregion

        #endregion

        public LedgerTransactionClient(LedgerClient ledgerClient)
        {
            Ledger = ledgerClient;
        }

        #region Ledger / Transaction
        /// <summary>
        /// <b>Title:</b> Send payment<br />
        /// <b>Credits:</b> 4 credits per API call.<br />
        /// <b>Description:</b>
        /// Send payment within the Tatum's ledger. All assets are settled instantly.
        /// When transaction is settled, 2 transaction records are created, 1 for each of the participants.These 2 records are connected together via transaction reference, which is the same for both of them.
        /// This method is used only for transferring assets between Tatum's accounts and will not send any funds to blockchain addresses.
        /// If there is insufficient balance on sender account, no transactions are stored.
        /// It is possible to perform an anonymous transaction - sender account is not visible for the recipient.
        /// Every transaction has it's value in the FIAT currency calculated automatically. FIAT value is based on the accountingCurrency of the account connected to the transaction and is available in the marketValue parameter of the transaction.
        /// </summary>
        /// <param name="senderAccountId">Internal sender account ID within Tatum platform</param>
        /// <param name="recipientAccountId">Internal recipient account ID within Tatum platform</param>
        /// <param name="amount">Amount to be sent.</param>
        /// <param name="anonymous">Anonymous transaction does not show sender account to recipient, default is false</param>
        /// <param name="compliant">Enable compliant checks. Transaction will not be processed, if compliant check fails.</param>
        /// <param name="transactionCode">For bookkeeping to distinct transaction purpose.</param>
        /// <param name="paymentId">Payment ID, External identifier of the payment, which can be used to pair transactions within Tatum accounts.</param>
        /// <param name="recipientNote">Note visible to both, sender and recipient</param>
        /// <param name="baseRate">Exchange rate of the base pair. Only applicable for Tatum's Virtual currencies Ledger transactions. Override default exchange rate for the Virtual Currency.</param>
        /// <param name="senderNote">Note visible to sender</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TatumReference> SendPayment(
            string senderAccountId,
            string recipientAccountId,
            decimal amount,
            bool anonymous = false,
            bool? compliant = null,
            string transactionCode = null,
            string paymentId = null,
            string recipientNote = null,
            decimal baseRate = 1.0m,
            string senderNote = null,
            CancellationToken ct = default)
            => SendPaymentAsync(senderAccountId, recipientAccountId, amount, anonymous, compliant, transactionCode, paymentId, recipientNote, baseRate, senderNote, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send payment<br />
        /// <b>Credits:</b> 4 credits per API call.<br />
        /// <b>Description:</b>
        /// Send payment within the Tatum's ledger. All assets are settled instantly.
        /// When transaction is settled, 2 transaction records are created, 1 for each of the participants.These 2 records are connected together via transaction reference, which is the same for both of them.
        /// This method is used only for transferring assets between Tatum's accounts and will not send any funds to blockchain addresses.
        /// If there is insufficient balance on sender account, no transactions are stored.
        /// It is possible to perform an anonymous transaction - sender account is not visible for the recipient.
        /// Every transaction has it's value in the FIAT currency calculated automatically. FIAT value is based on the accountingCurrency of the account connected to the transaction and is available in the marketValue parameter of the transaction.
        /// </summary>
        /// <param name="senderAccountId">Internal sender account ID within Tatum platform</param>
        /// <param name="recipientAccountId">Internal recipient account ID within Tatum platform</param>
        /// <param name="amount">Amount to be sent.</param>
        /// <param name="anonymous">Anonymous transaction does not show sender account to recipient, default is false</param>
        /// <param name="compliant">Enable compliant checks. Transaction will not be processed, if compliant check fails.</param>
        /// <param name="transactionCode">For bookkeeping to distinct transaction purpose.</param>
        /// <param name="paymentId">Payment ID, External identifier of the payment, which can be used to pair transactions within Tatum accounts.</param>
        /// <param name="recipientNote">Note visible to both, sender and recipient</param>
        /// <param name="baseRate">Exchange rate of the base pair. Only applicable for Tatum's Virtual currencies Ledger transactions. Override default exchange rate for the Virtual Currency.</param>
        /// <param name="senderNote">Note visible to sender</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TatumReference>> SendPaymentAsync(
            string senderAccountId,
            string recipientAccountId,
            decimal amount,
            bool anonymous = false,
            bool? compliant = null,
            string transactionCode = null,
            string paymentId = null,
            string recipientNote = null,
            decimal baseRate = 1.0m,
            string senderNote = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "senderAccountId", senderAccountId },
                { "recipientAccountId", recipientAccountId },
                { "amount", amount.ToString(CultureInfo.InvariantCulture) },
                { "anonymous", anonymous },
                { "baseRate", baseRate },
            };
            parameters.AddOptionalParameter("compliant", compliant);
            parameters.AddOptionalParameter("transactionCode", transactionCode);
            parameters.AddOptionalParameter("paymentId", paymentId);
            parameters.AddOptionalParameter("recipientNote", recipientNote);
            parameters.AddOptionalParameter("senderNote", senderNote);

            var credits = 4;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_SendPayment));
            return await Ledger.Tatum.SendTatumRequest<TatumReference>(url, HttpMethod.Post, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Find transactions for account.<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Find transactions for account identified by given account id.
        /// </summary>
        /// <param name="id">Account ID - source of transaction(s).</param>
        /// <param name="counterAccount">Counter account - transaction(s) destination account.</param>
        /// <param name="from">Starting date to search for transactions from in UTC millis. If not present, search all history.</param>
        /// <param name="to">Date until to search for transactions in UTC millis. If not present, search up till now.</param>
        /// <param name="currency">Currency of the transactions.</param>
        /// <param name="transactionType">Type of payment</param>
        /// <param name="opType">Type of operation</param>
        /// <param name="transactionCode">For bookkeeping to distinct transaction purpose.</param>
        /// <param name="paymentId">Payment ID defined in payment order by sender.</param>
        /// <param name="recipientNote">Recipient note defined in payment order by sender.</param>
        /// <param name="senderNote">Sender note defined in payment order by sender.</param>
        /// <param name="pageSize">Max number of items per page is 50. Either count, or pageSize is accepted.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="count">Get total count of transactions based on the filter. Either count, or pageSize is accepted.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<LedgerTransaction>> GetTransactionsByAccount(
            string id,
            string counterAccount = null,
            DateTime? from = null,
            DateTime? to = null,
            string currency = null,
            LedgerTransactionType? transactionType = null,
            LedgerOperationType? opType = null,
            string transactionCode = null,
            string paymentId = null,
            string recipientNote = null,
            string senderNote = null,
            int pageSize = 50,
            int offset = 0,
            bool? count = null,
            CancellationToken ct = default)
            => GetTransactionsByAccountAsync(id, counterAccount, from, to, currency, transactionType, opType, transactionCode, paymentId, recipientNote, senderNote, pageSize, offset, count, ct).Result;
        /// <summary>
        /// <b>Title:</b> Find transactions for account.<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Find transactions for account identified by given account id.
        /// </summary>
        /// <param name="id">Account ID - source of transaction(s).</param>
        /// <param name="counterAccount">Counter account - transaction(s) destination account.</param>
        /// <param name="from">Starting date to search for transactions from in UTC millis. If not present, search all history.</param>
        /// <param name="to">Date until to search for transactions in UTC millis. If not present, search up till now.</param>
        /// <param name="currency">Currency of the transactions.</param>
        /// <param name="transactionType">Type of payment</param>
        /// <param name="opType">Type of operation</param>
        /// <param name="transactionCode">For bookkeeping to distinct transaction purpose.</param>
        /// <param name="paymentId">Payment ID defined in payment order by sender.</param>
        /// <param name="recipientNote">Recipient note defined in payment order by sender.</param>
        /// <param name="senderNote">Sender note defined in payment order by sender.</param>
        /// <param name="pageSize">Max number of items per page is 50. Either count, or pageSize is accepted.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="count">Get total count of transactions based on the filter. Either count, or pageSize is accepted.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<LedgerTransaction>>> GetTransactionsByAccountAsync(
            string id,
            string counterAccount = null,
            DateTime? from = null,
            DateTime? to = null,
            string currency = null,
            LedgerTransactionType? transactionType = null,
            LedgerOperationType? opType = null,
            string transactionCode = null,
            string paymentId = null,
            string recipientNote = null,
            string senderNote = null,
            int pageSize = 50,
            int offset = 0,
            bool? count = null,
            CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);

            var parameters = new Dictionary<string, object>
            {
                { "id", id },
            };
            parameters.AddOptionalParameter("counterAccount", counterAccount);
            parameters.AddOptionalParameter("from", from?.ToUnixTimeMilliseconds());
            parameters.AddOptionalParameter("to", to?.ToUnixTimeMilliseconds());
            parameters.AddOptionalParameter("currency", currency);
            if (transactionType.HasValue)
                parameters.AddOptionalParameter("transactionType", JsonConvert.SerializeObject(transactionType, new LedgerTransactionTypeConverter(false)));
            if (opType.HasValue)
                parameters.AddOptionalParameter("opType", JsonConvert.SerializeObject(opType, new LedgerOperationTypeConverter(false)));
            parameters.AddOptionalParameter("transactionCode", transactionCode);
            parameters.AddOptionalParameter("paymentId", paymentId);
            parameters.AddOptionalParameter("recipientNote", recipientNote);
            parameters.AddOptionalParameter("senderNote", senderNote);

            var credits = 1;
            var qs = $"?pageSize={pageSize}&offset{offset}";
            if (count.HasValue) qs += $"&count={(count.Value ? "true" : "false")}";
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_GetTransactionsByAccount + qs));
            return await Ledger.Tatum.SendTatumRequest<IEnumerable<LedgerTransaction>>(url, HttpMethod.Post, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Find transactions for customer across all accounts of customer.<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Find transactions for all accounts of customer identified by given customer internal ID.
        /// </summary>
        /// <param name="id">Customer internal ID to search for.</param>
        /// <param name="account">Source account - source of transaction(s).</param>
        /// <param name="counterAccount">Counter account - transaction(s) destination account.</param>
        /// <param name="from">Starting date to search for transactions from in UTC millis. If not present, search all history.</param>
        /// <param name="to">Date until to search for transactions in UTC millis. If not present, search up till now.</param>
        /// <param name="currency">Currency of the transactions.</param>
        /// <param name="transactionType">Type of payment.</param>
        /// <param name="opType">Type of operation.</param>
        /// <param name="transactionCode">For bookkeeping to distinct transaction purpose.</param>
        /// <param name="paymentId">Payment ID defined in payment order by sender.</param>
        /// <param name="recipientNote">Recipient note defined in payment order by sender.</param>
        /// <param name="senderNote">Sender note defined in payment order by sender.</param>
        /// <param name="pageSize">Max number of items per page is 50. Either count, or pageSize is accepted.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="count">Get total count of transactions based on the filter. Either count, or pageSize is accepted.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<LedgerTransaction>> GetTransactionsByCustomer(
            string id,
            string account = null,
            string counterAccount = null,
            DateTime? from = null,
            DateTime? to = null,
            string currency = null,
            LedgerTransactionType? transactionType = null,
            LedgerOperationType? opType = null,
            string transactionCode = null,
            string paymentId = null,
            string recipientNote = null,
            string senderNote = null,
            int pageSize = 50,
            int offset = 0,
            bool? count = null,
            CancellationToken ct = default)
            => GetTransactionsByCustomerAsync(id, account, counterAccount, from, to, currency, transactionType, opType, transactionCode, paymentId, recipientNote, senderNote, pageSize, offset, count, ct).Result;
        /// <summary>
        /// <b>Title:</b> Find transactions for customer across all accounts of customer.<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Find transactions for all accounts of customer identified by given customer internal ID.
        /// </summary>
        /// <param name="id">Customer internal ID to search for.</param>
        /// <param name="account">Source account - source of transaction(s).</param>
        /// <param name="counterAccount">Counter account - transaction(s) destination account.</param>
        /// <param name="from">Starting date to search for transactions from in UTC millis. If not present, search all history.</param>
        /// <param name="to">Date until to search for transactions in UTC millis. If not present, search up till now.</param>
        /// <param name="currency">Currency of the transactions.</param>
        /// <param name="transactionType">Type of payment.</param>
        /// <param name="opType">Type of operation.</param>
        /// <param name="transactionCode">For bookkeeping to distinct transaction purpose.</param>
        /// <param name="paymentId">Payment ID defined in payment order by sender.</param>
        /// <param name="recipientNote">Recipient note defined in payment order by sender.</param>
        /// <param name="senderNote">Sender note defined in payment order by sender.</param>
        /// <param name="pageSize">Max number of items per page is 50. Either count, or pageSize is accepted.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="count">Get total count of transactions based on the filter. Either count, or pageSize is accepted.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<LedgerTransaction>>> GetTransactionsByCustomerAsync(
            string id,
            string account = null,
            string counterAccount = null,
            DateTime? from = null,
            DateTime? to = null,
            string currency = null,
            LedgerTransactionType? transactionType = null,
            LedgerOperationType? opType = null,
            string transactionCode = null,
            string paymentId = null,
            string recipientNote = null,
            string senderNote = null,
            int pageSize = 50,
            int offset = 0,
            bool? count = null,
            CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);

            var parameters = new Dictionary<string, object>
            {
                { "id", id },
            };
            parameters.AddOptionalParameter("account", account);
            parameters.AddOptionalParameter("counterAccount", counterAccount);
            parameters.AddOptionalParameter("from", from?.ToUnixTimeMilliseconds());
            parameters.AddOptionalParameter("to", to?.ToUnixTimeMilliseconds());
            parameters.AddOptionalParameter("currency", currency);
            if (transactionType.HasValue)
                parameters.AddOptionalParameter("transactionType", JsonConvert.SerializeObject(transactionType, new LedgerTransactionTypeConverter(false)));
            if (opType.HasValue)
                parameters.AddOptionalParameter("opType", JsonConvert.SerializeObject(opType, new LedgerOperationTypeConverter(false)));
            parameters.AddOptionalParameter("transactionCode", transactionCode);
            parameters.AddOptionalParameter("paymentId", paymentId);
            parameters.AddOptionalParameter("recipientNote", recipientNote);
            parameters.AddOptionalParameter("senderNote", senderNote);

            var credits = 1;
            var qs = $"?pageSize={pageSize}&offset{offset}";
            if (count.HasValue) qs += $"&count={(count.Value ? "true" : "false")}";
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_GetTransactionsByCustomer + qs));
            return await Ledger.Tatum.SendTatumRequest<IEnumerable<LedgerTransaction>>(url, HttpMethod.Post, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Find transactions for ledger.<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Find transactions across whole ledger.
        /// </summary>
        /// <param name="account">Source account - source of transaction(s).</param>
        /// <param name="counterAccount">Counter account - transaction(s) destination account.</param>
        /// <param name="from">Starting date to search for transactions from in UTC millis. If not present, search all history.</param>
        /// <param name="to">Date until to search for transactions in UTC millis. If not present, search up till now.</param>
        /// <param name="currency">Currency of the transactions.</param>
        /// <param name="transactionType">Type of payment.</param>
        /// <param name="opType">Type of operation.</param>
        /// <param name="transactionCode">For bookkeeping to distinct transaction purpose.</param>
        /// <param name="paymentId">Payment ID defined in payment order by sender.</param>
        /// <param name="recipientNote">Recipient note defined in payment order by sender.</param>
        /// <param name="senderNote">Sender note defined in payment order by sender.</param>
        /// <param name="pageSize">Max number of items per page is 50. Either count, or pageSize is accepted.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="count">Get total count of transactions based on the filter. Either count, or pageSize is accepted.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<LedgerTransaction>> GetTransactionsByLedger(
            string account = null,
            string counterAccount = null,
            DateTime? from = null,
            DateTime? to = null,
            string currency = null,
            LedgerTransactionType? transactionType = null,
            LedgerOperationType? opType = null,
            string transactionCode = null,
            string paymentId = null,
            string recipientNote = null,
            string senderNote = null,
            int pageSize = 50,
            int offset = 0,
            bool? count = null,
            CancellationToken ct = default)
            => GetTransactionsByLedgerAsync(account, counterAccount, from, to, currency, transactionType, opType, transactionCode, paymentId, recipientNote, senderNote, pageSize, offset, count, ct).Result;
        /// <summary>
        /// <b>Title:</b> Find transactions for ledger.<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Find transactions across whole ledger.
        /// </summary>
        /// <param name="account">Source account - source of transaction(s).</param>
        /// <param name="counterAccount">Counter account - transaction(s) destination account.</param>
        /// <param name="from">Starting date to search for transactions from in UTC millis. If not present, search all history.</param>
        /// <param name="to">Date until to search for transactions in UTC millis. If not present, search up till now.</param>
        /// <param name="currency">Currency of the transactions.</param>
        /// <param name="transactionType">Type of payment.</param>
        /// <param name="opType">Type of operation.</param>
        /// <param name="transactionCode">For bookkeeping to distinct transaction purpose.</param>
        /// <param name="paymentId">Payment ID defined in payment order by sender.</param>
        /// <param name="recipientNote">Recipient note defined in payment order by sender.</param>
        /// <param name="senderNote">Sender note defined in payment order by sender.</param>
        /// <param name="pageSize">Max number of items per page is 50. Either count, or pageSize is accepted.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="count">Get total count of transactions based on the filter. Either count, or pageSize is accepted.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<LedgerTransaction>>> GetTransactionsByLedgerAsync(
            string account = null,
            string counterAccount = null,
            DateTime? from = null,
            DateTime? to = null,
            string currency = null,
            LedgerTransactionType? transactionType = null,
            LedgerOperationType? opType = null,
            string transactionCode = null,
            string paymentId = null,
            string recipientNote = null,
            string senderNote = null,
            int pageSize = 50,
            int offset = 0,
            bool? count = null,
            CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("account", account);
            parameters.AddOptionalParameter("counterAccount", counterAccount);
            parameters.AddOptionalParameter("from", from?.ToUnixTimeMilliseconds());
            parameters.AddOptionalParameter("to", to?.ToUnixTimeMilliseconds());
            parameters.AddOptionalParameter("currency", currency);
            if (transactionType.HasValue)
                parameters.AddOptionalParameter("transactionType", JsonConvert.SerializeObject(transactionType, new LedgerTransactionTypeConverter(false)));
            if (opType.HasValue)
                parameters.AddOptionalParameter("opType", JsonConvert.SerializeObject(opType, new LedgerOperationTypeConverter(false)));
            parameters.AddOptionalParameter("transactionCode", transactionCode);
            parameters.AddOptionalParameter("paymentId", paymentId);
            parameters.AddOptionalParameter("recipientNote", recipientNote);
            parameters.AddOptionalParameter("senderNote", senderNote);

            var credits = 1;
            var qs = $"?pageSize={pageSize}&offset{offset}";
            if (count.HasValue) qs += $"&count={(count.Value ? "true" : "false")}";
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_GetTransactionsByLedger + qs));
            return await Ledger.Tatum.SendTatumRequest<IEnumerable<LedgerTransaction>>(url, HttpMethod.Post, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Find transactions with given reference across all accounts.<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Find transactions for all accounts with given reference.
        /// </summary>
        /// <param name="reference">reference</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<LedgerTransaction>> GetTransactionsByReference(string reference, CancellationToken ct = default) => GetTransactionsByReferenceAsync(reference, ct).Result;
        /// <summary>
        /// <b>Title:</b> Find transactions with given reference across all accounts.<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Find transactions for all accounts with given reference.
        /// </summary>
        /// <param name="reference">reference</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<LedgerTransaction>>> GetTransactionsByReferenceAsync(string reference, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_GetTransactionsByReference, reference));
            return await Ledger.Tatum.SendTatumRequest<IEnumerable<LedgerTransaction>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }
        #endregion

    }
}
