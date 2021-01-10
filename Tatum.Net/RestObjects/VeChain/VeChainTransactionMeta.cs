using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class VeChainTransactionMeta
    {
        [JsonProperty("blockID")]
        public string BlockId { get; set; }

        [JsonProperty("blockNumber")]
        public long BlockNumber { get; set; }

        [JsonProperty("blockTimestamp")]
        public long BlockTimestamp { get; set; }

        [JsonProperty("txID")]
        public string TransactionId { get; set; }

        [JsonProperty("txOrigin")]
        public string TransactionOrigin { get; set; }
    }
}