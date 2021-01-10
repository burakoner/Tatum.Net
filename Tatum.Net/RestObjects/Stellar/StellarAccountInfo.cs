using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class StellarAccountInfo
    {
        [JsonProperty("account_id")]
        public string AccountId { get; set; }

        [JsonProperty("balances")]
        public IEnumerable<StellarAccountBalance> Balances { get; set; }

        [JsonProperty("data_attr")]
        public object DataAttributes { get; set; }

        [JsonProperty("flags")]
        public StellarAccountFlags Flags { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("last_modified_ledger")]
        public long LastModifiedLedger { get; set; }

        [JsonProperty("last_modified_time")]
        public string LastModifiedTime { get; set; }

        [JsonProperty("num_sponsored")]
        public long NumberOfSponsored { get; set; }

        [JsonProperty("num_sponsoring")]
        public long NumberOfSponsoring { get; set; }

        [JsonProperty("paging_token")]
        public string PagingToken { get; set; }

        [JsonProperty("sequence")]
        public string Sequence { get; set; }

        [JsonProperty("signers")]
        public IEnumerable<StellarAccountSigner> Signers { get; set; }

        [JsonProperty("subentry_count")]
        public long SubentryCount { get; set; }

        [JsonProperty("thresholds")]
        public StellarAccountThresholds Thresholds { get; set; }
    }
}
