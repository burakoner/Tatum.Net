using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using Tatum.Net.Converters;
using Tatum.Net.Enums;

namespace Tatum.Net.RestObjects
{
    public class LedgerTransaction
    {
        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("counterAccountId")]
        public string CounterAccountId { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("anonymous")]
        public bool Anonymous { get; set; }

        [JsonProperty("created"), JsonConverter(typeof(TimestampConverter))]
        public DateTime Created { get; set; }

        [JsonProperty("marketValue")]
        public LedgerMarketValue MarketValue { get; set; }

        [JsonProperty("operationType"), JsonConverter(typeof(LedgerOperationTypeConverter))]
        public LedgerOperationType OperationType { get; set; }

        [JsonProperty("transactionType"), JsonConverter(typeof(LedgerTransactionTypeConverter))]
        public LedgerTransactionType TransactionType { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("transactionCode")]
        public string TransactionCode { get; set; }

        [JsonProperty("senderNote")]
        public string SenderNote { get; set; }

        [JsonProperty("recipientNote")]
        public string RecipientNote { get; set; }

        [JsonProperty("paymentId")]
        public string PaymentId { get; set; }

        [JsonProperty("attr")]
        public string Attr { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("txId")]
        public string TransactionId { get; set; }
    }
}
