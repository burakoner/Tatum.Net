using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class BlockchainHash
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }
    }
}
