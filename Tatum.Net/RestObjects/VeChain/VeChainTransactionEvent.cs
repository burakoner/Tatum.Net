using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class VeChainTransactionEvent
    {
        [JsonProperty("address")]
        public string Sender { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("topics")]
        public IEnumerable<string> Topics { get; set; }
    }
}