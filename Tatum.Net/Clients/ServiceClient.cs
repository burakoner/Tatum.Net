using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.Interfaces;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Clients
{
    public class ServiceClient : ITatumServiceClient
    {
        public TatumClient Tatum { get; protected set; }

        #region API Endpoints

        #region Tatum Service
        protected const string Endpoints_Consumption = "tatum/usage";
        protected const string Endpoints_ExchangeRates = "tatum/rate/{0}";
        protected const string Endpoints_Version = "tatum/version";
        #endregion

        #endregion

        public ServiceClient(TatumClient tatumClient)
        {
            Tatum = tatumClient;
        }


        #region Tatum / Service
        /// <summary>
        /// <b>Title:</b> List credit consumption for last month<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// List usage information of credits.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<ServiceUsage>> GetConsumptions(CancellationToken ct = default) => GetConsumptionsAsync(ct).Result;
        /// <summary>
        /// <b>Title:</b> List credit consumption for last month<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// List usage information of credits.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<ServiceUsage>>> GetConsumptionsAsync(CancellationToken ct = default)
        {
            var credits = 1;
            var url = Tatum.GetUrl(string.Format(Endpoints_Consumption));
            return await Tatum.SendTatumRequest<IEnumerable<ServiceUsage>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get currenct exchange rate of the supported FIAT / crypto asset<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get currenct exchange rate of the supported FIAT / crypto asset. Base pair is EUR by default. 
        /// E.g. to obtain exchange rate for the Bitcoin, response value for the API call will be expressed as 1 BTC = 10,000 EUR.
        /// </summary>
        /// <param name="currency">FIAT or crypto asset</param>
        /// <param name="basePair">FIAT to convert as a basePair</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<ServiceExchangeRate> GetExchangeRates(string currency, string basePair, CancellationToken ct = default) => GetExchangeRatesAsync(currency, basePair, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get currenct exchange rate of the supported FIAT / crypto asset<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get currenct exchange rate of the supported FIAT / crypto asset. Base pair is EUR by default. 
        /// E.g. to obtain exchange rate for the Bitcoin, response value for the API call will be expressed as 1 BTC = 10,000 EUR.
        /// </summary>
        /// <param name="currency">FIAT or crypto asset</param>
        /// <param name="basePair">FIAT to convert as a basePair</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<ServiceExchangeRate>> GetExchangeRatesAsync(string currency, string basePair, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "basePair", basePair },
            };

            var credits = 1;
            var url = Tatum.GetUrl(string.Format(Endpoints_ExchangeRates, currency));
            return await Tatum.SendTatumRequest<ServiceExchangeRate>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get API version<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get current version of the API.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<ServiceVersion> GetVersion(CancellationToken ct = default) => GetVersionAsync(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get API version<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get current version of the API.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<ServiceVersion>> GetVersionAsync(CancellationToken ct = default)
        {
            var credits = 1;
            var url = Tatum.GetUrl(string.Format(Endpoints_Version));
            return await Tatum.SendTatumRequest<ServiceVersion>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }
        #endregion

    }
}
