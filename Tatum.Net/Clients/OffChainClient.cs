namespace Tatum.Net.Clients
{
    public class OffChainClient
    {
        public TatumClient Tatum { get; protected set; }
        public OffChainAccountClient Account { get; protected set; }
        public OffChainBlockchainClient Blockchain { get; protected set; }
        public OffChainWithdrawalClient Withdrawal { get; protected set; }

        public OffChainClient(TatumClient tatumClient)
        {
            Tatum = tatumClient;
            Account = new OffChainAccountClient(this);
            Blockchain = new OffChainBlockchainClient(this);
            Withdrawal = new OffChainWithdrawalClient(this);
        }
    }
}
