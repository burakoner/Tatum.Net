using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class TronTRC10Token
    {
        [JsonProperty("ownerAddress")]
        public string OwnerAddress { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("abbr")]
        public string Abbreviation { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("totalSupply")]
        public decimal TotalSupply { get; set; }

        [JsonProperty("precision")]
        public int Precision { get; set; }
    }
}
