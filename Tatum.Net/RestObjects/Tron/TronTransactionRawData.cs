using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class TronTransactionRawData
    {
        [JsonProperty("expiration"), JsonConverter(typeof(TimestampConverter))]
        public DateTime Expiration { get; set; }

        [JsonProperty("timestamp"), JsonConverter(typeof(TimestampConverter))]
        public DateTime Timestamp { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("ref_block_bytes")]
        public string ReferenceBlockBytes { get; set; }

        [JsonProperty("ref_block_hash")]
        public string ReferenceBlockHash { get; set; }

        [JsonProperty("fee_limit")]
        public decimal FeeLimit { get; set; }

        [JsonProperty("contract")]
        public IEnumerable<TronContract> Contracts { get; set; }
    }
}
