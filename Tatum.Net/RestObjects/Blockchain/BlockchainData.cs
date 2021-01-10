using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class BlockchainData
    {
        [JsonProperty("data")]
        public string Data { get; set; }
    }
}
