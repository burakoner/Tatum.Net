using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.Enums;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumLedgerOrderBookClient
    {
        WebCallResult<bool> LedgerOrderBook_CancelAllOrders(string id, CancellationToken ct = default);
        Task<WebCallResult<bool>> LedgerOrderBook_CancelAllOrders_Async(string id, CancellationToken ct = default);
        WebCallResult<bool> LedgerOrderBook_CancelOrder(string id, CancellationToken ct = default);
        Task<WebCallResult<bool>> LedgerOrderBook_CancelOrder_Async(string id, CancellationToken ct = default);
        WebCallResult<IEnumerable<LedgerTrade>> LedgerOrderBook_GetBuyTrades(string id, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<LedgerTrade>>> LedgerOrderBook_GetBuyTrades_Async(string id, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        WebCallResult<IEnumerable<LedgerTrade>> LedgerOrderBook_GetHistoricalTrades(string id, string pair, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<LedgerTrade>>> LedgerOrderBook_GetHistoricalTrades_Async(string id, string pair, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        WebCallResult<IEnumerable<LedgerTrade>> LedgerOrderBook_GetSellTrades(string id, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<LedgerTrade>>> LedgerOrderBook_GetSellTrades_Async(string id, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        WebCallResult<LedgerTrade> LedgerOrderBook_GetTrade(string id, CancellationToken ct = default);
        Task<WebCallResult<LedgerTrade>> LedgerOrderBook_GetTrade_Async(string id, CancellationToken ct = default);
        WebCallResult<TatumId> LedgerOrderBook_PlaceOrder(LedgerTradeType type, decimal price, decimal amount, string pair, string currency1AccountId, string currency2AccountId, string feeAccountId = null, decimal? fee = null, CancellationToken ct = default);
        Task<WebCallResult<TatumId>> LedgerOrderBook_PlaceOrder_Async(LedgerTradeType type, decimal price, decimal amount, string pair, string currency1AccountId, string currency2AccountId, string feeAccountId = null, decimal? fee = null, CancellationToken ct = default);
    }
}
