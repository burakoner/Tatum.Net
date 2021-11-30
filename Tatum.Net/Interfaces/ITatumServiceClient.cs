using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumServiceClient
    {
        WebCallResult<IEnumerable<ServiceUsage>> GetConsumptions(CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<ServiceUsage>>> GetConsumptionsAsync(CancellationToken ct = default);
        WebCallResult<ServiceExchangeRate> GetExchangeRates(string currency, string basePair, CancellationToken ct = default);
        Task<WebCallResult<ServiceExchangeRate>> GetExchangeRatesAsync(string currency, string basePair, CancellationToken ct = default);
        WebCallResult<ServiceVersion> GetVersion(CancellationToken ct = default);
        Task<WebCallResult<ServiceVersion>> GetVersionAsync(CancellationToken ct = default);
    }
}
