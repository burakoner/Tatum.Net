using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class NeoScript
    {
        [JsonProperty("invocation")]
        public string Invocation { get; set; }

        [JsonProperty("verification")]
        public string Verification { get; set; }
    }
}
