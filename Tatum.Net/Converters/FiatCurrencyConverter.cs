using CryptoExchange.Net.Converters;
using System;
using System.Collections.Generic;
using Tatum.Net.Enums;

namespace Tatum.Net.Converters
{
    public class FiatCurrencyConverter : BaseConverter<FiatCurrency>
    {
        public FiatCurrencyConverter() : this(true) { }
        public FiatCurrencyConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<FiatCurrency, string>> Mapping
        {
            get
            {
                var lst = new List<KeyValuePair<FiatCurrency, string>>();
                foreach (FiatCurrency obj in Enum.GetValues(typeof(FiatCurrency)))
                {
                    lst.Add(new KeyValuePair<FiatCurrency, string>(obj, obj.ToString()));
                }
                return lst;
            }
        }
    }
}