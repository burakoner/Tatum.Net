using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class ScryptaSendOrderFromUTXO
    {
        [JsonProperty("txHash")]
        public string TxHash { get; set; }

        [JsonProperty("index")]
        public long Index { get; set; }

        [JsonProperty("privateKey")]
        public string PrivateKey { get; set; }

        [JsonProperty("signatureId")]
        public string SignatureId { get; set; }
    }
}
