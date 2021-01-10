using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class NeoContractProperties
    {
        [JsonProperty("storage")]
        public bool Storage { get; set; }

        [JsonProperty("dynamic_invoke")]
        public bool DynamicInvoke { get; set; }
    }
}
