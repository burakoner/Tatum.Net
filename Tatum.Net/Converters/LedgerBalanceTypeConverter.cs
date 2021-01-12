using CryptoExchange.Net.Converters;
using System.Collections.Generic;
using Tatum.Net.Enums;

namespace Tatum.Net.Converters
{
    internal class LedgerBalanceTypeConverter : BaseConverter<LedgerBalanceType>
    {
        public LedgerBalanceTypeConverter() : this(true) { }
        public LedgerBalanceTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<LedgerBalanceType, string>> Mapping => new List<KeyValuePair<LedgerBalanceType, string>>
        {
            new KeyValuePair<LedgerBalanceType, string>(LedgerBalanceType.Account, "account"),
            new KeyValuePair<LedgerBalanceType, string>(LedgerBalanceType.Available, "available"),
        };
    }
}