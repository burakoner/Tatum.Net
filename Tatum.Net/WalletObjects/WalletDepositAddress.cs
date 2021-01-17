namespace Tatum.Net.WalletObjects
{
    public class WalletDepositAddress
    {
        /// <summary>
        /// Used for Bitcoin, BitcoinCash, Ethereum, Litecoin, Scrypta, VeChain
        /// </summary>
        public string ExtendedPublicKey { get; set; }

        /// <summary>
        /// Private Key for Bitcoin, BitcoinCash, Ethereum, Litecoin, Scrypta, VeChain
        /// Secret Key for Ripple, Stellar
        /// Private Key for BinanceChain, NEO, TRON
        /// </summary>
        public string PrivateKey { get; set; }

        /// <summary>
        /// Used for Bitcoin, BitcoinCash, Ethereum, Litecoin, Scrypta, VeChain
        /// </summary>
        public string Mnemonics { get; set; }

        /// <summary>
        /// Used for all
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Tag or Memo
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Used for Bitcoin, BitcoinCash, Ethereum, Litecoin, Scrypta, VeChain
        /// </summary>
        public int? Index { get; set; }
    }
}
