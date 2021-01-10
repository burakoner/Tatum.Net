using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class RippleTransaction
    {
        [JsonProperty("meta")]
        public RippleTransactionMeta Meta { get; set; }

        [JsonProperty("tx")]
        public RippleTransactionData Data { get; set; }

        [JsonProperty("validated")]
        public bool Validated { get; set; }
    }
}
