using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class BinanceTransactionSignature
    {
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty("pub_key")]
        public BinanceTransactionPublicKey PublicKey { get; set; }

        [JsonProperty("sequence")]
        public string Sequence { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }
    }
}
