using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class VeChainBalance
    {
        [JsonProperty("balance")]
        public decimal Balance { get; set; }
    }
}
