using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class OffchainAccountIdAddressPair
    {
        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }
    }
}
