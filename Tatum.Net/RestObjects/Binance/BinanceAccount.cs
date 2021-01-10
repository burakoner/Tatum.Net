using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class BinanceAccount
    {
        [JsonProperty("account_number")]
        public long AccountNumber { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }
        
        [JsonProperty("balances")]
        public IEnumerable< BinanceAccountBalance> Balances { get; set; }

        [JsonProperty("flags")]
        public long Flags { get; set; }
        
        [JsonProperty("sequence")]
        public long Sequence { get; set; }
    }
}
