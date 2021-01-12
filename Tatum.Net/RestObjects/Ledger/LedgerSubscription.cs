using Newtonsoft.Json;
using Tatum.Net.Converters;
using Tatum.Net.Enums;

namespace Tatum.Net.RestObjects
{
    public class LedgerSubscription
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string SubscriptionId { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore), JsonConverter(typeof(LedgerSubscriptionTypeConverter))]
        public LedgerSubscriptionType Type { get; set; }

        [JsonProperty("attr", NullValueHandling = NullValueHandling.Ignore)]
        public LedgerSubscriptionAttributes Attributes { get; set; }
    }
}
