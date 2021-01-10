using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class RippleAccountData
    {
        [JsonProperty("Account")]
        public string Account { get; set; }
        
        [JsonProperty("AccountTxnID")]
        public string AccountTxnID { get; set; }
        
        [JsonProperty("Balance")]
        public string Balance { get; set; }
        
        [JsonProperty("Domain")]
        public string Domain { get; set; }
        
        [JsonProperty("EmailHash")]
        public string EmailHash { get; set; }
        
        [JsonProperty("Flags")]
        public long Flags { get; set; }
        
        [JsonProperty("LedgerEntryType")]
        public string LedgerEntryType { get; set; }
        
        [JsonProperty("MessageKey")]
        public string MessageKey { get; set; }
        
        [JsonProperty("OwnerCount")]
        public long OwnerCount { get; set; }
        
        [JsonProperty("PreviousTxnID")]
        public string PreviousTxnID { get; set; }
        
        [JsonProperty("PreviousTxnLgrSeq")]
        public long PreviousTxnLgrSeq { get; set; }
        
        [JsonProperty("RegularKey")]
        public string RegularKey { get; set; }
        
        [JsonProperty("Sequence")]
        public long Sequence { get; set; }
        
        [JsonProperty("TransferRate")]
        public long TransferRate { get; set; }
        
        [JsonProperty("index")]
        public string Index { get; set; }
        
        [JsonProperty("urlgravatar")]
        public string GravatarUrl { get; set; }

    }
}
