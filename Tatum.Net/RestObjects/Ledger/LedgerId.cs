using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class LedgerId
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
