using Newtonsoft.Json;
using Tatum.Net.Converters;
using Tatum.Net.Enums;

namespace Tatum.Net.RestObjects
{
    public class OffchainDepositAddressState
    {
        [JsonProperty("accountCode")]
        public string AccountCode { get; set; }

        [JsonProperty("accountNumber")]
        public string AcountNumber { get; set; }

        [JsonProperty("accountingCurrency")]
        public string AccountingCurrency { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("balance")]
        public OffchainDepositAddressBalance Balance { get; set; }

        [JsonProperty("currency"), JsonConverter(typeof(BlockchainTypeConverter))]
        public BlockchainType CryptoCurrency { get; set; }

        [JsonProperty("frozen")]
        public bool Frozen { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("xpub")]
        public string ExtendedPublicKey { get; set; }

    }
}
