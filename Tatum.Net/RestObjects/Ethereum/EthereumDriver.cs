using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class EthereumDriver
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        
        [JsonProperty("jsonrpc")]
        public string JsonRpc { get; set; }
        
        [JsonProperty("result")]
        public string Result { get; set; }
    }
}
