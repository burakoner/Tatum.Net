using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class StellarAccountThresholds
    {
        [JsonProperty("low_threshold")]
        public decimal LowThreshold { get; set; }

        [JsonProperty("med_threshold")]
        public decimal MediumThreshold { get; set; }

        [JsonProperty("high_threshold")]
        public decimal HighThreshold { get; set; }
    }
}
