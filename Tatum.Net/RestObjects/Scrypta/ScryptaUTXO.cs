using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class ScryptaUTXO
    {
        [JsonProperty("txid")]
        public string TransactionId { get; set; }

        [JsonProperty("vout")]
        public long Vout { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("scriptPubKey")]
        public string ScriptPublicKey { get; set; }

        [JsonProperty("block")]
        public long Block { get; set; }
    }
}
