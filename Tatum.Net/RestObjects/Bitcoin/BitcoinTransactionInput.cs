using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class BitcoinTransactionInput
    {
        [JsonProperty("prevout")]
        public BitcoinPrevout Prevout { get; set; }

        [JsonProperty("script")]
        public string Script { get; set; }

        [JsonProperty("witness")]
        public string Witness { get; set; }

        [JsonProperty("sequence")]
        public long Sequence { get; set; }

        [JsonProperty("coin")]
        public BitcoinUTXO Coin { get; set; }
    }
}
