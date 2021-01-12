using CryptoExchange.Net.Converters;
using System.Collections.Generic;
using Tatum.Net.Enums;

namespace Tatum.Net.Converters
{
    internal class LedgerOperationTypeConverter : BaseConverter<LedgerOperationType>
    {
        public LedgerOperationTypeConverter() : this(true) { }
        public LedgerOperationTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<LedgerOperationType, string>> Mapping => new List<KeyValuePair<LedgerOperationType, string>>
        {
            new KeyValuePair<LedgerOperationType, string>(LedgerOperationType.PAYMENT, "PAYMENT"),
            new KeyValuePair<LedgerOperationType, string>(LedgerOperationType.WITHDRAWAL, "WITHDRAWAL"),
            new KeyValuePair<LedgerOperationType, string>(LedgerOperationType.BLOCKCHAIN_TRANSACTION, "BLOCKCHAIN_TRANSACTION"),
            new KeyValuePair<LedgerOperationType, string>(LedgerOperationType.EXCHANGE, "EXCHANGE"),
            new KeyValuePair<LedgerOperationType, string>(LedgerOperationType.FAILED, "FAILED"),
            new KeyValuePair<LedgerOperationType, string>(LedgerOperationType.DEPOSIT, "DEPOSIT"),
            new KeyValuePair<LedgerOperationType, string>(LedgerOperationType.MINT, "MINT"),
            new KeyValuePair<LedgerOperationType, string>(LedgerOperationType.REVOKE, "REVOKE"),
        };
    }
}