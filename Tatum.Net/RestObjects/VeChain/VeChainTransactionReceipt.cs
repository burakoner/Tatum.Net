using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class VeChainTransactionReceipt
    {
        [JsonProperty("blockHash")]
        public string BlockHash { get; set; }

        [JsonProperty("blockNumber")]
        public long BlockNumber { get; set; }

        [JsonProperty("gasPayer")]
        public string GasPayer { get; set; }

        [JsonProperty("gasUsed")]
        public long GasUsed { get; set; }

        [JsonProperty("meta")]
        public VeChainTransactionMeta Meta { get; set; }

        [JsonProperty("outputs")]
        public IEnumerable<VeChainTransactionOutput> Outputs { get; set; }

        [JsonProperty("paid")]
        public string Paid { get; set; }

        [JsonProperty("reverted")]
        public bool Reverted { get; set; }

        [JsonProperty("reward")]
        public string Reward { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("transactionHash")]
        public string TransactionHash { get; set; }
    }
}