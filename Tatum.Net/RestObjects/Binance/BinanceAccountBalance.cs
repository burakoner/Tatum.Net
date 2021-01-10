using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class BinanceAccountBalance
    {
        [JsonProperty("free")]
        public decimal Free { get; set; }
        
        [JsonProperty("frozen")]
        public decimal Frozen { get; set; }
        
        [JsonProperty("locked")]
        public decimal Locked { get; set; }
        
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
    }
}
