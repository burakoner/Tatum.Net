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
        WebCallResult<bool> CancelAllOrders(string id, CancellationToken ct = default);
        Task<WebCallResult<bool>> CancelAllOrdersAsync(string id, CancellationToken ct = default);
        WebCallResult<bool> CancelOrder(string id, CancellationToken ct = default);
        Task<WebCallResult<bool>> CancelOrderAsync(string id, CancellationToken ct = default);
        WebCallResult<IEnumerable<LedgerTrade>> GetBuyTrades(string id, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<LedgerTrade>>> GetBuyTradesAsync(string id, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        WebCallResult<IEnumerable<LedgerTrade>> GetHistoricalTrades(string id, string pair, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<LedgerTrade>>> GetHistoricalTradesAsync(string id, string pair, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        WebCallResult<IEnumerable<LedgerTrade>> GetSellTrades(string id, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<LedgerTrade>>> GetSellTradesAsync(string id, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        WebCallResult<LedgerTrade> GetTrade(string id, CancellationToken ct = default);
        Task<WebCallResult<LedgerTrade>> GetTradeAsync(string id, CancellationToken ct = default);
        WebCallResult<TatumId> PlaceOrder(LedgerTradeType type, decimal price, decimal amount, string pair, string currency1AccountId, string currency2AccountId, string feeAccountId = null, decimal? fee = null, CancellationToken ct = default);
        Task<WebCallResult<TatumId>> PlaceOrderAsync(LedgerTradeType type, decimal price, decimal amount, string pair, string currency1AccountId, string currency2AccountId, string feeAccountId = null, decimal? fee = null, CancellationToken ct = default);
    }
}
