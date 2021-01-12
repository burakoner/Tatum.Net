using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class TatumKey
    {
        [JsonProperty("key")]
        public string Key { get; set; }
    }
}
