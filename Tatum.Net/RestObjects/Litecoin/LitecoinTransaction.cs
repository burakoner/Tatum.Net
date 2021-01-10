using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class LitecoinTransaction
    {
        [JsonProperty("blockNumber")]
        public long BlockNumber { get; set; }

        [JsonProperty("fee")]
        public decimal Fee { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("index")]
        public long Index { get; set; }

        [JsonProperty("flag")]
        public long Flag { get; set; }

        [JsonProperty("locktime")]
        public long LockTime { get; set; }

        [JsonProperty("inputs")]
        public IEnumerable<LitecoinTransactionInput> Inputs { get; set; }

        [JsonProperty("outputs")]
        public IEnumerable<LitecoinTransactionOutput> Outputs { get; set; }

        [JsonProperty("ps")]
        public long PS { get; set; }

        [JsonProperty("rate")]
        public decimal Rate { get; set; }

        [JsonProperty("version")]
        public long Version { get; set; }

        [JsonProperty("witnessHash")]
        public string WitnessHash { get; set; }
    }

}
