using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class VeChainBlock
    {
        [JsonProperty("beneficiary")]
        public string Beneficiary { get; set; }

        [JsonProperty("gasLimit")]
        public long GasLimit { get; set; }

        [JsonProperty("gasUsed")]
        public long GasUsed { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("isTrunk")]
        public bool IsTrunk { get; set; }

        [JsonProperty("number")]
        public long Number { get; set; }

        [JsonProperty("parentID")]
        public string ParentID { get; set; }

        [JsonProperty("receiptsRoot")]
        public string ReceiptsRoot { get; set; }

        [JsonProperty("signer")]
        public string Signer { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("stateRoot")]
        public string StateRoot { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("totalScore")]
        public long TotalScore { get; set; }

        [JsonProperty("transactions")]
        public IEnumerable<string> Transactions { get; set; }

        [JsonProperty("txsFeatures")]
        public long TransactionsFeatures { get; set; }

        [JsonProperty("txsRoot")]
        public string TransactionsRoot { get; set; }
    }
}
