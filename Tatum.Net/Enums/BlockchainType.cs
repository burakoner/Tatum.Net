using Tatum.Net.Atrributes;

namespace Tatum.Net.Enums
{
    public enum BlockchainType
    {
        [BlockchainOptions("NONE", "", false, false, false)]
        None,

        [BlockchainOptions("BNB", "bnb", true, true, true)]
        BinanceChain,

        [BlockchainOptions("BTC", "bitcoin", true, true, true)]
        Bitcoin,

        [BlockchainOptions("BCH", "bcash", true, true, true)]
        BitcoinCash,

        [BlockchainOptions("ETH", "ethereum", true, true, true)]
        Ethereum,

        [BlockchainOptions("LIBRA", "libra", false, true, false)]
        Libra,

        [BlockchainOptions("LTC", "litecoin", true, true, true)]
        Litecoin,

        [BlockchainOptions("NEO", "neo", true, true, false)]
        NEO,

        [BlockchainOptions("XRP", "xrp", true, true, true)]
        Ripple,

        [BlockchainOptions("LYRA", "scrypta", true, true, false)]
        Scrypta,

        [BlockchainOptions("XLM", "xlm", true, true, true)]
        Stellar,
        
        [BlockchainOptions("TRX", "tron", true, true, false)]
        TRON,

        [BlockchainOptions("VET", "vet", true, true, false)]
        VeChain,
    }


}
