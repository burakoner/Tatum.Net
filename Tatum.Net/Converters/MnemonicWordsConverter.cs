using CryptoExchange.Net.Converters;
using System;
using System.Collections.Generic;
using Tatum.Net.Enums;

namespace Tatum.Net.Converters
{
    internal class MnemonicWordsConverter : BaseConverter<MnemonicWords>
    {
        public MnemonicWordsConverter() : this(true) { }
        public MnemonicWordsConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<MnemonicWords, string>> Mapping
        {
            get
            {
                var lst = new List<KeyValuePair<MnemonicWords, string>>();
                foreach (MnemonicWords obj in Enum.GetValues(typeof(MnemonicWords)))
                {
                    lst.Add(new KeyValuePair<MnemonicWords, string>(obj, obj.ToString().ToLower().Replace("_", "")));
                }
                return lst;
            }
        }
    }
}