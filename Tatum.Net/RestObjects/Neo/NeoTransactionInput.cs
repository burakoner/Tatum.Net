using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class NeoTransactionInput
    {
        [JsonProperty("txid")]
        public string TransactionId { get; set; }

        [JsonProperty("vout")]
        public long Output { get; set; }
    }
}
