using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class BitcoinCashBlock
    {
        [JsonProperty("bits")]
        public string Bits { get; set; }

        [JsonProperty("time"), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime Time { get; set; }

        [JsonProperty("nonce")]
        public long Nonce { get; set; }

        [JsonProperty("difficulty")]
        public decimal Difficulty { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("confirmations")]
        public long Confirmations { get; set; }

        [JsonProperty("strippedsize")]
        public long Strippedsize { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("version")]
        public long Version { get; set; }

        [JsonProperty("versionHex")]
        public string VersionHex { get; set; }

        [JsonProperty("merkleRoot")]
        public string MerkleRoot { get; set; }

        [JsonProperty("previousblockhash")]
        public string PreviousBlockHash { get; set; }

        [JsonProperty("nextblockhash")]
        public string NextBlockHash { get; set; }

        [JsonProperty("tx")]
        public IEnumerable<BitcoinCashTransaction> Transactions { get; set; }
    }
}
