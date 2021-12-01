using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using Tatum.Net.Converters;
using Tatum.Net.Enums;

namespace Tatum.Net.RestObjects
{
    public class LedgerChartData
    {
        [JsonProperty("timestamp"), JsonConverter(typeof(TimestampConverter))]
        public DateTime Timestamp { get; set; }

        [JsonProperty("high")]
        public decimal? High { get; set; }

        [JsonProperty("low")]
        public decimal? Low { get; set; }
        
        [JsonProperty("open")]
        public decimal? Open { get; set; }
        
        [JsonProperty("close")]
        public decimal? Close { get; set; }
        
        [JsonProperty("volume")]
        public decimal? Volume { get; set; }
    }
}
