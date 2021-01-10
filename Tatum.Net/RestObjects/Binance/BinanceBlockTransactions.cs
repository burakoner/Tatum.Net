using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class BinanceBlockTransactions
    {
        [JsonProperty("blockHeight")]
        public string BlockHeight { get; set; }

        [JsonProperty("tx")]
        public IEnumerable<BinanceBlockTransaction> Transactions { get; set; }
    }
}
