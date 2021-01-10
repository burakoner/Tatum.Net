using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class BlockchainKey
    {
        [JsonProperty("key")]
        public string Key { get; set; }
    }
}
