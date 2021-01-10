using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class RippleTransactionData
    {
        [JsonProperty("Account")]
        public string Account { get; set; }

        [JsonProperty("Amount")]
        public string Amount { get; set; }

        [JsonProperty("Destination")]
        public string Destination { get; set; }

        [JsonProperty("Fee")]
        public string Fee { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("ledger_index")]
        public long LedgerIndex { get; set; }

        [JsonProperty("TransactionType")]
        public string TransactionType { get; set; }

        [JsonProperty("Flags")]
        public long Flags { get; set; }

        [JsonProperty("OfferSequence")]
        public long OfferSequence { get; set; }
        
        [JsonProperty("LastLedgerSequence")]
        public long LastLedgerSequence { get; set; }

        [JsonProperty("Sequence")]
        public long Sequence { get; set; }

        [JsonProperty("date")]
        public long Date { get; set; }

        [JsonProperty("inLedger")]
        public long InLedger { get; set; }

        [JsonProperty("SigningPubKey")]
        public string SigningPublicKey { get; set; }

        [JsonProperty("TxnSignature")]
        public string TransactionSignature { get; set; }

        [JsonProperty("meta")]
        public RippleTransactionMeta Meta { get; set; }

        [JsonProperty("validated")]
        public bool Validated { get; set; }

        [JsonProperty("TakerGets")]
        public object TakerGets { get; set; }

        [JsonProperty("TakerPays")]
        public object TakerPays { get; set; }
    }
}
