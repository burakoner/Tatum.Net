using CryptoExchange.Net.Converters;
using System.Collections.Generic;
using Tatum.Net.Enums;

namespace Tatum.Net.Converters
{
    public class LedgerSubscriptionTypeConverter : BaseConverter<LedgerSubscriptionType>
    {
        public LedgerSubscriptionTypeConverter() : this(true) { }
        public LedgerSubscriptionTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<LedgerSubscriptionType, string>> Mapping => new List<KeyValuePair<LedgerSubscriptionType, string>>
        {
            new KeyValuePair<LedgerSubscriptionType, string>(LedgerSubscriptionType.ACCOUNT_BALANCE_LIMIT, "ACCOUNT_BALANCE_LIMIT"),
            new KeyValuePair<LedgerSubscriptionType, string>(LedgerSubscriptionType.TRANSACTION_HISTORY_REPORT, "TRANSACTION_HISTORY_REPORT"),
            new KeyValuePair<LedgerSubscriptionType, string>(LedgerSubscriptionType.OFFCHAIN_WITHDRAWAL, "OFFCHAIN_WITHDRAWAL"),
            new KeyValuePair<LedgerSubscriptionType, string>(LedgerSubscriptionType.COMPLETE_BLOCKCHAIN_TRANSACTION, "COMPLETE_BLOCKCHAIN_TRANSACTION"),
            new KeyValuePair<LedgerSubscriptionType, string>(LedgerSubscriptionType.ACCOUNT_INCOMING_BLOCKCHAIN_TRANSACTION, "ACCOUNT_INCOMING_BLOCKCHAIN_TRANSACTION"),
            new KeyValuePair<LedgerSubscriptionType, string>(LedgerSubscriptionType.ACCOUNT_PENDING_BLOCKCHAIN_TRANSACTION, "ACCOUNT_PENDING_BLOCKCHAIN_TRANSACTION"),
        };
    }
}