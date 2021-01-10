using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class RippleChainInfo
    {
        [JsonProperty("ledger_hash")]
        public string LedgerHash { get; set; }

        [JsonProperty("ledger_index")]
        public long LedgerIndex { get; set; }
    }
}
