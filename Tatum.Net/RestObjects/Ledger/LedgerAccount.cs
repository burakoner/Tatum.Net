using Newtonsoft.Json;
using Tatum.Net.Converters;
using Tatum.Net.Enums;

namespace Tatum.Net.RestObjects
{
    public class LedgerAccount
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("customerId")]
        public string CustomerId { get; set; }
        
        [JsonProperty("accountCode")]
        public string AccountCode { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("frozen")]
        public bool Frozen { get; set; }
        
        [JsonProperty("currency"), JsonConverter(typeof(BlockchainTypeConverter))]
        public BlockchainType CryptoCurrency { get; set; }

        [JsonProperty("accountingCurrency"), JsonConverter(typeof(FiatCurrencyConverter))]
        public FiatCurrency FiatCurrency { get; set; }

        [JsonProperty("balance")]
        public LedgerAccountBalance Balance { get; set; }
    }
}
