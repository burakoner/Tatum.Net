using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class NeoGasAssetCouple
    {
        [JsonProperty("NEO")]
        public decimal NEO { get; set; }

        [JsonProperty("GAS")]
        public decimal GAS { get; set; }
    }
}
