using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class OffchainAccountIdTxIdPair
    {
        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("txId")]
        public string TransactionId { get; set; }
    }
}
