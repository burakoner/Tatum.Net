using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class TatumData
    {
        [JsonProperty("data")]
        public string Data { get; set; }
    }
}
