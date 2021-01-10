using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class BlockchainResponse
    {
        [JsonProperty("txId")]
        public string TransactionId { get; set; }

        [JsonProperty("failed")]
        public bool Failed { get; set; }
    }
}
