using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class TatumId
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
