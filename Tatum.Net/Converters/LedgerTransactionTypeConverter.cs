using CryptoExchange.Net.Converters;
using System.Collections.Generic;
using Tatum.Net.Enums;

namespace Tatum.Net.Converters
{
    public class LedgerTransactionTypeConverter : BaseConverter<LedgerTransactionType>
    {
        public LedgerTransactionTypeConverter() : this(true) { }
        public LedgerTransactionTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<LedgerTransactionType, string>> Mapping => new List<KeyValuePair<LedgerTransactionType, string>>
        {
            new KeyValuePair<LedgerTransactionType, string>(LedgerTransactionType.FAILED, "FAILED"),
            new KeyValuePair<LedgerTransactionType, string>(LedgerTransactionType.DEBIT_PAYMENT, "DEBIT_PAYMENT"),
            new KeyValuePair<LedgerTransactionType, string>(LedgerTransactionType.CREDIT_PAYMENT, "CREDIT_PAYMENT"),
            new KeyValuePair<LedgerTransactionType, string>(LedgerTransactionType.CREDIT_DEPOSIT, "CREDIT_DEPOSIT"),
            new KeyValuePair<LedgerTransactionType, string>(LedgerTransactionType.DEBIT_WITHDRAWAL, "DEBIT_WITHDRAWAL"),
            new KeyValuePair<LedgerTransactionType, string>(LedgerTransactionType.CANCEL_WITHDRAWAL, "CANCEL_WITHDRAWAL"),
            new KeyValuePair<LedgerTransactionType, string>(LedgerTransactionType.DEBIT_OUTGOING_PAYMENT, "DEBIT_OUTGOING_PAYMENT"),
            new KeyValuePair<LedgerTransactionType, string>(LedgerTransactionType.EXCHANGE_BUY, "EXCHANGE_BUY"),
            new KeyValuePair<LedgerTransactionType, string>(LedgerTransactionType.EXCHANGE_SELL, "EXCHANGE_SELL"),
            new KeyValuePair<LedgerTransactionType, string>(LedgerTransactionType.DEBIT_TRANSACTION, "DEBIT_TRANSACTION"),
            new KeyValuePair<LedgerTransactionType, string>(LedgerTransactionType.CREDIT_INCOMING_PAYMENT, "CREDIT_INCOMING_PAYMENT"),
        };
    }
}