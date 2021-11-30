using CryptoExchange.Net.Objects;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumSecurityAddressClient
    {
        WebCallResult<SecurityStatus> CheckMalicousAddress(string address, CancellationToken ct = default);
        Task<WebCallResult<SecurityStatus>> CheckMalicousAddressAsync(string address, CancellationToken ct = default);
    }
}
