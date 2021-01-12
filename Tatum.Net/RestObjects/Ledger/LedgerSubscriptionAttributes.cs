using Newtonsoft.Json;
using Tatum.Net.Converters;
using Tatum.Net.Enums;

namespace Tatum.Net.RestObjects
{
    public class LedgerSubscriptionAttributes
    {
        [JsonProperty("limit", NullValueHandling = NullValueHandling.Ignore)]
        public string Limit { get; set; }

        [JsonProperty("typeOfBalance", NullValueHandling = NullValueHandling.Ignore), JsonConverter(typeof(LedgerBalanceTypeConverter))]
        public LedgerBalanceType? TypeOfBalance { get; set; }

        [JsonProperty("interval", NullValueHandling = NullValueHandling.Ignore)]
        public int? Interval { get; set; }

        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public string Currency { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string AccountId { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }
    }
}
