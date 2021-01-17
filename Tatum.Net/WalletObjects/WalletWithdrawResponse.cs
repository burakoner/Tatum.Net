namespace Tatum.Net.WalletObjects
{
    public class WalletWithdrawResponse
    {
        public string Withdraw_TransactionId { get; set; }
        public bool Withdraw_Success { get; set; }
        
        public string Broadcast_TransactionId { get; set; }
        public bool Broadcast_Success { get; set; }
    }
}
