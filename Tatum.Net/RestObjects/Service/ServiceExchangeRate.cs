using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace Tatum.Net.RestObjects
{
    public class ServiceExchangeRate
    {
        [JsonProperty("id")]
        public string Currency { get; set; }

        [JsonProperty("basePair")]
        public string FiatCurrency { get; set; }

        [JsonProperty("value")]
        public decimal value { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("timestamp"), JsonConverter(typeof(TimestampConverter))]
        public DateTime Timestamp { get; set; }
    }
}
