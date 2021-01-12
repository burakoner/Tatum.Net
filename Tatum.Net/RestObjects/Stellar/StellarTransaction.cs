using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class StellarTransaction
    {
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("envelope_xdr")]
        public string EnvelopeXdr { get; set; }

        [JsonProperty("fee_account")]
        public string FeeAccount { get; set; }

        [JsonProperty("fee_charged")]
        public decimal FeeCharged { get; set; }

        [JsonProperty("fee_meta_xdr")]
        public string FeeMetaXdr { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("ledger_attr")]
        public long LedgerAttr { get; set; }

        [JsonProperty("max_fee")]
        public decimal MaxFee { get; set; }

        [JsonProperty("memo")]
        public string Memo { get; set; }

        [JsonProperty("memo_bytes")]
        public string MemoBytes { get; set; }

        [JsonProperty("memo_type")]
        public string MemoType { get; set; }

        [JsonProperty("operation_count")]
        public long OperationCount { get; set; }

        [JsonProperty("paging_token")]
        public string PagingToken { get; set; }

        [JsonProperty("result_meta_xdr")]
        public string ResultMetaXdr { get; set; }

        [JsonProperty("result_xdr")]
        public string ResultXdr { get; set; }

        [JsonProperty("signatures")]
        public IEnumerable<string> Signatures { get; set; }

        [JsonProperty("source_account")]
        public string SourceAccount { get; set; }

        [JsonProperty("source_account_sequence")]
        public string SourceAccountSequence { get; set; }

        [JsonProperty("successful")]
        public bool Successful { get; set; }

        [JsonProperty("valid_after")]
        public string ValidAfter { get; set; }
    }
}
