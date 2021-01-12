using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class LedgerBalance
    {
        [JsonProperty("accountBalance")]
        public decimal AccountBalance { get; set; }

        [JsonProperty("availableBalance")]
        public decimal AvailableBalance { get; set; }
    }
}
