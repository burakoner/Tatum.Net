namespace Tatum.Net.Clients
{
    public class LedgerClient
    {
        public TatumClient Tatum { get; protected set; }
        public LedgerAccountClient Account { get; protected set; }
        public LedgerTransactionClient Transaction { get; protected set; }
        public LedgerCustomerClient Customer { get; protected set; }
        public LedgerVirtualCurrencyClient VirtualCurrency { get; protected set; }
        public LedgerSubscriptionClient Subscription { get; protected set; }
        public LedgerOrderBookClient OrderBook { get; protected set; }

        public LedgerClient(TatumClient tatumClient)
        {
            Tatum = tatumClient;
            Account = new LedgerAccountClient(this);
            Transaction = new LedgerTransactionClient(this);
            Customer = new LedgerCustomerClient(this);
            VirtualCurrency = new LedgerVirtualCurrencyClient(this);
            Subscription = new LedgerSubscriptionClient(this);
            OrderBook = new LedgerOrderBookClient(this);
        }
    }
}
