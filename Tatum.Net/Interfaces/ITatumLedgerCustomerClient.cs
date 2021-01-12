using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumLedgerCustomerClient
    {
        WebCallResult<bool> LedgerCustomer_Activate(string id, CancellationToken ct = default);
        Task<WebCallResult<bool>> LedgerCustomer_Activate_Async(string id, CancellationToken ct = default);
        WebCallResult<bool> LedgerCustomer_Deactivate(string id, CancellationToken ct = default);
        Task<WebCallResult<bool>> LedgerCustomer_Deactivate_Async(string id, CancellationToken ct = default);
        WebCallResult<bool> LedgerCustomer_Disable(string id, CancellationToken ct = default);
        Task<WebCallResult<bool>> LedgerCustomer_Disable_Async(string id, CancellationToken ct = default);
        WebCallResult<bool> LedgerCustomer_Enable(string id, CancellationToken ct = default);
        Task<WebCallResult<bool>> LedgerCustomer_Enable_Async(string id, CancellationToken ct = default);
        WebCallResult<LedgerCustomer> LedgerCustomer_Get(string id, CancellationToken ct = default);
        Task<WebCallResult<LedgerCustomer>> LedgerCustomer_Get_Async(string id, CancellationToken ct = default);
        WebCallResult<IEnumerable<LedgerCustomer>> LedgerCustomer_ListAll(int pageSize = 50, int offset = 0, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<LedgerCustomer>>> LedgerCustomer_ListAll_Async(int pageSize = 50, int offset = 0, CancellationToken ct = default);
        WebCallResult<LedgerCustomer> LedgerCustomer_Update(string id, string externalId, string accountingCurrency = null, string customerCountry = null, string providerCountry = null, CancellationToken ct = default);
        Task<WebCallResult<LedgerCustomer>> LedgerCustomer_Update_Async(string id, string externalId, string accountingCurrency = null, string customerCountry = null, string providerCountry = null, CancellationToken ct = default);
    }
}
