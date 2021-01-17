using CryptoExchange.Net.Converters;
using System;
using System.Collections.Generic;
using Tatum.Net.Enums;

namespace Tatum.Net.Converters
{
    public class CountryCodeConverter : BaseConverter<CountryCode>
    {
        public CountryCodeConverter() : this(true) { }
        public CountryCodeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<CountryCode, string>> Mapping
        {
            get
            {
                var lst = new List<KeyValuePair<CountryCode, string>>();
                foreach (CountryCode obj in Enum.GetValues(typeof(CountryCode)))
                {
                    lst.Add(new KeyValuePair<CountryCode, string>(obj, obj.ToString()));
                }
                return lst;
            }
        }
    }
}