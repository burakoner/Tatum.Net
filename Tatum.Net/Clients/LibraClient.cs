using Tatum.Net.Interfaces;

namespace Tatum.Net.Clients
{
    public class LibraClient : ITatumBlockchainLibraClient
    {
        public TatumClient Tatum { get; protected set; }

        #region API Endpoints

        #region Blockchain - Libra
        protected const string Endpoints_BlockchainInformation = "libra/info";
        protected const string Endpoints_GetTransactionsByAccount = "libra/account/transaction/{0}";
        protected const string Endpoints_AccountInfo = "libra/account/{0}";
        protected const string Endpoints_GetTransactions = "libra/transaction/{0}/{0}";
        #endregion

        #endregion

        public LibraClient(TatumClient tatumClient)
        {
            Tatum = tatumClient;
        }


        #region Blockchain / Libra
        // N/A
        #endregion


    }
}
