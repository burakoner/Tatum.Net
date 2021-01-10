using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class BitcoinCashTransactionOutput
    {
        [JsonProperty("value")]
        public decimal Value { get; set; }

        [JsonProperty("n")]
        public decimal N { get; set; }

        [JsonProperty("scriptPubKey")]
        public BitcoinCashTransactionOutputScript ScriptPublicKey { get; set; }
    }
}
