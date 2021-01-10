using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class BinanceBlockTransaction
    {
        [JsonProperty("txHash")]
        public string TransactionHash { get; set; }

        [JsonProperty("blockHeight")]
        public long BlockHeight { get; set; }
        
        [JsonProperty("txType")]
        public string TransactionType { get; set; }
        
        [JsonProperty("timeStamp")]
        public string Timestamp { get; set; }
        
        [JsonProperty("fromAddr")]
        public string FromAddress { get; set; }
        
        [JsonProperty("toAddr")]
        public string ToAddress { get; set; }
        
        [JsonProperty("value")]
        public string Value { get; set; }
        
        [JsonProperty("txAsset")]
        public string TransactionAsset { get; set; }
        
        [JsonProperty("txFee")]
        public string TransactionFee { get; set; }
        
        [JsonProperty("orderId")]
        public string OrderId { get; set; }
        
        [JsonProperty("code")]
        public long Code { get; set; }
        
        [JsonProperty("data")]
        public string Data { get; set; }
        
        [JsonProperty("memo")]
        public string Memo { get; set; }
        
        [JsonProperty("source")]
        public long Source { get; set; }
        
        [JsonProperty("sequence")]
        public long Sequence { get; set; }
    }
}
