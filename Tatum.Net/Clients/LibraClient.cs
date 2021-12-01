namespace Tatum.Net.Clients
{
    public class LibraClient
    {
        public TatumClient Tatum { get; protected set; }

        protected const string Endpoints_BlockchainInformation = "libra/info";
        protected const string Endpoints_GetTransactionsByAccount = "libra/account/transaction/{0}";
        protected const string Endpoints_AccountInfo = "libra/account/{0}";
        protected const string Endpoints_GetTransactions = "libra/transaction/{0}/{0}";

        public LibraClient(TatumClient tatumClient)
        {
            Tatum = tatumClient;
        }

        // N/A

    }
}
