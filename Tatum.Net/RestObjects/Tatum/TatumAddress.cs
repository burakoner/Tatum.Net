using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class TatumAddress
    {
        [JsonProperty("address")]
        public string Address { get; set; }
    }
}
