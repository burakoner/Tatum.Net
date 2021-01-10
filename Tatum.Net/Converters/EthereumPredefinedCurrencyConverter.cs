using CryptoExchange.Net.Converters;
using System;
using System.Collections.Generic;
using Tatum.Net.Enums;

namespace Tatum.Net.Converters
{
    internal class EthereumPredefinedCurrencyConverter : BaseConverter<EthereumPredefinedCurrency>
    {
        public EthereumPredefinedCurrencyConverter() : this(true) { }
        public EthereumPredefinedCurrencyConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<EthereumPredefinedCurrency, string>> Mapping
        {
            get
            {
                var lst = new List<KeyValuePair<EthereumPredefinedCurrency, string>>();
                foreach (EthereumPredefinedCurrency obj in Enum.GetValues(typeof(EthereumPredefinedCurrency)))
                {
                    lst.Add(new KeyValuePair<EthereumPredefinedCurrency, string>(obj, obj.ToString()));
                }
                return lst;
            }
        }
    }
}