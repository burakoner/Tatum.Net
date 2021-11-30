using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;
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
    public class LedgerOrderBookClient : ITatumLedgerOrderBookClient
    {
        public LedgerClient Ledger { get; protected set; }

        #region API Endpoints

        #region Ledger Order Book
        protected const string Endpoints_ListHistory = "trade/history";
        protected const string Endpoints_ListBuys = "trade/buy";
        protected const string Endpoints_ListSells = "trade/sell";
        protected const string Endpoints_Place = "trade";
        protected const string Endpoints_Get = "trade/{0}";
        protected const string Endpoints_Cancel = "trade/{0}";
        protected const string Endpoints_CancelAll = "trade/account/{0}";
        #endregion

        #endregion

        public LedgerOrderBookClient(LedgerClient ledgerClient)
        {
            Ledger = ledgerClient;
        }

        #region Ledger / Order Book
        /// <summary>
        /// <b>Title:</b> List all historical trades<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// List all historical trades. It is possible to list all trades, trades for specific trading pair and/or account.
        /// </summary>
        /// <param name="id">Account ID. If present, only closed trades for given account will be present.</param>
        /// <param name="pair">Trade pair. If present, only closed trades on given trade pair will be present.</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<LedgerTrade>> GetHistoricalTrades(string id, string pair, int pageSize = 50, int offset = 0, CancellationToken ct = default) => GetHistoricalTradesAsync(id, pair, pageSize, offset, ct).Result;
        /// <summary>
        /// <b>Title:</b> List all historical trades<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// List all historical trades. It is possible to list all trades, trades for specific trading pair and/or account.
        /// </summary>
        /// <param name="id">Account ID. If present, only closed trades for given account will be present.</param>
        /// <param name="pair">Trade pair. If present, only closed trades on given trade pair will be present.</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<LedgerTrade>>> GetHistoricalTradesAsync(string id, string pair, int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);
            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };
            parameters.AddOptionalParameter("id", id);
            parameters.AddOptionalParameter("pair", pair);

            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_ListHistory));
            return await Ledger.Tatum.SendTatumRequest<IEnumerable<LedgerTrade>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> List all active buy trades<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// List all active buy trades.
        /// </summary>
        /// <param name="id">Account ID. If present, list current active buy trades for that account.</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<LedgerTrade>> GetBuyTrades(string id, int pageSize = 50, int offset = 0, CancellationToken ct = default) => GetBuyTradesAsync(id, pageSize, offset, ct).Result;
        /// <summary>
        /// <b>Title:</b> List all active buy trades<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// List all active buy trades.
        /// </summary>
        /// <param name="id">Account ID. If present, list current active buy trades for that account.</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<LedgerTrade>>> GetBuyTradesAsync(string id, int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);
            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };
            parameters.AddOptionalParameter("id", id);

            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_ListBuys));
            return await Ledger.Tatum.SendTatumRequest<IEnumerable<LedgerTrade>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> List all active sell trades<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// List all active sell trades.
        /// </summary>
        /// <param name="id">Account ID. If present, list current active sell trades for that account.</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<LedgerTrade>> GetSellTrades(string id, int pageSize = 50, int offset = 0, CancellationToken ct = default) => GetSellTradesAsync(id, pageSize, offset, ct).Result;
        /// <summary>
        /// <b>Title:</b> List all active sell trades<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// List all active sell trades.
        /// </summary>
        /// <param name="id">Account ID. If present, list current active sell trades for that account.</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<LedgerTrade>>> GetSellTradesAsync(string id, int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);
            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };
            parameters.AddOptionalParameter("id", id);

            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_ListSells));
            return await Ledger.Tatum.SendTatumRequest<IEnumerable<LedgerTrade>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Store buy / sell trade<br />
        /// <b>Credits:</b> 2 credits for API call, 2 credits for each fill of the counter trade. 1 API call + 2 fills = 6 credits.<br />
        /// <b>Description:</b>
        /// Store new buy / sell trade. If there is trade already available to fill, fill as much trades as possible.
        /// It is possible to charge fees for the trades.Fees are an extra amount on top of the trade amount and are paid in the currency of the 1st pair to the separate fee account, e.g. for BTC/ETH pair fees will be paid in BTC.
        /// </summary>
        /// <param name="type">Type of the trade, BUY or SELL</param>
        /// <param name="price">Price to buy / sell</param>
        /// <param name="amount">Amount of the trade to be bought / sold</param>
        /// <param name="pair">Trading pair</param>
        /// <param name="currency1AccountId">ID of the account of the currency 1 trade currency</param>
        /// <param name="currency2AccountId">ID of the account of the currency 2 trade currency</param>
        /// <param name="feeAccountId">ID of the account where fee will be paid, if any. Fee will be paid from the currency 1 account.</param>
        /// <param name="fee">Percentage of the trade amount to be paid as a fee.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TatumId> PlaceOrder(
            LedgerTradeType type,
            decimal price,
            decimal amount,
            string pair,
            string currency1AccountId,
            string currency2AccountId,
            string feeAccountId = null,
            decimal? fee = null,
            CancellationToken ct = default) => PlaceOrderAsync(type, price, amount, pair, currency1AccountId, currency2AccountId, feeAccountId, fee, ct).Result;
        /// <summary>
        /// <b>Title:</b> Store buy / sell trade<br />
        /// <b>Credits:</b> 2 credits for API call, 2 credits for each fill of the counter trade. 1 API call + 2 fills = 6 credits.<br />
        /// <b>Description:</b>
        /// Store new buy / sell trade. If there is trade already available to fill, fill as much trades as possible.
        /// It is possible to charge fees for the trades.Fees are an extra amount on top of the trade amount and are paid in the currency of the 1st pair to the separate fee account, e.g. for BTC/ETH pair fees will be paid in BTC.
        /// </summary>
        /// <param name="type">Type of the trade, BUY or SELL</param>
        /// <param name="price">Price to buy / sell</param>
        /// <param name="amount">Amount of the trade to be bought / sold</param>
        /// <param name="pair">Trading pair</param>
        /// <param name="currency1AccountId">ID of the account of the currency 1 trade currency</param>
        /// <param name="currency2AccountId">ID of the account of the currency 2 trade currency</param>
        /// <param name="feeAccountId">ID of the account where fee will be paid, if any. Fee will be paid from the currency 1 account.</param>
        /// <param name="fee">Percentage of the trade amount to be paid as a fee.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TatumId>> PlaceOrderAsync(
            LedgerTradeType type,
            decimal price,
            decimal amount,
            string pair,
            string currency1AccountId,
            string currency2AccountId,
            string feeAccountId = null,
            decimal? fee = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "type", JsonConvert.SerializeObject(type, new LedgerTradeTypeConverter(false)) },
                { "price", price.ToString(CultureInfo.InvariantCulture) },
                { "amount", amount.ToString(CultureInfo.InvariantCulture) },
                { "pair", pair },
                { "currency1AccountId", currency1AccountId },
                { "currency2AccountId", currency2AccountId },
            };
            parameters.AddOptionalParameter("feeAccountId", feeAccountId);
            parameters.AddOptionalParameter("fee", fee?.ToString(CultureInfo.InvariantCulture));

            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Place));
            return await Ledger.Tatum.SendTatumRequest<TatumId>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get existing trade<br />
        /// <b>Credits:</b> 1 credit for API call<br />
        /// <b>Description:</b>
        /// Get existing opened trade.
        /// </summary>
        /// <param name="id">Trade ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<LedgerTrade> GetTrade(string id, CancellationToken ct = default) => GetTradeAsync(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get existing trade<br />
        /// <b>Credits:</b> 1 credit for API call<br />
        /// <b>Description:</b>
        /// Get existing opened trade.
        /// </summary>
        /// <param name="id">Trade ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<LedgerTrade>> GetTradeAsync(string id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Get, id));
            return await Ledger.Tatum.SendTatumRequest<LedgerTrade>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Cancel existing trade<br />
        /// <b>Credits:</b> 1 credit for API call<br />
        /// <b>Description:</b>
        /// Cancel existing trade.
        /// </summary>
        /// <param name="id">Trade ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> CancelOrder(string id, CancellationToken ct = default) => CancelOrderAsync(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Cancel existing trade<br />
        /// <b>Credits:</b> 1 credit for API call<br />
        /// <b>Description:</b>
        /// Cancel existing trade.
        /// </summary>
        /// <param name="id">Trade ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> CancelOrderAsync(string id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Cancel, id));
            var result = await Ledger.Tatum.SendTatumRequest<string>(url, HttpMethod.Delete, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Cancel all existing trades for account<br />
        /// <b>Credits:</b> 1 credit for API call, 1 credit for each cancelled trade. 1 API call + 2 cancellations = 3 credits.<br />
        /// <b>Description:</b>
        /// Cancel all trades for account.
        /// </summary>
        /// <param name="id">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> CancelAllOrders(string id, CancellationToken ct = default) => CancelAllOrdersAsync(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Cancel all existing trades for account<br />
        /// <b>Credits:</b> 1 credit for API call, 1 credit for each cancelled trade. 1 API call + 2 cancellations = 3 credits.<br />
        /// <b>Description:</b>
        /// Cancel all trades for account.
        /// </summary>
        /// <param name="id">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> CancelAllOrdersAsync(string id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_CancelAll, id));
            var result = await Ledger.Tatum.SendTatumRequest<string>(url, HttpMethod.Delete, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }
        #endregion

    }
}
