using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class LitecoinChainInfo
    {
        [JsonProperty("chain")]
        public string Chain { get; set; }

        [JsonProperty("blocks")]
        public long Blocks { get; set; }

        [JsonProperty("headers")]
        public long Headers { get; set; }

        [JsonProperty("difficulty")]
        public decimal Difficulty { get; set; }

        [JsonProperty("bestblockhash")]
        public string BestBlockHash { get; set; }
    }
}
