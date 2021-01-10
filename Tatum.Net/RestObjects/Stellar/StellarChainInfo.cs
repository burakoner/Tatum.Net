using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class StellarChainInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("paging_token")]
        public string PagingToken { get; set; }
        
        [JsonProperty("hash")]
        public string Hash { get; set; }
        
        [JsonProperty("sequence")]
        public long Sequence { get; set; }
        
        [JsonProperty("successful_transaction_count")]
        public long SuccessfulTransactionCount { get; set; }
        
        [JsonProperty("failed_transaction_count")]
        public long FailedTransactionCount { get; set; }
        
        [JsonProperty("operation_count")]
        public long OperationCount { get; set; }
        
        [JsonProperty("closed_at")]
        public string ClosedAt { get; set; }
        
        [JsonProperty("total_coins")]
        public string TotalCoins { get; set; }
        
        [JsonProperty("fee_pool")]
        public string FeePool { get; set; }
        
        [JsonProperty("base_fee_in_stroops")]
        public string BaseFeeInStroops { get; set; }
        
        [JsonProperty("base_reserve_in_stroops")]
        public long BaseReserveInStroops { get; set; }
        
        [JsonProperty("max_tx_set_size")]
        public long MaxTransactionSetSize { get; set; }
        
        [JsonProperty("tx_set_operation_count")]
        public long TransactionSetOperationCount { get; set; }
        
        [JsonProperty("protocol_version")]
        public long ProtocolVersion { get; set; }
        
        [JsonProperty("header_xdr")]
        public string HeaderXdr { get; set; }
    }
}
