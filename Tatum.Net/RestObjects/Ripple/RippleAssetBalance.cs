using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class RippleAssetBalance
    {
        [JsonProperty("balance")]
        public string Balance { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}
