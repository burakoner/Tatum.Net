using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class RippleLedger
    {
        [JsonProperty("ledger")]
        public RippleLedgerDetails Ledger { get; set; }

        [JsonProperty("ledger_hash")]
        public string LedgerHash { get; set; }

        [JsonProperty("ledger_index")]
        public long LedgerIndex { get; set; }

        [JsonProperty("validated")]
        public bool Validated { get; set; }
    }
}
