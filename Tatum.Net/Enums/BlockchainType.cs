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

        [BlockchainOptions("VET", "vet", true, true, false)]
        VeChain,

        /*
        [BlockchainOptions("GAS", "bitcoin", true, true, false)]
        GAS,

        [BlockchainOptions("PLTC", "bitcoin", true, false, true)]
        PlatonCoin,

        [BlockchainOptions("BAT", "bitcoin", true, false, true)]
        BasicAttentionToken,

        [BlockchainOptions("USDT", "bitcoin", true, false, true)]
        Tether,

        [BlockchainOptions("USDC", "bitcoin", true, false, true)]
        USDCoin,

        [BlockchainOptions("TUSD", "bitcoin", true, false, true)]
        TrueUSD,

        [BlockchainOptions("MKR", "bitcoin", true, false, true)]
        Maker,

        [BlockchainOptions("LINK", "bitcoin", true, false, true)]
        ChainLink,

        [BlockchainOptions("PAX", "bitcoin", true, false, true)]
        PaxosStandard,

        [BlockchainOptions("PAXG", "bitcoin", true, false, true)]
        PaxosGold,

        [BlockchainOptions("UNI", "bitcoin", true, false, true)]
        Uniswap,

        [BlockchainOptions("LEO", "bitcoin", true, false, true)]
        UnusSedLeo,

        [BlockchainOptions("FREE", "bitcoin", true, false, true)]
        FreeCoin,

        [BlockchainOptions("XCON", "bitcoin", true, false, true)]
        ConnectCoin,
        */
    }


}
