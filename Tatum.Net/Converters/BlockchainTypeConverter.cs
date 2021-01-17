using CryptoExchange.Net.Converters;
using System;
using System.Collections.Generic;
using Tatum.Net.Enums;
using Tatum.Net.Helpers;

namespace Tatum.Net.Converters
{
    public class BlockchainTypeConverter : BaseConverter<BlockchainType>
    {
        public BlockchainTypeConverter() : this(true) { }
        public BlockchainTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<BlockchainType, string>> Mapping
        {
            get
            {
                var lst = new List<KeyValuePair<BlockchainType, string>>();
                foreach (BlockchainType obj in Enum.GetValues(typeof(BlockchainType)))
                {
                    var ops = obj.GetBlockchainOptions();
                    lst.Add(new KeyValuePair<BlockchainType, string>(obj, ops.Code));
                }
                return lst;
            }
        }
    }
}