using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class OffchainDepositAddressRequest
    {
        [JsonProperty("accountId")]
        public string LedgerAccountId { get; set; }

        [JsonProperty("derivationKey")]
        public int? DerivationKey { get; set; }
    }
}
