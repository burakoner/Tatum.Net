using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class TatumHash
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }
    }
}
