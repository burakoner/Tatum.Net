using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class BinanceTransactionMessageValue
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("ordertype")]
        public string OrderType { get; set; }
        
        [JsonProperty("price")]
        public string Price { get; set; }
        
        [JsonProperty("quantity")]
        public string Quantity { get; set; }
        
        [JsonProperty("side")]
        public string Side { get; set; }
        
        [JsonProperty("timeinforce")]
        public string Timeinforce { get; set; }
        
        [JsonProperty("refid")]
        public string ReferenceId { get; set; }
        
        [JsonProperty("sender")]
        public string Sender { get; set; }
        
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        
        [JsonProperty("chain_id")]
        public string ChainId { get; set; }
        
        [JsonProperty("payload")]
        public string Payload { get; set; }
        
        [JsonProperty("sequence")]
        public string Sequence { get; set; }
        
        [JsonProperty("validator_address")]
        public string ValidatorAddress { get; set; }
        
        [JsonProperty("inputs")]
        public IEnumerable<BinanceTransactionIO> Inputs { get; set; }
        
        [JsonProperty("outputs")]
        public IEnumerable<BinanceTransactionIO> Outputs { get; set; }
    }
}
