using System;

namespace Tatum.Net.Atrributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class BlockchainOptionsAttribute : Attribute
    {
        public string Code { get; set; }
        public string ChainSlug { get; set; }
        public bool MainnetSupport { get; set; }
        public bool TestnetSupport { get; set; }
        public bool OffchainSupport { get; set; }

        public BlockchainOptionsAttribute(string code, string slug, bool mainnetSupport, bool testnetSupport, bool offchainSupport)
        {
            Code = code;
            ChainSlug = slug;
            MainnetSupport = mainnetSupport;
            TestnetSupport = testnetSupport;
            OffchainSupport = offchainSupport;
        }

    }
}
