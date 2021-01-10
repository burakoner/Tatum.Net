using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class RippleAccount
    {
        [JsonProperty("validated")]
        public bool Validated { get; set; }

        [JsonProperty("account_data")]
        public RippleAccountData AccountData { get; set; }

        [JsonProperty("ledger_current_index")]
        public long LedgerCurrentIndex { get; set; }
    }
}
