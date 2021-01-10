using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class StellarAddressSecret
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }
    }
}
