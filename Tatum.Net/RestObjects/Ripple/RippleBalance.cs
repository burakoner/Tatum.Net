using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class RippleBalance
    {
        [JsonProperty("balance")]
        public string Balance { get; set; }

        [JsonProperty("assets")]
        public IEnumerable<RippleAssetBalance> AssetsBalances { get; set; }
    }
}
