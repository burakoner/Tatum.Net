using CryptoExchange.Net.Objects;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Clients
{
    public class SecurityClient
    {
        public TatumClient Tatum { get; protected set; }

        protected const string Endpoints_CheckMalicousAddress = "security/address/{0}";

        public SecurityClient(TatumClient tatumClient)
        {
            Tatum = tatumClient;
        }

        /// <summary>
        /// <b>Title:</b> Check malicous address<br />
        /// <b>Credits:</b> 1 credits per API call.<br />
        /// <b>Description:</b>
        /// Endpoint to check, if the blockchain address is safe to work with or not.
        /// </summary>
        /// <param name="address">Check, if the blockchain address is malicous. Malicous address can contain assets from the DarkWeb, is connected to the scam projects or contains stolen funds.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<SecurityStatus> CheckMalicousAddress(string address, CancellationToken ct = default) => CheckMalicousAddressAsync(address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Check malicous address<br />
        /// <b>Credits:</b> 1 credits per API call.<br />
        /// <b>Description:</b>
        /// Endpoint to check, if the blockchain address is safe to work with or not.
        /// </summary>
        /// <param name="address">Check, if the blockchain address is malicous. Malicous address can contain assets from the DarkWeb, is connected to the scam projects or contains stolen funds.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<SecurityStatus>> CheckMalicousAddressAsync(string address, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Tatum.GetUrl(string.Format(Endpoints_CheckMalicousAddress, address));
            return await Tatum.SendTatumRequest<SecurityStatus>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }
    }
}
