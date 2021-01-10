using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class LitecoinTransactionOutput
    {
        [JsonProperty("value")]
        public decimal Value { get; set; }

        [JsonProperty("script")]
        public string Script { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }
    }
}
