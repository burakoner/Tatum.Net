using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class BitcoinBalance
    {
        [JsonProperty("incoming")]
        public decimal Incoming { get; set; }

        [JsonProperty("outgoing")]
        public decimal Outgoing { get; set; }
    }
}
