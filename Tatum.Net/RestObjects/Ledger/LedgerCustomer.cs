using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class LedgerCustomer
    {
        [JsonProperty("externalId")]
        public string ExternalId { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("accountingCurrency")]
        public string AccountingCurrency { get; set; }

        [JsonProperty("customerCountry")]
        public string CustomerCountry { get; set; }

        [JsonProperty("providerCountry")]
        public string ProviderCountry { get; set; }
    }
}
