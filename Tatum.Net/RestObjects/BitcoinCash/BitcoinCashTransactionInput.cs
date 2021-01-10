using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class BitcoinCashTransactionInput
    {
        [JsonProperty("coinbase")]
        public string Coinbase { get; set; }

        [JsonProperty("sequence")]
        public long Sequence { get; set; }
    }
}
