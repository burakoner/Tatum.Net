using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class RippleAccountTransactions
    {
        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("ledger_index_max")]
        public long LedgerIndexMax { get; set; }

        [JsonProperty("ledger_index_min")]
        public long LedgerIndexMin { get; set; }

        [JsonProperty("limit")]
        public long Limit { get; set; }

        [JsonProperty("marker")]
        public RippleMarker Marker { get; set; }

        [JsonProperty("transactions")]
        public IEnumerable<RippleTransaction> Transactions { get; set; }
    }
}
