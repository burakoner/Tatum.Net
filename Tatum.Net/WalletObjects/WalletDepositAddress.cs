namespace Tatum.Net.WalletObjects
{
    public class WalletDepositAddress
    {
        public string ExtendedPublicKey { get; set; }
        public string SecretKey { get; set; }
        public string Mnemonics { get; set; }
        public string Address { get; set; }
        public string Tag { get; set; }
        public int? Index { get; set; }
    }
}
