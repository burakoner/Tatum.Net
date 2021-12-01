using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.Converters;
using Tatum.Net.Enums;
using Tatum.Net.Helpers;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Clients
{
    public class LedgerClient
    {
        public TatumClient Tatum { get; protected set; }
        public LedgerAccountClient Account { get; protected set; }
        public LedgerCustomerClient Customer { get; protected set; }
        public LedgerOrderBookClient OrderBook { get; protected set; }
        public LedgerSubscriptionClient Subscription { get; protected set; }
        public LedgerTransactionsClient Transactions { get; protected set; }
        public LedgerVirtualCurrencyClient VirtualCurrency { get; protected set; }

        public LedgerClient(TatumClient tatumClient)
        {
            Tatum = tatumClient;
            Account = new LedgerAccountClient(this);
            Customer = new LedgerCustomerClient(this);
            OrderBook = new LedgerOrderBookClient(this);
            Subscription = new LedgerSubscriptionClient(this);
            Transactions = new LedgerTransactionsClient(this);
            VirtualCurrency = new LedgerVirtualCurrencyClient(this);
        }
    }

    public class LedgerAccountClient
    {
        public LedgerClient Ledger { get; protected set; }

        protected const string Endpoints_Create = "ledger/account";
        protected const string Endpoints_List = "ledger/account";
        protected const string Endpoints_CreateBatch = "ledger/account/batch";
        protected const string Endpoints_ListByCustomer = "ledger/account/customer/{0}";
        protected const string Endpoints_GetById = "ledger/account/{0}";
        protected const string Endpoints_Update = "ledger/account/{0}";
        protected const string Endpoints_Balance = "ledger/account/{0}/balance";
        protected const string Endpoints_BlockAmount = "ledger/account/block/{0}";
        protected const string Endpoints_UnlockAmountAndTransfer = "ledger/account/block/{0}";
        protected const string Endpoints_UnblockAmount = "ledger/account/block/{0}";
        protected const string Endpoints_GetBlockedAmounts = "ledger/account/block/{0}";
        protected const string Endpoints_GetBlockedAmountById = "ledger/account/block/{0}/detail";
        protected const string Endpoints_UnblockAllBlockedAmounts = "ledger/account/block/account/{0}";
        protected const string Endpoints_ActivateLedgerAccount = "ledger/account/{0}/activate";
        protected const string Endpoints_DeactivateLedgerAccount = "ledger/account/{0}/deactivate";
        protected const string Endpoints_FreezeLedgerAccount = "ledger/account/{0}/freeze";
        protected const string Endpoints_UnfreezeLedgerAccount = "ledger/account/{0}/unfreeze";

        public LedgerAccountClient(LedgerClient ledgerClient)
        {
            Ledger = ledgerClient;
        }


        /// <summary>
        /// <b>Title:</b> Create new account<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Creates a new account for the customer. This will create an account on Tatum's private ledger. It is possible to create an account for every supported cryptocurrency, FIAT, any ERC20 token created within Tatum instance and Tatum virtual currencies. When the customer field is present, the account is added to the customer's list of accounts. When a customer does not exists, it is created as well.
        /// Every account has it's balance. Tatum supports 2 types of balances - accountBalance and availableBalance. Account balance represents all assets on the account, available and blocked. Available balance on the account represents account balance minus blocked amount on the account. Available balance should be used to determine how much can customer send or withdraw from the account.
        /// The account is always created with currency.When the currency is set, it cannot be changed.
        /// When account's currency is blockchain based currency, like BTC or ETH, account is usually created with xpub. Xpub represents extended public key of the blockchain wallet, which will be connected to this account. By adding xpub to the account, it will not connect any specific blockchain address to this account. Xpub is just a generator of addresses, not address itself. Every blockchain have different types of xpubs:
        /// - BTC - xpub can be obtained from generate wallet
        /// - LTC - xpub can be obtained from generate wallet
        /// - BCH - xpub can be obtained from generate wallet
        /// - ETH and ERC20 - xpub can be obtained from generate wallet
        /// - XRP - xpub is address field from generate account
        /// - XLM - xpub is address field from generate account
        /// - BNB - xpub is address field from generate account
        /// There are 2 options, how account can be connected to blockchain:
        /// - If xpub is present in the account, addresses are generated for the account via Create new deposit address. This is a preferred mechanism.
        /// - If xpub is not present in the account, addresses for this account are assigned manually via Assign address.This feature is used, when there are existing addresses, that should be used in Tatum.
        /// </summary>
        /// <param name="chain">Supported Blockchain by Tatum</param>
        /// <param name="options">Ledger Account Options</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<LedgerAccount> Create(BlockchainType chain, LedgerAccountOptions options = null, CancellationToken ct = default)
            => CreateAsync(chain, options, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create new account<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Creates a new account for the customer. This will create an account on Tatum's private ledger. It is possible to create an account for every supported cryptocurrency, FIAT, any ERC20 token created within Tatum instance and Tatum virtual currencies. When the customer field is present, the account is added to the customer's list of accounts. When a customer does not exists, it is created as well.
        /// Every account has it's balance. Tatum supports 2 types of balances - accountBalance and availableBalance. Account balance represents all assets on the account, available and blocked. Available balance on the account represents account balance minus blocked amount on the account. Available balance should be used to determine how much can customer send or withdraw from the account.
        /// The account is always created with currency.When the currency is set, it cannot be changed.
        /// When account's currency is blockchain based currency, like BTC or ETH, account is usually created with xpub. Xpub represents extended public key of the blockchain wallet, which will be connected to this account. By adding xpub to the account, it will not connect any specific blockchain address to this account. Xpub is just a generator of addresses, not address itself. Every blockchain have different types of xpubs:
        /// - BTC - xpub can be obtained from generate wallet
        /// - LTC - xpub can be obtained from generate wallet
        /// - BCH - xpub can be obtained from generate wallet
        /// - ETH and ERC20 - xpub can be obtained from generate wallet
        /// - XRP - xpub is address field from generate account
        /// - XLM - xpub is address field from generate account
        /// - BNB - xpub is address field from generate account
        /// There are 2 options, how account can be connected to blockchain:
        /// - If xpub is present in the account, addresses are generated for the account via Create new deposit address. This is a preferred mechanism.
        /// - If xpub is not present in the account, addresses for this account are assigned manually via Assign address.This feature is used, when there are existing addresses, that should be used in Tatum.
        /// </summary>
        /// <param name="chain">Supported Blockchain by Tatum</param>
        /// <param name="options">Ledger Account Options</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<LedgerAccount>> CreateAsync(BlockchainType chain, LedgerAccountOptions options = null, CancellationToken ct = default)
        {
            var ops = chain.GetBlockchainOptions();
            var parameters = new Dictionary<string, object> {
                { "currency", ops.Code },
            };
            if (options != null)
            {
                parameters.AddOptionalParameter("xpub", options.ExtendedPublicKey);
                parameters.AddOptionalParameter("compliant", options.Compliant);
                parameters.AddOptionalParameter("accountCode", options.AccountCode);
                parameters.AddOptionalParameter("accountingCurrency", JsonConvert.SerializeObject(options.AccountingCurrency, new FiatCurrencyConverter(false)));
                parameters.AddOptionalParameter("accountNumber", options.AccountNumber);
                parameters.AddOptionalParameter("customer", options.Customer);
            }

            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Create));
            return await Ledger.Tatum.SendTatumRequest<LedgerAccount>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> List all accounts<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// List all accounts. Also not active accounts are visible.
        /// </summary>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<LedgerAccount>> GetAccounts(int pageSize = 50, int offset = 0, CancellationToken ct = default)
            => GetAccountsAsync(pageSize, offset, ct).Result;
        /// <summary>
        /// <b>Title:</b> List all accounts<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// List all accounts. Also not active accounts are visible.
        /// </summary>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<LedgerAccount>>> GetAccountsAsync(int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);

            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };

            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_List));
            var result = await Ledger.Tatum.SendTatumRequest<IEnumerable<LedgerAccount>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<IEnumerable<LedgerAccount>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<IEnumerable<LedgerAccount>>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Create multiple accounts in a batch call<br />
        /// <b>Credits:</b> 2 credits per API call + 1 credit for every created account.<br />
        /// <b>Description:</b>
        /// Creates new accounts for the customer in a batch call.
        /// </summary>
        /// <param name="accounts">Ledger Accounts List</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<LedgerAccount>> CreateMultipleAccounts(IEnumerable<LedgerAccountOptions> accounts, CancellationToken ct = default)
            => CreateMultipleAccountsAsync(accounts, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create multiple accounts in a batch call<br />
        /// <b>Credits:</b> 2 credits per API call + 1 credit for every created account.<br />
        /// <b>Description:</b>
        /// Creates new accounts for the customer in a batch call.
        /// </summary>
        /// <param name="accounts">Ledger Accounts List</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<LedgerAccount>>> CreateMultipleAccountsAsync(IEnumerable<LedgerAccountOptions> accounts, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "accounts", accounts },
            };

            var credits = 2;
            foreach (var account in accounts) if (!string.IsNullOrEmpty(account.ExtendedPublicKey)) credits++;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_CreateBatch));
            return await Ledger.Tatum.SendTatumRequest<IEnumerable<LedgerAccount>>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> List all customer accounts<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// List all accounts associated with the customer. Only active accounts are visible.
        /// </summary>
        /// <param name="customer_id">Internal customer ID</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<LedgerAccount>> GetAllAccountsByCustomerId(string customer_id, int pageSize = 50, int offset = 0, CancellationToken ct = default)
            => GetAllAccountsByCustomerIdAsync(customer_id, pageSize, offset, ct).Result;
        /// <summary>
        /// <b>Title:</b> List all customer accounts<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// List all accounts associated with the customer. Only active accounts are visible.
        /// </summary>
        /// <param name="customer_id">Internal customer ID</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<LedgerAccount>>> GetAllAccountsByCustomerIdAsync(string customer_id, int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);

            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_ListByCustomer, customer_id));
            return await Ledger.Tatum.SendTatumRequest<IEnumerable<LedgerAccount>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get account by ID<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get active account by ID. Display all information regarding given account.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<LedgerAccount> GetAccountById(string account_id, int pageSize = 50, int offset = 0, CancellationToken ct = default)
            => GetAccountByIdAsync(account_id, pageSize, offset, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get account by ID<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get active account by ID. Display all information regarding given account.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<LedgerAccount>> GetAccountByIdAsync(string account_id, int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);

            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };

            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_GetById, account_id));
            return await Ledger.Tatum.SendTatumRequest<LedgerAccount>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Update account<br />
        /// <b>Credits:</b> 2 credit per API call.<br />
        /// <b>Description:</b>
        /// Update account by ID. Only a small number of fields can be updated.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="accountCode">For bookkeeping to distinct account purpose.</param>
        /// <param name="accountNumber">Account number from external system.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> UpdateAccount(string account_id, string accountCode, string accountNumber, CancellationToken ct = default) =>
            UpdateAccountAsync(account_id, accountCode, accountNumber, ct).Result;
        /// <summary>
        /// <b>Title:</b> Update account<br />
        /// <b>Credits:</b> 2 credit per API call.<br />
        /// <b>Description:</b>
        /// Update account by ID. Only a small number of fields can be updated.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="accountCode">For bookkeeping to distinct account purpose.</param>
        /// <param name="accountNumber">Account number from external system.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> UpdateAccountAsync(string account_id, string accountCode, string accountNumber, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "accountCode", accountCode },
                { "accountNumber", accountNumber },
            };

            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Update, account_id));
            var result = await Ledger.Tatum.SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Get account balance<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get balance for the account.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<LedgerBalance> GetAccountBalance(string account_id, CancellationToken ct = default)
            => GetAccountBalanceAsync(account_id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get account balance<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get balance for the account.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<LedgerBalance>> GetAccountBalanceAsync(string account_id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Balance, account_id));
            return await Ledger.Tatum.SendTatumRequest<LedgerBalance>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Block amount on account<br />
        /// <b>Credits:</b> 2 credit per API call.<br />
        /// <b>Description:</b>
        /// Block amount on the account. 
        /// Any number of distinct amounts can be blocked on one account. 
        /// Every new blockage has its distinct id, which is used as a reference. 
        /// When the amount is blocked, it is debited from the available balance of the account. 
        /// Account balance remains the same. Account balance represents the total amount of funds on the account. 
        /// The available balance represents the total amount of funds that can be used to perform transactions. 
        /// When an account is frozen, available balance is set to 0 minus all blockages for the account.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="amount">Amount to be blocked on account.</param>
        /// <param name="type">Type of blockage.</param>
        /// <param name="description">Description of blockage.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TatumId> BlockAmount(string account_id, decimal amount, string type, string description, CancellationToken ct = default)
            => BlockAmountAsync(account_id, amount, type, description, ct).Result;
        /// <summary>
        /// <b>Title:</b> Block amount on account<br />
        /// <b>Credits:</b> 2 credit per API call.<br />
        /// <b>Description:</b>
        /// Block amount on the account. 
        /// Any number of distinct amounts can be blocked on one account. 
        /// Every new blockage has its distinct id, which is used as a reference. 
        /// When the amount is blocked, it is debited from the available balance of the account. 
        /// Account balance remains the same. Account balance represents the total amount of funds on the account. 
        /// The available balance represents the total amount of funds that can be used to perform transactions. 
        /// When an account is frozen, available balance is set to 0 minus all blockages for the account.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="amount">Amount to be blocked on account.</param>
        /// <param name="type">Type of blockage.</param>
        /// <param name="description">Description of blockage.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TatumId>> BlockAmountAsync(string account_id, decimal amount, string type, string description, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "amount", amount.ToString() },
                { "type", type },
            };
            parameters.AddOptionalParameter("description", description);

            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_BlockAmount, account_id));
            return await Ledger.Tatum.SendTatumRequest<TatumId>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Unlock amount on account and perform transaction<br />
        /// <b>Credits:</b> 2 credit per API call.<br />
        /// <b>Description:</b>
        /// Unblock previously blocked amount on account and invoke ledger transaction from that account to different recipient. 
        /// If the request fails, amount is not unblocked. Amount to transfer can be smaller then amount of the blockage.
        /// </summary>
        /// <param name="blockage_id">Blockage ID</param>
        /// <param name="recipientAccountId">Internal recipient account ID within Tatum platform</param>
        /// <param name="amount">Amount to be sent. Amount can be smaller then the blocked amount.</param>
        /// <param name="anonymous">Anonymous transaction does not show sender account to recipient, default is false</param>
        /// <param name="compliant">Enable compliant checks. Transaction will not be processed, if compliant check fails.</param>
        /// <param name="transactionCode">For bookkeeping to distinct transaction purpose.</param>
        /// <param name="paymentId">Payment ID, External identifier of the payment, which can be used to pair transactions within Tatum accounts.</param>
        /// <param name="recipientNote">Note visible to both, sender and recipient</param>
        /// <param name="senderNote">Note visible to sender</param>
        /// <param name="baseRate">Exchange rate of the base pair. Only applicable for Tatum's Virtual currencies Ledger transactions. Override default exchange rate for the Virtual Currency. Default: 1</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TatumReference> UnlockAmountAndPerformTransaction(
            string blockage_id,
            string recipientAccountId,
            decimal amount,
            bool anonymous,
            bool compliant,
            string transactionCode,
            string paymentId,
            string recipientNote,
            string senderNote,
            decimal baseRate = 1,
            CancellationToken ct = default)
            => UnlockAmountAndPerformTransactionAsync(blockage_id, recipientAccountId, amount, anonymous, compliant, transactionCode, paymentId, recipientNote, senderNote, baseRate, ct).Result;
        /// <summary>
        /// <b>Title:</b> Unlock amount on account and perform transaction<br />
        /// <b>Credits:</b> 2 credit per API call.<br />
        /// <b>Description:</b>
        /// Unblock previously blocked amount on account and invoke ledger transaction from that account to different recipient. 
        /// If the request fails, amount is not unblocked. Amount to transfer can be smaller then amount of the blockage.
        /// </summary>
        /// <param name="blockage_id">Blockage ID</param>
        /// <param name="recipientAccountId">Internal recipient account ID within Tatum platform</param>
        /// <param name="amount">Amount to be sent. Amount can be smaller then the blocked amount.</param>
        /// <param name="anonymous">Anonymous transaction does not show sender account to recipient, default is false</param>
        /// <param name="compliant">Enable compliant checks. Transaction will not be processed, if compliant check fails.</param>
        /// <param name="transactionCode">For bookkeeping to distinct transaction purpose.</param>
        /// <param name="paymentId">Payment ID, External identifier of the payment, which can be used to pair transactions within Tatum accounts.</param>
        /// <param name="recipientNote">Note visible to both, sender and recipient</param>
        /// <param name="senderNote">Note visible to sender</param>
        /// <param name="baseRate">Exchange rate of the base pair. Only applicable for Tatum's Virtual currencies Ledger transactions. Override default exchange rate for the Virtual Currency. Default: 1</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TatumReference>> UnlockAmountAndPerformTransactionAsync(
            string blockage_id,
            string recipientAccountId,
            decimal amount,
            bool anonymous,
            bool compliant,
            string transactionCode,
            string paymentId,
            string recipientNote,
            string senderNote,
            decimal baseRate = 1,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "recipientAccountId", recipientAccountId },
                { "amount", amount.ToString(CultureInfo.InvariantCulture) },
                { "anonymous", anonymous },
                { "compliant", compliant },
                { "transactionCode", transactionCode },
                { "paymentId", paymentId },
                { "recipientNote", recipientNote },
                { "senderNote", senderNote },
                { "baseRate", baseRate },
            };

            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_UnlockAmountAndTransfer, blockage_id));
            return await Ledger.Tatum.SendTatumRequest<TatumReference>(url, HttpMethod.Put, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Unblock blocked amount on account<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Unblock previously blocked amount on account. Increase available balance on account, where amount was blocked.
        /// </summary>
        /// <param name="blockage_id">Blockage ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> UnblockAmount(string blockage_id, CancellationToken ct = default)
            => UnblockAmountAsync(blockage_id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Unblock blocked amount on account<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Unblock previously blocked amount on account. Increase available balance on account, where amount was blocked.
        /// </summary>
        /// <param name="blockage_id">Blockage ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> UnblockAmountAsync(string blockage_id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_UnblockAmount, blockage_id));
            var result = await Ledger.Tatum.SendTatumRequest<string>(url, HttpMethod.Delete, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Get blocked amounts on account<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get blocked amounts for account.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<LedgerBlockedAmount>> GetBlockedAmounts(string account_id, int pageSize = 50, int offset = 0, CancellationToken ct = default)
            => GetBlockedAmountsAsync(account_id, pageSize, offset, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get blocked amounts on account<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get blocked amounts for account.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<LedgerBlockedAmount>>> GetBlockedAmountsAsync(string account_id, int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);

            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };

            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_GetBlockedAmounts, account_id));
            return await Ledger.Tatum.SendTatumRequest<IEnumerable<LedgerBlockedAmount>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get blocked amount by ID<br />
        /// <b>Credits:</b>1 credit per API call.<br />
        /// <b>Description:</b>
        /// Gets blocked amount by id.
        /// </summary>
        /// <param name="id">Blocked amount ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<LedgerBlockedAmount> GetBlockedAmountById(string id, CancellationToken ct = default)
            => GetBlockedAmountByIdAsync(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get blocked amount by ID<br />
        /// <b>Credits:</b>1 credit per API call.<br />
        /// <b>Description:</b>
        /// Gets blocked amount by id.
        /// </summary>
        /// <param name="id">Blocked amount ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<LedgerBlockedAmount>> GetBlockedAmountByIdAsync(string id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_GetBlockedAmountById, id));
            return await Ledger.Tatum.SendTatumRequest<LedgerBlockedAmount>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: null, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Unblock all blocked amounts on account<br />
        /// <b>Credits:</b>1 credit per API call, 1 credits for each deleted blockage. 1 API call + 2 blockages = 3 credits.<br />
        /// <b>Description:</b>
        /// Unblock previously blocked amounts on account. Increase available balance on account, where amount was blocked.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> UnblockAllBlockedAmounts(string account_id, CancellationToken ct = default)
            => UnblockAllBlockedAmountsAsync(account_id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Unblock all blocked amounts on account<br />
        /// <b>Credits:</b>  credit per API call, 1 credits for each deleted blockage. 1 API call + 2 blockages = 3 credits.<br />
        /// <b>Description:</b>
        /// Unblock previously blocked amounts on account. Increase available balance on account, where amount was blocked.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> UnblockAllBlockedAmountsAsync(string account_id, CancellationToken ct = default)
        {
            var credits = 1; // 1 credit per API call, 1 credits for each deleted blockage. 1 API call + 2 blockages = 3 credits.
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_UnblockAllBlockedAmounts, account_id));
            var result = await Ledger.Tatum.SendTatumRequest<string>(url, HttpMethod.Delete, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Activate account<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Activate account.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> ActivateAccount(string account_id, CancellationToken ct = default)
            => ActivateAccountAsync(account_id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Activate account<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Activate account.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> ActivateAccountAsync(string account_id, CancellationToken ct = default)
        {
            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_ActivateLedgerAccount, account_id));
            var result = await Ledger.Tatum.SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Deactivate account<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Only accounts with zero account and available balance can be deactivated. 
        /// Deactivated accounts are not visible in the list of accounts, it is not possible to send funds to that accounts and perorm transactions. 
        /// However, they are still present in the ledger as well as all transaction history.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> DeactivateAccount(string account_id, CancellationToken ct = default)
            => DeactivateAccountAsync(account_id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Deactivate account<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Only accounts with zero account and available balance can be deactivated. 
        /// Deactivated accounts are not visible in the list of accounts, it is not possible to send funds to that accounts and perorm transactions. 
        /// However, they are still present in the ledger as well as all transaction history.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> DeactivateAccountAsync(string account_id, CancellationToken ct = default)
        {
            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_DeactivateLedgerAccount, account_id));
            var result = await Ledger.Tatum.SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Freeze account<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Freeze account will disable all outgoing transactions. 
        /// Incoming transactions on the account are available. 
        /// When an account is frozen, it's available balance is set to 0. 
        /// This operation will create new blockage of type ACCOUNT_FROZEN, which is automatically deleted, when the account is unfrozen.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> FreezeAccount(string account_id, CancellationToken ct = default)
            => FreezeAccountAsync(account_id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Freeze account<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Freeze account will disable all outgoing transactions. 
        /// Incoming transactions on the account are available. 
        /// When an account is frozen, it's available balance is set to 0. 
        /// This operation will create new blockage of type ACCOUNT_FROZEN, which is automatically deleted, when the account is unfrozen.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> FreezeAccountAsync(string account_id, CancellationToken ct = default)
        {
            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_FreezeLedgerAccount, account_id));
            var result = await Ledger.Tatum.SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Unfreeze account<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Unfreeze previously frozen account. Unfreezing non-frozen account will do no harm to the account.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> UnfreezeAccount(string account_id, CancellationToken ct = default)
            => UnfreezeAccountAsync(account_id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Unfreeze account<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Unfreeze previously frozen account. Unfreezing non-frozen account will do no harm to the account.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> UnfreezeAccountAsync(string account_id, CancellationToken ct = default)
        {
            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_UnfreezeLedgerAccount, account_id));
            var result = await Ledger.Tatum.SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }
    }

    public class LedgerCustomerClient
    {
        public LedgerClient Ledger { get; protected set; }

        protected const string Endpoints_List = "ledger/customer";
        protected const string Endpoints_Get = "ledger/customer/{0}";
        protected const string Endpoints_Update = "ledger/customer/{0}";
        protected const string Endpoints_Activate = "ledger/customer/{0}/activate";
        protected const string Endpoints_Deactivate = "ledger/customer/{0}/deactivate";
        protected const string Endpoints_Enable = "ledger/customer/{0}/enable";
        protected const string Endpoints_Disable = "ledger/customer/{0}/disable";

        public LedgerCustomerClient(LedgerClient ledgerClient)
        {
            Ledger = ledgerClient;
        }

        /// <summary>
        /// <b>Title:</b> List all customers<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// List of all customers. Also inactive an disabled customers are present.
        /// </summary>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<LedgerCustomer>> GetAllCustomers(int pageSize = 50, int offset = 0, CancellationToken ct = default)
            => GetAllCustomersAsync(pageSize, offset, ct).Result;
        /// <summary>
        /// <b>Title:</b> List all customers<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// List of all customers. Also inactive an disabled customers are present.
        /// </summary>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<LedgerCustomer>>> GetAllCustomersAsync(int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);
            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };

            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_List));
            return await Ledger.Tatum.SendTatumRequest<IEnumerable<LedgerCustomer>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get customer details<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Using anonymized external ID or internal customer ID you can access customer detail information. Internal ID is needed to call other customer related methods.
        /// </summary>
        /// <param name="id">Customer external or internal ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<LedgerCustomer> GetCustomer(string id, CancellationToken ct = default)
            => GetCustomerAsync(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get customer details<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Using anonymized external ID or internal customer ID you can access customer detail information. Internal ID is needed to call other customer related methods.
        /// </summary>
        /// <param name="id">Customer external or internal ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<LedgerCustomer>> GetCustomerAsync(string id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Get, id));
            return await Ledger.Tatum.SendTatumRequest<LedgerCustomer>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Update customer<br />
        /// <b>Credits:</b> 2 credit per API call.<br />
        /// <b>Description:</b>
        /// This method is helpful in case your primary system will change ID's or customer will change the country he/she is supposed to be in compliance with.
        /// </summary>
        /// <param name="id">Customer internal ID</param>
        /// <param name="externalId">External customer ID. If not set, it will not be updated.</param>
        /// <param name="accountingCurrency">All transaction will be accounted in this currency for all accounts. Currency can be overridden per account level. If not set, it will not be updated. ISO-4217</param>
        /// <param name="customerCountry">Country customer has to be compliant with. If not set, it will not be updated. ISO-3166-1.</param>
        /// <param name="providerCountry">Country service provider has to be compliant with. If not set, it will not be updated. ISO-3166-1</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<LedgerCustomer> UpdateCustomer(string id, string externalId, string accountingCurrency = null, string customerCountry = null, string providerCountry = null, CancellationToken ct = default)
            => UpdateCustomerAsync(id, externalId, accountingCurrency, customerCountry, providerCountry, ct).Result;
        /// <summary>
        /// <b>Title:</b> Update customer<br />
        /// <b>Credits:</b> 2 credit per API call.<br />
        /// <b>Description:</b>
        /// This method is helpful in case your primary system will change ID's or customer will change the country he/she is supposed to be in compliance with.
        /// </summary>
        /// <param name="id">Customer internal ID</param>
        /// <param name="externalId">External customer ID. If not set, it will not be updated.</param>
        /// <param name="accountingCurrency">All transaction will be accounted in this currency for all accounts. Currency can be overridden per account level. If not set, it will not be updated. ISO-4217</param>
        /// <param name="customerCountry">Country customer has to be compliant with. If not set, it will not be updated. ISO-3166-1.</param>
        /// <param name="providerCountry">Country service provider has to be compliant with. If not set, it will not be updated. ISO-3166-1</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<LedgerCustomer>> UpdateCustomerAsync(string id, string externalId, string accountingCurrency = null, string customerCountry = null, string providerCountry = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "externalId", externalId },
            };
            parameters.AddOptionalParameter("accountingCurrency", accountingCurrency);
            parameters.AddOptionalParameter("customerCountry", customerCountry);
            parameters.AddOptionalParameter("providerCountry", providerCountry);

            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Update, id));
            return await Ledger.Tatum.SendTatumRequest<LedgerCustomer>(url, HttpMethod.Put, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Activate customer<br />
        /// <b>Credits:</b> 2 credit per API call.<br />
        /// <b>Description:</b>
        /// Activated customer is able to do any operation.
        /// </summary>
        /// <param name="id">Customer internal ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> ActivateCustomer(string id, CancellationToken ct = default)
            => ActivateCustomerAsync(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Activate customer<br />
        /// <b>Credits:</b> 2 credit per API call.<br />
        /// <b>Description:</b>
        /// Activated customer is able to do any operation.
        /// </summary>
        /// <param name="id">Customer internal ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> ActivateCustomerAsync(string id, CancellationToken ct = default)
        {
            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Activate, id));
            var result = await Ledger.Tatum.SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Deactivate customer<br />
        /// <b>Credits:</b> 2 credit per API call.<br />
        /// <b>Description:</b>
        /// Deactivate customer is not able to do any operation. Customer can be deactivated only when all their accounts are already deactivated.
        /// </summary>
        /// <param name="id">Customer internal ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> DeactivateCustomer(string id, CancellationToken ct = default)
            => DeactivateCustomerAsync(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Deactivate customer<br />
        /// <b>Credits:</b> 2 credit per API call.<br />
        /// <b>Description:</b>
        /// Deactivate customer is not able to do any operation. Customer can be deactivated only when all their accounts are already deactivated.
        /// </summary>
        /// <param name="id">Customer internal ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> DeactivateCustomerAsync(string id, CancellationToken ct = default)
        {
            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Deactivate, id));
            var result = await Ledger.Tatum.SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Enable customer<br />
        /// <b>Credits:</b> 2 credit per API call.<br />
        /// <b>Description:</b>
        /// Enabled customer can perform all operations. By default all customers are enabled. All previously blocked account balances will be unblocked.
        /// </summary>
        /// <param name="id">Customer internal ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> EnableCustomer(string id, CancellationToken ct = default)
            => EnableCustomerAsync(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Enable customer<br />
        /// <b>Credits:</b> 2 credit per API call.<br />
        /// <b>Description:</b>
        /// Enabled customer can perform all operations. By default all customers are enabled. All previously blocked account balances will be unblocked.
        /// </summary>
        /// <param name="id">Customer internal ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> EnableCustomerAsync(string id, CancellationToken ct = default)
        {
            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Enable, id));
            var result = await Ledger.Tatum.SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Disable customer<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Disabled customer cannot perform end-user operations, such as create new accounts or send transactions. Available balance on all accounts is set to 0. Account balance will stay untouched.
        /// </summary>
        /// <param name="id">Customer internal ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> DisableCustomer(string id, CancellationToken ct = default)
            => DisableCustomerAsync(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Disable customer<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Disabled customer cannot perform end-user operations, such as create new accounts or send transactions. Available balance on all accounts is set to 0. Account balance will stay untouched.
        /// </summary>
        /// <param name="id">Customer internal ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> DisableCustomerAsync(string id, CancellationToken ct = default)
        {
            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Disable, id));
            var result = await Ledger.Tatum.SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }
    }
    
    public class LedgerOrderBookClient
    {
        public LedgerClient Ledger { get; protected set; }

        protected const string Endpoints_Place = "trade";
        protected const string Endpoints_Chart = "trade/chart";
        protected const string Endpoints_ListHistory = "trade/history";
        protected const string Endpoints_ListBuys = "trade/buy";
        protected const string Endpoints_ListSells = "trade/sell";
        protected const string Endpoints_Get = "trade/{0}";
        protected const string Endpoints_Cancel = "trade/{0}";
        protected const string Endpoints_CancelAll = "trade/account/{0}";

        public LedgerOrderBookClient(LedgerClient ledgerClient)
        {
            Ledger = ledgerClient;
        }

        /// <summary>
        /// <b>Title:</b> Store buy / sell trade<br />
        /// <b>Credits:</b> 2 credits for API call, 2 credits for each fill of the counter trade. 1 API call + 2 fills = 6 credits.<br />
        /// <b>Description:</b>
        /// Store new buy / sell trade. If there is trade already available to fill, fill as much trades as possible.
        /// It is possible to charge fees for the trades.Fees are an extra amount on top of the trade amount and are paid in the currency of the 1st pair to the separate fee account, e.g. for BTC/ETH pair fees will be paid in BTC.
        /// </summary>
        /// <param name="type">Type of the trade, BUY or SELL</param>
        /// <param name="price">Price to buy / sell</param>
        /// <param name="amount">Amount of the trade to be bought / sold</param>
        /// <param name="pair">Trading pair</param>
        /// <param name="currency1AccountId">ID of the account of the currency 1 trade currency</param>
        /// <param name="currency2AccountId">ID of the account of the currency 2 trade currency</param>
        /// <param name="feeAccountId">ID of the account where fee will be paid, if any. Fee will be paid from the currency 1 account.</param>
        /// <param name="fee">Percentage of the trade amount to be paid as a fee.</param>
        /// <param name="futureAttrs">Additional attributes for the future type.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TatumId> PlaceOrder(
            LedgerTradeType type,
            decimal price,
            decimal amount,
            string pair,
            string currency1AccountId,
            string currency2AccountId,
            string feeAccountId = null,
            decimal? fee = null,
            LedgerTradeFutureOrderAttributes futureAttrs = null,
            CancellationToken ct = default)
            => PlaceOrderAsync(type, price, amount, pair, currency1AccountId, currency2AccountId, feeAccountId, fee, futureAttrs, ct).Result;
        /// <summary>
        /// <b>Title:</b> Store buy / sell trade<br />
        /// <b>Credits:</b> 2 credits for API call, 2 credits for each fill of the counter trade. 1 API call + 2 fills = 6 credits.<br />
        /// <b>Description:</b>
        /// Store new buy / sell trade. If there is trade already available to fill, fill as much trades as possible.
        /// It is possible to charge fees for the trades.Fees are an extra amount on top of the trade amount and are paid in the currency of the 1st pair to the separate fee account, e.g. for BTC/ETH pair fees will be paid in BTC.
        /// </summary>
        /// <param name="type">Type of the trade, BUY or SELL</param>
        /// <param name="price">Price to buy / sell</param>
        /// <param name="amount">Amount of the trade to be bought / sold</param>
        /// <param name="pair">Trading pair</param>
        /// <param name="currency1AccountId">ID of the account of the currency 1 trade currency</param>
        /// <param name="currency2AccountId">ID of the account of the currency 2 trade currency</param>
        /// <param name="feeAccountId">ID of the account where fee will be paid, if any. Fee will be paid from the currency 1 account.</param>
        /// <param name="fee">Percentage of the trade amount to be paid as a fee.</param>
        /// <param name="futureAttrs">Additional attributes for the future type.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TatumId>> PlaceOrderAsync(
            LedgerTradeType type,
            decimal price,
            decimal amount,
            string pair,
            string currency1AccountId,
            string currency2AccountId,
            string feeAccountId = null,
            decimal? fee = null,
            LedgerTradeFutureOrderAttributes futureAttrs = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "type", JsonConvert.SerializeObject(type, new LedgerTradeTypeConverter(false)) },
                { "price", price.ToString(CultureInfo.InvariantCulture) },
                { "amount", amount.ToString(CultureInfo.InvariantCulture) },
                { "pair", pair },
                { "currency1AccountId", currency1AccountId },
                { "currency2AccountId", currency2AccountId },
            };
            parameters.AddOptionalParameter("feeAccountId", feeAccountId);
            parameters.AddOptionalParameter("fee", fee?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("attr", futureAttrs);

            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Place));
            return await Ledger.Tatum.SendTatumRequest<TatumId>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Obtain chart data from historical closed trades<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Obtain data from the closed trades for entering in the chart. Time interval is set between from and to and there is defined time frame. There can be obtained at most 200 time points in the time interval.
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="from">Start interval in UTC millis.</param>
        /// <param name="to">End interval in UTC millis.</param>
        /// <param name="frame">Time frame of the chart.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<LedgerChartData>> GetChartData(string pair, long from, long to, TimeFrame frame, CancellationToken ct = default)
            => GetChartDataAsync(pair, from, to, frame, ct).Result;
        /// <summary>
        /// <b>Title:</b> Obtain chart data from historical closed trades<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Obtain data from the closed trades for entering in the chart. Time interval is set between from and to and there is defined time frame. There can be obtained at most 200 time points in the time interval.
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="from">Start interval in UTC millis.</param>
        /// <param name="to">End interval in UTC millis.</param>
        /// <param name="frame">Time frame of the chart.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<LedgerChartData>>> GetChartDataAsync(string pair, long from, long to, TimeFrame frame, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "pair", pair },
                { "from", from },
                { "to", to },
                { "timeFrame", JsonConvert.SerializeObject(frame, new TimeFrameConverter(false)) },
            };

            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Chart));
            return await Ledger.Tatum.SendTatumRequest<IEnumerable<LedgerChartData>>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> List all historical trades<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// List all historical trades. It is possible to list all trades, trades for specific trading pair and/or account.
        /// </summary>
        /// <param name="id">Account ID. If present, only closed trades for given account will be present.</param>
        /// <param name="pair">Trade pair. If present, only closed trades on given trade pair will be present.</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<LedgerTrade>> GetHistoricalTrades(string id, string pair, int pageSize = 50, int offset = 0, CancellationToken ct = default) => GetHistoricalTradesAsync(id, pair, pageSize, offset, ct).Result;
        /// <summary>
        /// <b>Title:</b> List all historical trades<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// List all historical trades. It is possible to list all trades, trades for specific trading pair and/or account.
        /// </summary>
        /// <param name="id">Account ID. If present, only closed trades for given account will be present.</param>
        /// <param name="pair">Trade pair. If present, only closed trades on given trade pair will be present.</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<LedgerTrade>>> GetHistoricalTradesAsync(
            string id,
            string pair,
            int pageSize = 50,
            int offset = 0,
            CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);
            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };
            parameters.AddOptionalParameter("id", id);
            parameters.AddOptionalParameter("pair", pair);

            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_ListHistory));
            return await Ledger.Tatum.SendTatumRequest<IEnumerable<LedgerTrade>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }
        // TODO: List all historical trades



        /// <summary>
        /// <b>Title:</b> List all active buy trades<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// List all active buy trades.
        /// </summary>
        /// <param name="id">Account ID. If present, list current active buy trades for that account.</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<LedgerTrade>> GetBuyTrades(string id, int pageSize = 50, int offset = 0, CancellationToken ct = default) => GetBuyTradesAsync(id, pageSize, offset, ct).Result;
        /// <summary>
        /// <b>Title:</b> List all active buy trades<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// List all active buy trades.
        /// </summary>
        /// <param name="id">Account ID. If present, list current active buy trades for that account.</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<LedgerTrade>>> GetBuyTradesAsync(string id, int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);
            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };
            parameters.AddOptionalParameter("id", id);

            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_ListBuys));
            return await Ledger.Tatum.SendTatumRequest<IEnumerable<LedgerTrade>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }
        // TODO: List all active buy trades



        /// <summary>
        /// <b>Title:</b> List all active sell trades<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// List all active sell trades.
        /// </summary>
        /// <param name="id">Account ID. If present, list current active sell trades for that account.</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<LedgerTrade>> GetSellTrades(string id, int pageSize = 50, int offset = 0, CancellationToken ct = default) => GetSellTradesAsync(id, pageSize, offset, ct).Result;
        /// <summary>
        /// <b>Title:</b> List all active sell trades<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// List all active sell trades.
        /// </summary>
        /// <param name="id">Account ID. If present, list current active sell trades for that account.</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<LedgerTrade>>> GetSellTradesAsync(string id, int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);
            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };
            parameters.AddOptionalParameter("id", id);

            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_ListSells));
            return await Ledger.Tatum.SendTatumRequest<IEnumerable<LedgerTrade>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }
        // TODO: List all active sell trades





        /// <summary>
        /// <b>Title:</b> Get existing trade<br />
        /// <b>Credits:</b> 1 credit for API call<br />
        /// <b>Description:</b>
        /// Get existing opened trade.
        /// </summary>
        /// <param name="id">Trade ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<LedgerTrade> GetTrade(string id, CancellationToken ct = default) => GetTradeAsync(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get existing trade<br />
        /// <b>Credits:</b> 1 credit for API call<br />
        /// <b>Description:</b>
        /// Get existing opened trade.
        /// </summary>
        /// <param name="id">Trade ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<LedgerTrade>> GetTradeAsync(string id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Get, id));
            return await Ledger.Tatum.SendTatumRequest<LedgerTrade>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Cancel existing trade<br />
        /// <b>Credits:</b> 1 credit for API call<br />
        /// <b>Description:</b>
        /// Cancel existing trade.
        /// </summary>
        /// <param name="id">Trade ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> CancelOrder(string id, CancellationToken ct = default) => CancelOrderAsync(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Cancel existing trade<br />
        /// <b>Credits:</b> 1 credit for API call<br />
        /// <b>Description:</b>
        /// Cancel existing trade.
        /// </summary>
        /// <param name="id">Trade ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> CancelOrderAsync(string id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Cancel, id));
            var result = await Ledger.Tatum.SendTatumRequest<string>(url, HttpMethod.Delete, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Cancel all existing trades for account<br />
        /// <b>Credits:</b> 1 credit for API call, 1 credit for each cancelled trade. 1 API call + 2 cancellations = 3 credits.<br />
        /// <b>Description:</b>
        /// Cancel all trades for account.
        /// </summary>
        /// <param name="id">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> CancelAllOrders(string id, CancellationToken ct = default) => CancelAllOrdersAsync(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Cancel all existing trades for account<br />
        /// <b>Credits:</b> 1 credit for API call, 1 credit for each cancelled trade. 1 API call + 2 cancellations = 3 credits.<br />
        /// <b>Description:</b>
        /// Cancel all trades for account.
        /// </summary>
        /// <param name="id">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> CancelAllOrdersAsync(string id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_CancelAll, id));
            var result = await Ledger.Tatum.SendTatumRequest<string>(url, HttpMethod.Delete, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }
    }

    public class LedgerSubscriptionClient
    {
        public LedgerClient Ledger { get; protected set; }

        protected const string Endpoints_Create = "subscription";
        protected const string Endpoints_List = "subscription";
        protected const string Endpoints_EnableHMAC = "subscription";
        protected const string Endpoints_DisableHMAC = "subscription";
        protected const string Endpoints_Cancel = "subscription/{0}";
        protected const string Endpoints_Report = "subscription/report/{0}";

        public LedgerSubscriptionClient(LedgerClient ledgerClient)
        {
            Ledger = ledgerClient;
        }

        /// <summary>
        /// <b>Title:</b> Create new subscription<br />
        /// <b>Credits:</b> 2 credits for API call. Every type of subscription has different credit pricing.<br />
        /// <b>Description:</b>
        /// Create new subscription. Currently Tatum support multiple subscription types:
        /// <b>OFFCHAIN_WITHDRAWAL</b> - Off-chain scanning of outgoing transactions for BTC, BCH, LTC and ETH blockchains - by default addresses in registered for off-chain scanning are synchronized only against incoming transactions.By enabling this feature, off-chain accounts with connected blockchain addresses will be scanned also for outgoing transactions. 5 credits will be debited for every address registered for scanning every day.
        /// <b>ACCOUNT_BALANCE_LIMIT</b> - Report with all account balances above desired limit.
        /// <b>TRANSACTION_HISTORY_REPORT</b> - Report with all ledger transactions for last X hours, where X is set by the subscription attribute as interval.Maximum number of transactions returned by this report is 20000. Transactions are obtained from the time of the invocation of the GET method to obtain report - X hours.
        /// <b>ACCOUNT_INCOMING_BLOCKCHAIN_TRANSACTION</b> - Enable HTTP POST JSON notifications on incoming blockchain transactions on off-chain accounts. This web hook will be invoked, when the transaction is credited to the ledger account.Transaction is credited, when it has sufficient amount of blockchain confirmations. For BTC, LTC, BCH - 2 confirmations, ETH, ERC20 tokens, XLM, XRP, BNB - 1 confirmation.Request body of the POST request will be JSON object with attributes:
        /// - id (account id)
        /// - currency (account currency)
        /// - amount
        /// - date
        /// - txId (hash of the blockchain transaction).
        /// 1 credit will be debited for every monitored account every day.
        /// <b>ACCOUNT_PENDING_BLOCKCHAIN_TRANSACTION</b> - Enable HTTP POST JSON notifications on incoming blockchain transactions on off-chain accounts.This web hook will be invoked, when the transaction appears for the first time in the block, but is stil not credited to the ledger account, because it does not have enough confirmations.This web hook is invoked only for BTC, LTC and BCH accounts. Request body of the POST request will be JSON object with attributes:
        /// - id (account id)
        /// - currency (account currency)
        /// - amount
        /// - date
        /// - txId (hash of the blockchain transaction).
        /// 1 credit will be debited for every monitored account every day.
        /// <b>COMPLETE_BLOCKCHAIN_TRANSACTION</b> - Enable HTTP POST JSON notifications when blockchain transactions are included in the block.Request body of the POST request will be JSON object with attributes:
        /// - currency (blockchain)
        /// - withdrawalId (present, if transactin was connected to the offchain withdrawal transaction)
        /// - txId(hash of the blockchain transaction).
        /// 20 credit will be debited for every monitored transaction.
        /// Result of the operation is subscription ID, which can be used to cancel subscription or obtain additional data connected to it like reports.
        /// </summary>
        /// <param name="type">Type of the subscription.</param>
        /// <param name="account_id">ID of the account, on which the webhook will be applied, when new incoming blockchain transaction will be credited.</param>
        /// <param name="url">URL of the endpoint, where HTTP POST request will be sent, when new incoming blockchain transaction will be credited.</param>
        /// <param name="currency">Currency of the accounts, on which outgoing off-chain scanning will be enabled. BTC, LTC, BCH, ETH and ERC20 tokens are allowed.</param>
        /// <param name="interval">Number of hours to obtain transactions for.</param>
        /// <param name="limit">Limit to filter accounts with balance above it.</param>
        /// <param name="typeOfBalance">Type of balance to filter.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TatumId> Create(
            LedgerSubscriptionType type,
            string account_id = null,
            string url = null,
            string currency = null,
            int? interval = null,
            decimal? limit = null,
            LedgerBalanceType? typeOfBalance = null,
            CancellationToken ct = default)
            => CreateAsync(type, account_id, url, currency, interval, limit, typeOfBalance, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create new subscription<br />
        /// <b>Credits:</b> 2 credits for API call. Every type of subscription has different credit pricing.<br />
        /// <b>Description:</b>
        /// Create new subscription. Currently Tatum support multiple subscription types:
        /// <b>OFFCHAIN_WITHDRAWAL</b> - Off-chain scanning of outgoing transactions for BTC, BCH, LTC and ETH blockchains - by default addresses in registered for off-chain scanning are synchronized only against incoming transactions.By enabling this feature, off-chain accounts with connected blockchain addresses will be scanned also for outgoing transactions. 5 credits will be debited for every address registered for scanning every day.
        /// <b>ACCOUNT_BALANCE_LIMIT</b> - Report with all account balances above desired limit.
        /// <b>TRANSACTION_HISTORY_REPORT</b> - Report with all ledger transactions for last X hours, where X is set by the subscription attribute as interval.Maximum number of transactions returned by this report is 20000. Transactions are obtained from the time of the invocation of the GET method to obtain report - X hours.
        /// <b>ACCOUNT_INCOMING_BLOCKCHAIN_TRANSACTION</b> - Enable HTTP POST JSON notifications on incoming blockchain transactions on off-chain accounts. This web hook will be invoked, when the transaction is credited to the ledger account.Transaction is credited, when it has sufficient amount of blockchain confirmations. For BTC, LTC, BCH - 2 confirmations, ETH, ERC20 tokens, XLM, XRP, BNB - 1 confirmation.Request body of the POST request will be JSON object with attributes:
        /// - id (account id)
        /// - currency (account currency)
        /// - amount
        /// - date
        /// - txId (hash of the blockchain transaction).
        /// 1 credit will be debited for every monitored account every day.
        /// <b>ACCOUNT_PENDING_BLOCKCHAIN_TRANSACTION</b> - Enable HTTP POST JSON notifications on incoming blockchain transactions on off-chain accounts.This web hook will be invoked, when the transaction appears for the first time in the block, but is stil not credited to the ledger account, because it does not have enough confirmations.This web hook is invoked only for BTC, LTC and BCH accounts. Request body of the POST request will be JSON object with attributes:
        /// - id (account id)
        /// - currency (account currency)
        /// - amount
        /// - date
        /// - txId (hash of the blockchain transaction).
        /// 1 credit will be debited for every monitored account every day.
        /// <b>COMPLETE_BLOCKCHAIN_TRANSACTION</b> - Enable HTTP POST JSON notifications when blockchain transactions are included in the block.Request body of the POST request will be JSON object with attributes:
        /// - currency (blockchain)
        /// - withdrawalId (present, if transactin was connected to the offchain withdrawal transaction)
        /// - txId(hash of the blockchain transaction).
        /// 20 credit will be debited for every monitored transaction.
        /// Result of the operation is subscription ID, which can be used to cancel subscription or obtain additional data connected to it like reports.
        /// </summary>
        /// <param name="type">Type of the subscription.</param>
        /// <param name="account_id">ID of the account, on which the webhook will be applied, when new incoming blockchain transaction will be credited.</param>
        /// <param name="url">URL of the endpoint, where HTTP POST request will be sent, when new incoming blockchain transaction will be credited.</param>
        /// <param name="currency">Currency of the accounts, on which outgoing off-chain scanning will be enabled. BTC, LTC, BCH, ETH and ERC20 tokens are allowed.</param>
        /// <param name="interval">Number of hours to obtain transactions for.</param>
        /// <param name="limit">Limit to filter accounts with balance above it.</param>
        /// <param name="typeOfBalance">Type of balance to filter.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TatumId>> CreateAsync(
            LedgerSubscriptionType type,
            string account_id = null,
            string url = null,
            string currency = null,
            int? interval = null,
            decimal? limit = null,
            LedgerBalanceType? typeOfBalance = null,
            CancellationToken ct = default)
        {
            var lsa = new LedgerSubscriptionAttributes();
            if (type == LedgerSubscriptionType.ACCOUNT_BALANCE_LIMIT)
            {
                if (limit == null || typeOfBalance == null)
                    throw new ArgumentException("limit and typeOfBalance parameters are mandatory for ACCOUNT_BALANCE_LIMIT subscription type");

                lsa.Limit = limit?.ToString(CultureInfo.InvariantCulture);
                lsa.TypeOfBalance = typeOfBalance;
            }
            else if (type == LedgerSubscriptionType.TRANSACTION_HISTORY_REPORT)
            {
                if (interval == null)
                    throw new ArgumentException("linterval parameter is mandatory for TRANSACTION_HISTORY_REPORT subscription type");

                lsa.Interval = interval;
            }
            else if (type == LedgerSubscriptionType.OFFCHAIN_WITHDRAWAL)
            {
                if (string.IsNullOrEmpty(currency))
                    throw new ArgumentException("currency parameter is mandatory for OFFCHAIN_WITHDRAWAL subscription type");

                lsa.Currency = currency;
            }
            else if (type == LedgerSubscriptionType.COMPLETE_BLOCKCHAIN_TRANSACTION)
            {
                if (string.IsNullOrEmpty(currency))
                    throw new ArgumentException("currency parameter is mandatory for COMPLETE_BLOCKCHAIN_TRANSACTION subscription type");

                lsa.Currency = currency;
            }
            else if (type == LedgerSubscriptionType.ACCOUNT_INCOMING_BLOCKCHAIN_TRANSACTION)
            {
                if (string.IsNullOrEmpty(account_id) || string.IsNullOrEmpty(url))
                    throw new ArgumentException("account_id and url parameters are mandatory for ACCOUNT_INCOMING_BLOCKCHAIN_TRANSACTION subscription type");

                lsa.AccountId = account_id;
                lsa.Url = url;
            }
            else if (type == LedgerSubscriptionType.ACCOUNT_PENDING_BLOCKCHAIN_TRANSACTION)
            {
                if (string.IsNullOrEmpty(account_id) || string.IsNullOrEmpty(url))
                    throw new ArgumentException("account_id and url parameters are mandatory for ACCOUNT_INCOMING_BLOCKCHAIN_TRANSACTION subscription type");

                lsa.AccountId = account_id;
                lsa.Url = url;
            }

            var parameters = new Dictionary<string, object> {
                { "type", JsonConvert.SerializeObject(type, new LedgerSubscriptionTypeConverter(false)) },
                { "attr", lsa },
            };

            var credits = 2;
            var url_ = Ledger.Tatum.GetUrl(string.Format(Endpoints_Create));
            return await Ledger.Tatum.SendTatumRequest<TatumId>(url_, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> List all active subscriptions<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// List all active subscriptions.
        /// </summary>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<LedgerSubscription>> List(int pageSize = 50, int offset = 0, CancellationToken ct = default) => ListAsync(pageSize, offset, ct).Result;
        /// <summary>
        /// <b>Title:</b> List all active subscriptions<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// List all active subscriptions.
        /// </summary>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<LedgerSubscription>>> ListAsync(int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);

            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };

            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_List));
            return await Ledger.Tatum.SendTatumRequest<IEnumerable<LedgerSubscription>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Enable HMAC webhook digest<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Enable HMAC hash ID on the fired webhooks from Tatum API. In order to make sure that a webhook is sent by us, we have the possibility to sign it with the HMAC Sha512 Hex algorithm.
        /// To verify that a webhook is sent by us
        /// 1. Get a webhook x-payload-hash header value and payload as it is as a JSON file.
        /// 2. Convert the HTTP webhook body to stringify JSON without any spaces.In JavaScript, you would do it like this
        /// JSON.stringify(req.body)
        /// 3. Perform calculations on your side to create a digest using Secret Key, webhook payload in bytes and HMAC SHA512 algorithm.JavaScript example:
        /// require('crypto').createHmac('sha512', hmacSecret).update(JSON.stringify(req.body)).digest('base64')
        /// 4. Compare x-payload-hash header value with calculated digest as a Base64 string.
        /// </summary>
        /// <param name="hmacSecret">Your HMAC secret password, which is used for signing the webhook payload.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> EnableHmac(string hmacSecret, CancellationToken ct = default)
            => EnableHmacAsync(hmacSecret, ct).Result;
        /// <summary>
        /// <b>Title:</b> Enable HMAC webhook digest<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Enable HMAC hash ID on the fired webhooks from Tatum API. In order to make sure that a webhook is sent by us, we have the possibility to sign it with the HMAC Sha512 Hex algorithm.
        /// To verify that a webhook is sent by us
        /// 1. Get a webhook x-payload-hash header value and payload as it is as a JSON file.
        /// 2. Convert the HTTP webhook body to stringify JSON without any spaces.In JavaScript, you would do it like this
        /// JSON.stringify(req.body)
        /// 3. Perform calculations on your side to create a digest using Secret Key, webhook payload in bytes and HMAC SHA512 algorithm.JavaScript example:
        /// require('crypto').createHmac('sha512', hmacSecret).update(JSON.stringify(req.body)).digest('base64')
        /// 4. Compare x-payload-hash header value with calculated digest as a Base64 string.
        /// </summary>
        /// <param name="hmacSecret">Your HMAC secret password, which is used for signing the webhook payload.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> EnableHmacAsync(string hmacSecret, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "hmacSecret", hmacSecret },
            };

            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_EnableHMAC));
            var result = await Ledger.Tatum.SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, parameters: parameters, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Disable HMAC hash ID on the fired webhooks from Tatum API.<br />
        /// <b>Credits:</b> 2 credit per API call.<br />
        /// <b>Description:</b>
        /// List all active subscriptions.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> DisableHmac(CancellationToken ct = default)
            => DisableHmacAsync(ct).Result;
        /// <summary>
        /// <b>Title:</b> Disable HMAC hash ID on the fired webhooks from Tatum API.<br />
        /// <b>Credits:</b> 2 credit per API call.<br />
        /// <b>Description:</b>
        /// List all active subscriptions.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> DisableHmacAsync(CancellationToken ct = default)
        {
            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_EnableHMAC));
            var result = await Ledger.Tatum.SendTatumRequest<string>(url, HttpMethod.Delete, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

























        /// <summary>
        /// <b>Title:</b> Cancel existing subscription<br />
        /// <b>Credits:</b> 1 credit for API call<br />
        /// <b>Description:</b>
        /// Cancel existing subscription.
        /// </summary>
        /// <param name="id">Subscription ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> Cancel(string id, CancellationToken ct = default) => CancelAsync(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Cancel existing subscription<br />
        /// <b>Credits:</b> 1 credit for API call<br />
        /// <b>Description:</b>
        /// Cancel existing subscription.
        /// </summary>
        /// <param name="id">Subscription ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> CancelAsync(string id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Cancel, id));
            var result = await Ledger.Tatum.SendTatumRequest<string>(url, HttpMethod.Delete, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Obtain report for subscription<br />
        /// <b>Credits:</b> 1 credit for API call. Based on the required report type, additional credits may be charged.<br />
        /// <b>Description:</b>
        /// Obtain report from subscription based on its type. Following reports are supported:
        /// - ACCOUNT_BALANCE_LIMIT - obtain list of all ledger accounts with account balance above the limit. 1 credit per 50 returned records is charged.
        /// - TRANSACTION_HISTORY_REPORT - obtain list of all ledger transaction for last X hours from the time of invocation. 1 credit per 50 returned records is charged.
        /// </summary>
        /// <param name="id">Subscription ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<LedgerReport>> GetReport(string id, CancellationToken ct = default) => GetReportAsync(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Obtain report for subscription<br />
        /// <b>Credits:</b> 1 credit for API call. Based on the required report type, additional credits may be charged.<br />
        /// <b>Description:</b>
        /// Obtain report from subscription based on its type. Following reports are supported:
        /// - ACCOUNT_BALANCE_LIMIT - obtain list of all ledger accounts with account balance above the limit. 1 credit per 50 returned records is charged.
        /// - TRANSACTION_HISTORY_REPORT - obtain list of all ledger transaction for last X hours from the time of invocation. 1 credit per 50 returned records is charged.
        /// </summary>
        /// <param name="id">Subscription ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<LedgerReport>>> GetReportAsync(string id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Report, id));
            return await Ledger.Tatum.SendTatumRequest<IEnumerable<LedgerReport>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }
    }

    public class LedgerTransactionsClient
    {
        public LedgerClient Ledger { get; protected set; }

        protected const string Endpoints_SendPayment = "ledger/transaction";
        protected const string Endpoints_SendPaymentInBatch = "ledger/transaction/batch";
        protected const string Endpoints_GetTransactionsByAccount = "ledger/transaction/account";
        protected const string Endpoints_GetTransactionsByCustomer = "ledger/transaction/customer";
        protected const string Endpoints_GetTransactionsByLedger = "ledger/transaction/ledger";
        protected const string Endpoints_GetTransactionsByReference = "ledger/transaction/reference/{0}";

        public LedgerTransactionsClient(LedgerClient ledgerClient)
        {
            Ledger = ledgerClient;
        }

        /// <summary>
        /// <b>Title:</b> Send payment<br />
        /// <b>Credits:</b> 4 credits per API call.<br />
        /// <b>Description:</b>
        /// Send payment within the Tatum's ledger. All assets are settled instantly.
        /// When transaction is settled, 2 transaction records are created, 1 for each of the participants.These 2 records are connected together via transaction reference, which is the same for both of them.
        /// This method is used only for transferring assets between Tatum's accounts and will not send any funds to blockchain addresses.
        /// If there is insufficient balance on sender account, no transactions are stored.
        /// It is possible to perform an anonymous transaction - sender account is not visible for the recipient.
        /// Every transaction has it's value in the FIAT currency calculated automatically. FIAT value is based on the accountingCurrency of the account connected to the transaction and is available in the marketValue parameter of the transaction.
        /// </summary>
        /// <param name="senderAccountId">Internal sender account ID within Tatum platform</param>
        /// <param name="recipientAccountId">Internal recipient account ID within Tatum platform</param>
        /// <param name="amount">Amount to be sent.</param>
        /// <param name="anonymous">Anonymous transaction does not show sender account to recipient, default is false</param>
        /// <param name="compliant">Enable compliant checks. Transaction will not be processed, if compliant check fails.</param>
        /// <param name="transactionCode">For bookkeeping to distinct transaction purpose.</param>
        /// <param name="paymentId">Payment ID, External identifier of the payment, which can be used to pair transactions within Tatum accounts.</param>
        /// <param name="recipientNote">Note visible to both, sender and recipient</param>
        /// <param name="baseRate">Exchange rate of the base pair. Only applicable for Tatum's Virtual currencies Ledger transactions. Override default exchange rate for the Virtual Currency.</param>
        /// <param name="senderNote">Note visible to sender</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TatumReference> SendPayment(
            string senderAccountId,
            string recipientAccountId,
            decimal amount,
            bool anonymous = false,
            bool? compliant = null,
            string transactionCode = null,
            string paymentId = null,
            string recipientNote = null,
            decimal baseRate = 1.0m,
            string senderNote = null,
            CancellationToken ct = default)
            => SendPaymentAsync(senderAccountId, recipientAccountId, amount, anonymous, compliant, transactionCode, paymentId, recipientNote, baseRate, senderNote, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send payment<br />
        /// <b>Credits:</b> 4 credits per API call.<br />
        /// <b>Description:</b>
        /// Send payment within the Tatum's ledger. All assets are settled instantly.
        /// When transaction is settled, 2 transaction records are created, 1 for each of the participants.These 2 records are connected together via transaction reference, which is the same for both of them.
        /// This method is used only for transferring assets between Tatum's accounts and will not send any funds to blockchain addresses.
        /// If there is insufficient balance on sender account, no transactions are stored.
        /// It is possible to perform an anonymous transaction - sender account is not visible for the recipient.
        /// Every transaction has it's value in the FIAT currency calculated automatically. FIAT value is based on the accountingCurrency of the account connected to the transaction and is available in the marketValue parameter of the transaction.
        /// </summary>
        /// <param name="senderAccountId">Internal sender account ID within Tatum platform</param>
        /// <param name="recipientAccountId">Internal recipient account ID within Tatum platform</param>
        /// <param name="amount">Amount to be sent.</param>
        /// <param name="anonymous">Anonymous transaction does not show sender account to recipient, default is false</param>
        /// <param name="compliant">Enable compliant checks. Transaction will not be processed, if compliant check fails.</param>
        /// <param name="transactionCode">For bookkeeping to distinct transaction purpose.</param>
        /// <param name="paymentId">Payment ID, External identifier of the payment, which can be used to pair transactions within Tatum accounts.</param>
        /// <param name="recipientNote">Note visible to both, sender and recipient</param>
        /// <param name="baseRate">Exchange rate of the base pair. Only applicable for Tatum's Virtual currencies Ledger transactions. Override default exchange rate for the Virtual Currency.</param>
        /// <param name="senderNote">Note visible to sender</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TatumReference>> SendPaymentAsync(
            string senderAccountId,
            string recipientAccountId,
            decimal amount,
            bool anonymous = false,
            bool? compliant = null,
            string transactionCode = null,
            string paymentId = null,
            string recipientNote = null,
            decimal baseRate = 1.0m,
            string senderNote = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "senderAccountId", senderAccountId },
                { "recipientAccountId", recipientAccountId },
                { "amount", amount.ToString(CultureInfo.InvariantCulture) },
                { "anonymous", anonymous },
                { "baseRate", baseRate },
            };
            parameters.AddOptionalParameter("compliant", compliant);
            parameters.AddOptionalParameter("transactionCode", transactionCode);
            parameters.AddOptionalParameter("paymentId", paymentId);
            parameters.AddOptionalParameter("recipientNote", recipientNote);
            parameters.AddOptionalParameter("senderNote", senderNote);

            var credits = 4;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_SendPayment));
            return await Ledger.Tatum.SendTatumRequest<TatumReference>(url, HttpMethod.Post, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Send payment in batch<br />
        /// <b>Credits:</b> 2 + 2 * N per API call. (N - count of transactions)<br />
        /// <b>Description:</b>
        /// Sends the N payments within Tatum Private Ledger. All assets are settled instantly.
        /// When a transaction is settled, 2 transaction records are created, 1 for each of the participants.These 2 records are connected via a transaction reference, which is the same for both of them.
        /// This method is only used for transferring assets between accounts within Tatum and will not send any funds to blockchain addresses.
        /// If there is an insufficient balance in the sender account, no transaction is recorded.
        /// It is possible to perform an anonymous transaction where the sender account is not visible for the recipient.
        /// The FIAT currency value of every transaction is calculated automatically. The FIAT value is based on the accountingCurrency of the account connected to the transaction and is available in the marketValue parameter of the transaction.
        /// </summary>
        /// <param name="senderAccountId">Internal sender account ID within Tatum platform</param>
        /// <param name="transactions">Transactions List</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<TatumReference>> SendPaymentsInBatch(string senderAccountId, IEnumerable<LedgerTransactionRequest> transactions, CancellationToken ct = default)
            => SendPaymentsInBatchAsync(senderAccountId, transactions, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send payment in batch<br />
        /// <b>Credits:</b> 2 + 2 * N per API call. (N - count of transactions)<br />
        /// <b>Description:</b>
        /// Sends the N payments within Tatum Private Ledger. All assets are settled instantly.
        /// When a transaction is settled, 2 transaction records are created, 1 for each of the participants.These 2 records are connected via a transaction reference, which is the same for both of them.
        /// This method is only used for transferring assets between accounts within Tatum and will not send any funds to blockchain addresses.
        /// If there is an insufficient balance in the sender account, no transaction is recorded.
        /// It is possible to perform an anonymous transaction where the sender account is not visible for the recipient.
        /// The FIAT currency value of every transaction is calculated automatically. The FIAT value is based on the accountingCurrency of the account connected to the transaction and is available in the marketValue parameter of the transaction.
        /// </summary>
        /// <param name="senderAccountId">Internal sender account ID within Tatum platform</param>
        /// <param name="transactions">Transactions List</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<TatumReference>>> SendPaymentsInBatchAsync(string senderAccountId, IEnumerable<LedgerTransactionRequest> transactions, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "senderAccountId", senderAccountId },
                { "transaction", transactions },
            };

            var credits = 2 + 2 * transactions.Count();
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_SendPaymentInBatch));
            return await Ledger.Tatum.SendTatumRequest<IEnumerable<TatumReference>>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Find transactions for account.<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Find transactions for account identified by given account id.
        /// </summary>
        /// <param name="id">Account ID - source of transaction(s).</param>
        /// <param name="counterAccount">Counter account - transaction(s) destination account.</param>
        /// <param name="from">Starting date to search for transactions from in UTC millis. If not present, search all history.</param>
        /// <param name="to">Date until to search for transactions in UTC millis. If not present, search up till now.</param>
        /// <param name="currency">Currency of the transactions.</param>
        /// <param name="transactionType">Type of payment</param>
        /// <param name="opType">Type of operation</param>
        /// <param name="transactionCode">For bookkeeping to distinct transaction purpose.</param>
        /// <param name="paymentId">Payment ID defined in payment order by sender.</param>
        /// <param name="recipientNote">Recipient note defined in payment order by sender.</param>
        /// <param name="senderNote">Sender note defined in payment order by sender.</param>
        /// <param name="pageSize">Max number of items per page is 50. Either count, or pageSize is accepted.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="count">Get total count of transactions based on the filter. Either count, or pageSize is accepted.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<LedgerTransaction>> GetTransactionsByAccount(
            string id,
            string counterAccount = null,
            DateTime? from = null,
            DateTime? to = null,
            string currency = null,
            LedgerTransactionType? transactionType = null,
            LedgerOperationType? opType = null,
            string transactionCode = null,
            string paymentId = null,
            string recipientNote = null,
            string senderNote = null,
            int pageSize = 50,
            int offset = 0,
            bool? count = null,
            CancellationToken ct = default)
            => GetTransactionsByAccountAsync(id, counterAccount, from, to, currency, transactionType, opType, transactionCode, paymentId, recipientNote, senderNote, pageSize, offset, count, ct).Result;
        /// <summary>
        /// <b>Title:</b> Find transactions for account.<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Find transactions for account identified by given account id.
        /// </summary>
        /// <param name="id">Account ID - source of transaction(s).</param>
        /// <param name="counterAccount">Counter account - transaction(s) destination account.</param>
        /// <param name="from">Starting date to search for transactions from in UTC millis. If not present, search all history.</param>
        /// <param name="to">Date until to search for transactions in UTC millis. If not present, search up till now.</param>
        /// <param name="currency">Currency of the transactions.</param>
        /// <param name="transactionType">Type of payment</param>
        /// <param name="opType">Type of operation</param>
        /// <param name="transactionCode">For bookkeeping to distinct transaction purpose.</param>
        /// <param name="paymentId">Payment ID defined in payment order by sender.</param>
        /// <param name="recipientNote">Recipient note defined in payment order by sender.</param>
        /// <param name="senderNote">Sender note defined in payment order by sender.</param>
        /// <param name="pageSize">Max number of items per page is 50. Either count, or pageSize is accepted.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="count">Get total count of transactions based on the filter. Either count, or pageSize is accepted.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<LedgerTransaction>>> GetTransactionsByAccountAsync(
            string id,
            string counterAccount = null,
            DateTime? from = null,
            DateTime? to = null,
            string currency = null,
            LedgerTransactionType? transactionType = null,
            LedgerOperationType? opType = null,
            string transactionCode = null,
            string paymentId = null,
            string recipientNote = null,
            string senderNote = null,
            int pageSize = 50,
            int offset = 0,
            bool? count = null,
            CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);

            var parameters = new Dictionary<string, object>
            {
                { "id", id },
            };
            parameters.AddOptionalParameter("counterAccount", counterAccount);
            parameters.AddOptionalParameter("from", from?.ToUnixTimeMilliseconds());
            parameters.AddOptionalParameter("to", to?.ToUnixTimeMilliseconds());
            parameters.AddOptionalParameter("currency", currency);
            if (transactionType.HasValue)
                parameters.AddOptionalParameter("transactionType", JsonConvert.SerializeObject(transactionType, new LedgerTransactionTypeConverter(false)));
            if (opType.HasValue)
                parameters.AddOptionalParameter("opType", JsonConvert.SerializeObject(opType, new LedgerOperationTypeConverter(false)));
            parameters.AddOptionalParameter("transactionCode", transactionCode);
            parameters.AddOptionalParameter("paymentId", paymentId);
            parameters.AddOptionalParameter("recipientNote", recipientNote);
            parameters.AddOptionalParameter("senderNote", senderNote);

            var credits = 1;
            var qs = $"?pageSize={pageSize}&offset{offset}";
            if (count.HasValue) qs += $"&count={(count.Value ? "true" : "false")}";
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_GetTransactionsByAccount + qs));
            return await Ledger.Tatum.SendTatumRequest<IEnumerable<LedgerTransaction>>(url, HttpMethod.Post, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Find transactions for customer across all accounts of customer.<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Find transactions for all accounts of customer identified by given customer internal ID.
        /// </summary>
        /// <param name="id">Customer internal ID to search for.</param>
        /// <param name="account">Source account - source of transaction(s).</param>
        /// <param name="counterAccount">Counter account - transaction(s) destination account.</param>
        /// <param name="from">Starting date to search for transactions from in UTC millis. If not present, search all history.</param>
        /// <param name="to">Date until to search for transactions in UTC millis. If not present, search up till now.</param>
        /// <param name="currency">Currency of the transactions.</param>
        /// <param name="transactionType">Type of payment.</param>
        /// <param name="opType">Type of operation.</param>
        /// <param name="transactionCode">For bookkeeping to distinct transaction purpose.</param>
        /// <param name="paymentId">Payment ID defined in payment order by sender.</param>
        /// <param name="recipientNote">Recipient note defined in payment order by sender.</param>
        /// <param name="senderNote">Sender note defined in payment order by sender.</param>
        /// <param name="pageSize">Max number of items per page is 50. Either count, or pageSize is accepted.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="count">Get total count of transactions based on the filter. Either count, or pageSize is accepted.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<LedgerTransaction>> GetTransactionsByCustomer(
            string id,
            string account = null,
            string counterAccount = null,
            DateTime? from = null,
            DateTime? to = null,
            string currency = null,
            LedgerTransactionType? transactionType = null,
            LedgerOperationType? opType = null,
            string transactionCode = null,
            string paymentId = null,
            string recipientNote = null,
            string senderNote = null,
            int pageSize = 50,
            int offset = 0,
            bool? count = null,
            CancellationToken ct = default)
            => GetTransactionsByCustomerAsync(id, account, counterAccount, from, to, currency, transactionType, opType, transactionCode, paymentId, recipientNote, senderNote, pageSize, offset, count, ct).Result;
        /// <summary>
        /// <b>Title:</b> Find transactions for customer across all accounts of customer.<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Find transactions for all accounts of customer identified by given customer internal ID.
        /// </summary>
        /// <param name="id">Customer internal ID to search for.</param>
        /// <param name="account">Source account - source of transaction(s).</param>
        /// <param name="counterAccount">Counter account - transaction(s) destination account.</param>
        /// <param name="from">Starting date to search for transactions from in UTC millis. If not present, search all history.</param>
        /// <param name="to">Date until to search for transactions in UTC millis. If not present, search up till now.</param>
        /// <param name="currency">Currency of the transactions.</param>
        /// <param name="transactionType">Type of payment.</param>
        /// <param name="opType">Type of operation.</param>
        /// <param name="transactionCode">For bookkeeping to distinct transaction purpose.</param>
        /// <param name="paymentId">Payment ID defined in payment order by sender.</param>
        /// <param name="recipientNote">Recipient note defined in payment order by sender.</param>
        /// <param name="senderNote">Sender note defined in payment order by sender.</param>
        /// <param name="pageSize">Max number of items per page is 50. Either count, or pageSize is accepted.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="count">Get total count of transactions based on the filter. Either count, or pageSize is accepted.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<LedgerTransaction>>> GetTransactionsByCustomerAsync(
            string id,
            string account = null,
            string counterAccount = null,
            DateTime? from = null,
            DateTime? to = null,
            string currency = null,
            LedgerTransactionType? transactionType = null,
            LedgerOperationType? opType = null,
            string transactionCode = null,
            string paymentId = null,
            string recipientNote = null,
            string senderNote = null,
            int pageSize = 50,
            int offset = 0,
            bool? count = null,
            CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);

            var parameters = new Dictionary<string, object>
            {
                { "id", id },
            };
            parameters.AddOptionalParameter("account", account);
            parameters.AddOptionalParameter("counterAccount", counterAccount);
            parameters.AddOptionalParameter("from", from?.ToUnixTimeMilliseconds());
            parameters.AddOptionalParameter("to", to?.ToUnixTimeMilliseconds());
            parameters.AddOptionalParameter("currency", currency);
            if (transactionType.HasValue)
                parameters.AddOptionalParameter("transactionType", JsonConvert.SerializeObject(transactionType, new LedgerTransactionTypeConverter(false)));
            if (opType.HasValue)
                parameters.AddOptionalParameter("opType", JsonConvert.SerializeObject(opType, new LedgerOperationTypeConverter(false)));
            parameters.AddOptionalParameter("transactionCode", transactionCode);
            parameters.AddOptionalParameter("paymentId", paymentId);
            parameters.AddOptionalParameter("recipientNote", recipientNote);
            parameters.AddOptionalParameter("senderNote", senderNote);

            var credits = 1;
            var qs = $"?pageSize={pageSize}&offset{offset}";
            if (count.HasValue) qs += $"&count={(count.Value ? "true" : "false")}";
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_GetTransactionsByCustomer + qs));
            return await Ledger.Tatum.SendTatumRequest<IEnumerable<LedgerTransaction>>(url, HttpMethod.Post, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Find transactions for ledger.<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Find transactions across whole ledger.
        /// </summary>
        /// <param name="account">Source account - source of transaction(s).</param>
        /// <param name="counterAccount">Counter account - transaction(s) destination account.</param>
        /// <param name="from">Starting date to search for transactions from in UTC millis. If not present, search all history.</param>
        /// <param name="to">Date until to search for transactions in UTC millis. If not present, search up till now.</param>
        /// <param name="currency">Currency of the transactions.</param>
        /// <param name="transactionType">Type of payment.</param>
        /// <param name="opType">Type of operation.</param>
        /// <param name="transactionCode">For bookkeeping to distinct transaction purpose.</param>
        /// <param name="paymentId">Payment ID defined in payment order by sender.</param>
        /// <param name="recipientNote">Recipient note defined in payment order by sender.</param>
        /// <param name="senderNote">Sender note defined in payment order by sender.</param>
        /// <param name="pageSize">Max number of items per page is 50. Either count, or pageSize is accepted.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="count">Get total count of transactions based on the filter. Either count, or pageSize is accepted.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<LedgerTransaction>> GetTransactionsByLedger(
            string account = null,
            string counterAccount = null,
            DateTime? from = null,
            DateTime? to = null,
            string currency = null,
            LedgerTransactionType? transactionType = null,
            LedgerOperationType? opType = null,
            string transactionCode = null,
            string paymentId = null,
            string recipientNote = null,
            string senderNote = null,
            int pageSize = 50,
            int offset = 0,
            bool? count = null,
            CancellationToken ct = default)
            => GetTransactionsByLedgerAsync(account, counterAccount, from, to, currency, transactionType, opType, transactionCode, paymentId, recipientNote, senderNote, pageSize, offset, count, ct).Result;
        /// <summary>
        /// <b>Title:</b> Find transactions for ledger.<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Find transactions across whole ledger.
        /// </summary>
        /// <param name="account">Source account - source of transaction(s).</param>
        /// <param name="counterAccount">Counter account - transaction(s) destination account.</param>
        /// <param name="from">Starting date to search for transactions from in UTC millis. If not present, search all history.</param>
        /// <param name="to">Date until to search for transactions in UTC millis. If not present, search up till now.</param>
        /// <param name="currency">Currency of the transactions.</param>
        /// <param name="transactionType">Type of payment.</param>
        /// <param name="opType">Type of operation.</param>
        /// <param name="transactionCode">For bookkeeping to distinct transaction purpose.</param>
        /// <param name="paymentId">Payment ID defined in payment order by sender.</param>
        /// <param name="recipientNote">Recipient note defined in payment order by sender.</param>
        /// <param name="senderNote">Sender note defined in payment order by sender.</param>
        /// <param name="pageSize">Max number of items per page is 50. Either count, or pageSize is accepted.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="count">Get total count of transactions based on the filter. Either count, or pageSize is accepted.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<LedgerTransaction>>> GetTransactionsByLedgerAsync(
            string account = null,
            string counterAccount = null,
            DateTime? from = null,
            DateTime? to = null,
            string currency = null,
            LedgerTransactionType? transactionType = null,
            LedgerOperationType? opType = null,
            string transactionCode = null,
            string paymentId = null,
            string recipientNote = null,
            string senderNote = null,
            int pageSize = 50,
            int offset = 0,
            bool? count = null,
            CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("account", account);
            parameters.AddOptionalParameter("counterAccount", counterAccount);
            parameters.AddOptionalParameter("from", from?.ToUnixTimeMilliseconds());
            parameters.AddOptionalParameter("to", to?.ToUnixTimeMilliseconds());
            parameters.AddOptionalParameter("currency", currency);
            if (transactionType.HasValue)
                parameters.AddOptionalParameter("transactionType", JsonConvert.SerializeObject(transactionType, new LedgerTransactionTypeConverter(false)));
            if (opType.HasValue)
                parameters.AddOptionalParameter("opType", JsonConvert.SerializeObject(opType, new LedgerOperationTypeConverter(false)));
            parameters.AddOptionalParameter("transactionCode", transactionCode);
            parameters.AddOptionalParameter("paymentId", paymentId);
            parameters.AddOptionalParameter("recipientNote", recipientNote);
            parameters.AddOptionalParameter("senderNote", senderNote);

            var credits = 1;
            var qs = $"?pageSize={pageSize}&offset{offset}";
            if (count.HasValue) qs += $"&count={(count.Value ? "true" : "false")}";
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_GetTransactionsByLedger + qs));
            return await Ledger.Tatum.SendTatumRequest<IEnumerable<LedgerTransaction>>(url, HttpMethod.Post, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Find transactions with given reference across all accounts.<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Find transactions for all accounts with given reference.
        /// </summary>
        /// <param name="reference">reference</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<LedgerTransaction>> GetTransactionsByReference(string reference, CancellationToken ct = default) => GetTransactionsByReferenceAsync(reference, ct).Result;
        /// <summary>
        /// <b>Title:</b> Find transactions with given reference across all accounts.<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Find transactions for all accounts with given reference.
        /// </summary>
        /// <param name="reference">reference</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<LedgerTransaction>>> GetTransactionsByReferenceAsync(string reference, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_GetTransactionsByReference, reference));
            return await Ledger.Tatum.SendTatumRequest<IEnumerable<LedgerTransaction>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }
    }

    public class LedgerVirtualCurrencyClient
    {
        public LedgerClient Ledger { get; protected set; }

        protected const string Endpoints_Create = "ledger/virtualCurrency";
        protected const string Endpoints_Update = "ledger/virtualCurrency";
        protected const string Endpoints_Get = "ledger/virtualCurrency/{0}";
        protected const string Endpoints_Mint = "ledger/virtualCurrency/mint";
        protected const string Endpoints_Destroy = "ledger/virtualCurrency/revoke";

        public LedgerVirtualCurrencyClient(LedgerClient ledgerClient)
        {
            Ledger = ledgerClient;
        }

        /// <summary>
        /// <b>Title:</b> Create new virtual currency<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create new virtual currency with given supply stored in account. This will create Tatum internal virtual currency. Every virtual currency must be prefixed with VC_.
        /// Every virtual currency must be pegged to existing FIAT or supported cryptocurrency. 1 unit of virtual currency has then the same amount as 1 unit of the base currency it is pegged to.It is possible to set a custom base rate for the virtual currency. (baseRate = 2 => 1 VC unit = 2 basePair units)
        /// Tatum virtual currency acts as any other asset within Tatum.For creation of ERC20 token, see ERC20.
        /// This operation returns the newly created Tatum Ledger account with an initial balance set to the virtual currency's total supply. Total supply can be changed in the future.
        /// </summary>
        /// <param name="name">Virtual currency name. Must be prefixed with 'VC_'.</param>
        /// <param name="supply">Supply of virtual currency.</param>
        /// <param name="basePair">Base pair for virtual currency. Transaction value will be calculated according to this base pair. e.g. 1 VC_VIRTUAL is equal to 1 BTC, if basePair is set to BTC.</param>
        /// <param name="baseRate">Exchange rate of the base pair. Each unit of the created curency will represent value of baseRate*1 basePair.</param>
        /// <param name="customer">If customer is filled then is created or updated.</param>
        /// <param name="description">Used as a description within Tatum system.</param>
        /// <param name="accountCode">For bookkeeping to distinct account purpose.</param>
        /// <param name="accountNumber">Account number from external system.</param>
        /// <param name="accountingCurrency">All transaction will be billed in this currency for created account associated with this currency. If not set, EUR is used. ISO-4217</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<LedgerReport> Create(
            string name,
            string supply,
            string basePair,
            decimal baseRate = 1.0m,
            LedgerCustomerOptions customer = null,
            string description = null,
            string accountCode = null,
            string accountNumber = null,
            string accountingCurrency = null,
            CancellationToken ct = default)
            => CreateAsync(name, supply, basePair, baseRate, customer, description, accountCode, accountNumber, accountingCurrency, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create new virtual currency<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create new virtual currency with given supply stored in account. This will create Tatum internal virtual currency. Every virtual currency must be prefixed with VC_.
        /// Every virtual currency must be pegged to existing FIAT or supported cryptocurrency. 1 unit of virtual currency has then the same amount as 1 unit of the base currency it is pegged to.It is possible to set a custom base rate for the virtual currency. (baseRate = 2 => 1 VC unit = 2 basePair units)
        /// Tatum virtual currency acts as any other asset within Tatum.For creation of ERC20 token, see ERC20.
        /// This operation returns the newly created Tatum Ledger account with an initial balance set to the virtual currency's total supply. Total supply can be changed in the future.
        /// </summary>
        /// <param name="name">Virtual currency name. Must be prefixed with 'VC_'.</param>
        /// <param name="supply">Supply of virtual currency.</param>
        /// <param name="basePair">Base pair for virtual currency. Transaction value will be calculated according to this base pair. e.g. 1 VC_VIRTUAL is equal to 1 BTC, if basePair is set to BTC.</param>
        /// <param name="baseRate">Exchange rate of the base pair. Each unit of the created curency will represent value of baseRate*1 basePair.</param>
        /// <param name="customer">If customer is filled then is created or updated.</param>
        /// <param name="description">Used as a description within Tatum system.</param>
        /// <param name="accountCode">For bookkeeping to distinct account purpose.</param>
        /// <param name="accountNumber">Account number from external system.</param>
        /// <param name="accountingCurrency">All transaction will be billed in this currency for created account associated with this currency. If not set, EUR is used. ISO-4217</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<LedgerReport>> CreateAsync(
            string name,
            string supply,
            string basePair,
            decimal baseRate = 1.0m,
            LedgerCustomerOptions customer = null,
            string description = null,
            string accountCode = null,
            string accountNumber = null,
            string accountingCurrency = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "name", name },
                { "supply", supply },
                { "basePair", basePair },
                { "baseRate", baseRate },
            };
            parameters.AddOptionalParameter("customer", customer);
            parameters.AddOptionalParameter("description", description);
            parameters.AddOptionalParameter("accountCode", accountCode);
            parameters.AddOptionalParameter("accountNumber", accountNumber);
            parameters.AddOptionalParameter("accountingCurrency", accountingCurrency);

            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Create));
            return await Ledger.Tatum.SendTatumRequest<LedgerReport>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Update virtual currency<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Change base pair and/or base rate of existing virtual currency.
        /// </summary>
        /// <param name="name">Virtual currency name, which will be updated. It is not possible to update the name of the virtual currency.</param>
        /// <param name="basePair">Exchange rate of the base pair. Each unit of the created curency will represent value of baseRate*1 basePair.</param>
        /// <param name="baseRate">Base pair for virtual currency. Transaction value will be calculated according to this base pair. e.g. 1 VC_VIRTUAL is equal to 1 BTC, if basePair is set to BTC.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> Update(string name, string basePair = null, decimal? baseRate = null, CancellationToken ct = default) => UpdateAsync(name, basePair, baseRate, ct).Result;
        /// <summary>
        /// <b>Title:</b> Update virtual currency<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Change base pair and/or base rate of existing virtual currency.
        /// </summary>
        /// <param name="name">Virtual currency name, which will be updated. It is not possible to update the name of the virtual currency.</param>
        /// <param name="basePair">Exchange rate of the base pair. Each unit of the created curency will represent value of baseRate*1 basePair.</param>
        /// <param name="baseRate">Base pair for virtual currency. Transaction value will be calculated according to this base pair. e.g. 1 VC_VIRTUAL is equal to 1 BTC, if basePair is set to BTC.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> UpdateAsync(string name, string basePair = null, decimal? baseRate = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "name", name },
            };
            parameters.AddOptionalParameter("basePair", basePair);
            parameters.AddOptionalParameter("baseRate", baseRate);

            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Update));
            var result = await Ledger.Tatum.SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Get virtual currency<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get detail of virtual currency.
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<LedgerReport> Get(string name, CancellationToken ct = default) => GetAsync(name, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get virtual currency<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get detail of virtual currency.
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<LedgerReport>> GetAsync(string name, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Get, name));
            return await Ledger.Tatum.SendTatumRequest<LedgerReport>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Create new supply of virtual currency<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create new supply of virtual currency linked on the given accountId. Method increases the total supply of the currency.
        /// This method creates Ledger transaction with operationType MINT with undefined counterAccountId.
        /// </summary>
        /// <param name="accountId">Ledger account with currency of the virtual currency, on which the operation will be performed.</param>
        /// <param name="amount">Amount of virtual currency to operate within this operation.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="reference">Reference of the payment.</param>
        /// <param name="transactionCode">For bookkeeping to distinct transaction purpose.</param>
        /// <param name="recipientNote">Note visible to both, sender and recipient. Available for both Mint and Revoke operations</param>
        /// <param name="counterAccount">External account identifier. By default, there is no counterAccount present in the transaction.</param>
        /// <param name="senderNote">Note visible to sender. Available in Revoke operation.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TatumReference> Mint(
            string accountId,
            decimal amount,
            string paymentId = null,
            string reference = null,
            string transactionCode = null,
            string recipientNote = null,
            string counterAccount = null,
            string senderNote = null,
            CancellationToken ct = default)
            => MintAsync(accountId, amount, paymentId, reference, transactionCode, recipientNote, counterAccount, senderNote, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create new supply of virtual currency<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create new supply of virtual currency linked on the given accountId. Method increases the total supply of the currency.
        /// This method creates Ledger transaction with operationType MINT with undefined counterAccountId.
        /// </summary>
        /// <param name="accountId">Ledger account with currency of the virtual currency, on which the operation will be performed.</param>
        /// <param name="amount">Amount of virtual currency to operate within this operation.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="reference">Reference of the payment.</param>
        /// <param name="transactionCode">For bookkeeping to distinct transaction purpose.</param>
        /// <param name="recipientNote">Note visible to both, sender and recipient. Available for both Mint and Revoke operations</param>
        /// <param name="counterAccount">External account identifier. By default, there is no counterAccount present in the transaction.</param>
        /// <param name="senderNote">Note visible to sender. Available in Revoke operation.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TatumReference>> MintAsync(
            string accountId,
            decimal amount,
            string paymentId = null,
            string reference = null,
            string transactionCode = null,
            string recipientNote = null,
            string counterAccount = null,
            string senderNote = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "accountId", accountId },
                { "amount", amount.ToString(CultureInfo.InvariantCulture) },
            };
            parameters.AddOptionalParameter("paymentId", paymentId);
            parameters.AddOptionalParameter("reference", reference);
            parameters.AddOptionalParameter("transactionCode", transactionCode);
            parameters.AddOptionalParameter("recipientNote", recipientNote);
            parameters.AddOptionalParameter("counterAccount", counterAccount);
            parameters.AddOptionalParameter("senderNote", senderNote);

            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Mint));
            return await Ledger.Tatum.SendTatumRequest<TatumReference>(url, HttpMethod.Put, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Destroy supply of virtual currency<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Destroy supply of virtual currency linked on the given accountId. Method decreases the total supply of the currency.
        /// This method creates Ledger transaction with operationType REVOKE with undefined counterAccountId.
        /// </summary>
        /// <param name="accountId">Ledger account with currency of the virtual currency, on which the operation will be performed.</param>
        /// <param name="amount">Amount of virtual currency to operate within this operation.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="reference">Reference of the payment.</param>
        /// <param name="transactionCode">For bookkeeping to distinct transaction purpose.</param>
        /// <param name="recipientNote">Note visible to both, sender and recipient. Available for both Mint and Revoke operations</param>
        /// <param name="counterAccount">External account identifier. By default, there is no counterAccount present in the transaction.</param>
        /// <param name="senderNote">Note visible to sender. Available in Revoke operation.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TatumReference> Destroy(
            string accountId,
            decimal amount,
            string paymentId = null,
            string reference = null,
            string transactionCode = null,
            string recipientNote = null,
            string counterAccount = null,
            string senderNote = null,
            CancellationToken ct = default)
            => DestroyAsync(accountId, amount, paymentId, reference, transactionCode, recipientNote, counterAccount, senderNote, ct).Result;
        /// <summary>
        /// <b>Title:</b> Destroy supply of virtual currency<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Destroy supply of virtual currency linked on the given accountId. Method decreases the total supply of the currency.
        /// This method creates Ledger transaction with operationType REVOKE with undefined counterAccountId.
        /// </summary>
        /// <param name="accountId">Ledger account with currency of the virtual currency, on which the operation will be performed.</param>
        /// <param name="amount">Amount of virtual currency to operate within this operation.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="reference">Reference of the payment.</param>
        /// <param name="transactionCode">For bookkeeping to distinct transaction purpose.</param>
        /// <param name="recipientNote">Note visible to both, sender and recipient. Available for both Mint and Revoke operations</param>
        /// <param name="counterAccount">External account identifier. By default, there is no counterAccount present in the transaction.</param>
        /// <param name="senderNote">Note visible to sender. Available in Revoke operation.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TatumReference>> DestroyAsync(
            string accountId,
            decimal amount,
            string paymentId = null,
            string reference = null,
            string transactionCode = null,
            string recipientNote = null,
            string counterAccount = null,
            string senderNote = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "accountId", accountId },
                { "amount", amount.ToString(CultureInfo.InvariantCulture) },
            };
            parameters.AddOptionalParameter("paymentId", paymentId);
            parameters.AddOptionalParameter("reference", reference);
            parameters.AddOptionalParameter("transactionCode", transactionCode);
            parameters.AddOptionalParameter("recipientNote", recipientNote);
            parameters.AddOptionalParameter("counterAccount", counterAccount);
            parameters.AddOptionalParameter("senderNote", senderNote);

            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_Destroy));
            return await Ledger.Tatum.SendTatumRequest<TatumReference>(url, HttpMethod.Put, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }
    }
}
