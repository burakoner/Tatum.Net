using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class OffchainResponse
    {
        [JsonProperty("txId")]
        public string TransactionId { get; set; }

        [JsonProperty("completed")]
        public bool Completed { get; set; }
    }
}
