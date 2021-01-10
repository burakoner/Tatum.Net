using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class ServiceVersion
    {
        [JsonProperty("version")]
        public string Version { get; set; }
    }
}
