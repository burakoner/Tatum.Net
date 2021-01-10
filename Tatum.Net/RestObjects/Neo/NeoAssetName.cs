using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class NeoAssetName
    {
        [JsonProperty("lang")]
        public string Language { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

}
