using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class LitecoinTransactionInput
    {
        [JsonProperty("prevout")]
        public LitecoinPrevout Prevout { get; set; }

        [JsonProperty("script")]
        public string Script { get; set; }

        [JsonProperty("witness")]
        public string Witness { get; set; }

        [JsonProperty("sequence")]
        public long Sequence { get; set; }
    }
}
