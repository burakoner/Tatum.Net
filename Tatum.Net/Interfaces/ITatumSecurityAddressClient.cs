using CryptoExchange.Net.Objects;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumSecurityAddressClient
    {
        WebCallResult<SecurityStatus> Security_CheckMalicousAddress(string address, CancellationToken ct = default);
        Task<WebCallResult<SecurityStatus>> Security_CheckMalicousAddress_Async(string address, CancellationToken ct = default);
    }
}
