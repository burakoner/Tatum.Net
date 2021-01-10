using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class BitcoinTransaction
    {
        [JsonProperty("blockNumber")]
        public long BlockNumber { get; set; }

        [JsonProperty("fee")]
        public decimal Fee { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("index")]
        public long Index { get; set; }

        [JsonProperty("inputs")]
        public IEnumerable<BitcoinTransactionInput> Inputs { get; set; }

        [JsonProperty("outputs")]
        public IEnumerable<BitcoinTransactionOutput> Outputs { get; set; }

        [JsonProperty("locktime")]
        public long LockTime { get; set; }

        [JsonProperty("mtime"), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime Time { get; set; }

        [JsonProperty("rate")]
        public decimal Rate { get; set; }

        [JsonProperty("version")]
        public long Version { get; set; }

        [JsonProperty("witnessHash")]
        public string WitnessHash { get; set; }
    }
}
