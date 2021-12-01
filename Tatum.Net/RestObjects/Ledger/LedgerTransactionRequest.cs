using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using Tatum.Net.Converters;
using Tatum.Net.Enums;

namespace Tatum.Net.RestObjects
{
    public class LedgerTransactionRequest
    {
        [JsonProperty("recipientAccountId")]
        public string RecipientAccountId { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("anonymous")]
        public bool Anonymous { get; set; }
        
        [JsonProperty("compliant")]
        public bool compliant { get; set; }
        
        [JsonProperty("transactionCode")]
        public string TransactionCode { get; set; }
        
        [JsonProperty("paymentId")]
        public string PaymentId { get; set; }

        [JsonProperty("recipientNote")]
        public string RecipientNote { get; set; }

        [JsonProperty("baseRate")]
        public decimal BaseRate { get; set; }

        [JsonProperty("senderNote")]
        public string SenderNote { get; set; }
    }
}
