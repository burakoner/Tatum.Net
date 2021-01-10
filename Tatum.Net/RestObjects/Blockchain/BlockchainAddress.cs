using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class BlockchainAddress
    {
        [JsonProperty("address")]
        public string Address { get; set; }
    }
}
