using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class TronContract
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("parameter")]
        public TronContractParameter Parameter { get; set; }
    }
}
