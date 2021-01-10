using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class EthereumBalance
    {
        [JsonProperty("balance")]
        public string Balance { get; set; }
    }
}
