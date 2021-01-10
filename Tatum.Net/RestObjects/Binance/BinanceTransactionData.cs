using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class BinanceTransactionData
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public BinanceTransactionValue Value { get; set; }
    }
}
