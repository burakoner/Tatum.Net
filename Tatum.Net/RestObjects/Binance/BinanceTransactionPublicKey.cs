using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class BinanceTransactionPublicKey
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

}
