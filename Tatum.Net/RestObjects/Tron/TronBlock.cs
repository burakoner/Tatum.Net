using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class TronBlock
    {
        [JsonProperty("blockNumber")]
        public long BlockNumber { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("parentHash")]
        public string ParentHash { get; set; }

        [JsonProperty("timestamp"), JsonConverter(typeof(TimestampConverter))]
        public DateTime Timestamp { get; set; }

        [JsonProperty("witnessAddress")]
        public string WitnessAddress { get; set; }

        [JsonProperty("witnessSignature")]
        public string WitnessSignature { get; set; }

        [JsonProperty("transactions")]
        public IEnumerable<TronTransaction> Transactions { get; set; }
    }
}
