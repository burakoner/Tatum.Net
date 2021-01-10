using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class LedgerBlockedAmount
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("accountId")]
        public string AccountId { get; set; }
        
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
