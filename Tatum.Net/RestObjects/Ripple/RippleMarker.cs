using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class RippleMarker
    {
        [JsonProperty("ledger")]
        public long Ledger { get; set; }

        [JsonProperty("seq")]
        public long Sequence { get; set; }
    }
}
