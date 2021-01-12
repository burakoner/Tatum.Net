using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class TronContractParameter
    {
        [JsonProperty("type_url")]
        public string TypeUrl { get; set; }

        [JsonProperty("value")]
        public TronContractParameterValue Value { get; set; }
    }
}
