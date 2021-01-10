using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class LitecoinSendOrderFromAddress
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("privateKey")]
        public string PrivateKey { get; set; }

        [JsonProperty("signatureId")]
        public string SignatureId { get; set; }
    }
}
