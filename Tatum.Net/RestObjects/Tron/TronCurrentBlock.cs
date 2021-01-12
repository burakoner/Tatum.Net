using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class TronCurrentBlock
    {
        [JsonProperty("blockNumber")]
        public long BlockNumber { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("testnet")]
        public bool Testnet { get; set; }
    }
}
