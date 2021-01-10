using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class RippleChainFeeLevels
    {
        [JsonProperty("median_level")]
        public string MedianLevel { get; set; }

        [JsonProperty("minimum_level")]
        public string MinimumLevel { get; set; }

        [JsonProperty("open_ledger_level")]
        public string OpenLedgerLevel { get; set; }

        [JsonProperty("reference_level")]
        public string ReferenceLevel { get; set; }
    }
}
