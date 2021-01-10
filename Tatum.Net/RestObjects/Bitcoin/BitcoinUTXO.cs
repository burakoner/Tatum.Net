using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class BitcoinUTXO
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("index")]
        public long Index { get; set; }

        [JsonProperty("version")]
        public long Version { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("value")]
        public decimal Value { get; set; }

        [JsonProperty("script")]
        public string Script { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("coinbase")]
        public bool CoinBase { get; set; }
    }
}
