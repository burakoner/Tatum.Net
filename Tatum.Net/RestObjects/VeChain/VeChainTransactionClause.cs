using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class VeChainTransactionClause
    {
        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }
    }
}