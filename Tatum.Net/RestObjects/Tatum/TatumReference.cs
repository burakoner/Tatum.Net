using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class TatumReference
    {
        [JsonProperty("reference")]
        public string Reference { get; set; }
    }
}
