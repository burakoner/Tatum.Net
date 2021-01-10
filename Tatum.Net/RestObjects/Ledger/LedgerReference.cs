using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class LedgerReference
    {
        [JsonProperty("reference")]
        public string Reference { get; set; }
    }
}
