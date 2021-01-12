using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class TronInternalTransaction
    {
        [JsonProperty("internal_tx_id")]
        public string InternalTransactionId { get; set; }

        [JsonProperty("to_address")]
        public string ToAddress { get; set; }

        [JsonProperty("from_address")]
        public string FromAddress { get; set; }
    }
}
