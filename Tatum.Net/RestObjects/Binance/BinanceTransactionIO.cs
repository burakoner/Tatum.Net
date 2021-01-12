using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class BinanceTransactionIO
    {
        [JsonProperty("address")]
        public string address { get; set; }

        [JsonProperty("coins")]
        public IEnumerable<BinanceTransactionInputCoin> Coins { get; set; }
    }
}
