using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class OffchainWithdrawalAddress
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("derivationKey")]
        public long DerivationKey { get; set; }

        [JsonProperty("xpub")]
        public string ExtendedPublicKey { get; set; }

        [JsonProperty("destinationTag")]
        public long DestinationTag { get; set; }

        [JsonProperty("memo")]
        public string Memo { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
