using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class BitcoinPrevout
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("index")]
        public long Index { get; set; }
    }
}
