using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class NeoTransactionOutput
    {
        [JsonProperty("n")]
        public long N { get; set; }

        [JsonProperty("asset")]
        public string Asset { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }
    }
}
