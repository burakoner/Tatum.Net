using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class RippleTransactionTakerPay
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
