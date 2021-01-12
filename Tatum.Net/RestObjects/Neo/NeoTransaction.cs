using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class NeoTransaction
    {
        [JsonProperty("txid")]
        public string TransactionId { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("version")]
        public long Version { get; set; }

        [JsonProperty("attributes")]
        public IEnumerable<NeoTransactionAttribute> Attributes { get; set; }

        [JsonProperty("vin")]
        public IEnumerable<NeoTransactionInput> Inputs { get; set; }

        [JsonProperty("vout")]
        public IEnumerable<NeoTransactionOutput> Outputs { get; set; }

        [JsonProperty("sys_fee")]
        public string SystemFee { get; set; }

        [JsonProperty("net_fee")]
        public string NetFee { get; set; }

        [JsonProperty("scripts")]
        public IEnumerable<NeoScript> Scripts { get; set; }

        [JsonProperty("script")]
        public string Script { get; set; }

        [JsonProperty("gas")]
        public string Gas { get; set; }
    }
}
