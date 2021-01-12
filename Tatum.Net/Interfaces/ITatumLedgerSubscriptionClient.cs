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
        WebCallResult<bool> LedgerSubscription_Cancel(string id, CancellationToken ct = default);
        Task<WebCallResult<bool>> LedgerSubscription_Cancel_Async(string id, CancellationToken ct = default);
        WebCallResult<TatumId> LedgerSubscription_Create(LedgerSubscriptionType type, string account_id = null, string url = null, string currency = null, int? interval = null, decimal? limit = null, LedgerBalanceType? typeOfBalance = null, CancellationToken ct = default);
        Task<WebCallResult<TatumId>> LedgerSubscription_Create_Async(LedgerSubscriptionType type, string account_id = null, string url = null, string currency = null, int? interval = null, decimal? limit = null, LedgerBalanceType? typeOfBalance = null, CancellationToken ct = default);
        WebCallResult<IEnumerable<LedgerReport>> LedgerSubscription_GetReport(string id, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<LedgerReport>>> LedgerSubscription_GetReport_Async(string id, CancellationToken ct = default);
        WebCallResult<IEnumerable<LedgerSubscription>> LedgerSubscription_List(int pageSize = 50, int offset = 0, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<LedgerSubscription>>> LedgerSubscription_List_Async(int pageSize = 50, int offset = 0, CancellationToken ct = default);
    }
}
