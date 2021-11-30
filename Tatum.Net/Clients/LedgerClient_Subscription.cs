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
using Tatum.Net.Interfaces;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Clients
{
    public class LedgerSubscriptionClient : ITatumLedgerSubscriptionClient
    {
        public LedgerClient Ledger { get; protected set; }

        #region API Endpoints

        #region Ledger Subscription
        protected const string Endpoints_Create = "subscription";
        protected const string Endpoints_List = "subscription";
        protected const string Endpoints_Cancel = "subscription/{0}";
        protected const string Endpoints_Report = "subscription/report/{0}";
        #endregion

        #endregion

        public LedgerSubscriptionClient(LedgerClient ledgerClient)
        {
            Ledger = ledgerClient;
        }

        #region Ledger / Subscription
        /// <summary>
        /// <b>Title:</b> Create new subscription<br />
        /// <b>Credits:</b> 2 credits for API call. Every type of subscription has different credit pricing.<br />
        /// <b>Description:</b>
        /// Create new subscription. Currently Tatum support multiple subscription types:
        /// <b>OFFCHAIN_WITHDRAWAL</b> - Off-chain scanning of outgoing transactions for BTC, BCH, LTC and ETH blockchains - by default addresses in registered for off-chain scanning are synchronized only against incoming transactions.By enabling this feature, off-chain accounts with connected blockchain addresses will be scanned also for outgoing transactions. 5 credits will be debited for every address registered for scanning every day.
        /// <b>ACCOUNT_BALANCE_LIMIT</b> - Report with all account balances above desired limit.
        /// <b>TRANSACTION_HISTORY_REPORT</b> - Report with all ledger transactions for last X hours, where X is set by the subscription attribute as interval.Maximum number of transactions returned by this report is 20000. Transactions are obtained from the time of the invocation of the GET method to obtain report - X hours.
        /// <b>ACCOUNT_INCOMING_BLOCKCHAIN_TRANSACTION</b> - Enable HTTP POST JSON notifications on incoming blockchain transactions on off-chain accounts. This web hook will be invoked, when the transaction is credited to the ledger account.Transaction is credited, when it has sufficient amount of blockchain confirmations. For BTC, LTC, BCH - 2 confirmations, ETH, ERC20 tokens, XLM, XRP, BNB - 1 confirmation.Request body of the POST request will be JSON object with attributes:
        /// - id (account id)
        /// - currency (account currency)
        /// - amount
        /// - date
        /// - txId (hash of the blockchain transaction).
        /// 1 credit will be debited for every monitored account every day.
        /// <b>ACCOUNT_PENDING_BLOCKCHAIN_TRANSACTION</b> - Enable HTTP POST JSON notifications on incoming blockchain transactions on off-chain accounts.This web hook will be invoked, when the transaction appears for the first time in the block, but is stil not credited to the ledger account, because it does not have enough confirmations.This web hook is invoked only for BTC, LTC and BCH accounts. Request body of the POST request will be JSON object with attributes:
        /// - id (account id)
        /// - currency (account currency)
        /// - amount
        /// - date
        /// - txId (hash of the blockchain transaction).
        /// 1 credit will be debited for every monitored account every day.
        /// <b>COMPLETE_BLOCKCHAIN_TRANSACTION</b> - Enable HTTP POST JSON notifications when blockchain transactions are included in the block.Request body of the POST request will be JSON object with attributes:
        /// - currency (blockchain)
        /// - withdrawalId (present, if transactin was connected to the offchain withdrawal transaction)
        /// - txId(hash of the blockchain transaction).
        /// 20 credit will be debited for every monitored transaction.
        /// Result of the operation is subscription ID, which can be used to cancel subscription or obtain additional data connected to it like reports.
        /// </summary>
        /// <param name="type">Type of the subscription.</param>
        /// <param name="account_id">ID of the account, on which the webhook will be applied, when new incoming blockchain transaction will be credited.</param>
        /// <param name="url">URL of the endpoint, where HTTP POST request will be sent, when new incoming blockchain transaction will be credited.</param>
        /// <param name="currency">Currency of the accounts, on which outgoing off-chain scanning will be enabled. BTC, LTC, BCH, ETH and ERC20 tokens are allowed.</param>
        /// <param name="interval">Number of hours to obtain transactions for.</param>
        /// <param name="limit">Limit to filter accounts with balance above it.</param>
        /// <param name="typeOfBalance">Type of balance to filter.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TatumId> Create(
            LedgerSubscriptionType type,
            string account_id = null,
            string url = null,
            string currency = null,
            int? interval = null,
            decimal? limit = null,
            LedgerBalanceType? typeOfBalance = null,
            CancellationToken ct = default)
            => CreateAsync(type, account_id, url, currency, interval, limit, typeOfBalance, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create new subscription<br />
        /// <b>Credits:</b> 2 credits for API call. Every type of subscription has different credit pricing.<br />
        /// <b>Description:</b>
        /// Create new subscription. Currently Tatum support multiple subscription types:
        /// <b>OFFCHAIN_WITHDRAWAL</b> - Off-chain scanning of outgoing transactions for BTC, BCH, LTC and ETH blockchains - by default addresses in registered for off-chain scanning are synchronized only against incoming transactions.By enabling this feature, off-chain accounts with connected blockchain addresses will be scanned also for outgoing transactions. 5 credits will be debited for every address registered for scanning every day.
        /// <b>ACCOUNT_BALANCE_LIMIT</b> - Report with all account balances above desired limit.
        /// <b>TRANSACTION_HISTORY_REPORT</b> - Report with all ledger transactions for last X hours, where X is set by the subscription attribute as interval.Maximum number of transactions returned by this report is 20000. Transactions are obtained from the time of the invocation of the GET method to obtain report - X hours.
        /// <b>ACCOUNT_INCOMING_BLOCKCHAIN_TRANSACTION</b> - Enable HTTP POST JSON notifications on incoming blockchain transactions on off-chain accounts. This web hook will be invoked, when the transaction is credited to the ledger account.Transaction is credited, when it has sufficient amount of blockchain confirmations. For BTC, LTC, BCH - 2 confirmations, ETH, ERC20 tokens, XLM, XRP, BNB - 1 confirmation.Request body of the POST request will be JSON object with attributes:
        /// - id (account id)
        /// - currency (account currency)
        /// - amount
        /// - date
        /// - txId (hash of the blockchain transaction).
        /// 1 credit will be debited for every monitored account every day.
        /// <b>ACCOUNT_PENDING_BLOCKCHAIN_TRANSACTION</b> - Enable HTTP POST JSON notifications on incoming blockchain transactions on off-chain accounts.This web hook will be invoked, when the transaction appears for the first time in the block, but is stil not credited to the ledger account, because it does not have enough confirmations.This web hook is invoked only for BTC, LTC and BCH accounts. Request body of the POST request will be JSON object with attributes:
        /// - id (account id)
        /// - currency (account currency)
        /// - amount
        /// - date
        /// - txId (hash of the blockchain transaction).
        /// 1 credit will be debited for every monitored account every day.
        /// <b>COMPLETE_BLOCKCHAIN_TRANSACTION</b> - Enable HTTP POST JSON notifications when blockchain transactions are included in the block.Request body of the POST request will be JSON object with attributes:
        /// - currency (blockchain)
        /// - withdrawalId (present, if transactin was connected to the offchain withdrawal transaction)
        /// - txId(hash of the blockchain transaction).
        /// 20 credit will be debited for every monitored transaction.
        /// Result of the operation is subscription ID, which can be used to cancel subscription or obtain additional data connected to it like reports.
        /// </summary>
        /// <param name="type">Type of the subscription.</param>
        /// <param name="account_id">ID of the account, on which the webhook will be applied, when new incoming blockchain transaction will be credited.</param>
        /// <param name="url">URL of the endpoint, where HTTP POST request will be sent, when new incoming blockchain transaction will be credited.</param>
        /// <param name="currency">Currency of the accounts, on which outgoing off-chain scanning will be enabled. BTC, LTC, BCH, ETH and ERC20 tokens are allowed.</param>
        /// <param name="interval">Number of hours to obtain transactions for.</param>
        /// <param name="limit">Limit to filter accounts with balance above it.</param>
        /// <param name="typeOfBalance">Type of balance to filter.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TatumId>> CreateAsync(
            LedgerSubscriptionType type,
            string account_id = null,
            string url = null,
            string currency = null,
            int? interval = null,
            decimal? limit = null,
            LedgerBalanceType? typeOfBalance = null,
            CancellationToken ct = default)
        {
            var lsa = new LedgerSubscriptionAttributes();
            if (type == LedgerSubscriptionType.ACCOUNT_BALANCE_LIMIT)
            {
                if (limit == null || typeOfBalance == null)
                    throw new ArgumentException("limit and typeOfBalance parameters are mandatory for ACCOUNT_BALANCE_LIMIT subscription type");

                lsa.Limit = limit?.ToString(CultureInfo.InvariantCulture);
                lsa.TypeOfBalance = typeOfBalance;
            }
            else if (type == LedgerSubscriptionType.TRANSACTION_HISTORY_REPORT)
            {
                if (interval == null)
                    throw new ArgumentException("linterval parameter is mandatory for TRANSACTION_HISTORY_REPORT subscription type");

                lsa.Interval = interval;
            }
            else if (type == LedgerSubscriptionType.OFFCHAIN_WITHDRAWAL)
            {
                if (string.IsNullOrEmpty(currency))
                    throw new ArgumentException("currency parameter is mandatory for OFFCHAIN_WITHDRAWAL subscription type");

                lsa.Currency = currency;
            }
            else if (type == LedgerSubscriptionType.COMPLETE_BLOCKCHAIN_TRANSACTION)
            {
                if (string.IsNullOrEmpty(currency))
                    throw new ArgumentException("currency parameter is mandatory for COMPLETE_BLOCKCHAIN_TRANSACTION subscription type");

                lsa.Currency = currency;
            }
            else if (type == LedgerSubscriptionType.ACCOUNT_INCOMING_BLOCKCHAIN_TRANSACTION)
            {
                if (string.IsNullOrEmpty(account_id) || string.IsNullOrEmpty(url))
                    throw new ArgumentException("account_id and url parameters are mandatory for ACCOUNT_INCOMING_BLOCKCHAIN_TRANSACTION subscription type");

                lsa.AccountId = account_id;
                lsa.Url = url;
            }
            else if (type == LedgerSubscriptionType.ACCOUNT_PENDING_BLOCKCHAIN_TRANSACTION)
            {
                if (string.IsNullOrEmpty(account_id) || string.IsNullOrEmpty(url))
                    throw new ArgumentException("account_id and url parameters are mandatory for ACCOUNT_INCOMING_BLOCKCHAIN_TRANSACTION subscription type");

                lsa.AccountId = account_id;
                lsa.Url = url;
            }

            var parameters = new Dictionary<string, object> {
                { "type", JsonConvert.SerializeObject(type, new LedgerSubscriptionTypeConverter(false)) },
                { "attr", lsa },
            };

            var credits = 2;
            var url_ = Ledger.Tatum.GetUrl(string.Format(Endpoints_Create));
            return await Ledger.Tatum.SendTatumRequest<TatumId>(url_, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> List all active subscriptions<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// List all active subscriptions.
        /// </summary>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<LedgerSubscription>> List(int pageSize = 50, int offset = 0, CancellationToken ct = default) => ListAsync(pageSize, offset, ct).Result;
        /// <summary>
        /// <b>Title:</b> List all active subscriptions<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// List all active subscriptions.
        /// </summary>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<LedgerSubscription>>> ListAsync(int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);

            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };

            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_List));
            return await Ledger.Tatum.SendTatumRequest<IEnumerable<LedgerSubscription>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Cancel existing subscription<br />
        /// <b>Credits:</b> 1 credit for API call<br />
        /// <b>Description:</b>
        /// Cancel existing subscription.
        /// </summary>
        /// <param name="id">Subscription ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> Cancel(string id, CancellationToken ct = default) => CancelAsync(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Cancel existing subscription<br />
        /// <b>Credits:</b> 1 credit for API call<br />
        /// <b>Description:</b>
        /// Cancel existing subscription.
        /// </summary>
        /// <param name="id">Subscription ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> CancelAsync(string id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Cancel, id));
            var result = await Ledger.Tatum.SendTatumRequest<string>(url, HttpMethod.Delete, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Obtain report for subscription<br />
        /// <b>Credits:</b> 1 credit for API call. Based on the required report type, additional credits may be charged.<br />
        /// <b>Description:</b>
        /// Obtain report from subscription based on its type. Following reports are supported:
        /// - ACCOUNT_BALANCE_LIMIT - obtain list of all ledger accounts with account balance above the limit. 1 credit per 50 returned records is charged.
        /// - TRANSACTION_HISTORY_REPORT - obtain list of all ledger transaction for last X hours from the time of invocation. 1 credit per 50 returned records is charged.
        /// </summary>
        /// <param name="id">Subscription ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<LedgerReport>> GetReport(string id, CancellationToken ct = default) => GetReportAsync(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Obtain report for subscription<br />
        /// <b>Credits:</b> 1 credit for API call. Based on the required report type, additional credits may be charged.<br />
        /// <b>Description:</b>
        /// Obtain report from subscription based on its type. Following reports are supported:
        /// - ACCOUNT_BALANCE_LIMIT - obtain list of all ledger accounts with account balance above the limit. 1 credit per 50 returned records is charged.
        /// - TRANSACTION_HISTORY_REPORT - obtain list of all ledger transaction for last X hours from the time of invocation. 1 credit per 50 returned records is charged.
        /// </summary>
        /// <param name="id">Subscription ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<LedgerReport>>> GetReportAsync(string id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Report, id));
            return await Ledger.Tatum.SendTatumRequest<IEnumerable<LedgerReport>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }
        #endregion

    }
}
