using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class ScryptaTransaction
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("blockhash")]
        public string BlockHash { get; set; }

        [JsonProperty("time"), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime Time { get; set; }

        [JsonProperty("inputs")]
        public IEnumerable<ScryptaTransactionInput> Inputs { get; set; }

        [JsonProperty("outputs")]
        public IEnumerable<ScryptaTransactionOutput> Outputs { get; set; }
    }
}
