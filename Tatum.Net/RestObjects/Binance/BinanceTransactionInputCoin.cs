using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class BinanceTransactionInputCoin
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("denom")]
        public string Denom { get; set; }
    }
}
