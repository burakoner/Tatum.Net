using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class RippleLedgerDetails
    {
        [JsonProperty("accepted")]
        public bool Accepted { get; set; }

        [JsonProperty("account_hash")]
        public string AccountHash { get; set; }

        [JsonProperty("close_flags")]
        public long CloseFlags { get; set; }

        [JsonProperty("close_time")]
        public long CloseTime { get; set; }

        [JsonProperty("close_time_human")]
        public string CloseTimeHuman { get; set; }

        [JsonProperty("close_time_resolution")]
        public long CloseTimeResolution { get; set; }

        [JsonProperty("closed")]
        public bool Closed { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("ledger_hash")]
        public string LedgerHash { get; set; }

        [JsonProperty("ledger_index")]
        public long LedgerIndex { get; set; }

        [JsonProperty("parent_close_time")]
        public long ParentCloseTime { get; set; }

        [JsonProperty("parent_hash")]
        public string ParentHash { get; set; }

        [JsonProperty("seqNum")]
        public long SequenceNumber { get; set; }

        [JsonProperty("totalCoins")]
        public string TotalCoins { get; set; }

        [JsonProperty("total_coins")]
        public string Total_Coins { get; set; }

        [JsonProperty("transaction_hash")]
        public string TransactionHash { get; set; }

        [JsonProperty("transactions")]
        public IEnumerable<RippleTransactionData> Transactions { get; set; }

    }
}
