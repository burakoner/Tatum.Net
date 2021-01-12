using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class BinanceTransactionValue
    {
        [JsonProperty("memo")]
        public string Memo { get; set; }

        [JsonProperty("msg")]
        public IEnumerable<BinanceTransactionMessage> Messages { get; set; }

        [JsonProperty("signatures")]
        public IEnumerable<BinanceTransactionSignature> Signatures { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }
    }
}
