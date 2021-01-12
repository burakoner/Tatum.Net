using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class SecurityStatus
    {
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
