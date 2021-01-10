using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class VeChainTransaction
    {
        [JsonProperty("blockNumber")]
        public long BlockNumber { get; set; }

        [JsonProperty("blockRef")]
        public string BlockReference { get; set; }

        [JsonProperty("chainTag")]
        public string ChainTag { get; set; }

        [JsonProperty("clauses")]
        public IEnumerable<VeChainTransactionClause> Clauses { get; set; }

        [JsonProperty("delegator")]
        public string Delegator { get; set; }

        [JsonProperty("dependsOn")]
        public string DependsOn { get; set; }

        [JsonProperty("expiration")]
        public long Expiration { get; set; }

        [JsonProperty("gas")]
        public long Gas { get; set; }

        [JsonProperty("gasPriceCoef")]
        public long GasPriceCoef { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("meta")]
        public VeChainTransactionMeta Meta { get; set; }

        [JsonProperty("nonce")]
        public string Nonce { get; set; }

        [JsonProperty("origin")]
        public string Origin { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }
    }

}