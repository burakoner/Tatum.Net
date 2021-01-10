using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class NeoTransactionAttribute
    {
        [JsonProperty("usage")]
        public string Usage { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }
    }
}
