using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace Tatum.Net.RestObjects
{
    public class LedgerMarketValue
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("sourceDate"), JsonConverter(typeof(TimestampConverter))]
        public DateTime SourceDate { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }
    }
}
