using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class ScryptaBlock
    {
        [JsonProperty("confirmations")]
        public long Confirmations { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("time"), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime Time { get; set; }

        [JsonProperty("txs")]
        public IEnumerable<ScryptaTransaction> Transactions { get; set; }
    }
}
