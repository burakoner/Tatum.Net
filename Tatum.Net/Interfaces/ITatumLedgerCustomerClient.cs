using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumLedgerCustomerClient
    {
        WebCallResult<bool> Activate(string id, CancellationToken ct = default);
        Task<WebCallResult<bool>> ActivateAsync(string id, CancellationToken ct = default);
        WebCallResult<bool> Deactivate(string id, CancellationToken ct = default);
        Task<WebCallResult<bool>> DeactivateAsync(string id, CancellationToken ct = default);
        WebCallResult<bool> Disable(string id, CancellationToken ct = default);
        Task<WebCallResult<bool>> DisableAsync(string id, CancellationToken ct = default);
        WebCallResult<bool> Enable(string id, CancellationToken ct = default);
        Task<WebCallResult<bool>> EnableAsync(string id, CancellationToken ct = default);
        WebCallResult<LedgerCustomer> Get(string id, CancellationToken ct = default);
        Task<WebCallResult<LedgerCustomer>> GetAsync(string id, CancellationToken ct = default);
        WebCallResult<IEnumerable<LedgerCustomer>> ListAll(int pageSize = 50, int offset = 0, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<LedgerCustomer>>> ListAllAsync(int pageSize = 50, int offset = 0, CancellationToken ct = default);
        WebCallResult<LedgerCustomer> Update(string id, string externalId, string accountingCurrency = null, string customerCountry = null, string providerCountry = null, CancellationToken ct = default);
        Task<WebCallResult<LedgerCustomer>> UpdateAsync(string id, string externalId, string accountingCurrency = null, string customerCountry = null, string providerCountry = null, CancellationToken ct = default);
    }
}
