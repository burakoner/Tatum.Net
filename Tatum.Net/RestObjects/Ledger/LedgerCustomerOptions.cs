using Newtonsoft.Json;
using Tatum.Net.Converters;
using Tatum.Net.Enums;

namespace Tatum.Net.RestObjects
{
    public class LedgerCustomerOptions
    {
        /// <summary>
        /// All transaction will be accounted in this currency for all accounts. 
        /// Currency can be overridden per account level. If not set, EUR is used. ISO-4217
        /// </summary>
        [JsonProperty("accountingCurrency"), JsonConverter(typeof(FiatCurrencyConverter))]
        public FiatCurrency AccountingCurrency { get; set; }

        /// <summary>
        /// Country customer has to be compliant with. ISO-3166-1
        /// </summary>
        [JsonProperty("customerCountry"), JsonConverter(typeof(CountryCodeConverter))]
        public CountryCode CustomerCountry { get; set; }

        /// <summary>
        /// Customer external ID. 
        /// Use only anonymized identification you have in your system. 
        /// If customer with externalId does not exists new customer is created. 
        /// If customer with specified externalId already exists it is updated.
        /// </summary>
        [JsonProperty("externalId")]
        public string ExternalId { get; set; }

        /// <summary>
        /// Country service provider has to be compliant with. ISO-3166-1
        /// </summary>
        [JsonProperty("providerCountry"), JsonConverter(typeof(CountryCodeConverter))]
        public CountryCode ProviderCountry { get; set; }
    }
}
