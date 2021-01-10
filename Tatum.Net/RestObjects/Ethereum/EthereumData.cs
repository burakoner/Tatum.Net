using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class EthereumData
    {
        [JsonProperty("data")]
        public string Data { get; set; }
    }
}
