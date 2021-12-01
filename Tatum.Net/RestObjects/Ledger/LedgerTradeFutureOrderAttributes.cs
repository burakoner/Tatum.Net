using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using Tatum.Net.Converters;
using Tatum.Net.Enums;

namespace Tatum.Net.RestObjects
{
    public class LedgerTradeFutureOrderAttributes
    {
        [JsonProperty("sealDate"), JsonConverter(typeof(TimestampConverter))]
        public DateTime SealDate { get; set; }

        [JsonProperty("percentBlock", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentBlock { get; set; }
        
        [JsonProperty("percentPenalty", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentPenalty { get; set; }
    }
}
