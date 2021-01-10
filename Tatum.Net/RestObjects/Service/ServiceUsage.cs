using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class ServiceUsage
    {
        [JsonProperty("day")]
        public string Day { get; set; }

        [JsonProperty("usage")]
        public int Usage { get; set; }
    }
}
