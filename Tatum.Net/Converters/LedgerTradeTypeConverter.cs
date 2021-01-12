﻿using CryptoExchange.Net.Converters;
using System.Collections.Generic;
using Tatum.Net.Enums;

namespace Tatum.Net.Converters
{
    internal class LedgerTradeTypeConverter : BaseConverter<LedgerTradeType>
    {
        public LedgerTradeTypeConverter() : this(true) { }
        public LedgerTradeTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<LedgerTradeType, string>> Mapping => new List<KeyValuePair<LedgerTradeType, string>>
        {
            new KeyValuePair<LedgerTradeType, string>(LedgerTradeType.Buy, "BUY"),
            new KeyValuePair<LedgerTradeType, string>(LedgerTradeType.Sell, "SELL"),
        };
    }
}