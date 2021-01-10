using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class ScryptaSendOrderTo
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("value")]
        public decimal Value { get; set; }
    }

}
