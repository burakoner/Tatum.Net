using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class NeoBlock
    {
        [JsonProperty("confirmations")]
        public long Confirmations { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("index")]
        public long Index { get; set; }

        [JsonProperty("merkleroot")]
        public string MerkleRoot { get; set; }

        [JsonProperty("nextblockhash")]
        public string NextBlockHash { get; set; }

        [JsonProperty("nextconsensus")]
        public string NextConsensus { get; set; }

        [JsonProperty("nonce")]
        public string Nonce { get; set; }

        [JsonProperty("previousblockhash")]
        public string PreviousBlockHash { get; set; }

        [JsonProperty("script")]
        public NeoScript Script { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("tx")]
        public IEnumerable<NeoTransaction> Transactions { get; set; }

        [JsonProperty("version")]
        public long Version { get; set; }
    }
}
