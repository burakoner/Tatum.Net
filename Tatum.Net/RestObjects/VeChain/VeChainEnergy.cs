using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class VeChainEnergy
    {
        [JsonProperty("energy")]
        public decimal Energy { get; set; }
    }
}
