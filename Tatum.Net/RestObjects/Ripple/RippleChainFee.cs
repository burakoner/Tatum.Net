using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class RippleChainFee
    {
        [JsonProperty("current_ledger_size")]
        public string CurrentLedgerSize { get; set; }

        [JsonProperty("current_queue_size")]
        public string CurrentQueueSize { get; set; }

        [JsonProperty("drops")]
        public RippleChainFeeDrops Drops { get; set; }

        [JsonProperty("expected_ledger_size")]
        public string ExpectedLedgerSize { get; set; }

        [JsonProperty("ledger_current_index")]
        public string LedgerCurrentIndex { get; set; }

        [JsonProperty("levels")]
        public RippleChainFeeLevels Levels { get; set; }

        [JsonProperty("max_queue_size")]
        public string MaxQueueSize { get; set; }
    }
}
