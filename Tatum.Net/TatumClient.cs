using CryptoExchange.Net;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.Converters;
using Tatum.Net.CoreObjects;
using Tatum.Net.Enums;
using Tatum.Net.Helpers;
using Tatum.Net.Interfaces;
using Tatum.Net.RateLimiter;
using Tatum.Net.RestObjects;

namespace Tatum.Net
{
    public class TatumClient : RestClient, IRestClient, ITatumClient
    {
        #region Core Fields
        private static TatumClientOptions defaultOptions = new TatumClientOptions();
        private static TatumClientOptions DefaultOptions => defaultOptions.Copy();
        #endregion

        #region API Endpoints
        /* Version */
        private const int Endpoints_Version = 3;

        /* Ledger */
        private const string Endpoints_Ledger_Account = "ledger/account";
        private const string Endpoints_Ledger_AccountBatch = "ledger/accountcredits";
        private const string Endpoints_Ledger_AccountOfId = "ledger/account/{0}";
        private const string Endpoints_Ledger_AccountBalance = "ledger/account/{0}/balance";
        private const string Endpoints_Ledger_AccountOfCustomer = "ledger/account/customer/{0}";
        private const string Endpoints_Ledger_BlockAmount = "ledger/account/block/{0}";
        private const string Endpoints_Ledger_UnlockAmountAndTransfer = "ledger/account/block/{0}";
        private const string Endpoints_Ledger_BlockAccount = "ledger/account/block/account/{0}";
        private const string Endpoints_Ledger_ActivateLedgerAccount = "ledger/account/{0}/activate";
        private const string Endpoints_Ledger_DeactivateLedgerAccount = "ledger/account/{0}/deactivate";
        private const string Endpoints_Ledger_FreezeLedgerAccount = "ledger/account/{0}/freeze";
        private const string Endpoints_Ledger_UnfreezeLedgerAccount = "ledger/account/{0}/unfreeze";

        /* Blockchain - Shared*/
        private const string Endpoints_Blockchain_GenerateWallet = "{0}/wallet";
        private const string Endpoints_Blockchain_GenerateDepositAddress = "{0}/address/{1}/{2}";
        private const string Endpoints_Blockchain_GenerateWalletPrivateKey = "{0}/wallet/priv";

        /* Bitcoin */
        private const string Endpoints_Bitcoin_BlockchainInformation = "bitcoin/info";
        private const string Endpoints_Bitcoin_GetBlockHash = "bitcoin/block/hash/{0}";
        private const string Endpoints_Bitcoin_GetBlockByHash = "bitcoin/block/{0}";
        private const string Endpoints_Bitcoin_GetTransactionByHash = "bitcoin/transaction/{0}";
        private const string Endpoints_Bitcoin_GetTransactionsByAddress = "bitcoin/transaction/address/{0}";
        private const string Endpoints_Bitcoin_GetBalance = "bitcoin/address/balance/{0}";
        private const string Endpoints_Bitcoin_GetTransactionUTXO = "bitcoin/utxo/{0}/{1}";
        private const string Endpoints_Bitcoin_Transaction = "bitcoin/transaction";
        private const string Endpoints_Bitcoin_Broadcast = "bitcoin/broadcast";

        /* BitcoinCash */
        private const string Endpoints_BitcoinCash_BlockchainInformation = "bcash/info";
        private const string Endpoints_BitcoinCash_GetBlockHash = "bcash/block/hash/{0}";
        private const string Endpoints_BitcoinCash_GetBlockByHash = "bcash/block/{0}";
        private const string Endpoints_BitcoinCash_GetTransactionByHash = "bcash/transaction/{0}";
        private const string Endpoints_BitcoinCash_GetTransactionsByAddress = "bcash/transaction/address/{0}";
        private const string Endpoints_BitcoinCash_Transaction = "bcash/transaction";
        private const string Endpoints_BitcoinCash_Broadcast = "bcash/broadcast";

        /* Ethereum */
        private const string Endpoints_Ethereum_Web3HttpDriver = "ethereum/web3/{0}";
        private const string Endpoints_Ethereum_CurrentBlockNumber = "ethereum/block/current";
        private const string Endpoints_Ethereum_GetBlockByHash = "ethereum/block/{0}";
        private const string Endpoints_Ethereum_GetTransactionByHash = "ethereum/transaction/{0}";
        private const string Endpoints_Ethereum_GetETHBalance = "ethereum/account/balance/{0}";
        private const string Endpoints_Ethereum_GetOutgoingTransactionsCount = "ethereum/transaction/count/{0}";
        private const string Endpoints_Ethereum_GetTransactionsByAddress = "ethereum/account/transaction/{0}";
        private const string Endpoints_Ethereum_Send = "ethereum/transaction";
        private const string Endpoints_Ethereum_Broadcast = "ethereum/broadcast";
        private const string Endpoints_Ethereum_SmartContract = "ethereum/smartcontract";
        private const string Endpoints_Ethereum_ERC20Balance = "ethereum/account/balance/erc20/{0}";
        private const string Endpoints_Ethereum_ERC20DeploySmartContract = "ethereum/erc20/deploy";
        private const string Endpoints_Ethereum_ERC20Transfer = "ethereum/erc20/transaction";
        private const string Endpoints_Ethereum_ERC721Balance = "ethereum/erc721/balance/{0}/{1}";
        private const string Endpoints_Ethereum_ERC721DeploySmartContract = "ethereum/erc721/deploy";
        private const string Endpoints_Ethereum_ERC721Mint = "ethereum/erc721/mint";
        private const string Endpoints_Ethereum_ERC721MintMultiple = "ethereum/erc721/mint/batch";
        private const string Endpoints_Ethereum_ERC721Transfer = "ethereum/erc721/transaction";
        private const string Endpoints_Ethereum_ERC721Burn = "ethereum/erc721/burn";
        private const string Endpoints_Ethereum_ERC721Token = "ethereum/erc721/token/{0}/{1}/{2}";
        private const string Endpoints_Ethereum_ERC721TokenMetadata = "ethereum/erc721/metadata/{0}/{1}";
        private const string Endpoints_Ethereum_ERC721TokenOwner = "ethereum/erc721/owner/{0}/{1}";

        /* Litecoin */
        private const string Endpoints_Litecoin_BlockchainInformation = "litecoin/info";
        private const string Endpoints_Litecoin_GetBlockHash = "litecoin/block/hash/{0}";
        private const string Endpoints_Litecoin_GetBlockByHash = "litecoin/block/{0}";
        private const string Endpoints_Litecoin_GetTransactionByHash = "litecoin/transaction/{0}";
        private const string Endpoints_Litecoin_GetTransactionsByAddress = "litecoin/transaction/address/{0}";
        private const string Endpoints_Litecoin_GetBalance = "litecoin/address/balance/{0}";
        private const string Endpoints_Litecoin_GetTransactionUTXO = "litecoin/utxo/{0}/{1}";
        private const string Endpoints_Litecoin_Transaction = "litecoin/transaction";
        private const string Endpoints_Litecoin_Broadcast = "litecoin/broadcast";

        /* Ripple */
        private const string Endpoints_Ripple_GenerateAccount = "xrp/account";
        private const string Endpoints_Ripple_BlockchainInformation = "xrp/info";
        private const string Endpoints_Ripple_BlockchainFee = "xrp/fee";
        private const string Endpoints_Ripple_GetTransactionsByAccount = "xrp/account/tx/{0}";
        private const string Endpoints_Ripple_GetLedger = "xrp/ledger/{0}";
        private const string Endpoints_Ripple_GetTransactionByHash = "xrp/transaction/{0}";
        private const string Endpoints_Ripple_AccountInfo = "xrp/account/{0}";
        private const string Endpoints_Ripple_GetBalance = "xrp/account/{0}/balance";
        private const string Endpoints_Ripple_Send = "xrp/transaction";
        private const string Endpoints_Ripple_Trust = "xrp/trust";
        private const string Endpoints_Ripple_AccountSettings = "xrp/account/settings";
        private const string Endpoints_Ripple_Broadcast = "xrp/broadcast";

        /* Stellar */
        private const string Endpoints_Stellar_GenerateAccount = "xlm/account";
        private const string Endpoints_Stellar_BlockchainInformation = "xlm/info";
        private const string Endpoints_Stellar_BlockchainFee = "xlm/fee";
        private const string Endpoints_Stellar_GetLedger = "xlm/ledger/{0}";
        private const string Endpoints_Stellar_GetTransactionsInLedger = "xlm/ledger/{0}/transaction";
        private const string Endpoints_Stellar_GetTransactionsByAccount = "xlm/account/tx/{0}";
        private const string Endpoints_Stellar_GetTransactionByHash = "xlm/transaction/{0}";
        private const string Endpoints_Stellar_AccountInfo = "xlm/account/{0}";
        private const string Endpoints_Stellar_Send = "xlm/transaction";
        private const string Endpoints_Stellar_Trust = "xlm/trust";
        private const string Endpoints_Stellar_Broadcast = "xlm/broadcast";

        /* Records */
        private const string Endpoints_Records_Log = "record";

        /* Binance */
        private const string Endpoints_Binance_GenerateAccount = "bnb/account";
        private const string Endpoints_Binance_CurrentBlock = "bnb/block/current";
        private const string Endpoints_Binance_GetTransactionsInBlock = "bnb/block/{0}";
        private const string Endpoints_Binance_AccountInfo = "bnb/account/{0}";
        private const string Endpoints_Binance_GetTransaction = "bnb/transaction/{0}";
        private const string Endpoints_Binance_Send = "bnb/transaction";
        private const string Endpoints_Binance_Broadcast = "bnb/broadcast";

        /* VeChain */
        private const string Endpoints_VeChain_CurrentBlock = "vet/block/current";
        private const string Endpoints_VeChain_GetBlockByHash = "vet/block/{0}";
        private const string Endpoints_VeChain_GetBalance = "vet/account/balance/{0}";
        private const string Endpoints_VeChain_GetEnergy = "vet/account/energy/{0}";
        private const string Endpoints_VeChain_GetTransactionByHash = "vet/transaction/{0}";
        private const string Endpoints_VeChain_GetTransactionReceipt = "vet/transaction/{0}/receipt";
        private const string Endpoints_VeChain_Transaction = "vet/transaction";
        private const string Endpoints_VeChain_Gas = "vet/transaction/gas";
        private const string Endpoints_VeChain_Broadcast = "vet/broadcast";

        /* NEO */
        private const string Endpoints_NEO_GenerateAccount = "neo/wallet";
        private const string Endpoints_NEO_CurrentBlock = "neo/block/current";
        private const string Endpoints_NEO_GetBlock = "neo/block/{0}";
        private const string Endpoints_NEO_GetBalance = "neo/account/balance/{0}";
        private const string Endpoints_NEO_GetAssetInfo = "neo/asset/{0}";
        private const string Endpoints_NEO_GetUnspentTransactionOutputs = "neo/transaction/out/{0}/{1}";
        private const string Endpoints_NEO_GetTransactionsByAccount = "neo/account/tx/{0}";
        private const string Endpoints_NEO_GetContractInfo = "neo/contract/{0}";
        private const string Endpoints_NEO_GetTransactionByHash = "neo/transaction/{0}";
        private const string Endpoints_NEO_Send = "neo/transaction";
        private const string Endpoints_NEO_ClaimGAS = "neo/claim";
        private const string Endpoints_NEO_Invoke = "neo/invoke";
        private const string Endpoints_NEO_Broadcast = "neo/broadcast";

        /* Libra */
        private const string Endpoints_Libra_BlockchainInformation = "libra/info";
        private const string Endpoints_Libra_GetTransactionsByAccount = "libra/account/transaction/{0}";
        private const string Endpoints_Libra_AccountInfo = "libra/account/{0}";
        private const string Endpoints_Libra_GetTransactions = "libra/transaction/{0}/{0}";

        /* Scrypta */
        private const string Endpoints_Scrypta_BlockchainInformation = "scrypta/info";
        private const string Endpoints_Scrypta_GetBlockHash = "scrypta/block/hash/{0}";
        private const string Endpoints_Scrypta_GetBlockByHash = "scrypta/block/{0}";
        private const string Endpoints_Scrypta_GetTransactionByHash = "scrypta/transaction/{0}";
        private const string Endpoints_Scrypta_GetTransactionsByAddress = "scrypta/transaction/address/{0}";
        private const string Endpoints_Scrypta_GetSpendableUTXO = "scrypta/utxo/{0}";
        private const string Endpoints_Scrypta_GetTransactionUTXO = "scrypta/utxo/{0}/{1}";
        private const string Endpoints_Scrypta_Transaction = "scrypta/transaction";
        private const string Endpoints_Scrypta_Broadcast = "scrypta/broadcast";

        /* Service */
        private const string Endpoints_Service_Consumption = "tatum/usage";
        private const string Endpoints_Service_ExchangeRates = "tatum/rate/{0}";
        private const string Endpoints_Service_Version = "tatum/version";
        #endregion

        #region Constructor / Destructor
        /// <summary>
        /// Create a new instance of TatumClient using the default options
        /// </summary>
        public TatumClient(string apiKey) : this(apiKey, DefaultOptions)
        {
        }

        /// <summary>
        /// Create a new instance of the TatumClient with the provided options
        /// </summary>
        public TatumClient(TatumClientOptions options) : this(options.ApiCredentials.Key.GetString(), options)
        {
        }

        /// <summary>
        /// Create a new instance of the TatumClient with the provided options
        /// </summary>
        public TatumClient(string apiKey, TatumClientOptions options) : base("Tatum", options, new TatumAuthenticationProvider(apiKey))
        {
            requestBodyFormat = RequestBodyFormat.Json;
        }

        /// <summary>
        /// Sets the default options to use for new clients
        /// </summary>
        /// <param name="options">The options to use for new clients</param>
        public static void SetDefaultOptions(TatumClientOptions options)
        {
            defaultOptions = options;
        }

        /// <summary>
        /// Set API Key
        /// </summary>
        /// <param name="apiKey">The api key</param>
        public void SetApiCredentials(string apiKey)
        {
            SetAuthenticationProvider(new TatumAuthenticationProvider(apiKey));
        }
        #endregion

        #region Api Methods

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
        public WebCallResult<LedgerAccount> Ledger_CreateAccount(BlockchainType chain, LedgerAccountOptions options = null, CancellationToken ct = default) => Ledger_CreateAccount_Async(chain, options, ct).Result;
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
        public async Task<WebCallResult<LedgerAccount>> Ledger_CreateAccount_Async(BlockchainType chain, LedgerAccountOptions options = null, CancellationToken ct = default)
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
            var url = GetUrl(string.Format(Endpoints_Ledger_Account));
            return await SendTatumRequest<LedgerAccount>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
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
        public WebCallResult<IEnumerable<LedgerAccount>> Ledger_GetAccounts(int pageSize = 50, int offset = 0, CancellationToken ct = default) => Ledger_GetAccounts_Async(pageSize, offset, ct).Result;
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
        public async Task<WebCallResult<IEnumerable<LedgerAccount>>> Ledger_GetAccounts_Async(int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);

            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };

            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ledger_Account));
            var result = await SendTatumRequest<IEnumerable<LedgerAccount>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
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
        public WebCallResult<IEnumerable<LedgerAccount>> Ledger_CreateBatchAccounts(IEnumerable<LedgerAccountOptions> accounts, CancellationToken ct = default) => Ledger_CreateBatchAccounts_Async(accounts, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create multiple accounts in a batch call<br />
        /// <b>Credits:</b> 2 credits per API call + 1 credit for every created account.<br />
        /// <b>Description:</b>
        /// Creates new accounts for the customer in a batch call.
        /// </summary>
        /// <param name="accounts">Ledger Accounts List</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<IEnumerable<LedgerAccount>>> Ledger_CreateBatchAccounts_Async(IEnumerable<LedgerAccountOptions> accounts, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "accounts", accounts },
            };

            var credits = 2;
            foreach (var account in accounts) if (!string.IsNullOrEmpty(account.ExtendedPublicKey)) credits++;
            var url = GetUrl(string.Format(Endpoints_Ledger_AccountBatch));
            return await SendTatumRequest<IEnumerable<LedgerAccount>>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
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
        public WebCallResult<IEnumerable<LedgerAccount>> Ledger_GetAccountsByCustomerId(string customer_id, int pageSize = 50, int offset = 0, CancellationToken ct = default) => Ledger_GetAccountsByCustomerId_Async(customer_id, pageSize, offset, ct).Result;
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
        public async Task<WebCallResult<IEnumerable<LedgerAccount>>> Ledger_GetAccountsByCustomerId_Async(string customer_id, int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);

            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ledger_AccountOfCustomer, customer_id));
            return await SendTatumRequest<IEnumerable<LedgerAccount>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
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
        public WebCallResult<LedgerAccount> Ledger_GetAccountByAccountId(string account_id, int pageSize = 50, int offset = 0, CancellationToken ct = default) => Ledger_GetAccountByAccountId_Async(account_id, pageSize, offset, ct).Result;
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
        public async Task<WebCallResult<LedgerAccount>> Ledger_GetAccountByAccountId_Async(string account_id, int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);

            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };

            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ledger_AccountOfId, account_id));
            return await SendTatumRequest<LedgerAccount>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
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
        public WebCallResult<bool> Ledger_UpdateAccount(string account_id, string accountCode, string accountNumber, CancellationToken ct = default) => Ledger_UpdateAccount_Async(account_id, accountCode, accountNumber, ct).Result;
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
        public async Task<WebCallResult<bool>> Ledger_UpdateAccount_Async(string account_id, string accountCode, string accountNumber, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "accountCode", accountCode },
                { "accountNumber", accountNumber },
            };

            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Ledger_AccountOfId, account_id));
            var result = await SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
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
        public WebCallResult<LedgerAccountBalance> Ledger_GetAccountBalance(string account_id, CancellationToken ct = default) => Ledger_GetAccountBalance_Async(account_id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get account balance<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get balance for the account.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<LedgerAccountBalance>> Ledger_GetAccountBalance_Async(string account_id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ledger_AccountBalance, account_id));
            return await SendTatumRequest<LedgerAccountBalance>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
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
        public WebCallResult<LedgerId> Ledger_BlockAmount(string account_id, decimal amount, string type, string description, CancellationToken ct = default) => Ledger_BlockAmount_Async(account_id, amount, type, description, ct).Result;
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
        public async Task<WebCallResult<LedgerId>> Ledger_BlockAmount_Async(string account_id, decimal amount, string type, string description, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "amount", amount.ToString() },
                { "type", type },
            };
            parameters.AddOptionalParameter("description", description);

            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Ledger_BlockAmount, account_id));
            return await SendTatumRequest<LedgerId>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
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
        public WebCallResult<LedgerReference> Ledger_UnlockAmountAndPerformTransaction(
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
            => Ledger_UnlockAmountAndPerformTransaction_Async(blockage_id, recipientAccountId, amount, anonymous, compliant, transactionCode, paymentId, recipientNote, senderNote, baseRate, ct).Result;
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
        public async Task<WebCallResult<LedgerReference>> Ledger_UnlockAmountAndPerformTransaction_Async(
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
            var url = GetUrl(string.Format(Endpoints_Ledger_UnlockAmountAndTransfer, blockage_id));
            return await SendTatumRequest<LedgerReference>(url, HttpMethod.Put, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
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
        public WebCallResult<bool> Ledger_UnblockAmount(string blockage_id, CancellationToken ct = default) => Ledger_UnblockAmount_Async(blockage_id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Unblock blocked amount on account<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Unblock previously blocked amount on account. Increase available balance on account, where amount was blocked.
        /// </summary>
        /// <param name="blockage_id">Blockage ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<bool>> Ledger_UnblockAmount_Async(string blockage_id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ledger_BlockAmount, blockage_id));
            var result = await SendTatumRequest<string>(url, HttpMethod.Delete, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
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
        public WebCallResult<IEnumerable<LedgerBlockedAmount>> Ledger_GetBlockedAmounts(string account_id, int pageSize = 50, int offset = 0, CancellationToken ct = default) => Ledger_GetBlockedAmounts_Async(account_id, pageSize, offset, ct).Result;
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
        public async Task<WebCallResult<IEnumerable<LedgerBlockedAmount>>> Ledger_GetBlockedAmounts_Async(string account_id, int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);

            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };

            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ledger_BlockAmount, account_id));
            return await SendTatumRequest<IEnumerable<LedgerBlockedAmount>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
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
        public WebCallResult<bool> Ledger_UnblockAllBlockedAmounts(string account_id, CancellationToken ct = default) => Ledger_UnblockAllBlockedAmounts_Async(account_id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Unblock all blocked amounts on account<br />
        /// <b>Credits:</b>  credit per API call, 1 credits for each deleted blockage. 1 API call + 2 blockages = 3 credits.<br />
        /// <b>Description:</b>
        /// Unblock previously blocked amounts on account. Increase available balance on account, where amount was blocked.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<bool>> Ledger_UnblockAllBlockedAmounts_Async(string account_id, CancellationToken ct = default)
        {
            var credits = 1; // 1 credit per API call, 1 credits for each deleted blockage. 1 API call + 2 blockages = 3 credits.
            var url = GetUrl(string.Format(Endpoints_Ledger_BlockAccount, account_id));
            var result = await SendTatumRequest<string>(url, HttpMethod.Delete, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
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
        public WebCallResult<bool> Ledger_ActivateAccount(string account_id, CancellationToken ct = default) => Ledger_ActivateAccount_Async(account_id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Activate account<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Activate account.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<bool>> Ledger_ActivateAccount_Async(string account_id, CancellationToken ct = default)
        {
            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Ledger_ActivateLedgerAccount, account_id));
            var result = await SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
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
        public WebCallResult<bool> Ledger_DeactivateAccount(string account_id, CancellationToken ct = default) => Ledger_DeactivateAccount_Async(account_id, ct).Result;
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
        public async Task<WebCallResult<bool>> Ledger_DeactivateAccount_Async(string account_id, CancellationToken ct = default)
        {
            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Ledger_DeactivateLedgerAccount, account_id));
            var result = await SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
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
        public WebCallResult<bool> Ledger_FreezeLedgerAccount(string account_id, CancellationToken ct = default) => Ledger_FreezeLedgerAccount_Async(account_id, ct).Result;
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
        public async Task<WebCallResult<bool>> Ledger_FreezeLedgerAccount_Async(string account_id, CancellationToken ct = default)
        {
            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Ledger_FreezeLedgerAccount, account_id));
            var result = await SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
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
        public WebCallResult<bool> Ledger_UnfreezeLedgerAccount(string account_id, CancellationToken ct = default) => Ledger_UnfreezeLedgerAccount_Async(account_id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Unfreeze account<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Unfreeze previously frozen account. Unfreezing non-frozen account will do no harm to the account.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<bool>> Ledger_UnfreezeLedgerAccount_Async(string account_id, CancellationToken ct = default)
        {
            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Ledger_UnfreezeLedgerAccount, account_id));
            var result = await SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }
        #endregion

        #region Ledger / Transaction
        // TODO: Ledger / Transaction -> Send payment
        // TODO: Ledger / Transaction -> Find transactions for account.
        // TODO: Ledger / Transaction -> Find transactions for customer across all accounts of customer.
        // TODO: Ledger / Transaction -> Find transactions for ledger.
        // TODO: Ledger / Transaction -> Find transactions with given reference across all accounts.
        #endregion

        #region Ledger / Customer
        // TODO: Ledger / Customer -> List all customers
        // TODO: Ledger / Customer -> Get customer details
        // TODO: Ledger / Customer -> Update customer
        // TODO: Ledger / Customer -> Activate customer
        // TODO: Ledger / Customer -> Deactivate customer
        // TODO: Ledger / Customer -> Enable customer
        // TODO: Ledger / Customer -> Disable customer
        #endregion

        #region Ledger / Virtual Currency
        // TODO: Ledger / Virtual Currency -> Create new virtual currency
        // TODO: Ledger / Virtual Currency -> Update virtual currency
        // TODO: Ledger / Virtual Currency -> Get virtual currency
        // TODO: Ledger / Virtual Currency -> Create new supply of virtual currency
        // TODO: Ledger / Virtual Currency -> Destroy supply of virtual currency
        #endregion

        #region Ledger / Subscription
        // TODO: Ledger / Subscription -> Create new subscription
        // TODO: Ledger / Subscription -> List all active subscriptions
        // TODO: Ledger / Subscription -> Cancel existing subscription
        // TODO: Ledger / Subscription -> Obtain report for subscription
        #endregion

        #region Ledger / Order Book
        // TODO: Ledger / Order Book -> List all historical trades
        // TODO: Ledger / Order Book -> List all active buy trades
        // TODO: Ledger / Order Book -> List all active sell trades
        // TODO: Ledger / Order Book -> Store buy / sell trade
        // TODO: Ledger / Order Book -> Get existing trade
        // TODO: Ledger / Order Book -> Cancel existing trade
        // TODO: Ledger / Order Book -> Cancel all existing trades for account
        #endregion

        #region Security / Key Management System
        // TODO: Security / Key Management System -> Get pending transactions to sign
        // TODO: Security / Key Management System -> Complete pending transaction to sign
        // TODO: Security / Key Management System -> Get transaction details
        // TODO: Security / Key Management System -> Delete transaction
        // TODO: Security / Address -> Check malicous address
        #endregion

        #region Off-chain / Account
        // TODO: Off-chain / Account -> Create new deposit address
        // TODO: Off-chain / Account -> Get all deposit addresses for account
        // TODO: Off-chain / Account -> Create new deposit addresses in a batch call
        // TODO: Off-chain / Account -> Check, if deposit address is assigned
        // TODO: Off-chain / Account -> Remove address for account
        // TODO: Off-chain / Account -> Assign address for account
        #endregion

        #region Off-chain / Blockchain
        // TODO: Off-chain / Blockchain -> Send Bitcoin from Tatum account to address
        // TODO: Off-chain / Blockchain -> Send Bitcoin Cash from Tatum account to address
        // TODO: Off-chain / Blockchain -> Send Litecoin from Tatum account to address
        // TODO: Off-chain / Blockchain -> Send Ethereum from Tatum ledger to blockchain
        // TODO: Off-chain / Blockchain -> Create new ERC20 token
        // TODO: Off-chain / Blockchain -> Deploy Ethereum ERC20 Smart Contract Off-chain
        // TODO: Off-chain / Blockchain -> Set ERC20 token contract address
        // TODO: Off-chain / Blockchain -> Transfer Ethereum ERC20 from Tatum ledger to blockchain
        // TODO: Off-chain / Blockchain -> Send XLM / Asset from Tatum ledger to blockchain
        // TODO: Off-chain / Blockchain -> Create XLM based Asset
        // TODO: Off-chain / Blockchain -> Send XRP from Tatum ledger to blockchain
        // TODO: Off-chain / Blockchain -> Create XRP based Asset
        // TODO: Off-chain / Blockchain -> Send BNB from Tatum ledger to blockchain
        // TODO: Off-chain / Blockchain -> Create BNB based Asset
        #endregion

        #region Off-chain / Withdrawal
        // TODO: Off-chain / Withdrawal -> Store withdrawal
        // TODO: Off-chain / Withdrawal -> Complete withdrawal
        // TODO: Off-chain / Withdrawal -> Cancel withdrawal
        // TODO: Off-chain / Withdrawal -> Broadcast signed transaction and complete withdrawal
        #endregion

        #region Blockchain / Shared (Bitcoin, BitcoinCash, Ethereum, Litecoin, Scrypta, VeChain)
        internal WebCallResult<BlockchainWallet> Blockchain_GenerateWallet(BlockchainType chain, string mnemonics = null, CancellationToken ct = default) => Blockchain_GenerateWallet_Async(chain, new List<string> { mnemonics }, ct).Result;
        internal WebCallResult<BlockchainWallet> Blockchain_GenerateWallet(BlockchainType chain, IEnumerable<string> mnemonics = null, CancellationToken ct = default) => Blockchain_GenerateWallet_Async(chain, mnemonics, ct).Result;
        internal async Task<WebCallResult<BlockchainWallet>> Blockchain_GenerateWallet_Async(BlockchainType chain, string mnemonics = null, CancellationToken ct = default) => await Blockchain_GenerateWallet_Async(chain, new List<string> { mnemonics }, ct);
        internal async Task<WebCallResult<BlockchainWallet>> Blockchain_GenerateWallet_Async(BlockchainType chain, IEnumerable<string> mnemonics = null, CancellationToken ct = default)
        {
            if (!chain.IsOneOf(
                BlockchainType.Bitcoin,
                BlockchainType.BitcoinCash,
                BlockchainType.Ethereum,
                BlockchainType.Litecoin,
                BlockchainType.Scrypta,
                BlockchainType.VeChain))
                throw new ArgumentException("Wrong BlockchainType");

            var credict = new Dictionary<BlockchainType, int>
            {
                { BlockchainType.Bitcoin, 1 },
                { BlockchainType.BitcoinCash, 5 },
                { BlockchainType.Ethereum, 1 },
                { BlockchainType.Litecoin, 5 },
                { BlockchainType.Scrypta, 1 },
                { BlockchainType.VeChain, 5 },
            };

            var credits = credict[chain];
            var parameters = new Dictionary<string, object>();
            if (mnemonics != null && mnemonics.Count() > 0) parameters.Add("mnemonic", string.Join(" ", mnemonics));

            var ops = chain.GetBlockchainOptions();
            var url = GetUrl(string.Format(Endpoints_Blockchain_GenerateWallet, ops.ChainSlug));
            return await SendTatumRequest<BlockchainWallet>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        internal WebCallResult<BlockchainAddress> Blockchain_GenerateDepositAddress(BlockchainType chain, string xpub, int index, CancellationToken ct = default) => Blockchain_GenerateDepositAddress_Async(chain, xpub, index, ct).Result;
        internal async Task<WebCallResult<BlockchainAddress>> Blockchain_GenerateDepositAddress_Async(BlockchainType chain, string xpub, int index, CancellationToken ct = default)
        {
            if (!chain.IsOneOf(
                BlockchainType.Bitcoin,
                BlockchainType.BitcoinCash,
                BlockchainType.Ethereum,
                BlockchainType.Litecoin,
                BlockchainType.Scrypta,
                BlockchainType.VeChain))
                throw new ArgumentException("Wrong BlockchainType");

            var credict = new Dictionary<BlockchainType, int>
            {
                { BlockchainType.Bitcoin, 1 },
                { BlockchainType.BitcoinCash, 5 },
                { BlockchainType.Ethereum, 1 },
                { BlockchainType.Litecoin, 5 },
                { BlockchainType.Scrypta, 1 },
                { BlockchainType.VeChain, 5 },
            };

            var credits = credict[chain];
            var ops = chain.GetBlockchainOptions();
            var url = GetUrl(string.Format(Endpoints_Blockchain_GenerateDepositAddress, ops.ChainSlug, xpub, index));

            if (chain == BlockchainType.Scrypta)
            {
                var result = await SendTatumRequest<string>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
                if (!result.Success) return WebCallResult<BlockchainAddress>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

                return new WebCallResult<BlockchainAddress>(result.ResponseStatusCode, result.ResponseHeaders, new BlockchainAddress { Address = result.Data }, null);
            }
            else
            {
                return await SendTatumRequest<BlockchainAddress>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            }
        }

        internal WebCallResult<BlockchainKey> Blockchain_GeneratePrivateKey(BlockchainType chain, string mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKey_Async(chain, new List<string> { mnemonics }, index, ct).Result;
        internal WebCallResult<BlockchainKey> Blockchain_GeneratePrivateKey(BlockchainType chain, IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKey_Async(chain, mnemonics, index, ct).Result;
        internal async Task<WebCallResult<BlockchainKey>> Blockchain_GeneratePrivateKey_Async(BlockchainType chain, string mnemonics, int index, CancellationToken ct = default) => await Blockchain_GeneratePrivateKey_Async(chain, new List<string> { mnemonics }, index, default);
        internal async Task<WebCallResult<BlockchainKey>> Blockchain_GeneratePrivateKey_Async(BlockchainType chain, IEnumerable<string> mnemonics, int index, CancellationToken ct = default)
        {
            if (!chain.IsOneOf(
                BlockchainType.Bitcoin,
                BlockchainType.BitcoinCash,
                BlockchainType.Ethereum,
                BlockchainType.Litecoin,
                BlockchainType.Scrypta,
                BlockchainType.VeChain))
                throw new ArgumentException("Wrong BlockchainType");

            var credict = new Dictionary<BlockchainType, int>
            {
                { BlockchainType.Bitcoin, 1 },
                { BlockchainType.Ethereum, 1 },
                { BlockchainType.BitcoinCash, 5 },
                { BlockchainType.Litecoin, 5 },
                { BlockchainType.Scrypta, 1 },
                { BlockchainType.VeChain, 5 },
            };

            var credits = credict[chain];
            var parameters = new Dictionary<string, object> {
                { "index", index },
                { "mnemonic", string.Join(" ", mnemonics) },
            };

            var ops = chain.GetBlockchainOptions();
            var url = GetUrl(string.Format(Endpoints_Blockchain_GenerateWalletPrivateKey, ops.ChainSlug));
            return await SendTatumRequest<BlockchainKey>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }
        #endregion

        #region Blockchain / Bitcoin
        /// <summary>
        /// <b>Title:</b> Generate Bitcoin wallet<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// Tatum follows BIP44 specification and generates for Bitcoin wallet with derivation path m'/44'/0'/0'/0. More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. Generate BIP44 compatible Bitcoin wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainWallet> Bitcoin_GenerateWallet(string mnemonics, CancellationToken ct = default) => Blockchain_GenerateWallet_Async(BlockchainType.Bitcoin, new List<string> { mnemonics }, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Bitcoin wallet<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// Tatum follows BIP44 specification and generates for Bitcoin wallet with derivation path m'/44'/0'/0'/0. More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. Generate BIP44 compatible Bitcoin wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainWallet> Bitcoin_GenerateWallet(IEnumerable<string> mnemonics = null, CancellationToken ct = default) => Blockchain_GenerateWallet_Async(BlockchainType.Bitcoin, mnemonics, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Bitcoin wallet<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// Tatum follows BIP44 specification and generates for Bitcoin wallet with derivation path m'/44'/0'/0'/0. More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. Generate BIP44 compatible Bitcoin wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainWallet>> Bitcoin_GenerateWallet_Async(string mnemonics, CancellationToken ct = default) => await Blockchain_GenerateWallet_Async(BlockchainType.Bitcoin, new List<string> { mnemonics }, ct);
        /// <summary>
        /// <b>Title:</b> Generate Bitcoin wallet<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// Tatum follows BIP44 specification and generates for Bitcoin wallet with derivation path m'/44'/0'/0'/0. More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. Generate BIP44 compatible Bitcoin wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainWallet>> Bitcoin_GenerateWallet_Async(IEnumerable<string> mnemonics = null, CancellationToken ct = default) => await Blockchain_GenerateWallet_Async(BlockchainType.Bitcoin, mnemonics, ct);

        /// <summary>
        /// <b>Title:</b> Generate Bitcoin deposit address from Extended public key<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Generate Bitcoin deposit address from Extended public key. Deposit address is generated for the specific index - each extended public key can generate up to 2^32 addresses starting from index 0 until 2^31.
        /// </summary>
        /// <param name="xpub">Extended public key of wallet.</param>
        /// <param name="index">Derivation index of desired address to be generated.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainAddress> Bitcoin_GenerateDepositAddress(string xpub, int index, CancellationToken ct = default) => Blockchain_GenerateDepositAddress_Async(BlockchainType.Bitcoin, xpub, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Bitcoin deposit address from Extended public key<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Generate Bitcoin deposit address from Extended public key. Deposit address is generated for the specific index - each extended public key can generate up to 2^32 addresses starting from index 0 until 2^31.
        /// </summary>
        /// <param name="xpub">Extended public key of wallet.</param>
        /// <param name="index">Derivation index of desired address to be generated.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainAddress>> Bitcoin_GenerateDepositAddress_Async(string xpub, int index, CancellationToken ct = default) => await Blockchain_GenerateDepositAddress_Async(BlockchainType.Bitcoin, xpub, index, ct);

        /// <summary>
        /// <b>Title:</b> Generate Bitcoin private key<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Generate private key for address from mnemonic for given derivation path index. 
        /// Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to generate private key from.</param>
        /// <param name="index">Derivation index of private key to generate.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainKey> Bitcoin_GeneratePrivateKey(string mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKey_Async(BlockchainType.Bitcoin, new List<string> { mnemonics }, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Bitcoin private key<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Generate private key for address from mnemonic for given derivation path index. 
        /// Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to generate private key from.</param>
        /// <param name="index">Derivation index of private key to generate.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainKey> Bitcoin_GeneratePrivateKey(IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKey_Async(BlockchainType.Bitcoin, mnemonics, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Bitcoin private key<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Generate private key for address from mnemonic for given derivation path index. 
        /// Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to generate private key from.</param>
        /// <param name="index">Derivation index of private key to generate.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainKey>> Bitcoin_GeneratePrivateKey_Async(string mnemonics, int index, CancellationToken ct = default) => await Blockchain_GeneratePrivateKey_Async(BlockchainType.Bitcoin, new List<string> { mnemonics }, index, default);
        /// <summary>
        /// <b>Title:</b> Generate Bitcoin private key<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Generate private key for address from mnemonic for given derivation path index. 
        /// Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to generate private key from.</param>
        /// <param name="index">Derivation index of private key to generate.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainKey>> Bitcoin_GeneratePrivateKey_Async(IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => await Blockchain_GeneratePrivateKey_Async(BlockchainType.Bitcoin, mnemonics, index, default);

        /// <summary>
        /// <b>Title:</b> Get Blockchain Information<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Blockchain Information. Obtain basic info like testnet / mainent version of the chain, current block number and it's hash.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BitcoinChainInfo> Bitcoin_GetBlockchainInformation(CancellationToken ct = default) => Bitcoin_GetBlockchainInformation_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Blockchain Information<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Blockchain Information. Obtain basic info like testnet / mainent version of the chain, current block number and it's hash.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BitcoinChainInfo>> Bitcoin_GetBlockchainInformation_Async(CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Bitcoin_BlockchainInformation));
            return await SendTatumRequest<BitcoinChainInfo>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Block hash<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Block hash. Returns hash of the block to get the block detail.
        /// </summary>
        /// <param name="block_id">The number of blocks preceding a particular block on a block chain.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainHash> Bitcoin_GetBlockHash(long block_id, CancellationToken ct = default) => Bitcoin_GetBlockHash_Async(block_id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Block hash<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Block hash. Returns hash of the block to get the block detail.
        /// </summary>
        /// <param name="block_id">The number of blocks preceding a particular block on a block chain.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainHash>> Bitcoin_GetBlockHash_Async(long block_id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Bitcoin_GetBlockHash, block_id));
            return await SendTatumRequest<BlockchainHash>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Block by hash or height<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Block detail by block hash or height.
        /// </summary>
        /// <param name="hash_height">Block hash or height.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BitcoinBlock> Bitcoin_GetBlock(string hash_height, CancellationToken ct = default) => Bitcoin_GetBlock_Async(hash_height, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Block by hash or height<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Block detail by block hash or height.
        /// </summary>
        /// <param name="hash_height">Block hash or height.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BitcoinBlock>> Bitcoin_GetBlock_Async(string hash_height, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Bitcoin_GetBlockByHash, hash_height));
            return await SendTatumRequest<BitcoinBlock>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Transaction by hash<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Transaction detail by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BitcoinTransaction> Bitcoin_GetTransactionByHash(string hash, CancellationToken ct = default) => Bitcoin_GetTransactionByHash_Async(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Transaction by hash<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Transaction detail by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BitcoinTransaction>> Bitcoin_GetTransactionByHash_Async(string hash, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Bitcoin_GetTransactionByHash, hash));
            return await SendTatumRequest<BitcoinTransaction>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Transactions by address<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Transaction by address.
        /// </summary>
        /// <param name="address">Address</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<IEnumerable<BitcoinTransaction>> Bitcoin_GetTransactionsByAddress(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default) => Bitcoin_GetTransactionsByAddress_Async(address, pageSize, offset, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Transactions by address<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Transaction by address.
        /// </summary>
        /// <param name="address">Address</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<IEnumerable<BitcoinTransaction>>> Bitcoin_GetTransactionsByAddress_Async(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);

            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };

            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Bitcoin_GetTransactionsByAddress, address));
            return await SendTatumRequest<IEnumerable<BitcoinTransaction>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Balance of the address<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Balance of the address.
        /// </summary>
        /// <param name="address">Address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BitcoinBalance> Bitcoin_GetBalance(string address, CancellationToken ct = default) => Bitcoin_GetBalance_Async(address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Balance of the address<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Balance of the address.
        /// </summary>
        /// <param name="address">Address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BitcoinBalance>> Bitcoin_GetBalance_Async(string address, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Bitcoin_GetBalance, address));
            return await SendTatumRequest<BitcoinBalance>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get UTXO of Transaction<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get UTXO of given transaction and output index. UTXO means Unspent Transaction Output, which is in blockchain terminology assets, that user received on the specific address and does not spend it yet.
        /// In bitcoin-like blockchains(BTC, LTC, BCH), every transaction is built from the list of previously not spent transactions connected to the address.
        /// If user owns address A, receives in transaciont T1 10 BTC, he can spend in the next transaction UTXO T1 of total value 10 BTC.
        /// User can spend multiple UTXOs from different addresses in 1 transaction.
        /// If UTXO is not spent, data are returned, otherwise 404 error code.
        /// </summary>
        /// <param name="txhash">TX Hash</param>
        /// <param name="index">Index of tx output to check if spent or not</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BitcoinUTXO> Bitcoin_GetTransactionUTXO(string txhash, long index, CancellationToken ct = default) => Bitcoin_GetTransactionUTXO_Async(txhash, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get UTXO of Transaction<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get UTXO of given transaction and output index. UTXO means Unspent Transaction Output, which is in blockchain terminology assets, that user received on the specific address and does not spend it yet.
        /// In bitcoin-like blockchains(BTC, LTC, BCH), every transaction is built from the list of previously not spent transactions connected to the address.
        /// If user owns address A, receives in transaciont T1 10 BTC, he can spend in the next transaction UTXO T1 of total value 10 BTC.
        /// User can spend multiple UTXOs from different addresses in 1 transaction.
        /// If UTXO is not spent, data are returned, otherwise 404 error code.
        /// </summary>
        /// <param name="txhash">TX Hash</param>
        /// <param name="index">Index of tx output to check if spent or not</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BitcoinUTXO>> Bitcoin_GetTransactionUTXO_Async(string txhash, long index, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Bitcoin_GetTransactionUTXO, txhash, index));
            return await SendTatumRequest<BitcoinUTXO>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Send Bitcoin to blockchain addresses<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Bitcoin to blockchain addresses. It is possible to build a blockchain transaction in 2 ways:
        /// - fromAddress: assets will be sent from the list of addresses.For each of the addresses last 100 transactions will be scanned for any unspent UTXO and will be included in the transaction.
        /// - fromUTXO: assets will be sent from the list of unspent UTXOs. Each of the UTXO will be included in the transaction.
        /// In bitcoin-like blockchains, the transaction is created from the list of previously not spent UTXO.
        /// Every UTXO contains the number of funds, which can be spent. When the UTXO enters into the transaction, the whole amount is included and must be spent.
        /// For example, address A receives 2 transactions, T1 with 1 BTC and T2 with 2 BTC.The transaction, which will consume UTXOs for T1 and T2, will have available amount to spent 3 BTC = 1 BTC (T1) + 2 BTC(T2).
        /// There can be multiple recipients of the transactions, not only one.In the to section, every recipient address has it's corresponding amount. 
        /// When the amount of funds, that should receive the recipient is lower than the number of funds from the UTXOs, the difference is used as a transaction fee.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. 
        /// In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="fromAddress">Array of addresses and corresponding private keys. Tatum will automatically scan last 100 transactions for each address and will use all of the unspent values. We advise to use this option if you have 1 address per 1 transaction only.</param>
        /// <param name="fromUTXO">Array of transaction hashes, index of UTXO in it and corresponding private keys. Use this option if you want to calculate amount to send manually. Either fromUTXO or fromAddress must be present.</param>
        /// <param name="to">Array of addresses and values to send bitcoins to. Values must be set in BTC. Difference between from and to is transaction fee.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Bitcoin_Send(IEnumerable<BitcoinSendOrderFromAddress> fromAddress, IEnumerable<BitcoinSendOrderFromUTXO> fromUTXO, IEnumerable<BitcoinSendOrderTo> to, CancellationToken ct = default) => Bitcoin_Send_Async(fromAddress, fromUTXO, to, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send Bitcoin to blockchain addresses<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Bitcoin to blockchain addresses. It is possible to build a blockchain transaction in 2 ways:
        /// - fromAddress: assets will be sent from the list of addresses.For each of the addresses last 100 transactions will be scanned for any unspent UTXO and will be included in the transaction.
        /// - fromUTXO: assets will be sent from the list of unspent UTXOs. Each of the UTXO will be included in the transaction.
        /// In bitcoin-like blockchains, the transaction is created from the list of previously not spent UTXO.
        /// Every UTXO contains the number of funds, which can be spent. When the UTXO enters into the transaction, the whole amount is included and must be spent.
        /// For example, address A receives 2 transactions, T1 with 1 BTC and T2 with 2 BTC.The transaction, which will consume UTXOs for T1 and T2, will have available amount to spent 3 BTC = 1 BTC (T1) + 2 BTC(T2).
        /// There can be multiple recipients of the transactions, not only one.In the to section, every recipient address has it's corresponding amount. 
        /// When the amount of funds, that should receive the recipient is lower than the number of funds from the UTXOs, the difference is used as a transaction fee.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. 
        /// In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="fromAddress">Array of addresses and corresponding private keys. Tatum will automatically scan last 100 transactions for each address and will use all of the unspent values. We advise to use this option if you have 1 address per 1 transaction only.</param>
        /// <param name="fromUTXO">Array of transaction hashes, index of UTXO in it and corresponding private keys. Use this option if you want to calculate amount to send manually. Either fromUTXO or fromAddress must be present.</param>
        /// <param name="to">Array of addresses and values to send bitcoins to. Values must be set in BTC. Difference between from and to is transaction fee.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Bitcoin_Send_Async(IEnumerable<BitcoinSendOrderFromAddress> fromAddress, IEnumerable<BitcoinSendOrderFromUTXO> fromUTXO, IEnumerable<BitcoinSendOrderTo> to, CancellationToken ct = default)
        {
            if ((fromAddress == null || fromAddress.Count() == 0) && (fromUTXO == null || fromUTXO.Count() == 0))
                throw new ArgumentException("Either fromUTXO or fromAddress must be present.");

            var parameters = new Dictionary<string, object> {
                { "to", to },
            };
            parameters.AddOptionalParameter("fromAddress", fromAddress);
            parameters.AddOptionalParameter("fromUTXO", fromUTXO);

            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Bitcoin_Transaction));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Broadcast signed Bitcoin transaction<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed transaction to Bitcoin blockchain. 
        /// This method is used internally from Tatum KMS, Tatum Middleware or Tatum client libraries. 
        /// It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="signatureId">ID of prepared payment template to sign. Required only, when broadcasting transaction signed by Tatum KMS.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Bitcoin_Broadcast(string txData, string signatureId, CancellationToken ct = default) => Bitcoin_Broadcast_Async(txData, signatureId, ct).Result;
        /// <summary>
        /// <b>Title:</b> Broadcast signed Bitcoin transaction<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed transaction to Bitcoin blockchain. 
        /// This method is used internally from Tatum KMS, Tatum Middleware or Tatum client libraries. 
        /// It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="signatureId">ID of prepared payment template to sign. Required only, when broadcasting transaction signed by Tatum KMS.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Bitcoin_Broadcast_Async(string txData, string signatureId, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "txData", txData },
            };
            parameters.AddOptionalParameter("signatureId", signatureId);

            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Bitcoin_Broadcast));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        #endregion

        #region Blockchain / Bitcoin Cash
        /// <summary>
        /// <b>Title:</b> Generate Bitcoin Cash wallet<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. 
        /// It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. 
        /// Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// Tatum follows BIP44 specification and generates for Bitcoin Cash wallet with derivation path m'/44'/145'/0'/0. 
        /// More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. 
        /// Generate BIP44 compatible Bitcoin Cash wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainWallet> BitcoinCash_GenerateWallet(string mnemonics, CancellationToken ct = default) => Blockchain_GenerateWallet_Async(BlockchainType.BitcoinCash, new List<string> { mnemonics }, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Bitcoin Cash wallet<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. 
        /// It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. 
        /// Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// Tatum follows BIP44 specification and generates for Bitcoin Cash wallet with derivation path m'/44'/145'/0'/0. 
        /// More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. 
        /// Generate BIP44 compatible Bitcoin Cash wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainWallet> BitcoinCash_GenerateWallet(IEnumerable<string> mnemonics = null, CancellationToken ct = default) => Blockchain_GenerateWallet_Async(BlockchainType.BitcoinCash, mnemonics, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Bitcoin Cash wallet<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. 
        /// It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. 
        /// Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// Tatum follows BIP44 specification and generates for Bitcoin Cash wallet with derivation path m'/44'/145'/0'/0. 
        /// More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. 
        /// Generate BIP44 compatible Bitcoin Cash wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainWallet>> BitcoinCash_GenerateWallet_Async(string mnemonics, CancellationToken ct = default) => await Blockchain_GenerateWallet_Async(BlockchainType.BitcoinCash, new List<string> { mnemonics }, ct);
        /// <summary>
        /// <b>Title:</b> Generate Bitcoin Cash wallet<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. 
        /// It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. 
        /// Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// Tatum follows BIP44 specification and generates for Bitcoin Cash wallet with derivation path m'/44'/145'/0'/0. 
        /// More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. 
        /// Generate BIP44 compatible Bitcoin Cash wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainWallet>> BitcoinCash_GenerateWallet_Async(IEnumerable<string> mnemonics = null, CancellationToken ct = default) => await Blockchain_GenerateWallet_Async(BlockchainType.BitcoinCash, mnemonics, ct);

        /// <summary>
        /// <b>Title:</b> Get Bitcoin Cash Blockchain Information<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Cash Blockchain Information. Obtain basic info like testnet / mainent version of the chain, current block number and it's hash.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BitcoinCashChainInfo> BitcoinCash_GetBlockchainInformation(CancellationToken ct = default) => BitcoinCash_GetBlockchainInformation_Async(ct).Result;
        /// <b>Title:</b> Get Bitcoin Cash Blockchain Information<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Cash Blockchain Information. Obtain basic info like testnet / mainent version of the chain, current block number and it's hash.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BitcoinCashChainInfo>> BitcoinCash_GetBlockchainInformation_Async(CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_BitcoinCash_BlockchainInformation));
            return await SendTatumRequest<BitcoinCashChainInfo>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Bitcoin Cash Block hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Cash Block hash. Returns hash of the block to get the block detail.
        /// </summary>
        /// <param name="block_id">Block hash or height</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainHash> BitcoinCash_GetBlockHash(long block_id, CancellationToken ct = default) => BitcoinCash_GetBlockHash_Async(block_id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Bitcoin Cash Block hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Cash Block hash. Returns hash of the block to get the block detail.
        /// </summary>
        /// <param name="block_id">Block hash or height</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainHash>> BitcoinCash_GetBlockHash_Async(long block_id, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_BitcoinCash_GetBlockHash, block_id));
            return await SendTatumRequest<BlockchainHash>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Bitcoin Cash Block by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Cash Block detail by block hash or height.
        /// </summary>
        /// <param name="hash_height"></param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BitcoinCashBlock> BitcoinCash_GetBlock(string hash_height, CancellationToken ct = default) => BitcoinCash_GetBlock_Async(hash_height, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Bitcoin Cash Block by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Cash Block detail by block hash or height.
        /// </summary>
        /// <param name="hash_height"></param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BitcoinCashBlock>> BitcoinCash_GetBlock_Async(string hash_height, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_BitcoinCash_GetBlockByHash, hash_height));
            return await SendTatumRequest<BitcoinCashBlock>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Bitcoin Cash Transaction by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Cash Transaction by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BitcoinCashTransaction> BitcoinCash_GetTransactionByHash(string hash, CancellationToken ct = default) => BitcoinCash_GetTransactionByHash_Async(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Bitcoin Cash Transaction by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Cash Transaction by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BitcoinCashTransaction>> BitcoinCash_GetTransactionByHash_Async(string hash, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_BitcoinCash_GetTransactionByHash, hash));
            return await SendTatumRequest<BitcoinCashTransaction>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Bitcoin Cash Transactions by address<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Cash Transaction by address. Limit is 50 transaction per response.
        /// </summary>
        /// <param name="address">Address</param>
        /// <param name="skip">Define, how much transactions should be skipped to obtain another page.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<IEnumerable<BitcoinCashTransaction>> BitcoinCash_GetTransactionsByAddress(string address, int skip = 0, CancellationToken ct = default) => BitcoinCash_GetTransactionsByAddress_Async(address, skip, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Bitcoin Cash Transactions by address<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Cash Transaction by address. Limit is 50 transaction per response.
        /// </summary>
        /// <param name="address">Address</param>
        /// <param name="skip">Define, how much transactions should be skipped to obtain another page.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<IEnumerable<BitcoinCashTransaction>>> BitcoinCash_GetTransactionsByAddress_Async(string address, int skip = 0, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "skip", skip },
            };

            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_BitcoinCash_GetTransactionsByAddress, address));
            return await SendTatumRequest<IEnumerable<BitcoinCashTransaction>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Generate Bitcoin Cash deposit address from Extended public key<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate Bitcoin Cash deposit address from Extended public key. 
        /// Deposit address is generated for the specific index - each extended public key can generate up to 2^32 addresses starting from index 0 until 2^31. 
        /// Generates new format of address starting with bitcoincash: in case of mainnet, bchtest: in case of testnet..
        /// </summary>
        /// <param name="xpub">Extended public key of wallet.</param>
        /// <param name="index">Derivation index of desired address to be generated.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainAddress> BitcoinCash_GenerateDepositAddress(string xpub, int index, CancellationToken ct = default) => Blockchain_GenerateDepositAddress_Async(BlockchainType.BitcoinCash, xpub, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Bitcoin Cash deposit address from Extended public key<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate Bitcoin Cash deposit address from Extended public key. 
        /// Deposit address is generated for the specific index - each extended public key can generate up to 2^32 addresses starting from index 0 until 2^31. 
        /// Generates new format of address starting with bitcoincash: in case of mainnet, bchtest: in case of testnet..
        /// </summary>
        /// <param name="xpub">Extended public key of wallet.</param>
        /// <param name="index">Derivation index of desired address to be generated.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainAddress>> BitcoinCash_GenerateDepositAddress_Async(string xpub, int index, CancellationToken ct = default) => await Blockchain_GenerateDepositAddress_Async(BlockchainType.BitcoinCash, xpub, index, ct);

        /// <summary>
        /// <b>Title:</b> Generate Bitcoin Cash private key<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate private key for address from mnemonic for given derivation path index. 
        /// Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to generate private key from.</param>
        /// <param name="index">Derivation index of private key to generate.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainKey> BitcoinCash_GeneratePrivateKey(string mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKey_Async(BlockchainType.BitcoinCash, new List<string> { mnemonics }, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Bitcoin Cash private key<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate private key for address from mnemonic for given derivation path index. 
        /// Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to generate private key from.</param>
        /// <param name="index">Derivation index of private key to generate.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainKey> BitcoinCash_GeneratePrivateKey(IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKey_Async(BlockchainType.BitcoinCash, mnemonics, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Bitcoin Cash private key<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate private key for address from mnemonic for given derivation path index. 
        /// Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to generate private key from.</param>
        /// <param name="index">Derivation index of private key to generate.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainKey>> BitcoinCash_GeneratePrivateKey_Async(string mnemonics, int index, CancellationToken ct = default) => await Blockchain_GeneratePrivateKey_Async(BlockchainType.BitcoinCash, new List<string> { mnemonics }, index, default);
        /// <summary>
        /// <b>Title:</b> Generate Bitcoin Cash private key<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate private key for address from mnemonic for given derivation path index. 
        /// Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to generate private key from.</param>
        /// <param name="index">Derivation index of private key to generate.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainKey>> BitcoinCash_GeneratePrivateKey_Async(IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => await Blockchain_GeneratePrivateKey_Async(BlockchainType.BitcoinCash, mnemonics, index, default);

        /// <summary>
        /// <b>Title:</b> Send Bitcoin Cash to blockchain addresses<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Bitcoin Cash to blockchain addresses. It is possible to build a blockchain transaction in 1 way:
        /// - fromUTXO - assets will be sent from the list of unspent UTXOs.Each of the UTXO will be included in the transaction.
        /// In bitcoin-like blockchains, transaction is created from the list of previously not spent UTXO.
        /// Every UTXO contains amount of funds, which can be spent. When the UTXO enters into the transaction, the whole amount is included and must be spent.
        /// For example, address A receives 2 transactions, T1 with 1 BCH and T2 with 2 BCH.The transaction, which will consume UTXOs for T1 and T2, will have available amount to spent 3 BCH = 1 BCH (T1) + 2 BCH(T2).
        /// There can be multiple recipients of the transactions, not only one.In the to section, every recipient address has it's corresponding amount. 
        /// When the amount of funds, that should receive the recipient is lower than the amount of funds from the UTXOs, the difference is used as a transaction fee.
        /// This operation needs the private key of the blockchain address.
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key.
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds.
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds.
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request.
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="fromUTXO">Array of transaction hashes, index of UTXO in it and corresponding private keys. Use this option if you want to calculate amount to send manually. Either fromUTXO or fromAddress must be present.</param>
        /// <param name="to">Array of addresses and values to send bitcoins to. Values must be set in BCH. Difference between from and to is transaction fee.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> BitcoinCash_Send(IEnumerable<BitcoinCashSendOrderFromUTXO> fromUTXO, IEnumerable<BitcoinCashSendOrderTo> to, CancellationToken ct = default) => BitcoinCash_Send_Async(fromUTXO, to, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send Bitcoin Cash to blockchain addresses<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Bitcoin Cash to blockchain addresses. It is possible to build a blockchain transaction in 1 way:
        /// - fromUTXO - assets will be sent from the list of unspent UTXOs.Each of the UTXO will be included in the transaction.
        /// In bitcoin-like blockchains, transaction is created from the list of previously not spent UTXO.
        /// Every UTXO contains amount of funds, which can be spent. When the UTXO enters into the transaction, the whole amount is included and must be spent.
        /// For example, address A receives 2 transactions, T1 with 1 BCH and T2 with 2 BCH.The transaction, which will consume UTXOs for T1 and T2, will have available amount to spent 3 BCH = 1 BCH (T1) + 2 BCH(T2).
        /// There can be multiple recipients of the transactions, not only one.In the to section, every recipient address has it's corresponding amount. 
        /// When the amount of funds, that should receive the recipient is lower than the amount of funds from the UTXOs, the difference is used as a transaction fee.
        /// This operation needs the private key of the blockchain address.
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key.
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds.
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds.
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request.
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="fromUTXO">Array of transaction hashes, index of UTXO in it and corresponding private keys. Use this option if you want to calculate amount to send manually. Either fromUTXO or fromAddress must be present.</param>
        /// <param name="to">Array of addresses and values to send bitcoins to. Values must be set in BCH. Difference between from and to is transaction fee.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> BitcoinCash_Send_Async(IEnumerable<BitcoinCashSendOrderFromUTXO> fromUTXO, IEnumerable<BitcoinCashSendOrderTo> to, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "fromUTXO", fromUTXO },
                { "to", to },
            };

            var credits = 10;
            var url = GetUrl(string.Format(Endpoints_BitcoinCash_Transaction));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Broadcast signed Bitcoin Cash transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed transaction to Bitcoin Cash blockchain.
        /// This method is used internally from Tatum KMS, Tatum Middleware or Tatum client libraries.
        /// It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="signatureId">ID of prepared payment template to sign. Required only, when broadcasting transaction signed by Tatum KMS.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> BitcoinCash_Broadcast(string txData, string signatureId, CancellationToken ct = default) => BitcoinCash_Broadcast_Async(txData, signatureId, ct).Result;
        /// <summary>
        /// <b>Title:</b> Broadcast signed Bitcoin Cash transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed transaction to Bitcoin Cash blockchain.
        /// This method is used internally from Tatum KMS, Tatum Middleware or Tatum client libraries.
        /// It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="signatureId">ID of prepared payment template to sign. Required only, when broadcasting transaction signed by Tatum KMS.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> BitcoinCash_Broadcast_Async(string txData, string signatureId, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "txData", txData },
            };
            parameters.AddOptionalParameter("signatureId", signatureId);

            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_BitcoinCash_Broadcast));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }
        #endregion

        #region Blockchain / Ethereum
        /// <summary>
        /// <b>Title:</b> Generate Ethereum wallet<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. 
        /// It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. 
        /// Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// Tatum follows BIP44 specification and generates for Ethereum wallet with derivation path m'/44'/60'/0'/0. 
        /// More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. 
        /// Generate BIP44 compatible Ethereum wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainWallet> Ethereum_GenerateWallet(string mnemonics, CancellationToken ct = default) => Blockchain_GenerateWallet_Async(BlockchainType.Ethereum, new List<string> { mnemonics }, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Ethereum wallet<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. 
        /// It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. 
        /// Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// Tatum follows BIP44 specification and generates for Ethereum wallet with derivation path m'/44'/60'/0'/0. 
        /// More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. 
        /// Generate BIP44 compatible Ethereum wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainWallet> Ethereum_GenerateWallet(IEnumerable<string> mnemonics = null, CancellationToken ct = default) => Blockchain_GenerateWallet_Async(BlockchainType.Ethereum, mnemonics, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Ethereum wallet<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. 
        /// It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. 
        /// Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// Tatum follows BIP44 specification and generates for Ethereum wallet with derivation path m'/44'/60'/0'/0. 
        /// More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. 
        /// Generate BIP44 compatible Ethereum wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainWallet>> Ethereum_GenerateWallet_Async(string mnemonics, CancellationToken ct = default) => await Blockchain_GenerateWallet_Async(BlockchainType.Ethereum, new List<string> { mnemonics }, ct);
        /// <summary>
        /// <b>Title:</b> Generate Ethereum wallet<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. 
        /// It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. 
        /// Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// Tatum follows BIP44 specification and generates for Ethereum wallet with derivation path m'/44'/60'/0'/0. 
        /// More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. 
        /// Generate BIP44 compatible Ethereum wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainWallet>> Ethereum_GenerateWallet_Async(IEnumerable<string> mnemonics = null, CancellationToken ct = default) => await Blockchain_GenerateWallet_Async(BlockchainType.Ethereum, mnemonics, ct);

        /// <summary>
        /// <b>Title:</b> Generate Ethereum account address from Extended public key<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Generate Ethereum account deposit address from Extended public key. 
        /// Deposit address is generated for the specific index - each extended public key can generate up to 2^32 addresses starting from index 0 until 2^31.
        /// </summary>
        /// <param name="xpub">Extended public key of wallet.</param>
        /// <param name="index">Derivation index of desired address to be generated.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainAddress> Ethereum_GenerateDepositAddress(string xpub, int index, CancellationToken ct = default) => Blockchain_GenerateDepositAddress_Async(BlockchainType.Ethereum, xpub, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Ethereum account address from Extended public key<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Generate Ethereum account deposit address from Extended public key. 
        /// Deposit address is generated for the specific index - each extended public key can generate up to 2^32 addresses starting from index 0 until 2^31.
        /// </summary>
        /// <param name="xpub">Extended public key of wallet.</param>
        /// <param name="index">Derivation index of desired address to be generated.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainAddress>> Ethereum_GenerateDepositAddress_Async(string xpub, int index, CancellationToken ct = default) => await Blockchain_GenerateDepositAddress_Async(BlockchainType.Ethereum, xpub, index, ct);

        /// <summary>
        /// <b>Title:</b> Generate Ethereum private key<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Generate private key of address from mnemonic for given derivation path index. 
        /// Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to generate private key from.</param>
        /// <param name="index">Derivation index of private key to generate.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainKey> Ethereum_GeneratePrivateKey(string mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKey_Async(BlockchainType.Ethereum, new List<string> { mnemonics }, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Ethereum private key<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Generate private key of address from mnemonic for given derivation path index. 
        /// Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to generate private key from.</param>
        /// <param name="index">Derivation index of private key to generate.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainKey> Ethereum_GeneratePrivateKey(IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKey_Async(BlockchainType.Ethereum, mnemonics, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Ethereum private key<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Generate private key of address from mnemonic for given derivation path index. 
        /// Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to generate private key from.</param>
        /// <param name="index">Derivation index of private key to generate.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainKey>> Ethereum_GeneratePrivateKey_Async(string mnemonics, int index, CancellationToken ct = default) => await Blockchain_GeneratePrivateKey_Async(BlockchainType.Ethereum, new List<string> { mnemonics }, index, default);
        /// <summary>
        /// <b>Title:</b> Generate Ethereum private key<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Generate private key of address from mnemonic for given derivation path index. 
        /// Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to generate private key from.</param>
        /// <param name="index">Derivation index of private key to generate.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainKey>> Ethereum_GeneratePrivateKey_Async(IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => await Blockchain_GeneratePrivateKey_Async(BlockchainType.Ethereum, mnemonics, index, default);

        /// <summary>
        /// <b>Title:</b> Web3 HTTP driver<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Use this endpoint URL as a http-based web3 driver to connect directly to the Ethereum node provided by Tatum. 
        /// To learn more about Ethereum Web3, please visit Ethereum developer's guide.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<EthereumDriver> Ethereum_Web3HttpDriver(CancellationToken ct = default) => Ethereum_Web3HttpDriver_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Web3 HTTP driver<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Use this endpoint URL as a http-based web3 driver to connect directly to the Ethereum node provided by Tatum. 
        /// To learn more about Ethereum Web3, please visit Ethereum developer's guide.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<EthereumDriver>> Ethereum_Web3HttpDriver_Async(CancellationToken ct = default)
        {
            if (authProvider == null || authProvider.Credentials == null || authProvider.Credentials.Key == null)
                throw new ArgumentException("No valid API credentials provided. Api Key is needed.");

            var parameters = new Dictionary<string, object> {
                { "jsonrpc", "2.0" },
                { "method", "web3_clientVersion" },
                { "params", new List<string>() },
                { "id", 2 },
            };

            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Ethereum_Web3HttpDriver, authProvider.Credentials.Key.GetString()));
            return await SendTatumRequest<EthereumDriver>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get current block number<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum current block number. This is the number of the latest block in the blockchain.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<long> Ethereum_GetCurrentBlockNumber(CancellationToken ct = default) => Ethereum_GetCurrentBlockNumber_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get current block number<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum current block number. This is the number of the latest block in the blockchain.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<long>> Ethereum_GetCurrentBlockNumber_Async(CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ethereum_CurrentBlockNumber));
            var result = await SendTatumRequest<string>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<long>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<long>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.ToLong(), null);
        }

        /// <summary>
        /// <b>Title:</b> Get Ethereum block by hash<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum block by block hash or block number.
        /// </summary>
        /// <param name="hash_height">Block hash or block number</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<EthereumBlock> Ethereum_GetBlock(string hash_height, CancellationToken ct = default) => Ethereum_GetBlock_Async(hash_height, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Ethereum block by hash<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum block by block hash or block number.
        /// </summary>
        /// <param name="hash_height">Block hash or block number</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<EthereumBlock>> Ethereum_GetBlock_Async(string hash_height, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ethereum_GetBlockByHash, hash_height));
            return await SendTatumRequest<EthereumBlock>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Ethereum Account balance<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum account balance in ETH. This method does not prints any balance of the ERC20 or ERC721 tokens on the account.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<decimal> Ethereum_ETH_GetBalance(string address, CancellationToken ct = default) => Ethereum_ETH_GetBalance_Async(address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Ethereum Account balance<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum account balance in ETH. This method does not prints any balance of the ERC20 or ERC721 tokens on the account.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<decimal>> Ethereum_ETH_GetBalance_Async(string address, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ethereum_GetETHBalance, address));
            var result = await SendTatumRequest<EthereumBalance>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<decimal>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            var balance = result.Data.Balance.ToDecimal();
            return new WebCallResult<decimal>(result.ResponseStatusCode, result.ResponseHeaders, balance, null);
        }

        /// <summary>
        /// <b>Title:</b> Get Ethereum ERC20 Account balance<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum ERC20 Account balance in the smallest ERC20 unit. 
        /// It is possible to obtain the balance of any ERC20 token, either supported by Tatum natively or via the address of the ERC20 token.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="contractAddress">ERC20 contract address to get balance of. Either contractAddress, or currency must be present.</param>
        /// <param name="decimals">Decimal places for display balance</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<decimal> Ethereum_ERC20_GetBalance(string address, string contractAddress, int decimals = 0, CancellationToken ct = default) => Ethereum_ERC20_GetBalance_Async(address, contractAddress, decimals, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Ethereum ERC20 Account balance<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum ERC20 Account balance in the smallest ERC20 unit. 
        /// It is possible to obtain the balance of any ERC20 token, either supported by Tatum natively or via the address of the ERC20 token.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="contractAddress">ERC20 contract address to get balance of. Either contractAddress, or currency must be present.</param>
        /// <param name="decimals">Decimal places for display balance</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<decimal>> Ethereum_ERC20_GetBalance_Async(string address, string contractAddress, int decimals = 0, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "contractAddress", contractAddress },
            };
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ethereum_ERC20Balance, address));
            var result = await SendTatumRequest<EthereumBalance>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<decimal>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            var balance = new BigDecimal(BigInteger.Parse(result.Data.Balance), -decimals).ToDecimal();
            return new WebCallResult<decimal>(result.ResponseStatusCode, result.ResponseHeaders, balance, null);
        }

        /// <summary>
        /// <b>Title:</b> Get Ethereum Transaction<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum transaction by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<EthereumTransaction> Ethereum_GetTransactionByHash(string hash, CancellationToken ct = default) => Ethereum_GetTransactionByHash_Async(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Ethereum Transaction<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum transaction by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<EthereumTransaction>> Ethereum_GetTransactionByHash_Async(string hash, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ethereum_GetTransactionByHash, hash));
            return await SendTatumRequest<EthereumTransaction>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get count of outgoing Ethereum transactions<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get a number of outgoing Ethereum transactions for the address. 
        /// When a transaction is sent, there can be multiple outgoing transactions, which are not yet processed by the blockchain. 
        /// To distinguish between them, there is a counter called a nonce, which represents the order of the transaction in the list of outgoing transactions.
        /// </summary>
        /// <param name="address">address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<long> Ethereum_GetOutgoingTransactionsCount(string address, CancellationToken ct = default) => Ethereum_GetOutgoingTransactionsCount_Async(address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get count of outgoing Ethereum transactions<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get a number of outgoing Ethereum transactions for the address. 
        /// When a transaction is sent, there can be multiple outgoing transactions, which are not yet processed by the blockchain. 
        /// To distinguish between them, there is a counter called a nonce, which represents the order of the transaction in the list of outgoing transactions.
        /// </summary>
        /// <param name="address">address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<long>> Ethereum_GetOutgoingTransactionsCount_Async(string address, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ethereum_GetOutgoingTransactionsCount, address));
            var result = await SendTatumRequest<string>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<long>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<long>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.ToLong(), null);
        }

        /// <summary>
        /// <b>Title:</b> Get Ethereum transactions by address<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum transactions by address. This includes incoming and outgoing transactions for the address.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<IEnumerable<EthereumTransaction>> Ethereum_GetTransactionsByAddress(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default) => Ethereum_GetTransactionsByAddress_Async(address, pageSize, offset, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Ethereum transactions by address<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum transactions by address. This includes incoming and outgoing transactions for the address.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<IEnumerable<EthereumTransaction>>> Ethereum_GetTransactionsByAddress_Async(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);
            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };

            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ethereum_GetTransactionsByAddress, address));
            return await SendTatumRequest<IEnumerable<EthereumTransaction>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Send Ethereum / ERC20 from account to account<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Ethereum or Tatum supported ERC20 token from account to account.
        /// This operation needs the private key of the blockchain address.
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key.
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds.
        /// In this method, it is possible to enter privateKey or signatureId.
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds.
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request.
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="currency">Currency to transfer from Ethereum Blockchain Account.</param>
        /// <param name="amount">Amount to be sent in Ether.</param>
        /// <param name="to">Blockchain address to send assets</param>
        /// <param name="data">Additinal data, that can be passed to blockchain transaction as data property. Only for ETH transactions.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Ethereum_Send(
            EthereumPredefinedCurrency currency,
            string amount,
            string to,
            string data = null,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
            => Ethereum_Send_Async(currency, amount, to, data, signatureId, fromPrivateKey, nonce, fee, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send Ethereum / ERC20 from account to account<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Ethereum or Tatum supported ERC20 token from account to account.
        /// This operation needs the private key of the blockchain address.
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key.
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds.
        /// In this method, it is possible to enter privateKey or signatureId.
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds.
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request.
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="currency">Currency to transfer from Ethereum Blockchain Account.</param>
        /// <param name="amount">Amount to be sent in Ether.</param>
        /// <param name="to">Blockchain address to send assets</param>
        /// <param name="data">Additinal data, that can be passed to blockchain transaction as data property. Only for ETH transactions.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Ethereum_Send_Async(
            EthereumPredefinedCurrency currency,
            string amount,
            string to,
            string data = null,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "currency", JsonConvert.SerializeObject(currency, new EthereumPredefinedCurrencyConverter(false)) },
                { "amount", amount },
                { "to", to },
            };
            parameters.AddOptionalParameter("nonce", nonce);
            parameters.AddOptionalParameter("data", data);
            parameters.AddOptionalParameter("fee", fee);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("fromPrivateKey", fromPrivateKey);

            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Ethereum_Send));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Invoke Smart Contract method<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Invoke any method on an existing Smart Contract. 
        /// It is possibleto call read or write method on the Smsrt Contract defined via contractAddress. 
        /// For read operations, data is returned, for write operations, transaction Id of the associated transaction is returned.
        /// This operation needs the private key of the blockchain address.
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key.
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. 
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="contractAddress">Address of ERC20 token</param>
        /// <param name="methodName">Name of the method to invoke on smart contract.</param>
        /// <param name="methodABI">ABI of the method to invoke.</param>
        /// <param name="method_params">Parameters of the method to be invoked.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Ethereum_InvokeSmartContractMethod(
            string contractAddress,
            string methodName,
            object methodABI,
            IEnumerable<object> method_params,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
            => Ethereum_InvokeSmartContractMethod_Async(contractAddress, methodName, methodABI, method_params, signatureId, fromPrivateKey, nonce, fee, ct).Result;
        /// <summary>
        /// <b>Title:</b> Invoke Smart Contract method<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Invoke any method on an existing Smart Contract. 
        /// It is possibleto call read or write method on the Smsrt Contract defined via contractAddress. 
        /// For read operations, data is returned, for write operations, transaction Id of the associated transaction is returned.
        /// This operation needs the private key of the blockchain address.
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key.
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. 
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="contractAddress">Address of ERC20 token</param>
        /// <param name="methodName">Name of the method to invoke on smart contract.</param>
        /// <param name="methodABI">ABI of the method to invoke.</param>
        /// <param name="method_params">Parameters of the method to be invoked.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Ethereum_InvokeSmartContractMethod_Async(
            string contractAddress,
            string methodName,
            object methodABI,
            IEnumerable<object> method_params,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "contractAddress", contractAddress },
                { "methodName", methodName },
                { "methodABI", methodABI },
                { "params", method_params },
            };
            parameters.AddOptionalParameter("fee", fee);
            parameters.AddOptionalParameter("nonce", nonce);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("fromPrivateKey", fromPrivateKey);

            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Ethereum_SmartContract));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Deploy Ethereum ERC20 Smart Contract.<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Deploy Ethereum ERC20 Smart Contract.
        /// This method creates new ERC20(Fungible Tokens) Smart Contract on the blockchain.Smart contract is standardized and audited.
        /// It is possible to mint and burn tokens.It is possible to see the code of the deployed contract here. 
        /// Whole predefined supply of the tokens will be transferred to the chosen address.
        /// This operation needs the private key of the blockchain address.
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key.
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. 
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="name">Name of the ERC20 token</param>
        /// <param name="symbol">Symbol of the ERC20 token</param>
        /// <param name="supply">Max supply of ERC20 token.</param>
        /// <param name="digits">Number of decimal points</param>
        /// <param name="address">Address on Ethereum blockchain, where all created ERC20 tokens will be transferred.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of Ethereum account address, from which the fee for the deployment of ERC20 will be paid. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Ethereum_ERC20_DeploySmartContract(
            string name,
            string symbol,
            string supply,
            int digits,
            string address,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
            => Ethereum_ERC20_DeploySmartContract_Async(name, symbol, supply, digits, address, signatureId, fromPrivateKey, nonce, fee, ct).Result;
        /// <summary>
        /// <b>Title:</b> Deploy Ethereum ERC20 Smart Contract.<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Deploy Ethereum ERC20 Smart Contract.
        /// This method creates new ERC20(Fungible Tokens) Smart Contract on the blockchain.Smart contract is standardized and audited.
        /// It is possible to mint and burn tokens.It is possible to see the code of the deployed contract here. 
        /// Whole predefined supply of the tokens will be transferred to the chosen address.
        /// This operation needs the private key of the blockchain address.
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key.
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. 
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="name">Name of the ERC20 token</param>
        /// <param name="symbol">Symbol of the ERC20 token</param>
        /// <param name="supply">Max supply of ERC20 token.</param>
        /// <param name="digits">Number of decimal points</param>
        /// <param name="address">Address on Ethereum blockchain, where all created ERC20 tokens will be transferred.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of Ethereum account address, from which the fee for the deployment of ERC20 will be paid. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Ethereum_ERC20_DeploySmartContract_Async(
            string name,
            string symbol,
            string supply,
            int digits,
            string address,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "name", name },
                { "symbol", symbol },
                { "supply", supply },
                { "digits", digits },
                { "address", address },
            };
            parameters.AddOptionalParameter("fee", fee);
            parameters.AddOptionalParameter("nonce", nonce);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("fromPrivateKey", fromPrivateKey);

            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Ethereum_ERC20DeploySmartContract));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Transfer Ethereum ERC20<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Transfer Ethereum ERC20 Smart Contract Tokens from account to account. 
        /// Transfer any ERC20 tokens from smart contract defined in contractAddress. 
        /// This method invokes ERC20 method transfer() to transfer tokens.
        /// This operation needs the private key of the blockchain address.
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key.
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. 
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="contractAddress">Address of ERC20 token</param>
        /// <param name="to">Blockchain address to send ERC20 token to</param>
        /// <param name="amount">Amount to be sent.</param>
        /// <param name="digits">Number of decimal points that ERC20 token has.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Ethereum_ERC20_Transfer(
            string contractAddress,
            string to,
            string amount,
            int digits,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
            => Ethereum_ERC20_Transfer_Async(contractAddress, to, amount, digits, signatureId, fromPrivateKey, nonce, fee, ct).Result;
        /// <summary>
        /// <b>Title:</b> Transfer Ethereum ERC20<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Transfer Ethereum ERC20 Smart Contract Tokens from account to account. 
        /// Transfer any ERC20 tokens from smart contract defined in contractAddress. 
        /// This method invokes ERC20 method transfer() to transfer tokens.
        /// This operation needs the private key of the blockchain address.
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key.
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. 
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="contractAddress">Address of ERC20 token</param>
        /// <param name="to">Blockchain address to send ERC20 token to</param>
        /// <param name="amount">Amount to be sent.</param>
        /// <param name="digits">Number of decimal points that ERC20 token has.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Ethereum_ERC20_Transfer_Async(
            string contractAddress,
            string to,
            string amount,
            int digits,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "contractAddress", contractAddress },
                { "to", to },
                { "amount", amount },
                { "digits", digits },
            };
            parameters.AddOptionalParameter("fee", fee);
            parameters.AddOptionalParameter("nonce", nonce);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("fromPrivateKey", fromPrivateKey);

            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Ethereum_ERC20Transfer));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Deploy Ethereum ERC721 Smart Contract.<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Deploy Ethereum ERC721 Smart Contract. 
        /// This method creates new ERC721 Smart Contract (Non-Fungible Tokens) on the blockchain. 
        /// Smart contract is standardized and audited. It is possible to mint, burn and transfer tokens. 
        /// It is also possible to mint multiple tokens at once. It is possible to see the code of the deployed contract here.
        /// This operation needs the private key of the blockchain address.
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key.
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. 
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="name">Name of the ERC721 token</param>
        /// <param name="symbol">Symbol of the ERC721 token</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of Ethereum account address, from which gas for deployment of ERC721 will be paid. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Ethereum_ERC721_DeploySmartContract(
            string name,
            string symbol,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
            => Ethereum_ERC721_DeploySmartContract_Async(name, symbol, signatureId, fromPrivateKey, nonce, fee, ct).Result;
        /// <summary>
        /// <b>Title:</b> Deploy Ethereum ERC721 Smart Contract.<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Deploy Ethereum ERC721 Smart Contract. 
        /// This method creates new ERC721 Smart Contract (Non-Fungible Tokens) on the blockchain. 
        /// Smart contract is standardized and audited. It is possible to mint, burn and transfer tokens. 
        /// It is also possible to mint multiple tokens at once. It is possible to see the code of the deployed contract here.
        /// This operation needs the private key of the blockchain address.
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key.
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. 
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="name">Name of the ERC721 token</param>
        /// <param name="symbol">Symbol of the ERC721 token</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of Ethereum account address, from which gas for deployment of ERC721 will be paid. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Ethereum_ERC721_DeploySmartContract_Async(
            string name,
            string symbol,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "name", name },
                { "symbol", symbol },
            };
            parameters.AddOptionalParameter("fee", fee);
            parameters.AddOptionalParameter("nonce", nonce);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("fromPrivateKey", fromPrivateKey);

            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Ethereum_ERC721DeploySmartContract));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Mint Ethereum ERC721<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create one Ethereum ERC721 Smart Contract Token and transfer it to destination account. 
        /// Create and transfer any ERC721 token from smart contract defined in contractAddress. 
        /// It is possible to add URL to the created token with a more detailed information about it.
        /// This operation needs the private key of the blockchain address.
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key.
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds.
        /// In this method, it is possible to enter privateKey or signatureId.
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds.
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request.
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="contractAddress">Address of ERC721 token</param>
        /// <param name="tokenId">ID of token to be created.</param>
        /// <param name="to">Blockchain address to send ERC721 token to</param>
        /// <param name="url">Metadata of the token, usually as URL.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Ethereum_ERC721_Mint(
            string contractAddress,
            string tokenId,
            string to,
            string url,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
            => Ethereum_ERC721_Mint_Async(contractAddress, tokenId, to, url, signatureId, fromPrivateKey, nonce, fee, ct).Result;
        /// <summary>
        /// <b>Title:</b> Mint Ethereum ERC721<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create one Ethereum ERC721 Smart Contract Token and transfer it to destination account. 
        /// Create and transfer any ERC721 token from smart contract defined in contractAddress. 
        /// It is possible to add URL to the created token with a more detailed information about it.
        /// This operation needs the private key of the blockchain address.
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key.
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds.
        /// In this method, it is possible to enter privateKey or signatureId.
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds.
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request.
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="contractAddress">Address of ERC721 token</param>
        /// <param name="tokenId">ID of token to be created.</param>
        /// <param name="to">Blockchain address to send ERC721 token to</param>
        /// <param name="url">Metadata of the token, usually as URL.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Ethereum_ERC721_Mint_Async(
            string contractAddress,
            string tokenId,
            string to,
            string url,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "contractAddress", contractAddress },
                { "tokenId", tokenId },
                { "to", to },
                { "url", url },
            };
            parameters.AddOptionalParameter("fee", fee);
            parameters.AddOptionalParameter("nonce", nonce);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("fromPrivateKey", fromPrivateKey);

            var credits = 2;
            var url_ = GetUrl(string.Format(Endpoints_Ethereum_ERC721Mint));
            var result = await SendTatumRequest<BlockchainResponse>(url_, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Transfer Ethereum ERC721 Token<br />
        /// <b>Credits:</b> 2 credits per API call<br />
        /// <b>Description:</b>
        /// Transfer Ethereum ERC721 Smart Contract Tokens from account to account. 
        /// Transfer any ERC721 token from smart contract defined in contractAddress. 
        /// Only 1 specific token with specified tokenId can be transfered. This method invokes ERC721 method safeTransfer() to transfer the token.
        /// This operation needs the private key of the blockchain address. 
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key. 
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. 
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="contractAddress">Address of ERC721 token</param>
        /// <param name="tokenId">ID of token.</param>
        /// <param name="to">Blockchain address to send ERC721 token to</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Ethereum_ERC721_Transfer(
            string contractAddress,
            string tokenId,
            string to,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
            => Ethereum_ERC721_Transfer_Async(contractAddress, tokenId, to, signatureId, fromPrivateKey, nonce, fee, ct).Result;
        /// <summary>
        /// <b>Title:</b> Transfer Ethereum ERC721 Token<br />
        /// <b>Credits:</b> 2 credits per API call<br />
        /// <b>Description:</b>
        /// Transfer Ethereum ERC721 Smart Contract Tokens from account to account. 
        /// Transfer any ERC721 token from smart contract defined in contractAddress. 
        /// Only 1 specific token with specified tokenId can be transfered. This method invokes ERC721 method safeTransfer() to transfer the token.
        /// This operation needs the private key of the blockchain address. 
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key. 
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. 
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="contractAddress">Address of ERC721 token</param>
        /// <param name="tokenId">ID of token.</param>
        /// <param name="to">Blockchain address to send ERC721 token to</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Ethereum_ERC721_Transfer_Async(
            string contractAddress,
            string tokenId,
            string to,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "contractAddress", contractAddress },
                { "tokenId", tokenId },
                { "to", to },
            };
            parameters.AddOptionalParameter("fee", fee);
            parameters.AddOptionalParameter("nonce", nonce);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("fromPrivateKey", fromPrivateKey);

            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Ethereum_ERC721Transfer));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Mint Ethereum ERC721 Multiple Tokens<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create multiple Ethereum ERC721 Smart Contract Tokens and transfer them to destination account. 
        /// Create and transfer any ERC721 tokens from smart contract defined in contractAddress.
        /// This operation needs the private key of the blockchain address. 
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key. 
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. 
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="contractAddress">Address of ERC721 token</param>
        /// <param name="tokenId">ID of token to be created.</param>
        /// <param name="to">Blockchain address to send ERC721 token to.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Ethereum_ERC721_MintMultiple(
            string contractAddress,
            IEnumerable<string> tokenId,
            IEnumerable<string> to,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
            => Ethereum_ERC721_MintMultiple_Async(contractAddress, tokenId, to, signatureId, fromPrivateKey, nonce, fee, ct).Result;
        /// <summary>
        /// <b>Title:</b> Mint Ethereum ERC721 Multiple Tokens<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create multiple Ethereum ERC721 Smart Contract Tokens and transfer them to destination account. 
        /// Create and transfer any ERC721 tokens from smart contract defined in contractAddress.
        /// This operation needs the private key of the blockchain address. 
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key. 
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. 
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="contractAddress">Address of ERC721 token</param>
        /// <param name="tokenId">ID of token to be created.</param>
        /// <param name="to">Blockchain address to send ERC721 token to.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Ethereum_ERC721_MintMultiple_Async(
            string contractAddress,
            IEnumerable<string> tokenId,
            IEnumerable<string> to,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "contractAddress", contractAddress },
                { "tokenId", tokenId },
                { "to", to },
            };
            parameters.AddOptionalParameter("fee", fee);
            parameters.AddOptionalParameter("nonce", nonce);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("fromPrivateKey", fromPrivateKey);

            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Ethereum_ERC721MintMultiple));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Burn Ethereum ERC721<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Burn one Ethereum ERC721 Smart Contract Token. This method destroys any ERC721 token from smart contract defined in contractAddress.
        /// This operation needs the private key of the blockchain address. 
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key. 
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. 
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="contractAddress">Address of ERC721 token</param>
        /// <param name="tokenId">ID of token to be destroyed.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Ethereum_ERC721_Burn(
            string contractAddress,
            string tokenId,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
            => Ethereum_ERC721_Burn_Async(contractAddress, tokenId, signatureId, fromPrivateKey, nonce, fee, ct).Result;
        /// <summary>
        /// <b>Title:</b> Burn Ethereum ERC721<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Burn one Ethereum ERC721 Smart Contract Token. This method destroys any ERC721 token from smart contract defined in contractAddress.
        /// This operation needs the private key of the blockchain address. 
        /// Every time the funds are transferred, the transaction must be signed with the corresponding private key. 
        /// No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. 
        /// In this method, it is possible to enter privateKey or signatureId. 
        /// PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. 
        /// In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. 
        /// Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="contractAddress">Address of ERC721 token</param>
        /// <param name="tokenId">ID of token to be destroyed.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Ethereum_ERC721_Burn_Async(
            string contractAddress,
            string tokenId,
            string signatureId = null,
            string fromPrivateKey = null,
            long? nonce = null,
            EthereumFee fee = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "contractAddress", contractAddress },
                { "tokenId", tokenId },
            };
            parameters.AddOptionalParameter("fee", fee);
            parameters.AddOptionalParameter("nonce", nonce);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("fromPrivateKey", fromPrivateKey);

            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Ethereum_ERC721Burn));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Get Ethereum ERC721 Account balance<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum ERC721 Account balance. Returns number of tokens Account holds.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="contractAddress">ERC721 contract address</param>
        /// <param name="decimals">Decimal places for display balance</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<decimal> Ethereum_ERC721_GetBalance(string address, string contractAddress, int decimals = 0, CancellationToken ct = default) => Ethereum_GetERC721Balance_Async(address, contractAddress, decimals, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Ethereum ERC721 Account balance<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum ERC721 Account balance. Returns number of tokens Account holds.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="contractAddress">ERC721 contract address</param>
        /// <param name="decimals">Decimal places for display balance</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<decimal>> Ethereum_GetERC721Balance_Async(string address, string contractAddress, int decimals = 0, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ethereum_ERC721Balance, address, contractAddress));
            var result = await SendTatumRequest<EthereumData>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<decimal>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            var balance = new BigDecimal(BigInteger.Parse(result.Data.Data), -decimals).ToDecimal();
            return new WebCallResult<decimal>(result.ResponseStatusCode, result.ResponseHeaders, balance, null);
        }

        /// <summary>
        /// <b>Title:</b> Get Ethereum ERC721 Token<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum ERC721 token at given index of Account. Total number of tokens can be obtained from Get Balance operation.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="index">Token index</param>
        /// <param name="contractAddress">ERC721 contract address</param>
        /// <param name="decimals">Decimal places for display balance</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<decimal> Ethereum_ERC721_GetToken(string address, int index, string contractAddress, int decimals = 0, CancellationToken ct = default) => Ethereum_ERC721_GetToken_Async(address, index, contractAddress, decimals, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Ethereum ERC721 Token<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum ERC721 token at given index of Account. Total number of tokens can be obtained from Get Balance operation.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="index">Token index</param>
        /// <param name="contractAddress">ERC721 contract address</param>
        /// <param name="decimals">Decimal places for display balance</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<decimal>> Ethereum_ERC721_GetToken_Async(string address, int index, string contractAddress, int decimals = 0, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ethereum_ERC721Token, address, index, contractAddress));
            var result = await SendTatumRequest<EthereumData>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<decimal>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            var balance = new BigDecimal(BigInteger.Parse(result.Data.Data), -decimals).ToDecimal();
            return new WebCallResult<decimal>(result.ResponseStatusCode, result.ResponseHeaders, balance, null);
        }

        /// <summary>
        /// <b>Title:</b> Get Ethereum ERC721 Token Metadata<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum ERC721 token metadata.
        /// </summary>
        /// <param name="token">Token ID</param>
        /// <param name="contractAddress">ERC721 contract address</param>
        /// <param name="divider">Divider for result to get display balance</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<EthereumData> Ethereum_ERC721_GetTokenMetadata(string token, string contractAddress, decimal divider = 1, CancellationToken ct = default) => Ethereum_ERC721_GetTokenMetadata_Async(token, contractAddress, divider, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Ethereum ERC721 Token Metadata<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum ERC721 token metadata.
        /// </summary>
        /// <param name="token">Token ID</param>
        /// <param name="contractAddress">ERC721 contract address</param>
        /// <param name="divider">Divider for result to get display balance</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<EthereumData>> Ethereum_ERC721_GetTokenMetadata_Async(string token, string contractAddress, decimal divider = 1, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ethereum_ERC721TokenMetadata, token, contractAddress));
            return await SendTatumRequest<EthereumData>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Ethereum ERC721 Token owner<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum ERC721 token owner.
        /// </summary>
        /// <param name="token">Token ID</param>
        /// <param name="contractAddress">ERC721 contract address</param>
        /// <param name="divider">Divider for result to get display balance</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<EthereumData> Ethereum_ERC721_GetTokenOwner(string token, string contractAddress, decimal divider = 1, CancellationToken ct = default) => Ethereum_ERC721_GetTokenOwner_Async(token, contractAddress, divider, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Ethereum ERC721 Token owner<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum ERC721 token owner.
        /// </summary>
        /// <param name="token">Token ID</param>
        /// <param name="contractAddress">ERC721 contract address</param>
        /// <param name="divider">Divider for result to get display balance</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<EthereumData>> Ethereum_ERC721_GetTokenOwner_Async(string token, string contractAddress, decimal divider = 1, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ethereum_ERC721TokenOwner, token, contractAddress));
            return await SendTatumRequest<EthereumData>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Broadcast signed Ethereum transaction<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed transaction to Ethereum blockchain. 
        /// This method is used internally from Tatum KMS, Tatum Middleware or Tatum client libraries. 
        /// It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="signatureId">ID of prepared payment template to sign. Required only, when broadcasting transaction signed by Tatum KMS.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Ethereum_Broadcast(string txData, string signatureId, CancellationToken ct = default) => Ethereum_Broadcast_Async(txData, signatureId, ct).Result;
        /// <summary>
        /// <b>Title:</b> Broadcast signed Ethereum transaction<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed transaction to Ethereum blockchain. 
        /// This method is used internally from Tatum KMS, Tatum Middleware or Tatum client libraries. 
        /// It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="signatureId">ID of prepared payment template to sign. Required only, when broadcasting transaction signed by Tatum KMS.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Ethereum_Broadcast_Async(string txData, string signatureId, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "txData", txData },
            };
            parameters.AddOptionalParameter("signatureId", signatureId);

            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Ethereum_Broadcast));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }
        #endregion

        #region Blockchain / Litecoin
        /// <summary>
        /// <b>Title:</b> Generate Litecoin wallet<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. 
        /// It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. 
        /// Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// Tatum follows BIP44 specification and generates for Litecoin wallet with derivation path m'/44'/2'/0'/0. 
        /// More about BIP44 HD wallets can be found here - https://github.com/litecoin/bips/blob/master/bip-0044.mediawiki. 
        /// Generate BIP44 compatible Litecoin wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainWallet> Litecoin_GenerateWallet(string mnemonics, CancellationToken ct = default) => Blockchain_GenerateWallet_Async(BlockchainType.Litecoin, new List<string> { mnemonics }, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Litecoin wallet<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. 
        /// It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. 
        /// Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// Tatum follows BIP44 specification and generates for Litecoin wallet with derivation path m'/44'/2'/0'/0. 
        /// More about BIP44 HD wallets can be found here - https://github.com/litecoin/bips/blob/master/bip-0044.mediawiki. 
        /// Generate BIP44 compatible Litecoin wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainWallet> Litecoin_GenerateWallet(IEnumerable<string> mnemonics = null, CancellationToken ct = default) => Blockchain_GenerateWallet_Async(BlockchainType.Litecoin, mnemonics, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Litecoin wallet<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. 
        /// It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. 
        /// Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// Tatum follows BIP44 specification and generates for Litecoin wallet with derivation path m'/44'/2'/0'/0. 
        /// More about BIP44 HD wallets can be found here - https://github.com/litecoin/bips/blob/master/bip-0044.mediawiki. 
        /// Generate BIP44 compatible Litecoin wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainWallet>> Litecoin_GenerateWallet_Async(string mnemonics, CancellationToken ct = default) => await Blockchain_GenerateWallet_Async(BlockchainType.Litecoin, new List<string> { mnemonics }, ct);
        /// <summary>
        /// <b>Title:</b> Generate Litecoin wallet<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. 
        /// It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. 
        /// Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// Tatum follows BIP44 specification and generates for Litecoin wallet with derivation path m'/44'/2'/0'/0. 
        /// More about BIP44 HD wallets can be found here - https://github.com/litecoin/bips/blob/master/bip-0044.mediawiki. 
        /// Generate BIP44 compatible Litecoin wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainWallet>> Litecoin_GenerateWallet_Async(IEnumerable<string> mnemonics = null, CancellationToken ct = default) => await Blockchain_GenerateWallet_Async(BlockchainType.Litecoin, mnemonics, ct);

        /// <summary>
        /// <b>Title:</b> Get Litecoin Blockchain Information<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Litecoin Blockchain Information. Obtain basic info like testnet / mainent version of the chain, current block number and it's hash.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<LitecoinChainInfo> Litecoin_GetBlockchainInformation(CancellationToken ct = default) => Litecoin_GetBlockchainInformation_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Litecoin Blockchain Information<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Litecoin Blockchain Information. Obtain basic info like testnet / mainent version of the chain, current block number and it's hash.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<LitecoinChainInfo>> Litecoin_GetBlockchainInformation_Async(CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Litecoin_BlockchainInformation));
            return await SendTatumRequest<LitecoinChainInfo>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Litecoin Block hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Litecoin Block hash. Returns hash of the block to get the block detail.
        /// </summary>
        /// <param name="block_id">The number of blocks preceding a particular block on a block chain.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainHash> Litecoin_GetBlockHash(long block_id, CancellationToken ct = default) => Litecoin_GetBlockHash_Async(block_id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Litecoin Block hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Litecoin Block hash. Returns hash of the block to get the block detail.
        /// </summary>
        /// <param name="block_id">The number of blocks preceding a particular block on a block chain.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainHash>> Litecoin_GetBlockHash_Async(long block_id, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Litecoin_GetBlockHash, block_id));
            return await SendTatumRequest<BlockchainHash>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Litecoin Block by hash or height<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Litecoin Block detail by block hash or height.
        /// </summary>
        /// <param name="hash_height">Block hash or height.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<LitecoinBlock> Litecoin_GetBlock(string hash_height, CancellationToken ct = default) => Litecoin_GetBlock_Async(hash_height, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Litecoin Block by hash or height<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Litecoin Block detail by block hash or height.
        /// </summary>
        /// <param name="hash_height">Block hash or height.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<LitecoinBlock>> Litecoin_GetBlock_Async(string hash_height, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Litecoin_GetBlockByHash, hash_height));
            return await SendTatumRequest<LitecoinBlock>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Litecoin Transaction by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Litecoin Transaction detail by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<LitecoinTransaction> Litecoin_GetTransactionByHash(string hash, CancellationToken ct = default) => Litecoin_GetTransactionByHash_Async(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Litecoin Transaction by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Litecoin Transaction detail by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<LitecoinTransaction>> Litecoin_GetTransactionByHash_Async(string hash, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Litecoin_GetTransactionByHash, hash));
            return await SendTatumRequest<LitecoinTransaction>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Litecoin Transactions by address<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Litecoin Transaction by address.
        /// </summary>
        /// <param name="address">Address</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<IEnumerable<LitecoinTransaction>> Litecoin_GetTransactionsByAddress(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default) => Litecoin_GetTransactionsByAddress_Async(address, pageSize, offset, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Litecoin Transactions by address<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Litecoin Transaction by address.
        /// </summary>
        /// <param name="address">Address</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<IEnumerable<LitecoinTransaction>>> Litecoin_GetTransactionsByAddress_Async(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);

            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };

            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Litecoin_GetTransactionsByAddress, address));
            return await SendTatumRequest<IEnumerable<LitecoinTransaction>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Litecoin Balance of the address<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Litecoin Balance of the address.
        /// </summary>
        /// <param name="address">Address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<LitecoinBalance> Litecoin_GetBalance(string address, CancellationToken ct = default) => Litecoin_GetBalance_Async(address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Litecoin Balance of the address<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Litecoin Balance of the address.
        /// </summary>
        /// <param name="address">Address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<LitecoinBalance>> Litecoin_GetBalance_Async(string address, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Litecoin_GetBalance, address));
            return await SendTatumRequest<LitecoinBalance>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Litecoin UTXO of Transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get UTXO of given transaction and output index. 
        /// UTXO means Unspent Transaction Output, which is in blockchain terminology assets, that user received on the specific address and does not spend it yet.
        /// In bitcoin-like blockchains(BTC, LTC, BCH), every transaction is built from the list of previously not spent transactions connected to the address.
        /// If user owns address A, receives in transaciont T1 10 LTC, he can spend in the next transaction UTXO T1 of total value 10 LTC.
        /// User can spend multiple UTXOs from different addresses in 1 transaction.
        /// If UTXO is not spent, data are returned, otherwise 404 error code.
        /// </summary>
        /// <param name="txhash">TX Hash</param>
        /// <param name="index">Index of tx output to check if spent or not</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<LitecoinUTXO> Litecoin_GetTransactionUTXO(string txhash, long index, CancellationToken ct = default) => Litecoin_GetTransactionUTXO_Async(txhash, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Litecoin UTXO of Transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get UTXO of given transaction and output index. 
        /// UTXO means Unspent Transaction Output, which is in blockchain terminology assets, that user received on the specific address and does not spend it yet.
        /// In bitcoin-like blockchains(BTC, LTC, BCH), every transaction is built from the list of previously not spent transactions connected to the address.
        /// If user owns address A, receives in transaciont T1 10 LTC, he can spend in the next transaction UTXO T1 of total value 10 LTC.
        /// User can spend multiple UTXOs from different addresses in 1 transaction.
        /// If UTXO is not spent, data are returned, otherwise 404 error code.
        /// </summary>
        /// <param name="txhash">TX Hash</param>
        /// <param name="index">Index of tx output to check if spent or not</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<LitecoinUTXO>> Litecoin_GetTransactionUTXO_Async(string txhash, long index, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Litecoin_GetTransactionUTXO, txhash, index));
            return await SendTatumRequest<LitecoinUTXO>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Generate Litecoin deposit address from Extended public key<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate Litecoin deposit address from Extended public key. 
        /// Deposit address is generated for the specific index - each extended public key can generate up to 2^32 addresses starting from index 0 until 2^31.
        /// </summary>
        /// <param name="xpub">Extended public key of wallet.</param>
        /// <param name="index">Derivation index of desired address to be generated.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainAddress> Litecoin_GenerateDepositAddress(string xpub, int index, CancellationToken ct = default) => Blockchain_GenerateDepositAddress_Async(BlockchainType.Litecoin, xpub, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Litecoin deposit address from Extended public key<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate Litecoin deposit address from Extended public key. 
        /// Deposit address is generated for the specific index - each extended public key can generate up to 2^32 addresses starting from index 0 until 2^31.
        /// </summary>
        /// <param name="xpub">Extended public key of wallet.</param>
        /// <param name="index">Derivation index of desired address to be generated.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainAddress>> Litecoin_GenerateDepositAddress_Async(string xpub, int index, CancellationToken ct = default) => await Blockchain_GenerateDepositAddress_Async(BlockchainType.Litecoin, xpub, index, ct);

        /// <summary>
        /// <b>Title:</b> Generate Litecoin private key<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate private key for address from mnemonic for given derivation path index. 
        /// Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to generate private key from.</param>
        /// <param name="index">Derivation index of private key to generate</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainKey> Litecoin_GeneratePrivateKey(string mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKey_Async(BlockchainType.Litecoin, new List<string> { mnemonics }, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Litecoin private key<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate private key for address from mnemonic for given derivation path index. 
        /// Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to generate private key from.</param>
        /// <param name="index">Derivation index of private key to generate</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainKey> Litecoin_GeneratePrivateKey(IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKey_Async(BlockchainType.Litecoin, mnemonics, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Litecoin private key<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate private key for address from mnemonic for given derivation path index. 
        /// Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to generate private key from.</param>
        /// <param name="index">Derivation index of private key to generate</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainKey>> Litecoin_GeneratePrivateKey_Async(string mnemonics, int index, CancellationToken ct = default) => await Blockchain_GeneratePrivateKey_Async(BlockchainType.Litecoin, new List<string> { mnemonics }, index, default);
        /// <summary>
        /// <b>Title:</b> Generate Litecoin private key<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate private key for address from mnemonic for given derivation path index. 
        /// Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to generate private key from.</param>
        /// <param name="index">Derivation index of private key to generate</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainKey>> Litecoin_GeneratePrivateKey_Async(IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => await Blockchain_GeneratePrivateKey_Async(BlockchainType.Litecoin, mnemonics, index, default);

        /// <summary>
        /// <b>Title:</b> Send Litecoin to blockchain addresses<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Litecoin to blockchain addresses. It is possible to build a blockchain transaction in 2 ways:
        /// - fromAddress - assets will be sent from the list of addresses.For each of the addresses last 100 transactions will be scanned for any unspent UTXO and will be included in the transaction.
        /// - fromUTXO - assets will be sent from the list of unspent UTXOs. Each of the UTXO will be included in the transaction.
        /// In bitcoin-like blockchains, the transaction is created from the list of previously not spent UTXO.Every UTXO contains the number of funds, which can be spent. When the UTXO enters into the transaction, the whole amount is included and must be spent.For example, address A receives 2 transactions, T1 with 1 LTC and T2 with 2 LTC.The transaction, which will consume UTXOs for T1 and T2, will have available amount to spent 3 LTC = 1 LTC (T1) + 2 LTC(T2).
        /// There can be multiple recipients of the transactions, not only one.In the to section, every recipient address has it's corresponding amount. When the amount of funds, that should receive the recipient is lower than the number of funds from the UTXOs, the difference is used as a transaction fee.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="fromAddress">Array of addresses and corresponding private keys. Tatum will automatically scan last 100 transactions for each address and will use all of the unspent values. We advise to use this option if you have 1 address per 1 transaction only.</param>
        /// <param name="fromUTXO">Array of transaction hashes, index of UTXO in it and corresponding private keys. Use this option if you want to calculate amount to send manually. Either fromUTXO or fromAddress must be present.</param>
        /// <param name="to">Array of addresses and values to send Litecoins to. Values must be set in LTC. Difference between from and to is transaction fee.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Litecoin_Send(IEnumerable<LitecoinSendOrderFromAddress> fromAddress, IEnumerable<LitecoinSendOrderFromUTXO> fromUTXO, IEnumerable<LitecoinSendOrderTo> to, CancellationToken ct = default) => Litecoin_Send_Async(fromAddress, fromUTXO, to, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send Litecoin to blockchain addresses<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Litecoin to blockchain addresses. It is possible to build a blockchain transaction in 2 ways:
        /// - fromAddress - assets will be sent from the list of addresses.For each of the addresses last 100 transactions will be scanned for any unspent UTXO and will be included in the transaction.
        /// - fromUTXO - assets will be sent from the list of unspent UTXOs. Each of the UTXO will be included in the transaction.
        /// In bitcoin-like blockchains, the transaction is created from the list of previously not spent UTXO.Every UTXO contains the number of funds, which can be spent. When the UTXO enters into the transaction, the whole amount is included and must be spent.For example, address A receives 2 transactions, T1 with 1 LTC and T2 with 2 LTC.The transaction, which will consume UTXOs for T1 and T2, will have available amount to spent 3 LTC = 1 LTC (T1) + 2 LTC(T2).
        /// There can be multiple recipients of the transactions, not only one.In the to section, every recipient address has it's corresponding amount. When the amount of funds, that should receive the recipient is lower than the number of funds from the UTXOs, the difference is used as a transaction fee.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="fromAddress">Array of addresses and corresponding private keys. Tatum will automatically scan last 100 transactions for each address and will use all of the unspent values. We advise to use this option if you have 1 address per 1 transaction only.</param>
        /// <param name="fromUTXO">Array of transaction hashes, index of UTXO in it and corresponding private keys. Use this option if you want to calculate amount to send manually. Either fromUTXO or fromAddress must be present.</param>
        /// <param name="to">Array of addresses and values to send Litecoins to. Values must be set in LTC. Difference between from and to is transaction fee.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Litecoin_Send_Async(IEnumerable<LitecoinSendOrderFromAddress> fromAddress, IEnumerable<LitecoinSendOrderFromUTXO> fromUTXO, IEnumerable<LitecoinSendOrderTo> to, CancellationToken ct = default)
        {
            if ((fromAddress == null || fromAddress.Count() == 0) && (fromUTXO == null || fromUTXO.Count() == 0))
                throw new ArgumentException("Either fromUTXO or fromAddress must be present.");

            var parameters = new Dictionary<string, object> {
                { "to", to },
            };
            parameters.AddOptionalParameter("fromAddress", fromAddress);
            parameters.AddOptionalParameter("fromUTXO", fromUTXO);

            var credits = 10;
            var url = GetUrl(string.Format(Endpoints_Litecoin_Transaction));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Broadcast signed Litecoin transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed transaction to Litecoin blockchain. This method is used internally from Tatum KMS, Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="signatureId">ID of prepared payment template to sign. Required only, when broadcasting transaction signed by Tatum KMS.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Litecoin_Broadcast(string txData, string signatureId, CancellationToken ct = default) => Litecoin_Broadcast_Async(txData, signatureId, ct).Result;
        /// <summary>
        /// <b>Title:</b> Broadcast signed Litecoin transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed transaction to Litecoin blockchain. This method is used internally from Tatum KMS, Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="signatureId">ID of prepared payment template to sign. Required only, when broadcasting transaction signed by Tatum KMS.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Litecoin_Broadcast_Async(string txData, string signatureId, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "txData", txData },
            };
            parameters.AddOptionalParameter("signatureId", signatureId);

            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Litecoin_Broadcast));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        #endregion

        #region Blockchain / Ripple
        /// <summary>
        /// <b>Title:</b> Generate XRP account<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate XRP account. Tatum does not support HD wallet for XRP, only specific address and private key can be generated.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<RippleAddressSecret> Ripple_GenerateAccount(CancellationToken ct = default) => Ripple_GenerateAccount_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate XRP account<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate XRP account. Tatum does not support HD wallet for XRP, only specific address and private key can be generated.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<RippleAddressSecret>> Ripple_GenerateAccount_Async(CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Ripple_GenerateAccount));
            return await SendTatumRequest<RippleAddressSecret>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get XRP Blockchain Information<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XRP Blockchain last closed ledger index and hash.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<RippleChainInfo> Ripple_GetBlockchainInformation(CancellationToken ct = default) => Ripple_GetBlockchainInformation_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get XRP Blockchain Information<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XRP Blockchain last closed ledger index and hash.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<RippleChainInfo>> Ripple_GetBlockchainInformation_Async(CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Ripple_BlockchainInformation));
            return await SendTatumRequest<RippleChainInfo>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get actual Blockchain fee<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XRP Blockchain fee. Standard fee for the transaction is available in the drops.base_fee section and is 10 XRP drops by default. When there is a heavy traffic on the blockchain, fees are increasing according to current traffic.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<RippleChainFee> Ripple_GetBlockchainFee(CancellationToken ct = default) => Ripple_GetBlockchainFee_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get actual Blockchain fee<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XRP Blockchain fee. Standard fee for the transaction is available in the drops.base_fee section and is 10 XRP drops by default. When there is a heavy traffic on the blockchain, fees are increasing according to current traffic.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<RippleChainFee>> Ripple_GetBlockchainFee_Async(CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Ripple_BlockchainFee));
            return await SendTatumRequest<RippleChainFee>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Account transactions<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// List all Account transactions.
        /// </summary>
        /// <param name="account">Address of XRP account.</param>
        /// <param name="min">Ledger version to start scanning for transactions from.</param>
        /// <param name="marker">Marker from the last paginated request. It is stringified JSON from previous response.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<RippleAccountTransactions> Ripple_GetTransactionsByAccount(string account, int? min = null, RippleMarker marker = null, CancellationToken ct = default) => Ripple_GetTransactionsByAccount_Async(account, min, marker, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Account transactions<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// List all Account transactions.
        /// </summary>
        /// <param name="account">Address of XRP account.</param>
        /// <param name="min">Ledger version to start scanning for transactions from.</param>
        /// <param name="marker">Marker from the last paginated request. It is stringified JSON from previous response.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<RippleAccountTransactions>> Ripple_GetTransactionsByAccount_Async(string account, int? min = null, RippleMarker marker = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("min", min);
            parameters.AddOptionalParameter("marker", marker);

            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Ripple_GetTransactionsByAccount, account));
            return await SendTatumRequest<RippleAccountTransactions>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Ledger<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get ledger by sequence.
        /// </summary>
        /// <param name="index">Sequence of XRP ledger.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<RippleLedger> Ripple_GetLedger(long index, CancellationToken ct = default) => Ripple_GetLedger_Async(index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Ledger<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get ledger by sequence.
        /// </summary>
        /// <param name="index">Sequence of XRP ledger.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<RippleLedger>> Ripple_GetLedger_Async(long index, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Ripple_GetLedger, index));
            return await SendTatumRequest<RippleLedger>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get XRP Transaction by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XRP Transaction by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<RippleTransactionData> Ripple_GetTransactionByHash(string hash, CancellationToken ct = default) => Ripple_GetTransactionByHash_Async(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get XRP Transaction by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XRP Transaction by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<RippleTransactionData>> Ripple_GetTransactionByHash_Async(string hash, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Ripple_GetTransactionByHash, hash));
            return await SendTatumRequest<RippleTransactionData>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Account info<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XRP Account info.
        /// </summary>
        /// <param name="account">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<RippleAccount> Ripple_GetAccountInfo(string account, CancellationToken ct = default) => Ripple_GetAccountInfo_Async(account, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Account info<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XRP Account info.
        /// </summary>
        /// <param name="account">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<RippleAccount>> Ripple_GetAccountInfo_Async(string account, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Ripple_AccountInfo, account));
            return await SendTatumRequest<RippleAccount>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Account Balance<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XRP Account Balance. Obtain balance of the XRP and other assets on the account.
        /// </summary>
        /// <param name="account">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<RippleBalance> Ripple_GetBalance(string account, CancellationToken ct = default) => Ripple_GetBalance_Async(account, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Account Balance<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XRP Account Balance. Obtain balance of the XRP and other assets on the account.
        /// </summary>
        /// <param name="account">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<RippleBalance>> Ripple_GetBalance_Async(string account, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Ripple_GetBalance, account));
            return await SendTatumRequest<RippleBalance>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Send XRP from address to address<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send XRP from account to account.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="fromAccount">XRP account address. Must be the one used for generating deposit tags.</param>
        /// <param name="to">Blockchain address to send assets</param>
        /// <param name="amount">Amount to be sent, in XRP.</param>
        /// <param name="fromSecret">Secret for account. Secret, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the secret associated in signing application. Secret, or signature Id must be present.</param>
        /// <param name="fee">Fee to be paid, in XRP. If omitted, current fee will be calculated.</param>
        /// <param name="sourceTag">Source tag of sender account, if any.</param>
        /// <param name="destinationTag">Destination tag of recipient account, if any.</param>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets to create trust line for.</param>
        /// <param name="token">Asset name. Must be 160bit HEX string, e.g. SHA1.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Ripple_Send(
            string fromAccount,
            string to,
            string amount,
            string fromSecret = null,
            string signatureId = null,
            string fee = null,
            string sourceTag = null,
            string destinationTag = null,
            string issuerAccount = null,
            string token = null,
            CancellationToken ct = default) => Ripple_Send_Async(fromAccount, to, amount, fromSecret, signatureId, fee, sourceTag, destinationTag, issuerAccount, token, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send XRP from address to address<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send XRP from account to account.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="fromAccount">XRP account address. Must be the one used for generating deposit tags.</param>
        /// <param name="to">Blockchain address to send assets</param>
        /// <param name="amount">Amount to be sent, in XRP.</param>
        /// <param name="fromSecret">Secret for account. Secret, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the secret associated in signing application. Secret, or signature Id must be present.</param>
        /// <param name="fee">Fee to be paid, in XRP. If omitted, current fee will be calculated.</param>
        /// <param name="sourceTag">Source tag of sender account, if any.</param>
        /// <param name="destinationTag">Destination tag of recipient account, if any.</param>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets to create trust line for.</param>
        /// <param name="token">Asset name. Must be 160bit HEX string, e.g. SHA1.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Ripple_Send_Async(
            string fromAccount,
            string to,
            string amount,
            string fromSecret = null,
            string signatureId = null,
            string fee = null,
            string sourceTag = null,
            string destinationTag = null,
            string issuerAccount = null,
            string token = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "fromAccount", fromAccount },
                { "to", to },
                { "amount", amount },
            };
            parameters.AddOptionalParameter("fromSecret", fromSecret);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("fee", fee);
            parameters.AddOptionalParameter("sourceTag", sourceTag);
            parameters.AddOptionalParameter("destinationTag", destinationTag);
            parameters.AddOptionalParameter("issuerAccount", issuerAccount);
            parameters.AddOptionalParameter("token", token);

            var credits = 10;
            var url = GetUrl(string.Format(Endpoints_Ripple_Send));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Create / Update / Delete XRP trust line<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Create / Update / Delete XRP trust line between accounts to transfer private assets. By creating trustline for the first time, the asset is created automatically and can be used in the transactions.
        /// Account setting rippling must be enabled on the issuer account before the trust line creation to asset work correctly.Creating a trust line will cause an additional 5 XRP to be blocked on the account.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="fromAccount">XRP account address. Must be the one used for generating deposit tags.</param>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets to create trust line for.</param>
        /// <param name="limit">Amount of the assets to be permitted to send over this trust line. 0 means deletion of the trust line.</param>
        /// <param name="token">Asset name. Must be 160bit HEX string, e.g. SHA1.</param>
        /// <param name="fromSecret">Secret for account. Secret, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the secret associated in signing application. Secret, or signature Id must be present.</param>
        /// <param name="fee">Fee to be paid, in XRP. If omitted, current fee will be calculated.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Ripple_TrustLine(
            string fromAccount,
            string issuerAccount,
            string limit,
            string token,
            string fromSecret = null,
            string signatureId = null,
            string fee = null,
            CancellationToken ct = default) => Ripple_TrustLine_Async(fromAccount, issuerAccount, limit, token, fromSecret, signatureId, fee, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create / Update / Delete XRP trust line<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Create / Update / Delete XRP trust line between accounts to transfer private assets. By creating trustline for the first time, the asset is created automatically and can be used in the transactions.
        /// Account setting rippling must be enabled on the issuer account before the trust line creation to asset work correctly.Creating a trust line will cause an additional 5 XRP to be blocked on the account.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="fromAccount">XRP account address. Must be the one used for generating deposit tags.</param>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets to create trust line for.</param>
        /// <param name="limit">Amount of the assets to be permitted to send over this trust line. 0 means deletion of the trust line.</param>
        /// <param name="token">Asset name. Must be 160bit HEX string, e.g. SHA1.</param>
        /// <param name="fromSecret">Secret for account. Secret, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the secret associated in signing application. Secret, or signature Id must be present.</param>
        /// <param name="fee">Fee to be paid, in XRP. If omitted, current fee will be calculated.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Ripple_TrustLine_Async(
            string fromAccount,
            string issuerAccount,
            string limit,
            string token,
            string fromSecret = null,
            string signatureId = null,
            string fee = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "fromAccount", fromAccount },
                { "issuerAccount", issuerAccount },
                { "limit", limit },
                { "token", token },
            };
            parameters.AddOptionalParameter("fromSecret", fromSecret);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("fee", fee);

            var credits = 10;
            var url = GetUrl(string.Format(Endpoints_Ripple_Trust));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Modify XRP account<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Modify XRP account settings. If an XRP account should be an issuer of the custom asset, this accounts should have rippling enabled to true. In order to support off-chain processing, required destination tag should be set on the account.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="fromAccount">XRP account address. Must be the one used for generating deposit tags.</param>
        /// <param name="fromSecret">Secret for account. Secret, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Secret or signature Id must be present.</param>
        /// <param name="fee">Fee to be paid, in XRP. If omitted, current fee will be calculated.</param>
        /// <param name="rippling">Should be true, if an account is the issuer of assets.</param>
        /// <param name="requireDestinationTag">Should be true, if an account should support off-chain processing.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Ripple_ModifyAccountSettings(
            string fromAccount,
            string fromSecret = null,
            string signatureId = null,
            string fee = null,
            bool rippling = true,
            bool requireDestinationTag = true,
            CancellationToken ct = default) => Ripple_ModifyAccountSettings_Async(fromAccount, fromSecret, signatureId, fee, rippling, requireDestinationTag, ct).Result;
        /// <summary>
        /// <b>Title:</b> Modify XRP account<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Modify XRP account settings. If an XRP account should be an issuer of the custom asset, this accounts should have rippling enabled to true. In order to support off-chain processing, required destination tag should be set on the account.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="fromAccount">XRP account address. Must be the one used for generating deposit tags.</param>
        /// <param name="fromSecret">Secret for account. Secret, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Secret or signature Id must be present.</param>
        /// <param name="fee">Fee to be paid, in XRP. If omitted, current fee will be calculated.</param>
        /// <param name="rippling">Should be true, if an account is the issuer of assets.</param>
        /// <param name="requireDestinationTag">Should be true, if an account should support off-chain processing.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Ripple_ModifyAccountSettings_Async(
            string fromAccount,
            string fromSecret = null,
            string signatureId = null,
            string fee = null,
            bool rippling = true,
            bool requireDestinationTag = true,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "fromAccount", fromAccount },
                { "rippling", rippling },
                { "requireDestinationTag", requireDestinationTag },
            };
            parameters.AddOptionalParameter("fromSecret", fromSecret);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("fee", fee);

            var credits = 10;
            var url = GetUrl(string.Format(Endpoints_Ripple_AccountSettings));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Broadcast signed XRP transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed transaction to XRP blockchain. This method is used internally from Tatum KMS, Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="signatureId">ID of prepared payment template to sign. Required only, when broadcasting transaction signed by Tatum KMS.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Ripple_Broadcast(string txData, string signatureId, CancellationToken ct = default) => Ripple_Broadcast_Async(txData, signatureId, ct).Result;
        /// <summary>
        /// <b>Title:</b> Broadcast signed XRP transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed transaction to XRP blockchain. This method is used internally from Tatum KMS, Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="signatureId">ID of prepared payment template to sign. Required only, when broadcasting transaction signed by Tatum KMS.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Ripple_Broadcast_Async(string txData, string signatureId, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "txData", txData },
            };
            parameters.AddOptionalParameter("signatureId", signatureId);

            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Ripple_Broadcast));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        #endregion

        #region Blockchain / Stellar
        /// <summary>
        /// <b>Title:</b> Generate XLM account<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate XLM account. Tatum does not support HD wallet for XLM, only specific address and private key can be generated.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<StellarAddressSecret> Stellar_GenerateAccount(CancellationToken ct = default) => Stellar_GenerateAccount_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate XLM account<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate XLM account. Tatum does not support HD wallet for XLM, only specific address and private key can be generated.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<StellarAddressSecret>> Stellar_GenerateAccount_Async(CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Stellar_GenerateAccount));
            return await SendTatumRequest<StellarAddressSecret>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get XLM Blockchain Information<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Blockchain last closed ledger.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<StellarChainInfo> Stellar_GetBlockchainInformation(CancellationToken ct = default) => Stellar_GetBlockchainInformation_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get XLM Blockchain Information<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Blockchain last closed ledger.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<StellarChainInfo>> Stellar_GetBlockchainInformation_Async(CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Stellar_BlockchainInformation));
            return await SendTatumRequest<StellarChainInfo>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get XLM Blockchain Ledger by sequence<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Blockchain ledger for ledger sequence.
        /// </summary>
        /// <param name="sequence">Sequence of the ledger.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<StellarChainInfo> Stellar_GetLedger(long sequence, CancellationToken ct = default) => Stellar_GetLedger_Async(sequence, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get XLM Blockchain Ledger by sequence<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Blockchain ledger for ledger sequence.
        /// </summary>
        /// <param name="sequence">Sequence of the ledger.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<StellarChainInfo>> Stellar_GetLedger_Async(long sequence, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Stellar_GetLedger, sequence));
            return await SendTatumRequest<StellarChainInfo>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get XLM Blockchain Transactions in Ledger<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Blockchain transactions in the ledger.
        /// </summary>
        /// <param name="sequence">Sequence of the ledger.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<IEnumerable<StellarTransaction>> Stellar_GetTransactionsInLedger(long sequence, CancellationToken ct = default) => Stellar_GetTransactionsInLedger_Async(sequence, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get XLM Blockchain Transactions in Ledger<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Blockchain transactions in the ledger.
        /// </summary>
        /// <param name="sequence">Sequence of the ledger.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<IEnumerable<StellarTransaction>>> Stellar_GetTransactionsInLedger_Async(long sequence, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Stellar_GetTransactionsInLedger, sequence));
            return await SendTatumRequest<IEnumerable<StellarTransaction>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get actual XLM fee<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Blockchain fee in 1/10000000 of XLM (stroop)
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<decimal> Stellar_GetBlockchainFee(CancellationToken ct = default) => Stellar_GetBlockchainFee_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get actual XLM fee<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Blockchain fee in 1/10000000 of XLM (stroop)
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<decimal>> Stellar_GetBlockchainFee_Async(CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Stellar_BlockchainFee));
            var result = await SendTatumRequest<string>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<decimal>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<decimal>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.ToDecimal(), null);
        }

        /// <summary>
        /// <b>Title:</b> Get XLM Account transactions<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// List all XLM account transactions.
        /// </summary>
        /// <param name="account">Address of XLM account.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<IEnumerable<StellarTransaction>> Stellar_GetTransactionsByAccount(string account, CancellationToken ct = default) => Stellar_GetTransactionsByAccount_Async(account, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get XLM Account transactions<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// List all XLM account transactions.
        /// </summary>
        /// <param name="account">Address of XLM account.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<IEnumerable<StellarTransaction>>> Stellar_GetTransactionsByAccount_Async(string account, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Stellar_GetTransactionsByAccount, account));
            return await SendTatumRequest<IEnumerable<StellarTransaction>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get XLM Transaction by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Transaction by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<StellarTransaction> Stellar_GetTransactionByHash(string hash, CancellationToken ct = default) => Stellar_GetTransactionByHash_Async(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get XLM Transaction by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Transaction by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<StellarTransaction>> Stellar_GetTransactionByHash_Async(string hash, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Stellar_GetTransactionByHash, hash));
            return await SendTatumRequest<StellarTransaction>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get XLM Account info<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Account detail.
        /// </summary>
        /// <param name="account">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<StellarAccountInfo> Stellar_GetAccountInfo(string account, CancellationToken ct = default) => Stellar_GetAccountInfo_Async(account, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get XLM Account info<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Account detail.
        /// </summary>
        /// <param name="account">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<StellarAccountInfo>> Stellar_GetAccountInfo_Async(string account, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Stellar_AccountInfo, account));
            return await SendTatumRequest<StellarAccountInfo>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Send XLM from address to address<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send XLM from account to account. It is possbile to send native XLM asset, or any other custom asset present on the network.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="fromAccount">Blockchain account to send assets from</param>
        /// <param name="to">Blockchain address to send assets</param>
        /// <param name="amount">Amount to be sent, in XLM.</param>
        /// <param name="fromSecret">Secret for account. Secret, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the secret associated in signing application. Secret, or signature Id must be present.</param>
        /// <param name="token">Asset name. If not defined, transaction is being sent in native XLM asset.</param>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets to send, if not native XLM asset.</param>
        /// <param name="message">Short message to recipient. It can be either 28 characters long ASCII text, 64 characters long HEX string or uint64 number.</param>
        /// <param name="initialize">Set to true, if the destination address is not yet initialized and should be funded for the first time.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Stellar_Send(
            string fromAccount,
            string to,
            string amount,
            string fromSecret = null,
            string signatureId = null,
            string token = null,
            string issuerAccount = null,
            string message = null,
            bool initialize = false,
            CancellationToken ct = default) => Stellar_Send_Async(fromAccount, to, amount, fromSecret, signatureId, token, issuerAccount, message, initialize, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send XLM from address to address<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send XLM from account to account. It is possbile to send native XLM asset, or any other custom asset present on the network.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="fromAccount">Blockchain account to send assets from</param>
        /// <param name="to">Blockchain address to send assets</param>
        /// <param name="amount">Amount to be sent, in XLM.</param>
        /// <param name="fromSecret">Secret for account. Secret, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the secret associated in signing application. Secret, or signature Id must be present.</param>
        /// <param name="token">Asset name. If not defined, transaction is being sent in native XLM asset.</param>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets to send, if not native XLM asset.</param>
        /// <param name="message">Short message to recipient. It can be either 28 characters long ASCII text, 64 characters long HEX string or uint64 number.</param>
        /// <param name="initialize">Set to true, if the destination address is not yet initialized and should be funded for the first time.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Stellar_Send_Async(
            string fromAccount,
            string to,
            string amount,
            string fromSecret = null,
            string signatureId = null,
            string token = null,
            string issuerAccount = null,
            string message = null,
            bool initialize = false,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "fromAccount", fromAccount },
                { "to", to },
                { "amount", amount },
                { "initialize", initialize },
            };
            parameters.AddOptionalParameter("fromSecret", fromSecret);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("token", token);
            parameters.AddOptionalParameter("issuerAccount", issuerAccount);
            parameters.AddOptionalParameter("message", message);

            var credits = 10;
            var url = GetUrl(string.Format(Endpoints_Stellar_Send));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Create / Update / Delete XLM trust line<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Create / Update / Delete XLM trust line between accounts to transfer private assets. By creating trustline for the first time, the asset is created automatically and can be used in the transactions.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="fromAccount">XLM account address. Must be the one used for generating deposit tags.</param>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets to create trust line for.</param>
        /// <param name="token">Asset name.</param>
        /// <param name="fromSecret">Secret for account. Secret, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the secret associated in signing application. Secret, or signature Id must be present.</param>
        /// <param name="limit">Amount of the assets to be permitted to send over this trust line. 0 means deletion of the trust line. When no limit is specified, maximum amount available is permitted.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Stellar_TrustLine(
            string fromAccount,
            string issuerAccount,
            string token,
            string fromSecret = null,
            string signatureId = null,
            string limit = null,
            CancellationToken ct = default) => Stellar_TrustLine_Async(fromAccount, issuerAccount, token, fromSecret, signatureId, limit, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create / Update / Delete XLM trust line<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Create / Update / Delete XLM trust line between accounts to transfer private assets. By creating trustline for the first time, the asset is created automatically and can be used in the transactions.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="fromAccount">XLM account address. Must be the one used for generating deposit tags.</param>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets to create trust line for.</param>
        /// <param name="token">Asset name.</param>
        /// <param name="fromSecret">Secret for account. Secret, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the secret associated in signing application. Secret, or signature Id must be present.</param>
        /// <param name="limit">Amount of the assets to be permitted to send over this trust line. 0 means deletion of the trust line. When no limit is specified, maximum amount available is permitted.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Stellar_TrustLine_Async(
            string fromAccount,
            string issuerAccount,
            string token,
            string fromSecret = null,
            string signatureId = null,
            string limit = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "fromAccount", fromAccount },
                { "issuerAccount", issuerAccount },
                { "token", token },
            };
            parameters.AddOptionalParameter("fromSecret", fromSecret);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("limit", limit);

            var credits = 10;
            var url = GetUrl(string.Format(Endpoints_Stellar_Trust));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Broadcast signed XLM transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed transaction to XLM blockchain. This method is used internally from Tatum KMS, Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="signatureId">ID of prepared payment template to sign. Required only, when broadcasting transaction signed by Tatum KMS.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Stellar_Broadcast(string txData, string signatureId, CancellationToken ct = default) => Stellar_Broadcast_Async(txData, signatureId, ct).Result;
        /// <summary>
        /// <b>Title:</b> Broadcast signed XLM transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed transaction to XLM blockchain. This method is used internally from Tatum KMS, Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="signatureId">ID of prepared payment template to sign. Required only, when broadcasting transaction signed by Tatum KMS.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Stellar_Broadcast_Async(string txData, string signatureId, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "txData", txData },
            };
            parameters.AddOptionalParameter("signatureId", signatureId);

            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Stellar_Broadcast));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }
        #endregion

        #region Blockchain / Records
        /// <summary>
        /// <b>Title:</b> Store log record<br />
        /// <b>Credits:</b> 2 credits per API call. Additional credits are debited based on the size of the data, which are being stored and type of blockchain.<br />
        /// <b>Description:</b>
        /// Store record data on blockchain. Tatum currently supports Ethereum blockchain.
        /// Total cost of the transaction(in credits) on Ethereum blockchain is dependent on the size of the data.Data are stored as a HEX string and maximum data size is approximatelly 130 kB on mainnet, 30 kB on testnet.
        /// Each 5 characters of the data costs 1 credit, so API call with data of length 1 kB = 1024 characters would cost 205 credits.
        /// </summary>
        /// <param name="chain">Blockchain, where to store log data.</param>
        /// <param name="data">Log data to be stored on a blockchain.</param>
        /// <param name="fromPrivateKey">Private key of account, from which the transaction will be initiated. If not present, transaction fee will be debited from Tatum internal account and additional credits will be charged.</param>
        /// <param name="to">Blockchain address to store log on. If not defined, it will be stored on an address, from which the transaction was being made.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Records_SetData(BlockchainType chain, string data, string fromPrivateKey = null, string to = null, long? nonce = null, CancellationToken ct = default) => Records_SetData_Async(chain, data, fromPrivateKey, to, nonce, ct).Result;
        /// <summary>
        /// <b>Title:</b> Store log record<br />
        /// <b>Credits:</b> 2 credits per API call. Additional credits are debited based on the size of the data, which are being stored and type of blockchain.<br />
        /// <b>Description:</b>
        /// Store record data on blockchain. Tatum currently supports Ethereum blockchain.
        /// Total cost of the transaction(in credits) on Ethereum blockchain is dependent on the size of the data.Data are stored as a HEX string and maximum data size is approximatelly 130 kB on mainnet, 30 kB on testnet.
        /// Each 5 characters of the data costs 1 credit, so API call with data of length 1 kB = 1024 characters would cost 205 credits.
        /// </summary>
        /// <param name="chain">Blockchain, where to store log data.</param>
        /// <param name="data">Log data to be stored on a blockchain.</param>
        /// <param name="fromPrivateKey">Private key of account, from which the transaction will be initiated. If not present, transaction fee will be debited from Tatum internal account and additional credits will be charged.</param>
        /// <param name="to">Blockchain address to store log on. If not defined, it will be stored on an address, from which the transaction was being made.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Records_SetData_Async(BlockchainType chain, string data, string fromPrivateKey = null, string to = null, long? nonce = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "chain", JsonConvert.SerializeObject(chain, new BlockchainTypeConverter(false)) },
                { "data", data },
            };
            parameters.AddOptionalParameter("fromPrivateKey", fromPrivateKey);
            parameters.AddOptionalParameter("nonce", nonce);
            parameters.AddOptionalParameter("to", to);

            var credits = 2 + Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(data.Length) / 5));
            var url = GetUrl(string.Format(Endpoints_Records_Log));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Get log record<br />
        /// <b>Credits:</b> 1 credits per API call.<br />
        /// <b>Description:</b>
        /// Get log data from Ethereum blockchain.
        /// </summary>
        /// <param name="chain">Blockchain, from which to get log record</param>
        /// <param name="id">ID of log record / transaction on blockchain</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainData> Records_GetData(BlockchainType chain, string id, CancellationToken ct = default) => Records_GetData_Async(chain, id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get log record<br />
        /// <b>Credits:</b> 1 credits per API call.<br />
        /// <b>Description:</b>
        /// Get log data from Ethereum blockchain.
        /// </summary>
        /// <param name="chain">Blockchain, from which to get log record</param>
        /// <param name="id">ID of log record / transaction on blockchain</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainData>> Records_GetData_Async(BlockchainType chain, string id, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "chain", JsonConvert.SerializeObject(chain, new BlockchainTypeConverter(false)) },
                { "id", id },
            };
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Records_Log));
            return await SendTatumRequest<BlockchainData>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        #endregion

        #region Blockchain / Binance
        /// <summary>
        /// <b>Title:</b> Generate Binance wallet<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate BNB account. Tatum does not support HD wallet for BNB, only specific address and private key can be generated.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BinanceAddress> Binance_GenerateAccount(CancellationToken ct = default) => Binance_GenerateAccount_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Binance wallet<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate BNB account. Tatum does not support HD wallet for BNB, only specific address and private key can be generated.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BinanceAddress>> Binance_GenerateAccount_Async(CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Binance_GenerateAccount));
            return await SendTatumRequest<BinanceAddress>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Binance current block<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Binance current block number.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<long> Binance_GetCurrentBlock(CancellationToken ct = default) => Binance_GetCurrentBlock_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Binance current block<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Binance current block number.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<long>> Binance_GetCurrentBlock_Async(CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Binance_CurrentBlock));
            var result = await SendTatumRequest<string>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<long>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<long>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.ToLong(), null);
        }

        /// <summary>
        /// <b>Title:</b> Get Binance Transactions in Block<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Transactions in block by block height.
        /// </summary>
        /// <param name="height">Block height</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BinanceBlockTransactions> Binance_GetTransactionsInBlock(long height, CancellationToken ct = default) => Binance_GetTransactionsInBlock_Async(height, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Binance Transactions in Block<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Transactions in block by block height.
        /// </summary>
        /// <param name="height">Block height</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BinanceBlockTransactions>> Binance_GetTransactionsInBlock_Async(long height, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Binance_GetTransactionsInBlock, height));
            return await SendTatumRequest<BinanceBlockTransactions>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Binance Account<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Binance Account Detail by address.
        /// </summary>
        /// <param name="account">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BinanceAccount> Binance_GetAccountInfo(string account, CancellationToken ct = default) => Binance_GetAccountInfo_Async(account, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Binance Account<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Binance Account Detail by address.
        /// </summary>
        /// <param name="account">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BinanceAccount>> Binance_GetAccountInfo_Async(string account, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Binance_AccountInfo, account));
            return await SendTatumRequest<BinanceAccount>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Binance Transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Binance Transaction by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BinanceTransaction> Binance_GetTransaction(string hash, CancellationToken ct = default) => Binance_GetTransaction_Async(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Binance Transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Binance Transaction by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BinanceTransaction>> Binance_GetTransaction_Async(string hash, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Binance_GetTransaction, hash));
            return await SendTatumRequest<BinanceTransaction>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Send Binance / Binance Token from account to account<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Binance or Binance Token token from account to account.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="to">Blockchain address to send assets.</param>
        /// <param name="currency">Currency to transfer from Binance Blockchain Account.</param>
        /// <param name="amount">Amount to be sent in BNB.</param>
        /// <param name="fromPrivateKey">Private key of sender address.</param>
        /// <param name="signatureId">Signature hash of the mnemonic, which will be used to sign transactions locally. All signature Ids should be present, which might be used to sign transaction.</param>
        /// <param name="message">Message to recipient.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Binance_Send(
            string to,
            string currency,
            string amount,
            string fromPrivateKey = null,
            string signatureId = null,
            string message = null,
            CancellationToken ct = default) => Stellar_Send_Async(to, currency, amount, fromPrivateKey, signatureId, message, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send Binance / Binance Token from account to account<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Binance or Binance Token token from account to account.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="to">Blockchain address to send assets.</param>
        /// <param name="currency">Currency to transfer from Binance Blockchain Account.</param>
        /// <param name="amount">Amount to be sent in BNB.</param>
        /// <param name="fromPrivateKey">Private key of sender address.</param>
        /// <param name="signatureId">Signature hash of the mnemonic, which will be used to sign transactions locally. All signature Ids should be present, which might be used to sign transaction.</param>
        /// <param name="message">Message to recipient.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Stellar_Send_Async(
            string to,
            string currency,
            string amount,
            string fromPrivateKey = null,
            string signatureId = null,
            string message = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "to", to },
                { "amount", amount },
                { "currency", currency },
            };
            parameters.AddOptionalParameter("fromPrivateKey", fromPrivateKey);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("message", message);

            var credits = 10;
            var url = GetUrl(string.Format(Endpoints_Binance_Send));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Broadcast signed BNB transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed transaction to Binance blockchain. This method is used internally from Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Binance_Broadcast(string txData, CancellationToken ct = default) => Binance_Broadcast_Async(txData, ct).Result;
        /// <summary>
        /// <b>Title:</b> Broadcast signed BNB transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed transaction to Binance blockchain. This method is used internally from Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Binance_Broadcast_Async(string txData, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "txData", txData },
            };

            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Binance_Broadcast));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }
        #endregion

        #region Blockchain / Libra
        // N/A
        #endregion

        #region Blockchain / VeChain
        /// <summary>
        /// <b>Title:</b> Generate VeChain wallet<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// - Tatum follows BIP44 specification and generates for VeChain wallet with derivation path m'/44'/818'/0'/0. More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. Generate BIP44 compatible VeChain wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainWallet> VeChain_GenerateWallet(string mnemonics, CancellationToken ct = default) => Blockchain_GenerateWallet_Async(BlockchainType.VeChain, new List<string> { mnemonics }, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate VeChain wallet<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// - Tatum follows BIP44 specification and generates for VeChain wallet with derivation path m'/44'/818'/0'/0. More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. Generate BIP44 compatible VeChain wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainWallet> VeChain_GenerateWallet(IEnumerable<string> mnemonics = null, CancellationToken ct = default) => Blockchain_GenerateWallet_Async(BlockchainType.VeChain, mnemonics, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate VeChain wallet<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// - Tatum follows BIP44 specification and generates for VeChain wallet with derivation path m'/44'/818'/0'/0. More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. Generate BIP44 compatible VeChain wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainWallet>> VeChain_GenerateWallet_Async(string mnemonics, CancellationToken ct = default) => await Blockchain_GenerateWallet_Async(BlockchainType.VeChain, new List<string> { mnemonics }, ct);
        /// <summary>
        /// <b>Title:</b> Generate VeChain wallet<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// - Tatum follows BIP44 specification and generates for VeChain wallet with derivation path m'/44'/818'/0'/0. More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. Generate BIP44 compatible VeChain wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainWallet>> VeChain_GenerateWallet_Async(IEnumerable<string> mnemonics = null, CancellationToken ct = default) => await Blockchain_GenerateWallet_Async(BlockchainType.VeChain, mnemonics, ct);

        /// <summary>
        /// <b>Title:</b> Generate VeChain account address from Extended public key<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate VeChain account deposit address from Extended public key. Deposit address is generated for the specific index - each extended public key can generate up to 2^32 addresses starting from index 0 until 2^31.
        /// </summary>
        /// <param name="xpub">Extended public key of wallet.</param>
        /// <param name="index">Derivation index of desired address to be generated.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainAddress> VeChain_GenerateDepositAddress(string xpub, int index, CancellationToken ct = default) => Blockchain_GenerateDepositAddress_Async(BlockchainType.VeChain, xpub, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate VeChain wallet<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// - Tatum follows BIP44 specification and generates for VeChain wallet with derivation path m'/44'/818'/0'/0. More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. Generate BIP44 compatible VeChain wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainAddress>> VeChain_GenerateDepositAddress_Async(string xpub, int index, CancellationToken ct = default) => await Blockchain_GenerateDepositAddress_Async(BlockchainType.VeChain, xpub, index, ct);

        /// <summary>
        /// <b>Title:</b> Generate VeChain private key<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate private key of address from mnemonic for given derivation path index. Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics"></param>
        /// <param name="index"></param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainKey> VeChain_GeneratePrivateKey(string mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKey_Async(BlockchainType.VeChain, new List<string> { mnemonics }, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate VeChain private key<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate private key of address from mnemonic for given derivation path index. Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics"></param>
        /// <param name="index"></param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainKey> VeChain_GeneratePrivateKey(IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKey_Async(BlockchainType.VeChain, mnemonics, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate VeChain private key<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate private key of address from mnemonic for given derivation path index. Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics"></param>
        /// <param name="index"></param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainKey>> VeChain_GeneratePrivateKey_Async(string mnemonics, int index, CancellationToken ct = default) => await Blockchain_GeneratePrivateKey_Async(BlockchainType.VeChain, new List<string> { mnemonics }, index, default);
        /// <summary>
        /// <b>Title:</b> Generate VeChain private key<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate private key of address from mnemonic for given derivation path index. Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics"></param>
        /// <param name="index"></param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainKey>> VeChain_GeneratePrivateKey_Async(IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => await Blockchain_GeneratePrivateKey_Async(BlockchainType.VeChain, mnemonics, index, default);

        /// <summary>
        /// <b>Title:</b> Get VeChain current block<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get VeChain current block number.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<long> VeChain_GetCurrentBlock(CancellationToken ct = default) => VeChain_GetCurrentBlock_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate VeChain private key<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate private key of address from mnemonic for given derivation path index. Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics"></param>
        /// <param name="index"></param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<long>> VeChain_GetCurrentBlock_Async(CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_VeChain_CurrentBlock));
            var result = await SendTatumRequest<string>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<long>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<long>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.ToLong(), null);
        }

        /// <summary>
        /// <b>Title:</b> Get VeChain Block by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get VeChain Block by block hash or block number.
        /// </summary>
        /// <param name="hash_height">Block hash or block number</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<VeChainBlock> VeChain_GetBlock(string hash_height, CancellationToken ct = default) => VeChain_GetBlock_Async(hash_height, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get VeChain Block by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get VeChain Block by block hash or block number.
        /// </summary>
        /// <param name="hash_height">Block hash or block number</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<VeChainBlock>> VeChain_GetBlock_Async(string hash_height, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_VeChain_GetBlockByHash, hash_height));
            return await SendTatumRequest<VeChainBlock>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get VeChain Account balance<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get VeChain Account balance in VET.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<VeChainBalance> VeChain_GetBalance(string address, CancellationToken ct = default) => VeChain_GetBalance_Async(address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get VeChain Account balance<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get VeChain Account balance in VET.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<VeChainBalance>> VeChain_GetBalance_Async(string address, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_VeChain_GetBalance, address));
            return await SendTatumRequest<VeChainBalance>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get VeChain Account energy (VTHO)<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get VeChain Account energy in VTHO. VTHO is used for paying for the transaction fee.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<VeChainEnergy> VeChain_GetEnergy(string address, CancellationToken ct = default) => VeChain_GetEnergy_Async(address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get VeChain Account energy (VTHO)<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get VeChain Account energy in VTHO. VTHO is used for paying for the transaction fee.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<VeChainEnergy>> VeChain_GetEnergy_Async(string address, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_VeChain_GetEnergy, address));
            return await SendTatumRequest<VeChainEnergy>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get VeChain Transaction<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Get VeChain Transaction by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<VeChainTransaction> VeChain_GetTransactionByHash(string hash, CancellationToken ct = default) => VeChain_GetTransactionByHash_Async(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get VeChain Transaction<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Get VeChain Transaction by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<VeChainTransaction>> VeChain_GetTransactionByHash_Async(string hash, CancellationToken ct = default)
        {
            var credits = 10;
            var url = GetUrl(string.Format(Endpoints_VeChain_GetTransactionByHash, hash));
            return await SendTatumRequest<VeChainTransaction>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get VeChain Transaction Receipt<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get VeChain Transaction Receipt by transaction hash. Transaction receipt is available only after transaction is included in the block and contains information about paid fee or created contract address and much more.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<VeChainTransactionReceipt> VeChain_GetTransactionReceipt(string hash, CancellationToken ct = default) => VeChain_GetTransactionReceipt_Async(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get VeChain Transaction Receipt<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get VeChain Transaction Receipt by transaction hash. Transaction receipt is available only after transaction is included in the block and contains information about paid fee or created contract address and much more.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<VeChainTransactionReceipt>> VeChain_GetTransactionReceipt_Async(string hash, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_VeChain_GetTransactionReceipt, hash));
            return await SendTatumRequest<VeChainTransactionReceipt>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Send VeChain from account to account<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send VET from account to account. Fee for the transaction is paid in VTHO.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="to">Blockchain address to send assets</param>
        /// <param name="amount">Amount to be sent in VET</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="data">Additinal data, that can be passed to blockchain transaction as data property.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> VeChain_Send(string to, decimal amount, string fromPrivateKey = null, string signatureId = null, string data = null, VeChainFee fee = null, CancellationToken ct = default) => VeChain_Send_Async(to, amount, fromPrivateKey, signatureId, data, fee, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send VeChain from account to account<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send VET from account to account. Fee for the transaction is paid in VTHO.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="to">Blockchain address to send assets</param>
        /// <param name="amount">Amount to be sent in VET</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="data">Additinal data, that can be passed to blockchain transaction as data property.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> VeChain_Send_Async(string to, decimal amount, string fromPrivateKey = null, string signatureId = null, string data = null, VeChainFee fee = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                 { "to", to },
                 { "amount", amount },
             };
            parameters.AddOptionalParameter("fromPrivateKey", fromPrivateKey);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("data", data);
            parameters.AddOptionalParameter("fee", fee);

            var credits = 10;
            var url = GetUrl(string.Format(Endpoints_VeChain_Transaction));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Estimate VeChain Gas for transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Estimate gas required for transaction.
        /// </summary>
        /// <param name="from">Sender account address.</param>
        /// <param name="to">Recipient account address.</param>
        /// <param name="value">Amount to send.</param>
        /// <param name="data">Data to send to Smart Contract</param>
        /// <param name="nonce">Nonce</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<decimal> VeChain_EstimateGasForTransaction(string from, string to, decimal value, string data = null, long? nonce = null, CancellationToken ct = default) => VeChain_EstimateGasForTransaction_Async(from, to, value, data, nonce, ct).Result;
        /// <summary>
        /// <b>Title:</b> Estimate VeChain Gas for transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Estimate gas required for transaction.
        /// </summary>
        /// <param name="from">Sender account address.</param>
        /// <param name="to">Recipient account address.</param>
        /// <param name="value">Amount to send.</param>
        /// <param name="data">Data to send to Smart Contract</param>
        /// <param name="nonce">Nonce</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<decimal>> VeChain_EstimateGasForTransaction_Async(string from, string to, decimal value, string data = null, long? nonce = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                 { "from", from },
                 { "to", to },
                 { "value", value },
             };
            parameters.AddOptionalParameter("data", data);
            parameters.AddOptionalParameter("nonce", nonce);

            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_VeChain_Gas));
            var result = await SendTatumRequest<string>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<decimal>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<decimal>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.ToDecimal(), null);
        }

        /// <summary>
        /// <b>Title:</b> Broadcast signed VeChain transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed transaction to VeChain blockchain. This method is used internally from Tatum KMS, Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="signatureId">ID of prepared payment template to sign. Required only, when broadcasting transaction signed by Tatum KMS.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> VeChain_Broadcast(string txData, string signatureId, CancellationToken ct = default) => Bitcoin_Broadcast_Async(txData, signatureId, ct).Result;
        /// <summary>
        /// <b>Title:</b> Broadcast signed VeChain transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed transaction to VeChain blockchain. This method is used internally from Tatum KMS, Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="signatureId">ID of prepared payment template to sign. Required only, when broadcasting transaction signed by Tatum KMS.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> VeChain_Broadcast_Async(string txData, string signatureId, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                 { "txData", txData },
             };
            parameters.AddOptionalParameter("signatureId", signatureId);

            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_VeChain_Broadcast));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }
        #endregion

        #region Blockchain / NEO
        /// <summary>
        /// <b>Title:</b> Generate NEO account<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate NEO account. Tatum does not support HD wallet for NEO, only specific address and private key can be generated.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<NeoAccount> Neo_GenerateAccount(CancellationToken ct = default) => Neo_GenerateAccount_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate NEO account<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate NEO account. Tatum does not support HD wallet for NEO, only specific address and private key can be generated.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<NeoAccount>> Neo_GenerateAccount_Async(CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_NEO_GenerateAccount));
            return await SendTatumRequest<NeoAccount>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get current NEO block<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get current NEO block.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<long> Neo_GetCurrentBlock(CancellationToken ct = default) => Neo_GetCurrentBlock_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get current NEO block<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get current NEO block.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<long>> Neo_GetCurrentBlock_Async(CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_NEO_CurrentBlock));
            var result = await SendTatumRequest<string>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<long>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<long>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.ToLong(), null);
        }

        /// <summary>
        /// <b>Title:</b> Get NEO block<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get NEO block by hash or height.
        /// </summary>
        /// <param name="hash_height">Block hash or height.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<NeoBlock> Neo_GetBlock(string hash_height, CancellationToken ct = default) => Neo_GetBlock_Async(hash_height, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get NEO block<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get NEO block by hash or height.
        /// </summary>
        /// <param name="hash_height">Block hash or height.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<NeoBlock>> Neo_GetBlock_Async(string hash_height, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_NEO_GetBlock, hash_height));
            return await SendTatumRequest<NeoBlock>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get NEO Account balance<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Balance of all assets (NEO, GAS, etc.) and tokens for the Account.
        /// </summary>
        /// <param name="address">Address to get balance</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<NeoBalance> Neo_GetBalance(string address, CancellationToken ct = default) => Neo_GetBalance_Async(address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get NEO Account balance<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Balance of all assets (NEO, GAS, etc.) and tokens for the Account.
        /// </summary>
        /// <param name="address">Address to get balance</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<NeoBalance>> Neo_GetBalance_Async(string address, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_NEO_GetBalance, address));
            return await SendTatumRequest<NeoBalance>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Neo Asset details<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get information about asset.
        /// </summary>
        /// <param name="asset">Asset ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<NeoAsset> Neo_GetAssetInfo(string asset, CancellationToken ct = default) => Neo_GetAssetInfo_Async(asset, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Neo Asset details<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get information about asset.
        /// </summary>
        /// <param name="asset">Asset ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<NeoAsset>> Neo_GetAssetInfo_Async(string asset, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_NEO_GetAssetInfo, asset));
            return await SendTatumRequest<NeoAsset>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get NEO unspent transaction outputs<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get NEO unspent transaction outputs.
        /// </summary>
        /// <param name="txId">Transaction ID.</param>
        /// <param name="index">Index of output.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<NeoTransactionOutput> Neo_GetUnspentTransactionOutputs(string txId, long index, CancellationToken ct = default) => Neo_GetUnspentTransactionOutputs_Async(txId, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get NEO unspent transaction outputs<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get NEO unspent transaction outputs.
        /// </summary>
        /// <param name="txId">Transaction ID.</param>
        /// <param name="index">Index of output.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<NeoTransactionOutput>> Neo_GetUnspentTransactionOutputs_Async(string txId, long index, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_NEO_GetUnspentTransactionOutputs, txId, index));
            return await SendTatumRequest<NeoTransactionOutput>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get NEO Account transactions<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get NEO Account transactions.
        /// </summary>
        /// <param name="address">Example: AKL19WwiJ2fiTDkAnYQ7GJSTUBoJPTQKhn</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<IEnumerable<NeoAccountTransaction>> Neo_GetTransactionsByAccount(string address, CancellationToken ct = default) => Neo_GetTransactionsByAccount_Async(address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get NEO Account transactions<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get NEO Account transactions.
        /// </summary>
        /// <param name="address">Example: AKL19WwiJ2fiTDkAnYQ7GJSTUBoJPTQKhn</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<IEnumerable<NeoAccountTransaction>>> Neo_GetTransactionsByAccount_Async(string address, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_NEO_GetTransactionsByAccount, address));
            return await SendTatumRequest<IEnumerable<NeoAccountTransaction>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get NEO contract details<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get NEO contract details.
        /// </summary>
        /// <param name="scriptHash">Hash of smart contract</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<NeoContract> Neo_GetContractInfo(string scriptHash, CancellationToken ct = default) => Neo_GetContractInfo_Async(scriptHash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get NEO contract details<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get NEO contract details.
        /// </summary>
        /// <param name="scriptHash">Hash of smart contract</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<NeoContract>> Neo_GetContractInfo_Async(string scriptHash, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_NEO_GetContractInfo, scriptHash));
            return await SendTatumRequest<NeoContract>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get NEO transaction by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get NEO transaction by hash.
        /// </summary>
        /// <param name="hash">Transaction hash.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<NeoTransaction> Neo_GetTransactionByHash(string hash, CancellationToken ct = default) => Neo_GetTransactionByHash_Async(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get NEO transaction by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get NEO transaction by hash.
        /// </summary>
        /// <param name="hash">Transaction hash.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<NeoTransaction>> Neo_GetTransactionByHash_Async(string hash, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_NEO_GetTransactionByHash, hash));
            return await SendTatumRequest<NeoTransaction>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Send NEO assets<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Send NEO assets from address to address. It is possible to send NEO and GAS in the same transaction.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="to">Recipient address.</param>
        /// <param name="NEO_Amount">Assets to send.</param>
        /// <param name="GAS_Amount">Assets to send.</param>
        /// <param name="fromPrivateKey">Private key of address to send assets from.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Neo_Send(string to, decimal NEO_Amount, decimal GAS_Amount, string fromPrivateKey, CancellationToken ct = default) => Neo_Send_Async(to, NEO_Amount, GAS_Amount, fromPrivateKey, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send NEO assets<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Send NEO assets from address to address. It is possible to send NEO and GAS in the same transaction.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="to">Recipient address.</param>
        /// <param name="NEO_Amount">Assets to send.</param>
        /// <param name="GAS_Amount">Assets to send.</param>
        /// <param name="fromPrivateKey">Private key of address to send assets from.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Neo_Send_Async(string to, decimal NEO_Amount, decimal GAS_Amount, string fromPrivateKey, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "to", to },
                { "initialize", fromPrivateKey },
                { "assets", new NeoGasAssetCouple { NEO = NEO_Amount, GAS = GAS_Amount } },
            };

            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_NEO_Send));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Claim GAS<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Claim GAS for NEO account. Every account owner can claim for the GAS, which is produced for owning NEO on the address.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="privateKey">Private key of address to claim for GAS.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Neo_ClaimGAS(string privateKey, CancellationToken ct = default) => Neo_ClaimGAS_Async(privateKey, ct).Result;
        /// <summary>
        /// <b>Title:</b> Claim GAS<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Claim GAS for NEO account. Every account owner can claim for the GAS, which is produced for owning NEO on the address.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="privateKey">Private key of address to claim for GAS.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Neo_ClaimGAS_Async(string privateKey, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "privateKey", privateKey },
            };

            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_NEO_ClaimGAS));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Send NEO smart contract tokens<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Send NEO smart contract tokens. It is possible to transfer custom NEO-based tokens to another account.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="scriptHash">Function to be invoked on Smart Contract.</param>
        /// <param name="amount">Amount to be sent.</param>
        /// <param name="numOfDecimals">Number of decimals of asset being transferred.</param>
        /// <param name="fromPrivateKey">Private key of address to invoke smart contract.</param>
        /// <param name="to">Recipient address.</param>
        /// <param name="additionalInvocationGas">Additional GAS to be paid for smart contract invocation.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Neo_SendToken(string scriptHash, decimal amount, int numOfDecimals, string fromPrivateKey, string to, decimal additionalInvocationGas = 0, CancellationToken ct = default) => Neo_SendToken_Async(scriptHash, amount, numOfDecimals, fromPrivateKey, to, additionalInvocationGas, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send NEO smart contract tokens<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Send NEO smart contract tokens. It is possible to transfer custom NEO-based tokens to another account.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="scriptHash">Function to be invoked on Smart Contract.</param>
        /// <param name="amount">Amount to be sent.</param>
        /// <param name="numOfDecimals">Number of decimals of asset being transferred.</param>
        /// <param name="fromPrivateKey">Private key of address to invoke smart contract.</param>
        /// <param name="to">Recipient address.</param>
        /// <param name="additionalInvocationGas">Additional GAS to be paid for smart contract invocation.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Neo_SendToken_Async(string scriptHash, decimal amount, int numOfDecimals, string fromPrivateKey, string to, decimal additionalInvocationGas = 0, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "scriptHash", scriptHash },
                { "amount", amount },
                { "numOfDecimals", numOfDecimals },
                { "fromPrivateKey", fromPrivateKey },
                { "to", to },
                { "additionalInvocationGas", additionalInvocationGas },
            };

            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_NEO_Invoke));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Broadcast NEO transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast NEO transaction. This method is used internally from Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Neo_Broadcast(string txData, CancellationToken ct = default) => Neo_Broadcast_Async(txData, ct).Result;
        /// <summary>
        /// <b>Title:</b> Broadcast NEO transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast NEO transaction. This method is used internally from Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Neo_Broadcast_Async(string txData, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "txData", txData },
            };

            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_NEO_Broadcast));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }
        #endregion

        #region Blockchain / Scrypta
        /// <summary>
        /// <b>Title:</b> Generate Scrypta wallet<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// Tatum follows BIP44 specification and generates for Scrypta wallet with derivation path m'/44'/0'/0'/0. More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. Generate BIP44 compatible Scrypta wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to generate private key from.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainWallet> Scrypta_GenerateWallet(string mnemonics, CancellationToken ct = default) => Blockchain_GenerateWallet_Async(BlockchainType.Scrypta, new List<string> { mnemonics }, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Scrypta wallet<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// Tatum follows BIP44 specification and generates for Scrypta wallet with derivation path m'/44'/0'/0'/0. More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. Generate BIP44 compatible Scrypta wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to generate private key from.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainWallet> Scrypta_GenerateWallet(IEnumerable<string> mnemonics = null, CancellationToken ct = default) => Blockchain_GenerateWallet_Async(BlockchainType.Scrypta, mnemonics, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Scrypta wallet<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// Tatum follows BIP44 specification and generates for Scrypta wallet with derivation path m'/44'/0'/0'/0. More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. Generate BIP44 compatible Scrypta wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to generate private key from.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainWallet>> Scrypta_GenerateWallet_Async(string mnemonics, CancellationToken ct = default) => await Blockchain_GenerateWallet_Async(BlockchainType.Scrypta, new List<string> { mnemonics }, ct);
        /// <summary>
        /// <b>Title:</b> Generate Scrypta wallet<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// Tatum follows BIP44 specification and generates for Scrypta wallet with derivation path m'/44'/0'/0'/0. More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. Generate BIP44 compatible Scrypta wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to generate private key from.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainWallet>> Scrypta_GenerateWallet_Async(IEnumerable<string> mnemonics = null, CancellationToken ct = default) => await Blockchain_GenerateWallet_Async(BlockchainType.Scrypta, mnemonics, ct);

        /// <summary>
        /// <b>Title:</b> Generate Scrypta private key<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Generate private key for address from mnemonic for given derivation path index. Private key is generated for the concrete index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to generate private key from.</param>
        /// <param name="index">Derivation index of private key to generate.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainKey> Scrypta_GeneratePrivateKey(string mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKey_Async(BlockchainType.Scrypta, new List<string> { mnemonics }, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Scrypta private key<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Generate private key for address from mnemonic for given derivation path index. Private key is generated for the concrete index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to generate private key from.</param>
        /// <param name="index">Derivation index of private key to generate.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainKey> Scrypta_GeneratePrivateKey(IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKey_Async(BlockchainType.Scrypta, mnemonics, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Scrypta private key<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Generate private key for address from mnemonic for given derivation path index. Private key is generated for the concrete index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to generate private key from.</param>
        /// <param name="index">Derivation index of private key to generate.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainKey>> Scrypta_GeneratePrivateKey_Async(string mnemonics, int index, CancellationToken ct = default) => await Blockchain_GeneratePrivateKey_Async(BlockchainType.Scrypta, new List<string> { mnemonics }, index, default);
        /// <summary>
        /// <b>Title:</b> Generate Scrypta private key<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Generate private key for address from mnemonic for given derivation path index. Private key is generated for the concrete index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to generate private key from.</param>
        /// <param name="index">Derivation index of private key to generate.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainKey>> Scrypta_GeneratePrivateKey_Async(IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => await Blockchain_GeneratePrivateKey_Async(BlockchainType.Scrypta, mnemonics, index, default);

        /// <summary>
        /// <b>Title:</b> Get Block hash<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Scrypta Block hash. Returns hash of the block to get the block detail.
        /// </summary>
        /// <param name="block_id">The number of blocks preceding a particular block on a block chain.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<string> Scrypta_GetBlockHash(long block_id, CancellationToken ct = default) => Scrypta_GetBlockHash_Async(block_id, ct).Result;
        public async Task<WebCallResult<string>> Scrypta_GetBlockHash_Async(long block_id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Scrypta_GetBlockHash, block_id));
            return await SendTatumRequest<string>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Block by hash or height<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Scrypta Block detail by block hash or height.
        /// </summary>
        /// <param name="hash_height">Block hash or height.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<ScryptaBlock> Scrypta_GetBlock(string hash_height, CancellationToken ct = default) => Scrypta_GetBlock_Async(hash_height, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Block by hash or height<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Scrypta Block detail by block hash or height.
        /// </summary>
        /// <param name="hash_height">Block hash or height.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<ScryptaBlock>> Scrypta_GetBlock_Async(string hash_height, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Scrypta_GetBlockByHash, hash_height));
            return await SendTatumRequest<ScryptaBlock>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Send LYRA to blockchain addresses<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Scrypta to blockchain addresses. It is possible to build a blockchain transaction in 2 ways:
        /// - fromAddress - assets will be sent from the list of addresses.For each of the addresses last 100 transactions will be scanned for any unspent UTXO and will be included in the transaction.
        /// - fromUTXO - assets will be sent from the list of unspent UTXOs. Each of the UTXO will be included in the transaction.
        /// In scrypta-like blockchains, the transaction is created from the list of previously not spent UTXO.Every UTXO contains the number of funds, which can be spent. When the UTXO enters into the transaction, the whole amount is included and must be spent.For example, address A receives 2 transactions, T1 with 1 LYRA and T2 with 2 LYRA.The transaction, which will consume UTXOs for T1 and T2, will have available amount to spent 3 LYRA = 1 LYRA (T1) + 2 LYRA(T2).
        /// There can be multiple recipients of the transactions, not only one.In the to section, every recipient address has it's corresponding amount. When the amount of funds, that should receive the recipient is lower than the number of funds from the UTXOs, the difference is used as a transaction fee.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="fromAddress">Array of addresses and corresponding private keys. Tatum will automatically scan last 100 transactions for each address and will use all of the unspent values. We advise to use this option if you have 1 address per 1 transaction only.</param>
        /// <param name="fromUTXO">Array of transaction hashes, index of UTXO in it and corresponding private keys. Use this option if you want to calculate amount to send manually. Either fromUTXO or fromAddress must be present.</param>
        /// <param name="to">Array of addresses and values to send bitcoins to. Values must be set in BTC. Difference between from and to is transaction fee.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Scrypta_Send(IEnumerable<ScryptaSendOrderFromAddress> fromAddress, IEnumerable<ScryptaSendOrderFromUTXO> fromUTXO, IEnumerable<ScryptaSendOrderTo> to, CancellationToken ct = default) => Scrypta_Send_Async(fromAddress, fromUTXO, to, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send LYRA to blockchain addresses<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Scrypta to blockchain addresses. It is possible to build a blockchain transaction in 2 ways:
        /// - fromAddress - assets will be sent from the list of addresses.For each of the addresses last 100 transactions will be scanned for any unspent UTXO and will be included in the transaction.
        /// - fromUTXO - assets will be sent from the list of unspent UTXOs. Each of the UTXO will be included in the transaction.
        /// In scrypta-like blockchains, the transaction is created from the list of previously not spent UTXO.Every UTXO contains the number of funds, which can be spent. When the UTXO enters into the transaction, the whole amount is included and must be spent.For example, address A receives 2 transactions, T1 with 1 LYRA and T2 with 2 LYRA.The transaction, which will consume UTXOs for T1 and T2, will have available amount to spent 3 LYRA = 1 LYRA (T1) + 2 LYRA(T2).
        /// There can be multiple recipients of the transactions, not only one.In the to section, every recipient address has it's corresponding amount. When the amount of funds, that should receive the recipient is lower than the number of funds from the UTXOs, the difference is used as a transaction fee.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="fromAddress">Array of addresses and corresponding private keys. Tatum will automatically scan last 100 transactions for each address and will use all of the unspent values. We advise to use this option if you have 1 address per 1 transaction only.</param>
        /// <param name="fromUTXO">Array of transaction hashes, index of UTXO in it and corresponding private keys. Use this option if you want to calculate amount to send manually. Either fromUTXO or fromAddress must be present.</param>
        /// <param name="to">Array of addresses and values to send bitcoins to. Values must be set in BTC. Difference between from and to is transaction fee.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Scrypta_Send_Async(IEnumerable<ScryptaSendOrderFromAddress> fromAddress, IEnumerable<ScryptaSendOrderFromUTXO> fromUTXO, IEnumerable<ScryptaSendOrderTo> to, CancellationToken ct = default)
        {
            if ((fromAddress == null || fromAddress.Count() == 0) && (fromUTXO == null || fromUTXO.Count() == 0))
                throw new ArgumentException("Either fromUTXO or fromAddress must be present.");

            var parameters = new Dictionary<string, object> {
                { "to", to },
            };
            parameters.AddOptionalParameter("fromAddress", fromAddress);
            parameters.AddOptionalParameter("fromUTXO", fromUTXO);

            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Scrypta_Transaction));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Get Scrypta Transaction by hash<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Scrypta Transaction detail by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<ScryptaTransaction> Scrypta_GetTransactionByHash(string hash, CancellationToken ct = default) => Scrypta_GetTransactionByHash_Async(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Scrypta Transaction by hash<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Scrypta Transaction detail by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<ScryptaTransaction>> Scrypta_GetTransactionByHash_Async(string hash, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Scrypta_GetTransactionByHash, hash));
            return await SendTatumRequest<ScryptaTransaction>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Transactions by address<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Scrypta Transactions by address.
        /// </summary>
        /// <param name="address">Address</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<IEnumerable<ScryptaTransaction>> Scrypta_GetTransactionsByAddress(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default) => Scrypta_GetTransactionsByAddress_Async(address, pageSize, offset, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Transactions by address<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Scrypta Transactions by address.
        /// </summary>
        /// <param name="address">Address</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<IEnumerable<ScryptaTransaction>>> Scrypta_GetTransactionsByAddress_Async(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);

            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };

            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Scrypta_GetTransactionsByAddress, address));
            return await SendTatumRequest<IEnumerable<ScryptaTransaction>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Scrypta spendable UTXO<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Scrypta spendable UTXO.
        /// </summary>
        /// <param name="address">Address</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<IEnumerable<ScryptaUTXO>> Scrypta_GetSpendableUTXO(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default) => Scrypta_GetSpendableUTXO_Async(address, pageSize, offset, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Scrypta spendable UTXO<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Scrypta spendable UTXO.
        /// </summary>
        /// <param name="address">Address</param>
        /// <param name="pageSize">Max number of items per page is 50.</param>
        /// <param name="offset">Offset to obtain next page of the data.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<IEnumerable<ScryptaUTXO>>> Scrypta_GetSpendableUTXO_Async(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);

            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };

            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Scrypta_GetSpendableUTXO, address));
            return await SendTatumRequest<IEnumerable<ScryptaUTXO>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get UTXO of Transaction<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get UTXO of given transaction and output index. UTXO means Unspent Transaction Output, which is in blockchain terminology assets, that user received on the concrete address and does not spend it yet.
        /// In scrypta-like blockchains(LYRA, LTC, BCH), every transaction is built from the list of previously not spent transactions connected to the address.If user owns address A, receives in transaciont T1 10 LYRA, he can spend in the next transaction UTXO T1 of total value 10 LYRA.User can spend multiple UTXOs from different addresses in 1 transaction.
        /// If UTXO is not spent, data are returned, otherwise 404 error code.
        /// </summary>
        /// <param name="txhash">TX Hash</param>
        /// <param name="index">Index of tx output to check if spent or not</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<ScryptaUTXO> Scrypta_GetTransactionUTXO(string txhash, long index, CancellationToken ct = default) => Scrypta_GetTransactionUTXO_Async(txhash, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get UTXO of Transaction<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get UTXO of given transaction and output index. UTXO means Unspent Transaction Output, which is in blockchain terminology assets, that user received on the concrete address and does not spend it yet.
        /// In scrypta-like blockchains(LYRA, LTC, BCH), every transaction is built from the list of previously not spent transactions connected to the address.If user owns address A, receives in transaciont T1 10 LYRA, he can spend in the next transaction UTXO T1 of total value 10 LYRA.User can spend multiple UTXOs from different addresses in 1 transaction.
        /// If UTXO is not spent, data are returned, otherwise 404 error code.
        /// </summary>
        /// <param name="txhash">TX Hash</param>
        /// <param name="index">Index of tx output to check if spent or not</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<ScryptaUTXO>> Scrypta_GetTransactionUTXO_Async(string txhash, long index, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Scrypta_GetTransactionUTXO, txhash, index));
            return await SendTatumRequest<ScryptaUTXO>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Generate Scrypta deposit address from Extended public key<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Generate Scrypta deposit address from Extended public key. Deposit address is generated for the concrete index - each extended public key can generate up to 2^32 addresses starting from index 0 until 2^31.
        /// </summary>
        /// <param name="xpub">Extended public key of wallet.</param>
        /// <param name="index">Derivation index of desired address to be generated.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainAddress> Scrypta_GenerateDepositAddress(string xpub, int index, CancellationToken ct = default) => Blockchain_GenerateDepositAddress_Async(BlockchainType.Scrypta, xpub, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Scrypta deposit address from Extended public key<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Generate Scrypta deposit address from Extended public key. Deposit address is generated for the concrete index - each extended public key can generate up to 2^32 addresses starting from index 0 until 2^31.
        /// </summary>
        /// <param name="xpub">Extended public key of wallet.</param>
        /// <param name="index">Derivation index of desired address to be generated.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainAddress>> Scrypta_GenerateDepositAddress_Async(string xpub, int index, CancellationToken ct = default) => await Blockchain_GenerateDepositAddress_Async(BlockchainType.Scrypta, xpub, index, ct);

        /// <summary>
        /// <b>Title:</b> Get Blockchain Information<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Scrypta Blockchain Information. Obtain basic info like testnet / mainent version of the chain, current block number and it's hash.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<ScryptaChainInfo> Scrypta_GetBlockchainInformation(CancellationToken ct = default) => Scrypta_GetBlockchainInformation_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Blockchain Information<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Scrypta Blockchain Information. Obtain basic info like testnet / mainent version of the chain, current block number and it's hash.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<ScryptaChainInfo>> Scrypta_GetBlockchainInformation_Async(CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Scrypta_BlockchainInformation));
            return await SendTatumRequest<ScryptaChainInfo>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Broadcast signed Scrypta transaction<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed transaction to Scrypta blockchain. This method is used internally from Tatum KMS, Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="signatureId">ID of prepared payment template to sign. Required only, when broadcasting transaction signed by Tatum KMS.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BlockchainResponse> Scrypta_Broadcast(string txData, string signatureId, CancellationToken ct = default) => Scrypta_Broadcast_Async(txData, signatureId, ct).Result;
        /// <summary>
        /// <b>Title:</b> Broadcast signed Scrypta transaction<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed transaction to Scrypta blockchain. This method is used internally from Tatum KMS, Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="signatureId">ID of prepared payment template to sign. Required only, when broadcasting transaction signed by Tatum KMS.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BlockchainResponse>> Scrypta_Broadcast_Async(string txData, string signatureId, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "txData", txData },
            };
            parameters.AddOptionalParameter("signatureId", signatureId);

            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Scrypta_Broadcast));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }
        #endregion

        #region Tatum / Service
        /// <summary>
        /// <b>Title:</b> List credit consumption for last month<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// List usage information of credits.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<IEnumerable<ServiceUsage>> Service_GetConsumptions(CancellationToken ct = default) => Service_GetConsumptions_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> List credit consumption for last month<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// List usage information of credits.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<IEnumerable<ServiceUsage>>> Service_GetConsumptions_Async(CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Service_Consumption));
            return await SendTatumRequest<IEnumerable<ServiceUsage>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get currenct exchange rate of the supported FIAT / crypto asset<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get currenct exchange rate of the supported FIAT / crypto asset. Base pair is EUR by default. 
        /// E.g. to obtain exchange rate for the Bitcoin, response value for the API call will be expressed as 1 BTC = 10,000 EUR.
        /// </summary>
        /// <param name="currency">FIAT or crypto asset</param>
        /// <param name="basePair">FIAT to convert as a basePair</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<ServiceExchangeRate> Service_GetExchangeRates(string currency, string basePair, CancellationToken ct = default) => Service_GetExchangeRates_Async(currency, basePair, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get currenct exchange rate of the supported FIAT / crypto asset<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get currenct exchange rate of the supported FIAT / crypto asset. Base pair is EUR by default. 
        /// E.g. to obtain exchange rate for the Bitcoin, response value for the API call will be expressed as 1 BTC = 10,000 EUR.
        /// </summary>
        /// <param name="currency">FIAT or crypto asset</param>
        /// <param name="basePair">FIAT to convert as a basePair</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<ServiceExchangeRate>> Service_GetExchangeRates_Async(string currency, string basePair, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "basePair", basePair },
            };

            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Service_ExchangeRates, currency));
            return await SendTatumRequest<ServiceExchangeRate>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get API version<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get current version of the API.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<ServiceVersion> Service_GetVersion(CancellationToken ct = default) => Service_GetVersion_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get API version<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get current version of the API.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<ServiceVersion>> Service_GetVersion_Async(CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Service_Version));
            return await SendTatumRequest<ServiceVersion>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }
        #endregion

        #endregion

        #region Protected Methods

        protected override Error ParseErrorResponse(JToken error)
        {
            if (error["statusCode"] == null || error["message"] == null)
                return new TatumError(error.ToString());

            return new TatumError((int)error["statusCode"], (string)error["message"]);
        }

        protected Uri GetUrl(string endpoint, int apiversion = Endpoints_Version)
        {
            return new Uri($"{BaseAddress.TrimEnd('/')}/v{apiversion}/{endpoint}");
        }

        protected async Task<WebCallResult<T>> SendTatumRequest<T>(
            Uri uri, 
            HttpMethod method, 
            CancellationToken cancellationToken,
            Dictionary<string, object> parameters = null, 
            bool signed = false, 
            bool checkResult = true, 
            PostParameters? postPosition = null, 
            ArrayParametersSerialization? arraySerialization = null, 
            int credits = 1) where T : class
        {
            foreach (var limiter in RateLimiters)
            {
                if (limiter is RateLimiterCredit creditLimiter)
                {
                    var limitResult = creditLimiter.LimitRequest(this, uri.AbsolutePath, RateLimitBehaviour, credits);
                    if (!limitResult.Success)
                    {
                        log.Write(LogVerbosity.Debug, $"Request {uri.AbsolutePath} failed because of rate limit");
                        return new WebCallResult<T>(null, null, null, limitResult.Error);
                    }

                    if (limitResult.Data > 0)
                        log.Write(LogVerbosity.Debug, $"Request {uri.AbsolutePath} was limited by {limitResult.Data}ms by {limiter.GetType().Name}");
                }
            }

            return await SendRequest<T>(uri, method, cancellationToken, parameters, signed, checkResult, postPosition, arraySerialization);
        }
        #endregion

    }
}