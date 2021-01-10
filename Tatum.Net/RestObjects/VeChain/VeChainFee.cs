using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class VeChainFee
    {
        [JsonProperty("gasLimit")]
        public decimal GasLimit { get; set; }
    }
}
