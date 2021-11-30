using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.Enums;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumLedgerSubscriptionClient
    {
        WebCallResult<bool> Cancel(string id, CancellationToken ct = default);
        Task<WebCallResult<bool>> CancelAsync(string id, CancellationToken ct = default);
        WebCallResult<TatumId> Create(LedgerSubscriptionType type, string account_id = null, string url = null, string currency = null, int? interval = null, decimal? limit = null, LedgerBalanceType? typeOfBalance = null, CancellationToken ct = default);
        Task<WebCallResult<TatumId>> CreateAsync(LedgerSubscriptionType type, string account_id = null, string url = null, string currency = null, int? interval = null, decimal? limit = null, LedgerBalanceType? typeOfBalance = null, CancellationToken ct = default);
        WebCallResult<IEnumerable<LedgerReport>> GetReport(string id, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<LedgerReport>>> GetReportAsync(string id, CancellationToken ct = default);
        WebCallResult<IEnumerable<LedgerSubscription>> List(int pageSize = 50, int offset = 0, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<LedgerSubscription>>> ListAsync(int pageSize = 50, int offset = 0, CancellationToken ct = default);
    }
}
