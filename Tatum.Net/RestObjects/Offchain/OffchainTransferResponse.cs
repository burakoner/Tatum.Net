using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class OffchainTransferResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("txId")]
        public string TransactionId { get; set; }

        [JsonProperty("completed")]
        public bool Completed { get; set; }
    }
}
