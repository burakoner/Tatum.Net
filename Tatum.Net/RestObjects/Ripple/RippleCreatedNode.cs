using Newtonsoft.Json;
using System.Collections.Generic;
using Tatum.Net.Attributes;

namespace Tatum.Net.RestObjects
{
    public class RippleCreatedNode
    {
        [JsonProperty("LedgerEntryType")]
        public string LedgerEntryType { get; set; }

        [JsonProperty("LedgerIndex")]
        public string LedgerIndex { get; set; }

        [JsonProperty("NewFields")]
        public RippleTransactionAffectedFields NewFields { get; set; }

        [JsonProperty("FinalFields")]
        public RippleTransactionAffectedFields FinalFields { get; set; }

        [JsonProperty("PreviousFields")]
        public RippleTransactionAffectedFields PreviousFields { get; set; }

        [JsonProperty("PreviousTxnID")]
        public string PreviousTxnID { get; set; }

        [JsonProperty("PreviousTxnLgrSeq")]
        public long PreviousTxnLgrSeq { get; set; }
    }

    [JsonConverter(typeof(TypedDataConverter<RippleTransactionAffectedFields>))]
    public class RippleTransactionAffectedFields
    {
        [TypedData]
        public Dictionary<string, object> Fields { get; set; } = new Dictionary<string, object>();
    }
}
