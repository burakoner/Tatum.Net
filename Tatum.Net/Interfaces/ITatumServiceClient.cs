using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumServiceClient
    {
        WebCallResult<IEnumerable<ServiceUsage>> Service_GetConsumptions(CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<ServiceUsage>>> Service_GetConsumptions_Async(CancellationToken ct = default);
        WebCallResult<ServiceExchangeRate> Service_GetExchangeRates(string currency, string basePair, CancellationToken ct = default);
        Task<WebCallResult<ServiceExchangeRate>> Service_GetExchangeRates_Async(string currency, string basePair, CancellationToken ct = default);
        WebCallResult<ServiceVersion> Service_GetVersion(CancellationToken ct = default);
        Task<WebCallResult<ServiceVersion>> Service_GetVersion_Async(CancellationToken ct = default);
    }
}
