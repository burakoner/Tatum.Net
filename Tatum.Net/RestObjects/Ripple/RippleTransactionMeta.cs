using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class RippleTransactionMeta
    {
        [JsonProperty("AffectedNodes")]
        public IEnumerable<RippleAffectedNode> AffectedNodes { get; set; }

        [JsonProperty("TransactionIndex")]
        public long TransactionIndex { get; set; }

        [JsonProperty("TransactionResult")]
        public string TransactionResult { get; set; }

        [JsonProperty("delivered_amount")]
        public string DeliveredAmount { get; set; }
    }
}
