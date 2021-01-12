using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class TronWallet
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("privateKey")]
        public string PrivateKey { get; set; }
    }
}
