using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using Tatum.Net.Converters;
using Tatum.Net.Enums;

namespace Tatum.Net.RestObjects
{
    public class LedgerTrade
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type"), JsonConverter(typeof(LedgerTradeTypeConverter))]
        public LedgerTradeType Type { get; set; }

        [JsonProperty("price")]
        public decimal? Price { get; set; }

        [JsonProperty("amount")]
        public decimal? Amount { get; set; }

        [JsonProperty("pair")]
        public string Pair { get; set; }

        [JsonProperty("fill")]
        public decimal? Fill { get; set; }

        [JsonProperty("feeAccountId")]
        public string FeeAccountId { get; set; }

        [JsonProperty("fee")]
        public decimal? Fee { get; set; }

        [JsonProperty("currency1AccountId")]
        public string Currency1AccountId { get; set; }

        [JsonProperty("currency2AccountId")]
        public string Currency2AccountId { get; set; }

        [JsonProperty("created"), JsonConverter(typeof(TimestampConverter))]
        public DateTime Created { get; set; }
    }
}
