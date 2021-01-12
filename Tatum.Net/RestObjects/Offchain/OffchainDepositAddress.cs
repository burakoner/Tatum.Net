using Newtonsoft.Json;
using Tatum.Net.Converters;
using Tatum.Net.Enums;

namespace Tatum.Net.RestObjects
{
    public class OffchainDepositAddress
    {
        [JsonProperty("xpub")]
        public string ExtendedPublicKey { get; set; }

        [JsonProperty("derivationKey")]
        public int DerivationKey { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("currency"), JsonConverter(typeof(BlockchainTypeConverter))]
        public BlockchainType CryptoCurrency { get; set; }

        [JsonProperty("destinationTag")]
        public long? DestinationTag { get; set; }

        [JsonProperty("memo")]
        public string Memo { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
