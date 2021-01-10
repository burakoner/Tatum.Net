using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class BinanceTransactionMessage
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public BinanceTransactionMessageValue Value { get; set; }
    }
}
