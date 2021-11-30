using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.Converters;
using Tatum.Net.Enums;
using Tatum.Net.Helpers;
using Tatum.Net.Interfaces;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Clients
{
    public class LedgerAccountClient : ITatumLedgerAccountClient
    {
        public LedgerClient Ledger { get; protected set; }

        #region API Endpoints

        #region Ledger Account
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
        protected const string Endpoints_UnblockAllBlockedAmounts = "ledger/account/block/account/{0}";
        protected const string Endpoints_ActivateLedgerAccount = "ledger/account/{0}/activate";
        protected const string Endpoints_DeactivateLedgerAccount = "ledger/account/{0}/deactivate";
        protected const string Endpoints_FreezeLedgerAccount = "ledger/account/{0}/freeze";
        protected const string Endpoints_UnfreezeLedgerAccount = "ledger/account/{0}/unfreeze";
        #endregion

        #endregion

        public LedgerAccountClient(LedgerClient ledgerClient)
        {
            Ledger = ledgerClient;
        }

        #region Ledger / Account

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
        public virtual WebCallResult<LedgerAccount> Create(BlockchainType chain, LedgerAccountOptions options = null, CancellationToken ct = default) => CreateAsync(chain, options, ct).Result;
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
        public virtual WebCallResult<IEnumerable<LedgerAccount>> GetAccounts(int pageSize = 50, int offset = 0, CancellationToken ct = default) => GetAccountsAsync(pageSize, offset, ct).Result;
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
        public virtual WebCallResult<IEnumerable<LedgerAccount>> CreateBatch(IEnumerable<LedgerAccountOptions> accounts, CancellationToken ct = default) => CreateBatchAsync(accounts, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create multiple accounts in a batch call<br />
        /// <b>Credits:</b> 2 credits per API call + 1 credit for every created account.<br />
        /// <b>Description:</b>
        /// Creates new accounts for the customer in a batch call.
        /// </summary>
        /// <param name="accounts">Ledger Accounts List</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<LedgerAccount>>> CreateBatchAsync(IEnumerable<LedgerAccountOptions> accounts, CancellationToken ct = default)
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
        public virtual WebCallResult<IEnumerable<LedgerAccount>> GetByCustomerId(string customer_id, int pageSize = 50, int offset = 0, CancellationToken ct = default) => GetByCustomerIdAsync(customer_id, pageSize, offset, ct).Result;
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
        public virtual async Task<WebCallResult<IEnumerable<LedgerAccount>>> GetByCustomerIdAsync(string customer_id, int pageSize = 50, int offset = 0, CancellationToken ct = default)
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
        public virtual WebCallResult<LedgerAccount> GetById(string account_id, int pageSize = 50, int offset = 0, CancellationToken ct = default) => GetByIdAsync(account_id, pageSize, offset, ct).Result;
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
        public virtual async Task<WebCallResult<LedgerAccount>> GetByIdAsync(string account_id, int pageSize = 50, int offset = 0, CancellationToken ct = default)
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
        public virtual WebCallResult<bool> Update(string account_id, string accountCode, string accountNumber, CancellationToken ct = default) => UpdateAsync(account_id, accountCode, accountNumber, ct).Result;
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
        public virtual async Task<WebCallResult<bool>> UpdateAsync(string account_id, string accountCode, string accountNumber, CancellationToken ct = default)
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
        public virtual WebCallResult<LedgerBalance> GetBalance(string account_id, CancellationToken ct = default) => GetBalanceAsync(account_id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get account balance<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get balance for the account.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<LedgerBalance>> GetBalanceAsync(string account_id, CancellationToken ct = default)
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
        public virtual WebCallResult<TatumId> BlockAmount(string account_id, decimal amount, string type, string description, CancellationToken ct = default) => BlockAmountAsync(account_id, amount, type, description, ct).Result;
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
        public virtual WebCallResult<bool> UnblockAmount(string blockage_id, CancellationToken ct = default) => UnblockAmountAsync(blockage_id, ct).Result;
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
        public virtual WebCallResult<IEnumerable<LedgerBlockedAmount>> GetBlockedAmounts(string account_id, int pageSize = 50, int offset = 0, CancellationToken ct = default) => GetBlockedAmountsAsync(account_id, pageSize, offset, ct).Result;
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
        /// <b>Title:</b> Unblock all blocked amounts on account<br />
        /// <b>Credits:</b>1 credit per API call, 1 credits for each deleted blockage. 1 API call + 2 blockages = 3 credits.<br />
        /// <b>Description:</b>
        /// Unblock previously blocked amounts on account. Increase available balance on account, where amount was blocked.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> UnblockAllBlockedAmounts(string account_id, CancellationToken ct = default) => UnblockAllBlockedAmountsAsync(account_id, ct).Result;
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
        public virtual WebCallResult<bool> Activate(string account_id, CancellationToken ct = default) => ActivateAsync(account_id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Activate account<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Activate account.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> ActivateAsync(string account_id, CancellationToken ct = default)
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
        public virtual WebCallResult<bool> Deactivate(string account_id, CancellationToken ct = default) => DeactivateAsync(account_id, ct).Result;
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
        public virtual async Task<WebCallResult<bool>> DeactivateAsync(string account_id, CancellationToken ct = default)
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
        public virtual WebCallResult<bool> Freeze(string account_id, CancellationToken ct = default) => FreezeAsync(account_id, ct).Result;
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
        public virtual async Task<WebCallResult<bool>> FreezeAsync(string account_id, CancellationToken ct = default)
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
        public virtual WebCallResult<bool> Unfreeze(string account_id, CancellationToken ct = default) => UnfreezeAsync(account_id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Unfreeze account<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Unfreeze previously frozen account. Unfreezing non-frozen account will do no harm to the account.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> UnfreezeAsync(string account_id, CancellationToken ct = default)
        {
            var credits = 2;
            var url = Ledger.Tatum.GetUrl(string.Format(Endpoints_UnfreezeLedgerAccount, account_id));
            var result = await Ledger.Tatum.SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }
        #endregion

    }
}
