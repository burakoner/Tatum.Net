namespace Tatum.Net.WalletObjects
{
    public class WalletBalance
    {
        /// <summary>
        /// Used for Bitcoin, Ethereum, Litecoin, VeChain
        /// </summary>
        public decimal? IncomingBalance { get; set; }

        /// <summary>
        /// Used for Bitcoin, Ethereum, Litecoin, VeChain
        /// </summary>
        public decimal? OutgoingBalance { get; set; }

        /// <summary>
        /// Used for All
        /// </summary>
        public decimal? CurrentBalance { get; set; }
    }
}
