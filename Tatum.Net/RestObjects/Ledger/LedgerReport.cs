using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class LedgerReport
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("balance")]
        public LedgerBalance Balance { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("frozen")]
        public bool Frozen { get; set; }

        [JsonProperty("customerId")]
        public string CustomerId { get; set; }

        [JsonProperty("accountCode")]
        public string AccountCode { get; set; }

        [JsonProperty("xpub")]
        public string ExtendedPublicKey { get; set; }

    }
}
