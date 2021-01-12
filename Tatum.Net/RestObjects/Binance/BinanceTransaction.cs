using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class BinanceTransaction
    {
        [JsonProperty("code")]
        public long Code { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("log")]
        public string Log { get; set; }

        [JsonProperty("ok")]
        public bool OK { get; set; }

        [JsonProperty("tx")]
        public BinanceTransactionData Transaction { get; set; }
    }
}
