using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class KMSWithdrawalResponse
    {
        [JsonProperty("address")]
        public KMSWithdrawalAddress Address { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("vIn")]
        public string VInput { get; set; }

        [JsonProperty("vInIndex")]
        public long VInputIndex { get; set; }

        [JsonProperty("scriptPubKey")]
        public string ScriptPublicKey { get; set; }
    }
}