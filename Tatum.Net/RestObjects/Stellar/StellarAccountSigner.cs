using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class StellarAccountSigner
    {
        [JsonProperty("weight")]
        public decimal Weight { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
