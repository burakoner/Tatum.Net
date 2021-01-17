using CryptoExchange.Net;
using CryptoExchange.Net.Interfaces;
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
using Tatum.Net.RestObjects;

namespace Tatum.Net
{
    public class TatumClient : RestClient,
        IRestClient,
        ITatumClient,
        ITatumLedgerAccountClient,
        ITatumLedgerTransactionClient,
        ITatumLedgerCustomerClient,
        ITatumLedgerVirtualCurrencyClient,
        ITatumLedgerSubscriptionClient,
        ITatumLedgerOrderBookClient,
        ITatumSecurityKMSClient,
        ITatumSecurityAddressClient,
        ITatumOffchainAccountClient,
        ITatumOffchainBlockchainClient,
        ITatumOffchainWithdrawalClient,
        ITatumBlockchainBitcoinClient,
        ITatumBlockchainEthereumClient,
        ITatumBlockchainBitcoinCashClient,
        ITatumBlockchainLitecoinClient,
        ITatumBlockchainRippleClient,
        ITatumBlockchainStellarClient,
        ITatumBlockchainRecordsClient,
        ITatumBlockchainBinanceClient,
        ITatumBlockchainVeChainClient,
        ITatumBlockchainNeoClient,
        ITatumBlockchainLibraClient,
        ITatumBlockchainTronClient,
        ITatumBlockchainScryptaClient,
        ITatumServiceClient
    {
        #region Core Fields
        protected static TatumClientOptions defaultOptions = new TatumClientOptions();
        protected static TatumClientOptions DefaultOptions => defaultOptions.Copy();
        #endregion

        #region API Endpoints

        #region Version
        protected const int Endpoints_Version = 3;
        #endregion

        #region Ledger Account
        protected const string Endpoints_Ledger_Account_Create = "ledger/account";
        protected const string Endpoints_Ledger_Account_List = "ledger/account";
        protected const string Endpoints_Ledger_Account_CreateBatch = "ledger/account/batch";
        protected const string Endpoints_Ledger_Account_ListByCustomer = "ledger/account/customer/{0}";
        protected const string Endpoints_Ledger_Account_GetById = "ledger/account/{0}";
        protected const string Endpoints_Ledger_Account_Update = "ledger/account/{0}";
        protected const string Endpoints_Ledger_Account_Balance = "ledger/account/{0}/balance";
        protected const string Endpoints_Ledger_Account_BlockAmount = "ledger/account/block/{0}";
        protected const string Endpoints_Ledger_Account_UnlockAmountAndTransfer = "ledger/account/block/{0}";
        protected const string Endpoints_Ledger_Account_UnblockAmount = "ledger/account/block/{0}";
        protected const string Endpoints_Ledger_Account_GetBlockedAmounts = "ledger/account/block/{0}";
        protected const string Endpoints_Ledger_Account_UnblockAllBlockedAmounts = "ledger/account/block/account/{0}";
        protected const string Endpoints_Ledger_Account_ActivateLedgerAccount = "ledger/account/{0}/activate";
        protected const string Endpoints_Ledger_Account_DeactivateLedgerAccount = "ledger/account/{0}/deactivate";
        protected const string Endpoints_Ledger_Account_FreezeLedgerAccount = "ledger/account/{0}/freeze";
        protected const string Endpoints_Ledger_Account_UnfreezeLedgerAccount = "ledger/account/{0}/unfreeze";
        #endregion

        #region Ledger Transaction
        protected const string Endpoints_Ledger_Transaction_SendPayment = "ledger/transaction";
        protected const string Endpoints_Ledger_Transaction_GetTransactionsByAccount = "ledger/transaction/account";
        protected const string Endpoints_Ledger_Transaction_GetTransactionsByCustomer = "ledger/transaction/customer";
        protected const string Endpoints_Ledger_Transaction_GetTransactionsByLedger = "ledger/transaction/ledger";
        protected const string Endpoints_Ledger_Transaction_GetTransactionsByReference = "ledger/transaction/reference/{0}";
        #endregion

        #region Ledger Customer
        protected const string Endpoints_Ledger_Customer_List = "ledger/customer";
        protected const string Endpoints_Ledger_Customer_Get = "ledger/customer/{0}";
        protected const string Endpoints_Ledger_Customer_Update = "ledger/customer/{0}";
        protected const string Endpoints_Ledger_Customer_Activate = "ledger/customer/{0}/activate";
        protected const string Endpoints_Ledger_Customer_Deactivate = "ledger/customer/{0}/deactivate";
        protected const string Endpoints_Ledger_Customer_Enable = "ledger/customer/{0}/enable";
        protected const string Endpoints_Ledger_Customer_Disable = "ledger/customer/{0}/disable";
        #endregion

        #region Ledger Virtual Currency
        protected const string Endpoints_Ledger_VirtualCurrency_Create = "ledger/virtualCurrency";
        protected const string Endpoints_Ledger_VirtualCurrency_Update = "ledger/virtualCurrency";
        protected const string Endpoints_Ledger_VirtualCurrency_Get = "ledger/virtualCurrency/{0}";
        protected const string Endpoints_Ledger_VirtualCurrency_Mint = "ledger/virtualCurrency/mint";
        protected const string Endpoints_Ledger_VirtualCurrency_Destroy = "ledger/virtualCurrency/revoke";
        #endregion

        #region Ledger Subscription
        protected const string Endpoints_Ledger_Subscription_Create = "subscription";
        protected const string Endpoints_Ledger_Subscription_List = "subscription";
        protected const string Endpoints_Ledger_Subscription_Cancel = "subscription/{0}";
        protected const string Endpoints_Ledger_Subscription_Report = "subscription/report/{0}";
        #endregion

        #region Ledger Order Book
        protected const string Endpoints_Ledger_OrderBook_ListHistory = "trade/history";
        protected const string Endpoints_Ledger_OrderBook_ListBuys = "trade/buy";
        protected const string Endpoints_Ledger_OrderBook_ListSells = "trade/sell";
        protected const string Endpoints_Ledger_OrderBook_Place = "trade";
        protected const string Endpoints_Ledger_OrderBook_Get = "trade/{0}";
        protected const string Endpoints_Ledger_OrderBook_Cancel = "trade/{0}";
        protected const string Endpoints_Ledger_OrderBook_CancelAll = "trade/account/{0}";
        #endregion

        #region Security Key Management System
        protected const string Endpoints_KMS_GetPendingTransactions = "kms/pending/{0}";
        protected const string Endpoints_KMS_CompletePendingTransaction = "kms/{0}/{1}";
        protected const string Endpoints_KMS_Transaction = "kms/{0}";
        #endregion

        #region Security Address
        protected const string Endpoints_Security_CheckMalicousAddress = "security/address/{0}";
        #endregion

        #region Off-chain Account
        protected const string Endpoints_Offchain_Account_DepositAddress = "offchain/account/{0}/address";
        protected const string Endpoints_Offchain_Account_DepositAddressBatch = "offchain/account/address/batch";
        protected const string Endpoints_Offchain_Account_CheckAddress = "offchain/account/address/{0}/{1}";
        protected const string Endpoints_Offchain_Account_RemoveAddress = "offchain/account/{0}/address/{1}";
        protected const string Endpoints_Offchain_Account_AssignAddress = "offchain/account/{0}/address/{1}";
        #endregion

        #region Off-chain Blockchain
        protected const string Endpoints_Offchain_Blockchain_Transfer = "offchain/{0}/transfer";
        protected const string Endpoints_Offchain_Blockchain_BitcoinTransfer = "offchain/bitcoin/transfer";
        protected const string Endpoints_Offchain_Blockchain_BitcoinCashTransfer = "offchain/bcash/transfer";
        protected const string Endpoints_Offchain_Blockchain_LitecoinTransfer = "offchain/litecoin/transfer";
        protected const string Endpoints_Offchain_Blockchain_EthereumTransfer = "offchain/ethereum/transfer";
        protected const string Endpoints_Offchain_Blockchain_CreateERC20Token = "offchain/ethereum/erc20";
        protected const string Endpoints_Offchain_Blockchain_DeployERC20Token = "offchain/ethereum/erc20/deploy";
        protected const string Endpoints_Offchain_Blockchain_SetERC20TokenContractAddress = "offchain/ethereum/erc20/{0}/{1}";
        protected const string Endpoints_Offchain_Blockchain_TransferERC20Token = "offchain/ethereum/erc20/transfer";
        protected const string Endpoints_Offchain_Blockchain_StellarTransfer = "offchain/xlm/transfer";
        protected const string Endpoints_Offchain_Blockchain_CreateXLMAsset = "offchain/xlm/asset";
        protected const string Endpoints_Offchain_Blockchain_RippleTransfer = "offchain/xrp/transfer";
        protected const string Endpoints_Offchain_Blockchain_CreateXRPAsset = "offchain/xrp/asset";
        protected const string Endpoints_Offchain_Blockchain_BinanceTransfer = "offchain/bnb/transfer";
        protected const string Endpoints_Offchain_Blockchain_CreateBNBAsset = "offchain/bnb/asset";
        #endregion

        #region Off-chain Withdrawal
        protected const string Endpoints_Offchain_Withdrawal_Store = "offchain/withdrawal";
        protected const string Endpoints_Offchain_Withdrawal_Complete = "offchain/withdrawal/{0}/{1}";
        protected const string Endpoints_Offchain_Withdrawal_Cancel = "offchain/withdrawal/{0";
        protected const string Endpoints_Offchain_Withdrawal_Broadcast = "offchain/withdrawal/broadcast";
        #endregion

        #region Blockchain - Shared
        protected const string Endpoints_Blockchain_GenerateWallet = "{0}/wallet";
        protected const string Endpoints_Blockchain_GenerateDepositAddress = "{0}/address/{1}/{2}";
        protected const string Endpoints_Blockchain_GenerateWalletPrivateKey = "{0}/wallet/priv";
        #endregion

        #region Blockchain - Bitcoin
        protected const string Endpoints_Bitcoin_BlockchainInformation = "bitcoin/info";
        protected const string Endpoints_Bitcoin_GetBlockHash = "bitcoin/block/hash/{0}";
        protected const string Endpoints_Bitcoin_GetBlockByHash = "bitcoin/block/{0}";
        protected const string Endpoints_Bitcoin_GetTransactionByHash = "bitcoin/transaction/{0}";
        protected const string Endpoints_Bitcoin_GetTransactionsByAddress = "bitcoin/transaction/address/{0}";
        protected const string Endpoints_Bitcoin_GetBalance = "bitcoin/address/balance/{0}";
        protected const string Endpoints_Bitcoin_GetTransactionUTXO = "bitcoin/utxo/{0}/{1}";
        protected const string Endpoints_Bitcoin_Transaction = "bitcoin/transaction";
        protected const string Endpoints_Bitcoin_Broadcast = "bitcoin/broadcast";
        #endregion

        #region Blockchain - Ethereum
        protected const string Endpoints_Ethereum_Web3HttpDriver = "ethereum/web3/{0}";
        protected const string Endpoints_Ethereum_CurrentBlockNumber = "ethereum/block/current";
        protected const string Endpoints_Ethereum_GetBlockByHash = "ethereum/block/{0}";
        protected const string Endpoints_Ethereum_GetTransactionByHash = "ethereum/transaction/{0}";
        protected const string Endpoints_Ethereum_GetETHBalance = "ethereum/account/balance/{0}";
        protected const string Endpoints_Ethereum_GetOutgoingTransactionsCount = "ethereum/transaction/count/{0}";
        protected const string Endpoints_Ethereum_GetTransactionsByAddress = "ethereum/account/transaction/{0}";
        protected const string Endpoints_Ethereum_Send = "ethereum/transaction";
        protected const string Endpoints_Ethereum_Broadcast = "ethereum/broadcast";
        protected const string Endpoints_Ethereum_SmartContract = "ethereum/smartcontract";
        protected const string Endpoints_Ethereum_ERC20Balance = "ethereum/account/balance/erc20/{0}";
        protected const string Endpoints_Ethereum_ERC20DeploySmartContract = "ethereum/erc20/deploy";
        protected const string Endpoints_Ethereum_ERC20Transfer = "ethereum/erc20/transaction";
        protected const string Endpoints_Ethereum_ERC721Balance = "ethereum/erc721/balance/{0}/{1}";
        protected const string Endpoints_Ethereum_ERC721DeploySmartContract = "ethereum/erc721/deploy";
        protected const string Endpoints_Ethereum_ERC721Mint = "ethereum/erc721/mint";
        protected const string Endpoints_Ethereum_ERC721MintMultiple = "ethereum/erc721/mint/batch";
        protected const string Endpoints_Ethereum_ERC721Transfer = "ethereum/erc721/transaction";
        protected const string Endpoints_Ethereum_ERC721Burn = "ethereum/erc721/burn";
        protected const string Endpoints_Ethereum_ERC721Token = "ethereum/erc721/token/{0}/{1}/{2}";
        protected const string Endpoints_Ethereum_ERC721TokenMetadata = "ethereum/erc721/metadata/{0}/{1}";
        protected const string Endpoints_Ethereum_ERC721TokenOwner = "ethereum/erc721/owner/{0}/{1}";
        #endregion

        #region Blockchain - BitcoinCash
        protected const string Endpoints_BitcoinCash_BlockchainInformation = "bcash/info";
        protected const string Endpoints_BitcoinCash_GetBlockHash = "bcash/block/hash/{0}";
        protected const string Endpoints_BitcoinCash_GetBlockByHash = "bcash/block/{0}";
        protected const string Endpoints_BitcoinCash_GetTransactionByHash = "bcash/transaction/{0}";
        protected const string Endpoints_BitcoinCash_GetTransactionsByAddress = "bcash/transaction/address/{0}";
        protected const string Endpoints_BitcoinCash_Transaction = "bcash/transaction";
        protected const string Endpoints_BitcoinCash_Broadcast = "bcash/broadcast";
        #endregion

        #region Blockchain - Litecoin
        protected const string Endpoints_Litecoin_BlockchainInformation = "litecoin/info";
        protected const string Endpoints_Litecoin_GetBlockHash = "litecoin/block/hash/{0}";
        protected const string Endpoints_Litecoin_GetBlockByHash = "litecoin/block/{0}";
        protected const string Endpoints_Litecoin_GetTransactionByHash = "litecoin/transaction/{0}";
        protected const string Endpoints_Litecoin_GetTransactionsByAddress = "litecoin/transaction/address/{0}";
        protected const string Endpoints_Litecoin_GetBalance = "litecoin/address/balance/{0}";
        protected const string Endpoints_Litecoin_GetTransactionUTXO = "litecoin/utxo/{0}/{1}";
        protected const string Endpoints_Litecoin_Transaction = "litecoin/transaction";
        protected const string Endpoints_Litecoin_Broadcast = "litecoin/broadcast";
        #endregion

        #region Blockchain - Ripple
        protected const string Endpoints_Ripple_GenerateAccount = "xrp/account";
        protected const string Endpoints_Ripple_BlockchainInformation = "xrp/info";
        protected const string Endpoints_Ripple_BlockchainFee = "xrp/fee";
        protected const string Endpoints_Ripple_GetTransactionsByAccount = "xrp/account/tx/{0}";
        protected const string Endpoints_Ripple_GetLedger = "xrp/ledger/{0}";
        protected const string Endpoints_Ripple_GetTransactionByHash = "xrp/transaction/{0}";
        protected const string Endpoints_Ripple_AccountInfo = "xrp/account/{0}";
        protected const string Endpoints_Ripple_GetBalance = "xrp/account/{0}/balance";
        protected const string Endpoints_Ripple_Send = "xrp/transaction";
        protected const string Endpoints_Ripple_Trust = "xrp/trust";
        protected const string Endpoints_Ripple_AccountSettings = "xrp/account/settings";
        protected const string Endpoints_Ripple_Broadcast = "xrp/broadcast";
        #endregion

        #region Blockchain - Stellar
        protected const string Endpoints_Stellar_GenerateAccount = "xlm/account";
        protected const string Endpoints_Stellar_BlockchainInformation = "xlm/info";
        protected const string Endpoints_Stellar_BlockchainFee = "xlm/fee";
        protected const string Endpoints_Stellar_GetLedger = "xlm/ledger/{0}";
        protected const string Endpoints_Stellar_GetTransactionsInLedger = "xlm/ledger/{0}/transaction";
        protected const string Endpoints_Stellar_GetTransactionsByAccount = "xlm/account/tx/{0}";
        protected const string Endpoints_Stellar_GetTransactionByHash = "xlm/transaction/{0}";
        protected const string Endpoints_Stellar_AccountInfo = "xlm/account/{0}";
        protected const string Endpoints_Stellar_Send = "xlm/transaction";
        protected const string Endpoints_Stellar_Trust = "xlm/trust";
        protected const string Endpoints_Stellar_Broadcast = "xlm/broadcast";
        #endregion

        #region Blockchain - Records
        protected const string Endpoints_Records_Log = "record";
        #endregion

        #region Blockchain - Binance
        protected const string Endpoints_Binance_GenerateAccount = "bnb/account";
        protected const string Endpoints_Binance_CurrentBlock = "bnb/block/current";
        protected const string Endpoints_Binance_GetTransactionsInBlock = "bnb/block/{0}";
        protected const string Endpoints_Binance_AccountInfo = "bnb/account/{0}";
        protected const string Endpoints_Binance_GetTransaction = "bnb/transaction/{0}";
        protected const string Endpoints_Binance_Send = "bnb/transaction";
        protected const string Endpoints_Binance_Broadcast = "bnb/broadcast";
        #endregion

        #region Blockchain - VeChain
        protected const string Endpoints_VeChain_CurrentBlock = "vet/block/current";
        protected const string Endpoints_VeChain_GetBlockByHash = "vet/block/{0}";
        protected const string Endpoints_VeChain_GetBalance = "vet/account/balance/{0}";
        protected const string Endpoints_VeChain_GetEnergy = "vet/account/energy/{0}";
        protected const string Endpoints_VeChain_GetTransactionByHash = "vet/transaction/{0}";
        protected const string Endpoints_VeChain_GetTransactionReceipt = "vet/transaction/{0}/receipt";
        protected const string Endpoints_VeChain_Transaction = "vet/transaction";
        protected const string Endpoints_VeChain_Gas = "vet/transaction/gas";
        protected const string Endpoints_VeChain_Broadcast = "vet/broadcast";
        #endregion

        #region Blockchain - NEO
        protected const string Endpoints_NEO_GenerateAccount = "neo/wallet";
        protected const string Endpoints_NEO_CurrentBlock = "neo/block/current";
        protected const string Endpoints_NEO_GetBlock = "neo/block/{0}";
        protected const string Endpoints_NEO_GetBalance = "neo/account/balance/{0}";
        protected const string Endpoints_NEO_GetAssetInfo = "neo/asset/{0}";
        protected const string Endpoints_NEO_GetUnspentTransactionOutputs = "neo/transaction/out/{0}/{1}";
        protected const string Endpoints_NEO_GetTransactionsByAccount = "neo/account/tx/{0}";
        protected const string Endpoints_NEO_GetContractInfo = "neo/contract/{0}";
        protected const string Endpoints_NEO_GetTransactionByHash = "neo/transaction/{0}";
        protected const string Endpoints_NEO_Send = "neo/transaction";
        protected const string Endpoints_NEO_ClaimGAS = "neo/claim";
        protected const string Endpoints_NEO_Invoke = "neo/invoke";
        protected const string Endpoints_NEO_Broadcast = "neo/broadcast";
        #endregion

        #region Blockchain - Libra
        protected const string Endpoints_Libra_BlockchainInformation = "libra/info";
        protected const string Endpoints_Libra_GetTransactionsByAccount = "libra/account/transaction/{0}";
        protected const string Endpoints_Libra_AccountInfo = "libra/account/{0}";
        protected const string Endpoints_Libra_GetTransactions = "libra/transaction/{0}/{0}";
        #endregion

        #region Blockchain - TRON
        protected const string Endpoints_TRON_GenerateAccount = "tron/account";
        protected const string Endpoints_TRON_CurrentBlock = "tron/info";
        protected const string Endpoints_TRON_GetBlock = "tron/block/{0}";
        protected const string Endpoints_TRON_GetTransactionsByAccount = "tron/transaction/account/{0}";
        protected const string Endpoints_TRON_GetTransactionByHash = "tron/transaction/{0}";
        protected const string Endpoints_TRON_Send = "tron/transaction";
        protected const string Endpoints_TRON_Broadcast = "tron/broadcast";

        protected const string Endpoints_TRON_Freeze = "tron/freezeBalance";
        protected const string Endpoints_TRON_TRC10GetToken = "tron/trc10/detail/{0}";
        protected const string Endpoints_TRON_TRC10CreateToken = "tron/trc10/deploy";
        protected const string Endpoints_TRON_TRC10Send = "tron/trc10/transaction";
        protected const string Endpoints_TRON_TRC20CreateToken = "tron/trc20/deploy";
        protected const string Endpoints_TRON_TRC20Send = "tron/trc20/transaction";
        #endregion

        #region Blockchain - Scrypta
        protected const string Endpoints_Scrypta_BlockchainInformation = "scrypta/info";
        protected const string Endpoints_Scrypta_GetBlockHash = "scrypta/block/hash/{0}";
        protected const string Endpoints_Scrypta_GetBlockByHash = "scrypta/block/{0}";
        protected const string Endpoints_Scrypta_GetTransactionByHash = "scrypta/transaction/{0}";
        protected const string Endpoints_Scrypta_GetTransactionsByAddress = "scrypta/transaction/address/{0}";
        protected const string Endpoints_Scrypta_GetSpendableUTXO = "scrypta/utxo/{0}";
        protected const string Endpoints_Scrypta_GetTransactionUTXO = "scrypta/utxo/{0}/{1}";
        protected const string Endpoints_Scrypta_Transaction = "scrypta/transaction";
        protected const string Endpoints_Scrypta_Broadcast = "scrypta/broadcast";
        #endregion

        #region Tatum Service
        protected const string Endpoints_Service_Consumption = "tatum/usage";
        protected const string Endpoints_Service_ExchangeRates = "tatum/rate/{0}";
        protected const string Endpoints_Service_Version = "tatum/version";
        #endregion

        #endregion

        #region Constructor / Destructor
        /// <summary>
        /// Create a new instance of TatumClient using the default options
        /// </summary>
        public TatumClient() : this("", DefaultOptions)
        {
        }

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
        public virtual WebCallResult<LedgerAccount> LedgerAccount_Create(BlockchainType chain, LedgerAccountOptions options = null, CancellationToken ct = default) => LedgerAccount_Create_Async(chain, options, ct).Result;
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
        public virtual async Task<WebCallResult<LedgerAccount>> LedgerAccount_Create_Async(BlockchainType chain, LedgerAccountOptions options = null, CancellationToken ct = default)
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
            var url = GetUrl(string.Format(Endpoints_Ledger_Account_Create));
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
        public virtual WebCallResult<IEnumerable<LedgerAccount>> LedgerAccount_GetAccounts(int pageSize = 50, int offset = 0, CancellationToken ct = default) => LedgerAccount_GetAccounts_Async(pageSize, offset, ct).Result;
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
        public virtual async Task<WebCallResult<IEnumerable<LedgerAccount>>> LedgerAccount_GetAccounts_Async(int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);

            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };

            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ledger_Account_List));
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
        public virtual WebCallResult<IEnumerable<LedgerAccount>> LedgerAccount_CreateBatch(IEnumerable<LedgerAccountOptions> accounts, CancellationToken ct = default) => LedgerAccount_CreateBatch_Async(accounts, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create multiple accounts in a batch call<br />
        /// <b>Credits:</b> 2 credits per API call + 1 credit for every created account.<br />
        /// <b>Description:</b>
        /// Creates new accounts for the customer in a batch call.
        /// </summary>
        /// <param name="accounts">Ledger Accounts List</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<LedgerAccount>>> LedgerAccount_CreateBatch_Async(IEnumerable<LedgerAccountOptions> accounts, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "accounts", accounts },
            };

            var credits = 2;
            foreach (var account in accounts) if (!string.IsNullOrEmpty(account.ExtendedPublicKey)) credits++;
            var url = GetUrl(string.Format(Endpoints_Ledger_Account_CreateBatch));
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
        public virtual WebCallResult<IEnumerable<LedgerAccount>> LedgerAccount_GetByCustomerId(string customer_id, int pageSize = 50, int offset = 0, CancellationToken ct = default) => LedgerAccount_GetByCustomerId_Async(customer_id, pageSize, offset, ct).Result;
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
        public virtual async Task<WebCallResult<IEnumerable<LedgerAccount>>> LedgerAccount_GetByCustomerId_Async(string customer_id, int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);

            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ledger_Account_ListByCustomer, customer_id));
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
        public virtual WebCallResult<LedgerAccount> LedgerAccount_GetById(string account_id, int pageSize = 50, int offset = 0, CancellationToken ct = default) => LedgerAccount_GetById_Async(account_id, pageSize, offset, ct).Result;
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
        public virtual async Task<WebCallResult<LedgerAccount>> LedgerAccount_GetById_Async(string account_id, int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);

            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };

            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ledger_Account_GetById, account_id));
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
        public virtual WebCallResult<bool> LedgerAccount_Update(string account_id, string accountCode, string accountNumber, CancellationToken ct = default) => LedgerAccount_Update_Async(account_id, accountCode, accountNumber, ct).Result;
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
        public virtual async Task<WebCallResult<bool>> LedgerAccount_Update_Async(string account_id, string accountCode, string accountNumber, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "accountCode", accountCode },
                { "accountNumber", accountNumber },
            };

            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Ledger_Account_Update, account_id));
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
        public virtual WebCallResult<LedgerBalance> LedgerAccount_GetBalance(string account_id, CancellationToken ct = default) => LedgerAccount_GetBalance_Async(account_id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get account balance<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get balance for the account.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<LedgerBalance>> LedgerAccount_GetBalance_Async(string account_id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ledger_Account_Balance, account_id));
            return await SendTatumRequest<LedgerBalance>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
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
        public virtual WebCallResult<TatumId> LedgerAccount_BlockAmount(string account_id, decimal amount, string type, string description, CancellationToken ct = default) => LedgerAccount_BlockAmount_Async(account_id, amount, type, description, ct).Result;
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
        public virtual async Task<WebCallResult<TatumId>> LedgerAccount_BlockAmount_Async(string account_id, decimal amount, string type, string description, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "amount", amount.ToString() },
                { "type", type },
            };
            parameters.AddOptionalParameter("description", description);

            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Ledger_Account_BlockAmount, account_id));
            return await SendTatumRequest<TatumId>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
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
        public virtual WebCallResult<TatumReference> LedgerAccount_UnlockAmountAndPerformTransaction(
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
            => LedgerAccount_UnlockAmountAndPerformTransaction_Async(blockage_id, recipientAccountId, amount, anonymous, compliant, transactionCode, paymentId, recipientNote, senderNote, baseRate, ct).Result;
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
        public virtual async Task<WebCallResult<TatumReference>> LedgerAccount_UnlockAmountAndPerformTransaction_Async(
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
            var url = GetUrl(string.Format(Endpoints_Ledger_Account_UnlockAmountAndTransfer, blockage_id));
            return await SendTatumRequest<TatumReference>(url, HttpMethod.Put, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
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
        public virtual WebCallResult<bool> LedgerAccount_UnblockAmount(string blockage_id, CancellationToken ct = default) => LedgerAccount_UnblockAmount_Async(blockage_id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Unblock blocked amount on account<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Unblock previously blocked amount on account. Increase available balance on account, where amount was blocked.
        /// </summary>
        /// <param name="blockage_id">Blockage ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> LedgerAccount_UnblockAmount_Async(string blockage_id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ledger_Account_UnblockAmount, blockage_id));
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
        public virtual WebCallResult<IEnumerable<LedgerBlockedAmount>> LedgerAccount_GetBlockedAmounts(string account_id, int pageSize = 50, int offset = 0, CancellationToken ct = default) => LedgerAccount_GetBlockedAmounts_Async(account_id, pageSize, offset, ct).Result;
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
        public virtual async Task<WebCallResult<IEnumerable<LedgerBlockedAmount>>> LedgerAccount_GetBlockedAmounts_Async(string account_id, int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);

            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };

            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ledger_Account_GetBlockedAmounts, account_id));
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
        public virtual WebCallResult<bool> LedgerAccount_UnblockAllBlockedAmounts(string account_id, CancellationToken ct = default) => LedgerAccount_UnblockAllBlockedAmounts_Async(account_id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Unblock all blocked amounts on account<br />
        /// <b>Credits:</b>  credit per API call, 1 credits for each deleted blockage. 1 API call + 2 blockages = 3 credits.<br />
        /// <b>Description:</b>
        /// Unblock previously blocked amounts on account. Increase available balance on account, where amount was blocked.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> LedgerAccount_UnblockAllBlockedAmounts_Async(string account_id, CancellationToken ct = default)
        {
            var credits = 1; // 1 credit per API call, 1 credits for each deleted blockage. 1 API call + 2 blockages = 3 credits.
            var url = GetUrl(string.Format(Endpoints_Ledger_Account_UnblockAllBlockedAmounts, account_id));
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
        public virtual WebCallResult<bool> LedgerAccount_Activate(string account_id, CancellationToken ct = default) => LedgerAccount_Activate_Async(account_id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Activate account<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Activate account.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> LedgerAccount_Activate_Async(string account_id, CancellationToken ct = default)
        {
            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Ledger_Account_ActivateLedgerAccount, account_id));
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
        public virtual WebCallResult<bool> LedgerAccount_Deactivate(string account_id, CancellationToken ct = default) => LedgerAccount_Deactivate_Async(account_id, ct).Result;
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
        public virtual async Task<WebCallResult<bool>> LedgerAccount_Deactivate_Async(string account_id, CancellationToken ct = default)
        {
            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Ledger_Account_DeactivateLedgerAccount, account_id));
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
        public virtual WebCallResult<bool> LedgerAccount_Freeze(string account_id, CancellationToken ct = default) => LedgerAccount_Freeze_Async(account_id, ct).Result;
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
        public virtual async Task<WebCallResult<bool>> LedgerAccount_Freeze_Async(string account_id, CancellationToken ct = default)
        {
            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Ledger_Account_FreezeLedgerAccount, account_id));
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
        public virtual WebCallResult<bool> LedgerAccount_Unfreeze(string account_id, CancellationToken ct = default) => LedgerAccount_Unfreeze_Async(account_id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Unfreeze account<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Unfreeze previously frozen account. Unfreezing non-frozen account will do no harm to the account.
        /// </summary>
        /// <param name="account_id">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> LedgerAccount_Unfreeze_Async(string account_id, CancellationToken ct = default)
        {
            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Ledger_Account_UnfreezeLedgerAccount, account_id));
            var result = await SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }
        #endregion

        #region Ledger / Transaction
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
        public virtual WebCallResult<TatumReference> LedgerTransaction_SendPayment(
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
            => LedgerTransaction_SendPayment_Async(senderAccountId, recipientAccountId, amount, anonymous, compliant, transactionCode, paymentId, recipientNote, baseRate, senderNote, ct).Result;
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
        public virtual async Task<WebCallResult<TatumReference>> LedgerTransaction_SendPayment_Async(
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
            var url = GetUrl(string.Format(Endpoints_Ledger_Transaction_SendPayment));
            return await SendTatumRequest<TatumReference>(url, HttpMethod.Post, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
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
        public virtual WebCallResult<IEnumerable<LedgerTransaction>> LedgerTransaction_GetTransactionsByAccount(
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
            => LedgerTransaction_GetTransactionsByAccount_Async(id, counterAccount, from, to, currency, transactionType, opType, transactionCode, paymentId, recipientNote, senderNote, pageSize, offset, count, ct).Result;
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
        public virtual async Task<WebCallResult<IEnumerable<LedgerTransaction>>> LedgerTransaction_GetTransactionsByAccount_Async(
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
            var url = GetUrl(string.Format(Endpoints_Ledger_Transaction_GetTransactionsByAccount + qs));
            return await SendTatumRequest<IEnumerable<LedgerTransaction>>(url, HttpMethod.Post, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
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
        public virtual WebCallResult<IEnumerable<LedgerTransaction>> LedgerTransaction_GetTransactionsByCustomer(
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
            => LedgerTransaction_GetTransactionsByCustomer_Async(id, account, counterAccount, from, to, currency, transactionType, opType, transactionCode, paymentId, recipientNote, senderNote, pageSize, offset, count, ct).Result;
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
        public virtual async Task<WebCallResult<IEnumerable<LedgerTransaction>>> LedgerTransaction_GetTransactionsByCustomer_Async(
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
            var url = GetUrl(string.Format(Endpoints_Ledger_Transaction_GetTransactionsByCustomer + qs));
            return await SendTatumRequest<IEnumerable<LedgerTransaction>>(url, HttpMethod.Post, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
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
        public virtual WebCallResult<IEnumerable<LedgerTransaction>> LedgerTransaction_GetTransactionsByLedger(
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
            => LedgerTransaction_GetTransactionsByLedger_Async(account, counterAccount, from, to, currency, transactionType, opType, transactionCode, paymentId, recipientNote, senderNote, pageSize, offset, count, ct).Result;
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
        public virtual async Task<WebCallResult<IEnumerable<LedgerTransaction>>> LedgerTransaction_GetTransactionsByLedger_Async(
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
            var url = GetUrl(string.Format(Endpoints_Ledger_Transaction_GetTransactionsByLedger + qs));
            return await SendTatumRequest<IEnumerable<LedgerTransaction>>(url, HttpMethod.Post, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
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
        public virtual WebCallResult<IEnumerable<LedgerTransaction>> LedgerTransaction_GetTransactionsByReference(string reference, CancellationToken ct = default) => LedgerTransaction_GetTransactionsByReference_Async(reference, ct).Result;
        /// <summary>
        /// <b>Title:</b> Find transactions with given reference across all accounts.<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Find transactions for all accounts with given reference.
        /// </summary>
        /// <param name="reference">reference</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<LedgerTransaction>>> LedgerTransaction_GetTransactionsByReference_Async(string reference, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ledger_Transaction_GetTransactionsByReference, reference));
            return await SendTatumRequest<IEnumerable<LedgerTransaction>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }
        #endregion

        #region Ledger / Customer
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
        public virtual WebCallResult<IEnumerable<LedgerCustomer>> LedgerCustomer_ListAll(int pageSize = 50, int offset = 0, CancellationToken ct = default) => LedgerCustomer_ListAll_Async(pageSize, offset, ct).Result;
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
        public virtual async Task<WebCallResult<IEnumerable<LedgerCustomer>>> LedgerCustomer_ListAll_Async(int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);
            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };

            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ledger_Customer_List));
            return await SendTatumRequest<IEnumerable<LedgerCustomer>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
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
        public virtual WebCallResult<LedgerCustomer> LedgerCustomer_Get(string id, CancellationToken ct = default) => LedgerCustomer_Get_Async(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get customer details<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Using anonymized external ID or internal customer ID you can access customer detail information. Internal ID is needed to call other customer related methods.
        /// </summary>
        /// <param name="id">Customer external or internal ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<LedgerCustomer>> LedgerCustomer_Get_Async(string id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ledger_Customer_Get, id));
            return await SendTatumRequest<LedgerCustomer>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
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
        public virtual WebCallResult<LedgerCustomer> LedgerCustomer_Update(string id, string externalId, string accountingCurrency = null, string customerCountry = null, string providerCountry = null, CancellationToken ct = default) => LedgerCustomer_Update_Async(id, externalId, accountingCurrency, customerCountry, providerCountry, ct).Result;
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
        public virtual async Task<WebCallResult<LedgerCustomer>> LedgerCustomer_Update_Async(string id, string externalId, string accountingCurrency = null, string customerCountry = null, string providerCountry = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "externalId", externalId },
            };
            parameters.AddOptionalParameter("accountingCurrency", accountingCurrency);
            parameters.AddOptionalParameter("customerCountry", customerCountry);
            parameters.AddOptionalParameter("providerCountry", providerCountry);

            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Ledger_Customer_Update, id));
            return await SendTatumRequest<LedgerCustomer>(url, HttpMethod.Put, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
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
        public virtual WebCallResult<bool> LedgerCustomer_Activate(string id, CancellationToken ct = default) => LedgerCustomer_Activate_Async(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Activate customer<br />
        /// <b>Credits:</b> 2 credit per API call.<br />
        /// <b>Description:</b>
        /// Activated customer is able to do any operation.
        /// </summary>
        /// <param name="id">Customer internal ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> LedgerCustomer_Activate_Async(string id, CancellationToken ct = default)
        {
            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Ledger_Customer_Activate, id));
            var result = await SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
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
        public virtual WebCallResult<bool> LedgerCustomer_Deactivate(string id, CancellationToken ct = default) => LedgerCustomer_Deactivate_Async(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Deactivate customer<br />
        /// <b>Credits:</b> 2 credit per API call.<br />
        /// <b>Description:</b>
        /// Deactivate customer is not able to do any operation. Customer can be deactivated only when all their accounts are already deactivated.
        /// </summary>
        /// <param name="id">Customer internal ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> LedgerCustomer_Deactivate_Async(string id, CancellationToken ct = default)
        {
            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Ledger_Customer_Deactivate, id));
            var result = await SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
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
        public virtual WebCallResult<bool> LedgerCustomer_Enable(string id, CancellationToken ct = default) => LedgerCustomer_Enable_Async(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Enable customer<br />
        /// <b>Credits:</b> 2 credit per API call.<br />
        /// <b>Description:</b>
        /// Enabled customer can perform all operations. By default all customers are enabled. All previously blocked account balances will be unblocked.
        /// </summary>
        /// <param name="id">Customer internal ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> LedgerCustomer_Enable_Async(string id, CancellationToken ct = default)
        {
            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Ledger_Customer_Enable, id));
            var result = await SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
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
        public virtual WebCallResult<bool> LedgerCustomer_Disable(string id, CancellationToken ct = default) => LedgerCustomer_Disable_Async(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Disable customer<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Disabled customer cannot perform end-user operations, such as create new accounts or send transactions. Available balance on all accounts is set to 0. Account balance will stay untouched.
        /// </summary>
        /// <param name="id">Customer internal ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> LedgerCustomer_Disable_Async(string id, CancellationToken ct = default)
        {
            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Ledger_Customer_Disable, id));
            var result = await SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }
        #endregion

        #region Ledger / Virtual Currency
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
        public virtual WebCallResult<LedgerReport> LedgerVirtualCurrency_Create(
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
            => LedgerVirtualCurrency_Create_Async(name, supply, basePair, baseRate, customer, description, accountCode, accountNumber, accountingCurrency, ct).Result;
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
        public virtual async Task<WebCallResult<LedgerReport>> LedgerVirtualCurrency_Create_Async(
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
            var url = GetUrl(string.Format(Endpoints_Ledger_VirtualCurrency_Create));
            return await SendTatumRequest<LedgerReport>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
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
        public virtual WebCallResult<bool> LedgerVirtualCurrency_Update(string name, string basePair = null, decimal? baseRate = null, CancellationToken ct = default) => LedgerVirtualCurrency_Update_Async(name, basePair, baseRate, ct).Result;
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
        public virtual async Task<WebCallResult<bool>> LedgerVirtualCurrency_Update_Async(string name, string basePair = null, decimal? baseRate = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "name", name },
            };
            parameters.AddOptionalParameter("basePair", basePair);
            parameters.AddOptionalParameter("baseRate", baseRate);

            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Ledger_VirtualCurrency_Update));
            var result = await SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
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
        public virtual WebCallResult<LedgerReport> LedgerVirtualCurrency_Get(string name, CancellationToken ct = default) => LedgerVirtualCurrency_Get_Async(name, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get virtual currency<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get detail of virtual currency.
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<LedgerReport>> LedgerVirtualCurrency_Get_Async(string name, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ledger_VirtualCurrency_Get, name));
            return await SendTatumRequest<LedgerReport>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
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
        public virtual WebCallResult<TatumReference> LedgerVirtualCurrency_Mint(
            string accountId,
            decimal amount,
            string paymentId = null,
            string reference = null,
            string transactionCode = null,
            string recipientNote = null,
            string counterAccount = null,
            string senderNote = null,
            CancellationToken ct = default)
            => LedgerVirtualCurrency_Mint_Async(accountId, amount, paymentId, reference, transactionCode, recipientNote, counterAccount, senderNote, ct).Result;
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
        public virtual async Task<WebCallResult<TatumReference>> LedgerVirtualCurrency_Mint_Async(
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
            var url = GetUrl(string.Format(Endpoints_Ledger_VirtualCurrency_Mint));
            return await SendTatumRequest<TatumReference>(url, HttpMethod.Put, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
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
        public virtual WebCallResult<TatumReference> LedgerVirtualCurrency_Destroy(
            string accountId,
            decimal amount,
            string paymentId = null,
            string reference = null,
            string transactionCode = null,
            string recipientNote = null,
            string counterAccount = null,
            string senderNote = null,
            CancellationToken ct = default)
            => LedgerVirtualCurrency_Destroy_Async(accountId, amount, paymentId, reference, transactionCode, recipientNote, counterAccount, senderNote, ct).Result;
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
        public virtual async Task<WebCallResult<TatumReference>> LedgerVirtualCurrency_Destroy_Async(
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
            var url = GetUrl(string.Format(Endpoints_Ledger_VirtualCurrency_Destroy));
            return await SendTatumRequest<TatumReference>(url, HttpMethod.Put, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }
        #endregion

        #region Ledger / Subscription
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
        public virtual WebCallResult<TatumId> LedgerSubscription_Create(
            LedgerSubscriptionType type,
            string account_id = null,
            string url = null,
            string currency = null,
            int? interval = null,
            decimal? limit = null,
            LedgerBalanceType? typeOfBalance = null,
            CancellationToken ct = default)
            => LedgerSubscription_Create_Async(type, account_id, url, currency, interval, limit, typeOfBalance, ct).Result;
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
        public virtual async Task<WebCallResult<TatumId>> LedgerSubscription_Create_Async(
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
            var url_ = GetUrl(string.Format(Endpoints_Ledger_Subscription_Create));
            return await SendTatumRequest<TatumId>(url_, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
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
        public virtual WebCallResult<IEnumerable<LedgerSubscription>> LedgerSubscription_List(int pageSize = 50, int offset = 0, CancellationToken ct = default) => LedgerSubscription_List_Async(pageSize, offset, ct).Result;
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
        public virtual async Task<WebCallResult<IEnumerable<LedgerSubscription>>> LedgerSubscription_List_Async(int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);

            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };

            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ledger_Subscription_List));
            return await SendTatumRequest<IEnumerable<LedgerSubscription>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
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
        public virtual WebCallResult<bool> LedgerSubscription_Cancel(string id, CancellationToken ct = default) => LedgerSubscription_Cancel_Async(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Cancel existing subscription<br />
        /// <b>Credits:</b> 1 credit for API call<br />
        /// <b>Description:</b>
        /// Cancel existing subscription.
        /// </summary>
        /// <param name="id">Subscription ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> LedgerSubscription_Cancel_Async(string id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ledger_Subscription_Cancel, id));
            var result = await SendTatumRequest<string>(url, HttpMethod.Delete, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
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
        public virtual WebCallResult<IEnumerable<LedgerReport>> LedgerSubscription_GetReport(string id, CancellationToken ct = default) => LedgerSubscription_GetReport_Async(id, ct).Result;
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
        public virtual async Task<WebCallResult<IEnumerable<LedgerReport>>> LedgerSubscription_GetReport_Async(string id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ledger_Subscription_Report, id));
            return await SendTatumRequest<IEnumerable<LedgerReport>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }
        #endregion

        #region Ledger / Order Book
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
        public virtual WebCallResult<IEnumerable<LedgerTrade>> LedgerOrderBook_GetHistoricalTrades(string id, string pair, int pageSize = 50, int offset = 0, CancellationToken ct = default) => LedgerOrderBook_GetHistoricalTrades_Async(id, pair, pageSize, offset, ct).Result;
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
        public virtual async Task<WebCallResult<IEnumerable<LedgerTrade>>> LedgerOrderBook_GetHistoricalTrades_Async(string id, string pair, int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);
            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };
            parameters.AddOptionalParameter("id", id);
            parameters.AddOptionalParameter("pair", pair);

            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ledger_OrderBook_ListHistory));
            return await SendTatumRequest<IEnumerable<LedgerTrade>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

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
        public virtual WebCallResult<IEnumerable<LedgerTrade>> LedgerOrderBook_GetBuyTrades(string id, int pageSize = 50, int offset = 0, CancellationToken ct = default) => LedgerOrderBook_GetBuyTrades_Async(id, pageSize, offset, ct).Result;
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
        public virtual async Task<WebCallResult<IEnumerable<LedgerTrade>>> LedgerOrderBook_GetBuyTrades_Async(string id, int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);
            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };
            parameters.AddOptionalParameter("id", id);

            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ledger_OrderBook_ListBuys));
            return await SendTatumRequest<IEnumerable<LedgerTrade>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

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
        public virtual WebCallResult<IEnumerable<LedgerTrade>> LedgerOrderBook_GetSellTrades(string id, int pageSize = 50, int offset = 0, CancellationToken ct = default) => LedgerOrderBook_GetSellTrades_Async(id, pageSize, offset, ct).Result;
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
        public virtual async Task<WebCallResult<IEnumerable<LedgerTrade>>> LedgerOrderBook_GetSellTrades_Async(string id, int pageSize = 50, int offset = 0, CancellationToken ct = default)
        {
            pageSize.ValidateIntBetween(nameof(pageSize), 1, 50);
            var parameters = new Dictionary<string, object> {
                { "pageSize", pageSize },
                { "offset", offset },
            };
            parameters.AddOptionalParameter("id", id);

            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ledger_OrderBook_ListSells));
            return await SendTatumRequest<IEnumerable<LedgerTrade>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
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
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TatumId> LedgerOrderBook_PlaceOrder(
            LedgerTradeType type,
            decimal price,
            decimal amount,
            string pair,
            string currency1AccountId,
            string currency2AccountId,
            string feeAccountId = null,
            decimal? fee = null,
            CancellationToken ct = default) => LedgerOrderBook_PlaceOrder_Async(type, price, amount, pair, currency1AccountId, currency2AccountId, feeAccountId, fee, ct).Result;
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
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TatumId>> LedgerOrderBook_PlaceOrder_Async(
            LedgerTradeType type,
            decimal price,
            decimal amount,
            string pair,
            string currency1AccountId,
            string currency2AccountId,
            string feeAccountId = null,
            decimal? fee = null,
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

            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Ledger_OrderBook_Place));
            return await SendTatumRequest<TatumId>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get existing trade<br />
        /// <b>Credits:</b> 1 credit for API call<br />
        /// <b>Description:</b>
        /// Get existing opened trade.
        /// </summary>
        /// <param name="id">Trade ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<LedgerTrade> LedgerOrderBook_GetTrade(string id, CancellationToken ct = default) => LedgerOrderBook_GetTrade_Async(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get existing trade<br />
        /// <b>Credits:</b> 1 credit for API call<br />
        /// <b>Description:</b>
        /// Get existing opened trade.
        /// </summary>
        /// <param name="id">Trade ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<LedgerTrade>> LedgerOrderBook_GetTrade_Async(string id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ledger_OrderBook_Get, id));
            return await SendTatumRequest<LedgerTrade>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
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
        public virtual WebCallResult<bool> LedgerOrderBook_CancelOrder(string id, CancellationToken ct = default) => LedgerOrderBook_CancelOrder_Async(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Cancel existing trade<br />
        /// <b>Credits:</b> 1 credit for API call<br />
        /// <b>Description:</b>
        /// Cancel existing trade.
        /// </summary>
        /// <param name="id">Trade ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> LedgerOrderBook_CancelOrder_Async(string id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ledger_OrderBook_Cancel, id));
            var result = await SendTatumRequest<string>(url, HttpMethod.Delete, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
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
        public virtual WebCallResult<bool> LedgerOrderBook_CancelAllOrders(string id, CancellationToken ct = default) => LedgerOrderBook_CancelAllOrders_Async(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Cancel all existing trades for account<br />
        /// <b>Credits:</b> 1 credit for API call, 1 credit for each cancelled trade. 1 API call + 2 cancellations = 3 credits.<br />
        /// <b>Description:</b>
        /// Cancel all trades for account.
        /// </summary>
        /// <param name="id">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> LedgerOrderBook_CancelAllOrders_Async(string id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Ledger_OrderBook_CancelAll, id));
            var result = await SendTatumRequest<string>(url, HttpMethod.Delete, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }
        #endregion

        #region Security / Key Management System
        /// <summary>
        /// <b>Title:</b> Get pending transactions to sign<br />
        /// <b>Credits:</b> 1 credits per API call.<br />
        /// <b>Description:</b>
        /// Get list of pending transaction to be signed and broadcast using Tatum KMS.
        /// </summary>
        /// <param name="chain">Blockchain to get pending transactions for.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<KMSPendingTransaction>> KMS_GetPendingTransactions(BlockchainType chain, CancellationToken ct = default) => KMS_GetPendingTransactions_Async(chain, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get pending transactions to sign<br />
        /// <b>Credits:</b> 1 credits per API call.<br />
        /// <b>Description:</b>
        /// Get list of pending transaction to be signed and broadcast using Tatum KMS.
        /// </summary>
        /// <param name="chain">Blockchain to get pending transactions for.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<KMSPendingTransaction>>> KMS_GetPendingTransactions_Async(BlockchainType chain, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_KMS_GetPendingTransactions, JsonConvert.SerializeObject(chain, new BlockchainTypeConverter(false))));
            return await SendTatumRequest<IEnumerable<KMSPendingTransaction>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Complete pending transaction to sign<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Mark pending transaction to sign as a complete and update it with a transactionID from the blockchain.
        /// </summary>
        /// <param name="id">ID of pending transaction</param>
        /// <param name="txId">transaction ID of blockchain transaction</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> KMS_CompletePendingTransaction(string id, string txId, CancellationToken ct = default) => KMS_CompletePendingTransaction_Async(id, txId, ct).Result;
        /// <summary>
        /// <b>Title:</b> Complete pending transaction to sign<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Mark pending transaction to sign as a complete and update it with a transactionID from the blockchain.
        /// </summary>
        /// <param name="id">ID of pending transaction</param>
        /// <param name="txId">transaction ID of blockchain transaction</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> KMS_CompletePendingTransaction_Async(string id, string txId, CancellationToken ct = default)
        {
            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_KMS_CompletePendingTransaction, id, txId));
            var result = await SendTatumRequest<string>(url, HttpMethod.Delete, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Get transaction details<br />
        /// <b>Credits:</b> 1 credits per API call.<br />
        /// <b>Description:</b>
        /// Get detail of transaction to be signed / that was already signed and contains transactionId.
        /// </summary>
        /// <param name="id">ID of transaction</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<KMSPendingTransaction> KMS_GetTransaction(string id, CancellationToken ct = default) => KMS_GetTransaction_Async(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get transaction details<br />
        /// <b>Credits:</b> 1 credits per API call.<br />
        /// <b>Description:</b>
        /// Get detail of transaction to be signed / that was already signed and contains transactionId.
        /// </summary>
        /// <param name="id">ID of transaction</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<KMSPendingTransaction>> KMS_GetTransaction_Async(string id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_KMS_Transaction, id));
            return await SendTatumRequest<KMSPendingTransaction>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Delete transaction<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Delete transaction to be signed. When deleting offchain transaction, linked withdrawal will be cancelled automatically.
        /// </summary>
        /// <param name="id">ID of transaction,</param>
        /// <param name="revert">Defines whether fee should be reverted to account balance as well as amount. Defaults to true. Revert true would be typically used when withdrawal was not broadcast to blockchain. False is used usually for Ethereum ERC20 based currencies.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> KMS_DeleteTransaction(string id, bool revert = true, CancellationToken ct = default) => KMS_DeleteTransaction_Async(id, revert, ct).Result;
        /// <summary>
        /// <b>Title:</b> Delete transaction<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Delete transaction to be signed. When deleting offchain transaction, linked withdrawal will be cancelled automatically.
        /// </summary>
        /// <param name="id">ID of transaction,</param>
        /// <param name="revert">Defines whether fee should be reverted to account balance as well as amount. Defaults to true. Revert true would be typically used when withdrawal was not broadcast to blockchain. False is used usually for Ethereum ERC20 based currencies.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> KMS_DeleteTransaction_Async(string id, bool revert = true, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "revert", revert }
            };

            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_KMS_Transaction, id));
            var result = await SendTatumRequest<string>(url, HttpMethod.Delete, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }
        #endregion

        #region Security / Address
        /// <summary>
        /// <b>Title:</b> Check malicous address<br />
        /// <b>Credits:</b> 1 credits per API call.<br />
        /// <b>Description:</b>
        /// Endpoint to check, if the blockchain address is safe to work with or not.
        /// </summary>
        /// <param name="address">Check, if the blockchain address is malicous. Malicous address can contain assets from the DarkWeb, is connected to the scam projects or contains stolen funds.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<SecurityStatus> Security_CheckMalicousAddress(string address, CancellationToken ct = default) => Security_CheckMalicousAddress_Async(address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Check malicous address<br />
        /// <b>Credits:</b> 1 credits per API call.<br />
        /// <b>Description:</b>
        /// Endpoint to check, if the blockchain address is safe to work with or not.
        /// </summary>
        /// <param name="address">Check, if the blockchain address is malicous. Malicous address can contain assets from the DarkWeb, is connected to the scam projects or contains stolen funds.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<SecurityStatus>> Security_CheckMalicousAddress_Async(string address, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Security_CheckMalicousAddress, address));
            return await SendTatumRequest<SecurityStatus>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }
        #endregion

        #region Off-chain / Account
        /// <summary>
        /// <b>Title:</b> Create new deposit address<br />
        /// <b>Credits:</b> 2 credits per API call and 5 credits for each address registered for scanning every day.<br />
        /// <b>Description:</b>
        /// Create a new deposit address for the account. This method associates public blockchain's ledger address with the account on Tatum's private ledger.
        /// It is possible to generate multiple blockchain addresses for the same ledger account.By this, it is possible to aggregate various blockchain transactions from different addresses into the same account.Depending on the currency of an account, this method will either generate a public address for Bitcoin, Bitcoin Cash, Litecoin or Ethereum, DestinationTag in case of XRP or message in case of XLM.More information about supported blockchains and address types can be found here.
        /// Addresses are generated in the natural order of the Extended public key provided in the account.Derivation index is the representation of that order - starts from 0 and ends at 2^31. When a new address is generated, the last not used index is used to generate an address.It is possible to skip some of the addresses to the different index, which means all the skipped addresses will no longer be used.
        /// </summary>
        /// <param name="account">Account ID</param>
        /// <param name="index">Derivation path index for specific address. If not present, last used index for given xpub of account + 1 is used. We recommend not to pass this value manually, since when some of the indexes are skipped, it is not possible to use them lately to generate address from it.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OffchainDepositAddress> OffchainAccount_GenerateDepositAddress(string account, int? index = null, CancellationToken ct = default) => OffchainAccount_GenerateDepositAddress_Async(account, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create new deposit address<br />
        /// <b>Credits:</b> 2 credits per API call and 5 credits for each address registered for scanning every day.<br />
        /// <b>Description:</b>
        /// Create a new deposit address for the account. This method associates public blockchain's ledger address with the account on Tatum's private ledger.
        /// It is possible to generate multiple blockchain addresses for the same ledger account.By this, it is possible to aggregate various blockchain transactions from different addresses into the same account.Depending on the currency of an account, this method will either generate a public address for Bitcoin, Bitcoin Cash, Litecoin or Ethereum, DestinationTag in case of XRP or message in case of XLM.More information about supported blockchains and address types can be found here.
        /// Addresses are generated in the natural order of the Extended public key provided in the account.Derivation index is the representation of that order - starts from 0 and ends at 2^31. When a new address is generated, the last not used index is used to generate an address.It is possible to skip some of the addresses to the different index, which means all the skipped addresses will no longer be used.
        /// </summary>
        /// <param name="account">Account ID</param>
        /// <param name="index">Derivation path index for specific address. If not present, last used index for given xpub of account + 1 is used. We recommend not to pass this value manually, since when some of the indexes are skipped, it is not possible to use them lately to generate address from it.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OffchainDepositAddress>> OffchainAccount_GenerateDepositAddress_Async(string account, int? index = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("index", index);

            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Offchain_Account_DepositAddress, account));
            return await SendTatumRequest<OffchainDepositAddress>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get all deposit addresses for account<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get all deposit addresses generated for account. It is possible to deposit funds from another blockchain address to any of associated addresses and they will be credited on the Tatum Ledger account connected to the address.
        /// </summary>
        /// <param name="account">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OffchainDepositAddress>> OffchainAccount_GetAllDepositAddresses(string account, CancellationToken ct = default) => OffchainAccount_GetAllDepositAddresses_Async(account, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get all deposit addresses for account<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get all deposit addresses generated for account. It is possible to deposit funds from another blockchain address to any of associated addresses and they will be credited on the Tatum Ledger account connected to the address.
        /// </summary>
        /// <param name="account">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OffchainDepositAddress>>> OffchainAccount_GetAllDepositAddresses_Async(string account, CancellationToken ct = default)
        {
            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Offchain_Account_DepositAddress, account));
            return await SendTatumRequest<IEnumerable<OffchainDepositAddress>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Create new deposit addresses in a batch call<br />
        /// <b>Credits:</b> 2 credits per API call, 1 credit for every address created and 5 credits for each address registered for scanning every day.<br />
        /// <b>Description:</b>
        /// Create new deposit addressess for the account. This method associates public blockchain's ledger address with the account on Tatum's private ledger.
        /// It is possible to generate multiple blockchain addresses for the same ledger account.By this, it is possible to aggregate various blockchain transactions from different addresses into the same account.Depending on the currency of an account, this method will either generate a public address for Bitcoin, Bitcoin Cash, Litecoin or Ethereum, DestinationTag in case of XRP or message in case of XLM.More information about supported blockchains and address types can be found here.
        /// Addresses are generated in the natural order of the Extended public key provided in the account.Derivation index is the representation of that order - starts from 0 and ends at 2^31. When a new address is generated, the last not used index is used to generate an address.It is possible to skip some of the addresses to the different index, which means all the skipped addresses will no longer be used.
        /// </summary>
        /// <param name="addresses"></param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OffchainDepositAddress>> OffchainAccount_GenerateMultipleDepositAddresses(IEnumerable<OffchainDepositAddressRequest> addresses, CancellationToken ct = default) => OffchainAccount_GenerateMultipleDepositAddresses_Async(addresses, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create new deposit addresses in a batch call<br />
        /// <b>Credits:</b> 2 credits per API call, 1 credit for every address created and 5 credits for each address registered for scanning every day.<br />
        /// <b>Description:</b>
        /// Create new deposit addressess for the account. This method associates public blockchain's ledger address with the account on Tatum's private ledger.
        /// It is possible to generate multiple blockchain addresses for the same ledger account.By this, it is possible to aggregate various blockchain transactions from different addresses into the same account.Depending on the currency of an account, this method will either generate a public address for Bitcoin, Bitcoin Cash, Litecoin or Ethereum, DestinationTag in case of XRP or message in case of XLM.More information about supported blockchains and address types can be found here.
        /// Addresses are generated in the natural order of the Extended public key provided in the account.Derivation index is the representation of that order - starts from 0 and ends at 2^31. When a new address is generated, the last not used index is used to generate an address.It is possible to skip some of the addresses to the different index, which means all the skipped addresses will no longer be used.
        /// </summary>
        /// <param name="addresses"></param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OffchainDepositAddress>>> OffchainAccount_GenerateMultipleDepositAddresses_Async(IEnumerable<OffchainDepositAddressRequest> addresses, CancellationToken ct = default)
        {
            if (addresses == null || addresses.Count() == 0)
                throw new ArgumentException("addresses parameter must contain one element at least");

            var parameters = new Dictionary<string, object>
            {
                { "addresses", addresses }
            };

            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Offchain_Account_DepositAddressBatch, addresses));
            return await SendTatumRequest<IEnumerable<OffchainDepositAddress>>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Check, if deposit address is assigned<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Check, whether blockchain address for given currency is registered within Tatum and assigned to Tatum Account. Returns account this address belongs to, otherwise throws an error.
        /// </summary>
        /// <param name="chain">Blockchain Type</param>
        /// <param name="address">Blockchain Address to check</param>
        /// <param name="index">In case of XLM or XRP, this is a memo or DestinationTag to search for.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OffchainDepositAddressState> OffchainAccount_CheckAddress(BlockchainType chain, string address, int? index = null, CancellationToken ct = default) => OffchainAccount_CheckAddress_Async(chain, address, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Check, if deposit address is assigned<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Check, whether blockchain address for given currency is registered within Tatum and assigned to Tatum Account. Returns account this address belongs to, otherwise throws an error.
        /// </summary>
        /// <param name="chain">Blockchain Type</param>
        /// <param name="address">Blockchain Address to check</param>
        /// <param name="index">In case of XLM or XRP, this is a memo or DestinationTag to search for.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OffchainDepositAddressState>> OffchainAccount_CheckAddress_Async(BlockchainType chain, string address, int? index = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("index", index);

            var credits = 1;
            var ops = chain.GetBlockchainOptions();
            var url = GetUrl(string.Format(Endpoints_Offchain_Account_CheckAddress, address, ops.Code));
            return await SendTatumRequest<OffchainDepositAddressState>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Remove address for account<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Remove blockchain address from the Ledger account. Tatum will not check for any incoming deposits on this address for this account. It will not be possible to generate the address in the future anymore.
        /// </summary>
        /// <param name="account">Account ID</param>
        /// <param name="address">Blockchain address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> OffchainAccount_RemoveAddressFromAccount(string account, string address, CancellationToken ct = default) => OffchainAccount_RemoveAddressFromAccount_Async(account, address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Remove address for account<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Remove blockchain address from the Ledger account. Tatum will not check for any incoming deposits on this address for this account. It will not be possible to generate the address in the future anymore.
        /// </summary>
        /// <param name="account">Account ID</param>
        /// <param name="address">Blockchain address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> OffchainAccount_RemoveAddressFromAccount_Async(string account, string address, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Offchain_Account_RemoveAddress, account, address));
            var result = await SendTatumRequest<string>(url, HttpMethod.Delete, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Assign address for account<br />
        /// <b>Credits:</b> 2 credits for API call and 5 credits for each address registered for scanning every day.<br />
        /// <b>Description:</b>
        /// This method is used when the account has no default xpub assigned, and addresses are handled manually. It is possible to pair any number of blockchain address to the account.
        /// </summary>
        /// <param name="account">Account ID</param>
        /// <param name="address">Blockchain address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OffchainDepositAddress>> OffchainAccount_AssignAddressToAccount(string account, string address, CancellationToken ct = default) => OffchainAccount_AssignAddressToAccount_Async(account, address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Assign address for account<br />
        /// <b>Credits:</b> 2 credits for API call and 5 credits for each address registered for scanning every day.<br />
        /// <b>Description:</b>
        /// This method is used when the account has no default xpub assigned, and addresses are handled manually. It is possible to pair any number of blockchain address to the account.
        /// </summary>
        /// <param name="account">Account ID</param>
        /// <param name="address">Blockchain address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OffchainDepositAddress>>> OffchainAccount_AssignAddressToAccount_Async(string account, string address, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Offchain_Account_AssignAddress, account, address));
            return await SendTatumRequest<IEnumerable<OffchainDepositAddress>>(url, HttpMethod.Post, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }
        #endregion

        #region Off-chain / Blockchain
        protected virtual async Task<WebCallResult<OffchainTransferResponse>> OffchainBlockchain_Send_Async(
            BlockchainType chain,
            string senderAccountId, string to_address, decimal amount, bool? compliant = null, decimal? fee = null,
            IEnumerable<decimal> multipleAmounts = null,
            IEnumerable<OffchainAddressPrivateKeyPair> keyPairs = null,
            string attr = null, string mnemonic = null, string signatureId = null,
            string xpub = null, string paymentId = null, string senderNote = null,
            CancellationToken ct = default)
        {
            if (!chain.IsOneOf(
                BlockchainType.Bitcoin,
                BlockchainType.BitcoinCash,
                BlockchainType.Litecoin))
                throw new ArgumentException("Wrong BlockchainType");

            var credict = new Dictionary<BlockchainType, int>
            {
                { BlockchainType.Bitcoin, 2 },
                { BlockchainType.BitcoinCash, 10 },
                { BlockchainType.Litecoin, 10 },
            };

            var ci = CultureInfo.InvariantCulture;
            var credits = credict[chain];
            var parameters = new Dictionary<string, object>
            {
                { "senderAccountId", senderAccountId },
                { "address", to_address },
                { "amount", amount.ToString(ci) },
            };
            parameters.AddOptionalParameter("compliant", compliant);
            parameters.AddOptionalParameter("fee", fee?.ToString(ci));
            if (multipleAmounts != null)
            {
                var lst = new List<string>();
                foreach (var ma in multipleAmounts) lst.Add(ma.ToString(ci));
                parameters.AddOptionalParameter("multipleAmounts", lst);
            }
            parameters.AddOptionalParameter("keyPair", keyPairs);
            parameters.AddOptionalParameter("attr", attr);
            parameters.AddOptionalParameter("mnemonic", mnemonic);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("xpub", xpub);
            parameters.AddOptionalParameter("paymentId", paymentId);
            parameters.AddOptionalParameter("senderNote", senderNote);

            var ops = chain.GetBlockchainOptions();
            var url = GetUrl(string.Format(Endpoints_Offchain_Blockchain_Transfer, ops.ChainSlug));
            var result = await SendTatumRequest<OffchainTransferResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success/* || !result.Data.Completed*/) return WebCallResult<OffchainTransferResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<OffchainTransferResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Send Bitcoin from Tatum account to address<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Bitcoin from Tatum account to address. This will create Tatum internal withdrawal request with ID. If every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If Bitcoin server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// There are two possibilites how the transaction on the blockchain can be created:
        /// - Using mnemonic - all of the addresses, that are generated from the mnemonic are scanned for the incoming deposits which are used as a source of the transaction.Assets, which are not used in a transaction are moved to the system address wih the derivation index 0. Address with index 0 cannot be assigned automatically to any account and is used for custodial wallet use cases. For non-custodial wallets, field attr should be present and it should be address with the index 1 of the connected wallet.
        /// - Using keyPair - addresses which are used as a source of the transaction are entered manually
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This method is a helper method, which internally wraps these steps:
        /// 1. Store withdrawal - create a ledger transaction, which debits the assets on the sender account.
        /// 2. Perform blockchain transaction -
        /// 3. Complete withdrawal - move the withdrawal to the completed state, when all of the previous steps were successful.
        /// When some of the steps fails, Cancel withdrawal operation is used, which cancels withdrawal and creates refund transaction to the sender account.This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="to_address">Blockchain address to send assets to. For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain addresses as a comma separated string.</param>
        /// <param name="amount">Amount to be withdrawn to blockchain.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="fee">Fee to be submitted as a transaction fee to blockchain. If none is set, default value of 0.0005 BTC is used.</param>
        /// <param name="multipleAmounts">For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain amounts. List of recipient addresses must be present in the address field and total sum of amounts must be equal to the amount field.</param>
        /// <param name="keyPairs">Array of assigned blockchain addresses with their private keys. Either mnemonic, keyPair or signature Id must be present - depends on the type of account and xpub. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="attr">Used to parametrize withdrawal as a change address for left coins from transaction. XPub or attr must be used.</param>
        /// <param name="mnemonic">Mnemonic seed - usually 12-24 words with access to whole wallet. Either mnemonic, keyPair or signature Id must be present - depends on the type of account and xpub. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="signatureId">Signature hash of the mnemonic, which will be used to sign transactions locally. All signature Ids should be present, which might be used to sign transaction. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="xpub">Extended public key (xpub) of the wallet associated with the accounts. Should be present, when mnemonic is used.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OffchainTransferResponse> OffchainBlockchain_SendBitcoin(
            string senderAccountId, string to_address, decimal amount, bool? compliant = null, decimal? fee = null,
            IEnumerable<decimal> multipleAmounts = null, IEnumerable<OffchainAddressPrivateKeyPair> keyPairs = null,
            string attr = null, string mnemonic = null, string signatureId = null,
            string xpub = null, string paymentId = null, string senderNote = null,
            CancellationToken ct = default)
            => OffchainBlockchain_Send_Async(BlockchainType.Bitcoin, senderAccountId, to_address, amount, compliant, fee, multipleAmounts, keyPairs, attr, mnemonic, signatureId, xpub, paymentId, senderNote, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send Bitcoin from Tatum account to address<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Bitcoin from Tatum account to address. This will create Tatum internal withdrawal request with ID. If every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If Bitcoin server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// There are two possibilites how the transaction on the blockchain can be created:
        /// - Using mnemonic - all of the addresses, that are generated from the mnemonic are scanned for the incoming deposits which are used as a source of the transaction.Assets, which are not used in a transaction are moved to the system address wih the derivation index 0. Address with index 0 cannot be assigned automatically to any account and is used for custodial wallet use cases. For non-custodial wallets, field attr should be present and it should be address with the index 1 of the connected wallet.
        /// - Using keyPair - addresses which are used as a source of the transaction are entered manually
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This method is a helper method, which internally wraps these steps:
        /// 1. Store withdrawal - create a ledger transaction, which debits the assets on the sender account.
        /// 2. Perform blockchain transaction -
        /// 3. Complete withdrawal - move the withdrawal to the completed state, when all of the previous steps were successful.
        /// When some of the steps fails, Cancel withdrawal operation is used, which cancels withdrawal and creates refund transaction to the sender account.This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="to_address">Blockchain address to send assets to. For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain addresses as a comma separated string.</param>
        /// <param name="amount">Amount to be withdrawn to blockchain.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="fee">Fee to be submitted as a transaction fee to blockchain. If none is set, default value of 0.0005 BTC is used.</param>
        /// <param name="multipleAmounts">For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain amounts. List of recipient addresses must be present in the address field and total sum of amounts must be equal to the amount field.</param>
        /// <param name="keyPairs">Array of assigned blockchain addresses with their private keys. Either mnemonic, keyPair or signature Id must be present - depends on the type of account and xpub. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="attr">Used to parametrize withdrawal as a change address for left coins from transaction. XPub or attr must be used.</param>
        /// <param name="mnemonic">Mnemonic seed - usually 12-24 words with access to whole wallet. Either mnemonic, keyPair or signature Id must be present - depends on the type of account and xpub. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="signatureId">Signature hash of the mnemonic, which will be used to sign transactions locally. All signature Ids should be present, which might be used to sign transaction. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="xpub">Extended public key (xpub) of the wallet associated with the accounts. Should be present, when mnemonic is used.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OffchainTransferResponse>> OffchainBlockchain_SendBitcoin_Async(
            string senderAccountId, string to_address, decimal amount, bool? compliant = null, decimal? fee = null,
            IEnumerable<decimal> multipleAmounts = null, IEnumerable<OffchainAddressPrivateKeyPair> keyPairs = null,
            string attr = null, string mnemonic = null, string signatureId = null,
            string xpub = null, string paymentId = null, string senderNote = null,
            CancellationToken ct = default)
            => await OffchainBlockchain_Send_Async(BlockchainType.Bitcoin, senderAccountId, to_address, amount, compliant, fee, multipleAmounts, keyPairs, attr, mnemonic, signatureId, xpub, paymentId, senderNote, ct);

        /// <summary>
        /// <b>Title:</b> Send Bitcoin Cash from Tatum account to address<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Bitcoin Cash from Tatum account to address. This will create Tatum internal withdrawal request with ID. If every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If Bitcoin Cash server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// There are two possibilites how the transaction on the blockchain can be created:
        /// - Using mnemonic - all of the addresses, that are generated from the mnemonic are scanned for the incoming deposits which are used as a source of the transaction.Assets, which are not used in a transaction are moved to the system address wih the derivation index 0. Address with index 0 cannot be assigned automatically to any account and is used for custodial wallet use cases. For non-custodial wallets, field attr should be present and it should be address with the index 1 of the connected wallet.
        /// - Using keyPair - addresses which are used as a source of the transaction are entered manually
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This method is a helper method, which internally wraps these steps:
        /// 1. Store withdrawal - create a ledger transaction, which debits the assets on the sender account.
        /// 2. Perform blockchain transaction -
        /// 3. Complete withdrawal - move the withdrawal to the completed state, when all of the previous steps were successful.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="to_address">Blockchain address to send assets to. For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain addresses as a comma separated string.</param>
        /// <param name="amount">Amount to be withdrawn to blockchain.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="fee">Fee to be submitted as a transaction fee to blockchain. If none is set, default value of 0.0005 BTC is used.</param>
        /// <param name="multipleAmounts">For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain amounts. List of recipient addresses must be present in the address field and total sum of amounts must be equal to the amount field.</param>
        /// <param name="keyPairs">Array of assigned blockchain addresses with their private keys. Either mnemonic, keyPair or signature Id must be present - depends on the type of account and xpub. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="attr">Used to parametrize withdrawal as a change address for left coins from transaction. XPub or attr must be used.</param>
        /// <param name="mnemonic">Mnemonic seed - usually 12-24 words with access to whole wallet. Either mnemonic, keyPair or signature Id must be present - depends on the type of account and xpub. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="signatureId">Signature hash of the mnemonic, which will be used to sign transactions locally. All signature Ids should be present, which might be used to sign transaction. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="xpub">Extended public key (xpub) of the wallet associated with the accounts. Should be present, when mnemonic is used.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OffchainTransferResponse> OffchainBlockchain_SendBitcoinCash(
            string senderAccountId, string to_address, decimal amount, bool? compliant = null, decimal? fee = null,
            IEnumerable<decimal> multipleAmounts = null, IEnumerable<OffchainAddressPrivateKeyPair> keyPairs = null,
            string attr = null, string mnemonic = null, string signatureId = null,
            string xpub = null, string paymentId = null, string senderNote = null,
            CancellationToken ct = default)
            => OffchainBlockchain_Send_Async(BlockchainType.BitcoinCash, senderAccountId, to_address, amount, compliant, fee, multipleAmounts, keyPairs, attr, mnemonic, signatureId, xpub, paymentId, senderNote, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send Bitcoin Cash from Tatum account to address<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Bitcoin Cash from Tatum account to address. This will create Tatum internal withdrawal request with ID. If every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If Bitcoin Cash server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// There are two possibilites how the transaction on the blockchain can be created:
        /// - Using mnemonic - all of the addresses, that are generated from the mnemonic are scanned for the incoming deposits which are used as a source of the transaction.Assets, which are not used in a transaction are moved to the system address wih the derivation index 0. Address with index 0 cannot be assigned automatically to any account and is used for custodial wallet use cases. For non-custodial wallets, field attr should be present and it should be address with the index 1 of the connected wallet.
        /// - Using keyPair - addresses which are used as a source of the transaction are entered manually
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This method is a helper method, which internally wraps these steps:
        /// 1. Store withdrawal - create a ledger transaction, which debits the assets on the sender account.
        /// 2. Perform blockchain transaction -
        /// 3. Complete withdrawal - move the withdrawal to the completed state, when all of the previous steps were successful.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="to_address">Blockchain address to send assets to. For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain addresses as a comma separated string.</param>
        /// <param name="amount">Amount to be withdrawn to blockchain.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="fee">Fee to be submitted as a transaction fee to blockchain. If none is set, default value of 0.0005 BTC is used.</param>
        /// <param name="multipleAmounts">For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain amounts. List of recipient addresses must be present in the address field and total sum of amounts must be equal to the amount field.</param>
        /// <param name="keyPairs">Array of assigned blockchain addresses with their private keys. Either mnemonic, keyPair or signature Id must be present - depends on the type of account and xpub. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="attr">Used to parametrize withdrawal as a change address for left coins from transaction. XPub or attr must be used.</param>
        /// <param name="mnemonic">Mnemonic seed - usually 12-24 words with access to whole wallet. Either mnemonic, keyPair or signature Id must be present - depends on the type of account and xpub. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="signatureId">Signature hash of the mnemonic, which will be used to sign transactions locally. All signature Ids should be present, which might be used to sign transaction. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="xpub">Extended public key (xpub) of the wallet associated with the accounts. Should be present, when mnemonic is used.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OffchainTransferResponse>> OffchainBlockchain_SendBitcoinCash_Async(
            string senderAccountId, string to_address, decimal amount, bool? compliant = null, decimal? fee = null,
            IEnumerable<decimal> multipleAmounts = null, IEnumerable<OffchainAddressPrivateKeyPair> keyPairs = null,
            string attr = null, string mnemonic = null, string signatureId = null,
            string xpub = null, string paymentId = null, string senderNote = null,
            CancellationToken ct = default)
            => await OffchainBlockchain_Send_Async(BlockchainType.BitcoinCash, senderAccountId, to_address, amount, compliant, fee, multipleAmounts, keyPairs, attr, mnemonic, signatureId, xpub, paymentId, senderNote, ct);

        /// <summary>
        /// <b>Title:</b> Send Litecoin from Tatum account to address<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Litecoin from Tatum account to address. This will create Tatum internal withdrawal request with ID. If every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If Litecoin server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// There are two possibilites how the transaction on the blockchain can be created:
        /// - Using mnemonic - all of the addresses, that are generated from the mnemonic are scanned for the incoming deposits which are used as a source of the transaction.Assets, which are not used in a transaction are moved to the system address wih the derivation index 0. Address with index 0 cannot be assigned automatically to any account and is used for custodial wallet use cases. For non-custodial wallets, field attr should be present and it should be address with the index 1 of the connected wallet.
        /// - Using keyPair - addresses which are used as a source of the transaction are entered manually
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This method is a helper method, which internally wraps these steps:
        /// 1. Store withdrawal - create a ledger transaction, which debits the assets on the sender account.
        /// 2. Perform blockchain transaction -
        /// 3. Complete withdrawal - move the withdrawal to the completed state, when all of the previous steps were successful.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="to_address">Blockchain address to send assets to. For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain addresses as a comma separated string.</param>
        /// <param name="amount">Amount to be withdrawn to blockchain.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="fee">Fee to be submitted as a transaction fee to blockchain. If none is set, default value of 0.0005 BTC is used.</param>
        /// <param name="multipleAmounts">For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain amounts. List of recipient addresses must be present in the address field and total sum of amounts must be equal to the amount field.</param>
        /// <param name="keyPairs">Array of assigned blockchain addresses with their private keys. Either mnemonic, keyPair or signature Id must be present - depends on the type of account and xpub. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="attr">Used to parametrize withdrawal as a change address for left coins from transaction. XPub or attr must be used.</param>
        /// <param name="mnemonic">Mnemonic seed - usually 12-24 words with access to whole wallet. Either mnemonic, keyPair or signature Id must be present - depends on the type of account and xpub. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="signatureId">Signature hash of the mnemonic, which will be used to sign transactions locally. All signature Ids should be present, which might be used to sign transaction. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="xpub">Extended public key (xpub) of the wallet associated with the accounts. Should be present, when mnemonic is used.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OffchainTransferResponse> OffchainBlockchain_SendLitecoin(
            string senderAccountId, string to_address, decimal amount, bool? compliant = null, decimal? fee = null,
            IEnumerable<decimal> multipleAmounts = null, IEnumerable<OffchainAddressPrivateKeyPair> keyPairs = null,
            string attr = null, string mnemonic = null, string signatureId = null,
            string xpub = null, string paymentId = null, string senderNote = null,
            CancellationToken ct = default)
            => OffchainBlockchain_Send_Async(BlockchainType.Litecoin, senderAccountId, to_address, amount, compliant, fee, multipleAmounts, keyPairs, attr, mnemonic, signatureId, xpub, paymentId, senderNote, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send Litecoin from Tatum account to address<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Litecoin from Tatum account to address. This will create Tatum internal withdrawal request with ID. If every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If Litecoin server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// There are two possibilites how the transaction on the blockchain can be created:
        /// - Using mnemonic - all of the addresses, that are generated from the mnemonic are scanned for the incoming deposits which are used as a source of the transaction.Assets, which are not used in a transaction are moved to the system address wih the derivation index 0. Address with index 0 cannot be assigned automatically to any account and is used for custodial wallet use cases. For non-custodial wallets, field attr should be present and it should be address with the index 1 of the connected wallet.
        /// - Using keyPair - addresses which are used as a source of the transaction are entered manually
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This method is a helper method, which internally wraps these steps:
        /// 1. Store withdrawal - create a ledger transaction, which debits the assets on the sender account.
        /// 2. Perform blockchain transaction -
        /// 3. Complete withdrawal - move the withdrawal to the completed state, when all of the previous steps were successful.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="to_address">Blockchain address to send assets to. For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain addresses as a comma separated string.</param>
        /// <param name="amount">Amount to be withdrawn to blockchain.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="fee">Fee to be submitted as a transaction fee to blockchain. If none is set, default value of 0.0005 BTC is used.</param>
        /// <param name="multipleAmounts">For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain amounts. List of recipient addresses must be present in the address field and total sum of amounts must be equal to the amount field.</param>
        /// <param name="keyPairs">Array of assigned blockchain addresses with their private keys. Either mnemonic, keyPair or signature Id must be present - depends on the type of account and xpub. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="attr">Used to parametrize withdrawal as a change address for left coins from transaction. XPub or attr must be used.</param>
        /// <param name="mnemonic">Mnemonic seed - usually 12-24 words with access to whole wallet. Either mnemonic, keyPair or signature Id must be present - depends on the type of account and xpub. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="signatureId">Signature hash of the mnemonic, which will be used to sign transactions locally. All signature Ids should be present, which might be used to sign transaction. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="xpub">Extended public key (xpub) of the wallet associated with the accounts. Should be present, when mnemonic is used.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OffchainTransferResponse>> OffchainBlockchain_SendLitecoin_Async(
            string senderAccountId, string to_address, decimal amount, bool? compliant = null, decimal? fee = null,
            IEnumerable<decimal> multipleAmounts = null, IEnumerable<OffchainAddressPrivateKeyPair> keyPairs = null,
            string attr = null, string mnemonic = null, string signatureId = null,
            string xpub = null, string paymentId = null, string senderNote = null,
            CancellationToken ct = default)
            => await OffchainBlockchain_Send_Async(BlockchainType.Litecoin, senderAccountId, to_address, amount, compliant, fee, multipleAmounts, keyPairs, attr, mnemonic, signatureId, xpub, paymentId, senderNote, ct);

        /// <summary>
        /// <b>Title:</b> Send Ethereum from Tatum ledger to blockchain<br />
        /// <b>Credits:</b> 4 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Ethereum from Tatum Ledger to account. This will create Tatum internal withdrawal request with ID. If every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If Ethereum server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="to_address">Blockchain address to send assets</param>
        /// <param name="amount">Amount to be sent in Ether.</param>
        /// <param name="currency">Currency to transfer from Ethereum Blockchain Account. Required only for calls from Tatum Middleware.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="privateKey">Private key of sender address. Either mnemonic and index, privateKey or signature Id must be present - depends on the type of account and xpub.</param>
        /// <param name="signatureId">Identifier of the mnemonic / private key associated in signing application. When hash identifies mnemonic, index must be present to represent specific account to pay from. Private key, mnemonic or signature Id must be present.</param>
        /// <param name="index">Derivation index of sender address.</param>
        /// <param name="mnemonic">Mnemonic to generate private key for sender address. Either mnemonic and index, privateKey or signature Id must be present - depends on the type of account and xpub.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OffchainTransferResponse> OffchainBlockchain_SendEthereum(
            string senderAccountId, string to_address, decimal amount, string currency = null, long? nonce = null,
            bool? compliant = null, string privateKey = null, string signatureId = null, int? index = null, string mnemonic = null,
            string paymentId = null, string senderNote = null,
            CancellationToken ct = default)
            => OffchainBlockchain_SendEthereum_Async(
            senderAccountId, to_address, amount, currency, nonce,
            compliant, privateKey, signatureId, index, mnemonic,
            paymentId, senderNote, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send Ethereum from Tatum ledger to blockchain<br />
        /// <b>Credits:</b> 4 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Ethereum from Tatum Ledger to account. This will create Tatum internal withdrawal request with ID. If every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If Ethereum server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="to_address">Blockchain address to send assets</param>
        /// <param name="amount">Amount to be sent in Ether.</param>
        /// <param name="currency">Currency to transfer from Ethereum Blockchain Account. Required only for calls from Tatum Middleware.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="privateKey">Private key of sender address. Either mnemonic and index, privateKey or signature Id must be present - depends on the type of account and xpub.</param>
        /// <param name="signatureId">Identifier of the mnemonic / private key associated in signing application. When hash identifies mnemonic, index must be present to represent specific account to pay from. Private key, mnemonic or signature Id must be present.</param>
        /// <param name="index">Derivation index of sender address.</param>
        /// <param name="mnemonic">Mnemonic to generate private key for sender address. Either mnemonic and index, privateKey or signature Id must be present - depends on the type of account and xpub.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OffchainTransferResponse>> OffchainBlockchain_SendEthereum_Async(
            string senderAccountId, string to_address, decimal amount, string currency = null, long? nonce = null,
            bool? compliant = null, string privateKey = null, string signatureId = null, int? index = null, string mnemonic = null,
            string paymentId = null, string senderNote = null,
            CancellationToken ct = default)
        {
            var credits = 4;
            var ci = CultureInfo.InvariantCulture;
            var parameters = new Dictionary<string, object>
            {
                { "senderAccountId", senderAccountId },
                { "address", to_address },
                { "amount", amount.ToString(ci) },
            };
            parameters.AddOptionalParameter("currency", currency);
            parameters.AddOptionalParameter("nonce", nonce);
            parameters.AddOptionalParameter("compliant", compliant);
            parameters.AddOptionalParameter("privateKey", privateKey);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("index", index);
            parameters.AddOptionalParameter("mnemonic", mnemonic);
            parameters.AddOptionalParameter("paymentId", paymentId);
            parameters.AddOptionalParameter("senderNote", senderNote);

            var url = GetUrl(string.Format(Endpoints_Offchain_Blockchain_EthereumTransfer));
            var result = await SendTatumRequest<OffchainTransferResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success/* || !result.Data.Completed*/) return WebCallResult<OffchainTransferResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<OffchainTransferResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Create new ERC20 token<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// First step to create new ERC20 token with given supply on Ethereum blockchain with support of Tatum's private ledger.
        /// This method only creates Tatum Private ledger virtual currency with predefined parameters.It will not generate any blockchain smart contract.
        /// The whole supply of ERC20 token is stored in the customer's newly created account. Then it is possible to create new Tatum accounts with ERC20 token name as account's currency.
        /// Newly created account is frozen until the specific ERC20 smart contract address is linked with the Tatum virtual currency, representing the token.
        /// Order of the steps to create ERC20 smart contract with Tatum private ledger support:
        /// 1. Create ERC20 token - creates a virtual currency within Tatum
        /// 2. Deploy ERC20 smart contract - create new ERC20 smart contract on the blockchain
        /// 3. Store ERC20 smart contract address - link newly created ERC20 smart contract address with Tatum virtual currency - this operation enables frozen account and enables offchain synchronization for ERC20 Tatum accounts
        /// There is a helper method Deploy Ethereum ERC20 Smart Contract Off-chain, which wraps first 2 steps into 1 method.
        /// Address on the blockchain, where all initial supply will be transferred, can be defined via the address or xpub and derivationIndex.When xpub is present, the account connected to this virtualCurrency will be set as the account's xpub.
        /// </summary>
        /// <param name="symbol">ERC20 token name. Used as a identifier within Tatum system and also in blockchain as a currency symbol.</param>
        /// <param name="supply">Supply of ERC20 token.</param>
        /// <param name="description">Used as a description within Tatum system and in blockchain as a currency name.</param>
        /// <param name="basePair">Base pair for ERC20 token. Transaction value will be calculated according to this base pair.</param>
        /// <param name="customer">If customer is filled then is created or updated.</param>
        /// <param name="accountingCurrency">All transaction will be billed in this currency for created account associated with this currency. If not set, EUR is used. ISO-4217</param>
        /// <param name="derivationIndex">Derivation index for xpub to generate specific deposit address.</param>
        /// <param name="xpub">Extended public key (xpub), from which address, where all initial supply will be stored, will be generated. Either xpub and derivationIndex, or address must be present, not both.</param>
        /// <param name="address">Address on Ethereum blockchain, where all initial supply will be stored. Either xpub and derivationIndex, or address must be present, not both.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OffchainAccountIdAddressPair> OffchainBlockchain_CreateERC20Token(
            string symbol, string supply, string description, string basePair,
            LedgerCustomerOptions customer = null, string accountingCurrency = null, int? derivationIndex = null,
            string xpub = null, string address = null,
            CancellationToken ct = default)
            => OffchainBlockchain_CreateERC20Token_Async(
            symbol, supply, description, basePair,
            customer, accountingCurrency, derivationIndex,
            xpub, address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create new ERC20 token<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// First step to create new ERC20 token with given supply on Ethereum blockchain with support of Tatum's private ledger.
        /// This method only creates Tatum Private ledger virtual currency with predefined parameters.It will not generate any blockchain smart contract.
        /// The whole supply of ERC20 token is stored in the customer's newly created account. Then it is possible to create new Tatum accounts with ERC20 token name as account's currency.
        /// Newly created account is frozen until the specific ERC20 smart contract address is linked with the Tatum virtual currency, representing the token.
        /// Order of the steps to create ERC20 smart contract with Tatum private ledger support:
        /// 1. Create ERC20 token - creates a virtual currency within Tatum
        /// 2. Deploy ERC20 smart contract - create new ERC20 smart contract on the blockchain
        /// 3. Store ERC20 smart contract address - link newly created ERC20 smart contract address with Tatum virtual currency - this operation enables frozen account and enables offchain synchronization for ERC20 Tatum accounts
        /// There is a helper method Deploy Ethereum ERC20 Smart Contract Off-chain, which wraps first 2 steps into 1 method.
        /// Address on the blockchain, where all initial supply will be transferred, can be defined via the address or xpub and derivationIndex.When xpub is present, the account connected to this virtualCurrency will be set as the account's xpub.
        /// </summary>
        /// <param name="symbol">ERC20 token name. Used as a identifier within Tatum system and also in blockchain as a currency symbol.</param>
        /// <param name="supply">Supply of ERC20 token.</param>
        /// <param name="description">Used as a description within Tatum system and in blockchain as a currency name.</param>
        /// <param name="basePair">Base pair for ERC20 token. Transaction value will be calculated according to this base pair.</param>
        /// <param name="customer">If customer is filled then is created or updated.</param>
        /// <param name="accountingCurrency">All transaction will be billed in this currency for created account associated with this currency. If not set, EUR is used. ISO-4217</param>
        /// <param name="derivationIndex">Derivation index for xpub to generate specific deposit address.</param>
        /// <param name="xpub">Extended public key (xpub), from which address, where all initial supply will be stored, will be generated. Either xpub and derivationIndex, or address must be present, not both.</param>
        /// <param name="address">Address on Ethereum blockchain, where all initial supply will be stored. Either xpub and derivationIndex, or address must be present, not both.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OffchainAccountIdAddressPair>> OffchainBlockchain_CreateERC20Token_Async(
            string symbol, string supply, string description, string basePair,
            LedgerCustomerOptions customer = null, string accountingCurrency = null, int? derivationIndex = null,
            string xpub = null, string address = null,
            CancellationToken ct = default)
        {
            var credits = 2;
            var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "supply", supply },
                { "description", description },
                { "basePair", basePair },
            };
            parameters.AddOptionalParameter("customer", customer);
            parameters.AddOptionalParameter("accountingCurrency", accountingCurrency);
            parameters.AddOptionalParameter("derivationIndex", derivationIndex);
            parameters.AddOptionalParameter("xpub", xpub);
            parameters.AddOptionalParameter("address", address);

            var url = GetUrl(string.Format(Endpoints_Offchain_Blockchain_CreateERC20Token));
            var result = await SendTatumRequest<OffchainAccountIdAddressPair>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success/* || !result.Data.Completed*/) return WebCallResult<OffchainAccountIdAddressPair>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<OffchainAccountIdAddressPair>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Deploy Ethereum ERC20 Smart Contract Off-chain<br />
        /// <b>Credits:</b> 4 credits per API call.<br />
        /// <b>Description:</b>
        /// Deploy Ethereum ERC20 Smart Contract. This is a helper method, which is combination of Create new ERC20 token and Deploy blockchain ERC20.
        /// After deploying a contract to blockchain, the contract address will become available and must be stored within Tatum.Otherwise, it will not be possible to interact with it and starts automatic blockchain synchronization.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="symbol">Name of the ERC20 token - stored as a symbol on Blockchain</param>
        /// <param name="supply">max supply of ERC20 token.</param>
        /// <param name="description">Description of the ERC20 token</param>
        /// <param name="basePair">Base pair for ERC20 token. 1 token will be equal to 1 unit of base pair. Transaction value will be calculated according to this base pair.</param>
        /// <param name="customer">If customer is filled then is created or updated.</param>
        /// <param name="xpub">Extended public key (xpub), from which address, where all initial supply will be stored, will be generated. Either xpub and derivationIndex, or address must be present, not both.</param>
        /// <param name="derivationIndex">Derivation index for xpub to generate specific deposit address.</param>
        /// <param name="address">Address on Ethereum blockchain, where all initial supply will be stored. Either xpub and derivationIndex, or address must be present, not both.</param>
        /// <param name="mnemonic">Mnemonic to generate private key for the deploy account of ERC20, from which the gas will be paid (index will be used). If address is not present, mnemonic is used to generate xpub and index is set to 1. Either mnemonic and index or privateKey and address must be present, not both.</param>
        /// <param name="index">derivation index of address to pay for deployment of ERC20</param>
        /// <param name="privateKey">Private key of Ethereum account address, from which gas for deployment of ERC20 will be paid. Private key, mnemonic or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the mnemonic / private key associated in signing application. When hash identifies mnemonic, index must be present to represent specific account to pay from. Private key, mnemonic or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OffchainAccountIdTxIdPair> OffchainBlockchain_DeployERC20Token(
            string symbol, string supply, string description, string basePair,
            LedgerCustomerOptions customer = null, string xpub = null, int? derivationIndex = null, string address = null,
            string mnemonic = null, int? index = null, string privateKey = null, string signatureId = null, long? nonce = null,
            CancellationToken ct = default)
            => OffchainBlockchain_DeployERC20Token_Async(
            symbol, supply, description, basePair,
            customer, xpub, derivationIndex, address,
            mnemonic, index, privateKey, signatureId, nonce, ct).Result;
        /// <summary>
        /// <b>Title:</b> Deploy Ethereum ERC20 Smart Contract Off-chain<br />
        /// <b>Credits:</b> 4 credits per API call.<br />
        /// <b>Description:</b>
        /// Deploy Ethereum ERC20 Smart Contract. This is a helper method, which is combination of Create new ERC20 token and Deploy blockchain ERC20.
        /// After deploying a contract to blockchain, the contract address will become available and must be stored within Tatum.Otherwise, it will not be possible to interact with it and starts automatic blockchain synchronization.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="symbol">Name of the ERC20 token - stored as a symbol on Blockchain</param>
        /// <param name="supply">max supply of ERC20 token.</param>
        /// <param name="description">Description of the ERC20 token</param>
        /// <param name="basePair">Base pair for ERC20 token. 1 token will be equal to 1 unit of base pair. Transaction value will be calculated according to this base pair.</param>
        /// <param name="customer">If customer is filled then is created or updated.</param>
        /// <param name="xpub">Extended public key (xpub), from which address, where all initial supply will be stored, will be generated. Either xpub and derivationIndex, or address must be present, not both.</param>
        /// <param name="derivationIndex">Derivation index for xpub to generate specific deposit address.</param>
        /// <param name="address">Address on Ethereum blockchain, where all initial supply will be stored. Either xpub and derivationIndex, or address must be present, not both.</param>
        /// <param name="mnemonic">Mnemonic to generate private key for the deploy account of ERC20, from which the gas will be paid (index will be used). If address is not present, mnemonic is used to generate xpub and index is set to 1. Either mnemonic and index or privateKey and address must be present, not both.</param>
        /// <param name="index">derivation index of address to pay for deployment of ERC20</param>
        /// <param name="privateKey">Private key of Ethereum account address, from which gas for deployment of ERC20 will be paid. Private key, mnemonic or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the mnemonic / private key associated in signing application. When hash identifies mnemonic, index must be present to represent specific account to pay from. Private key, mnemonic or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OffchainAccountIdTxIdPair>> OffchainBlockchain_DeployERC20Token_Async(
            string symbol, string supply, string description, string basePair,
            LedgerCustomerOptions customer = null, string xpub = null, int? derivationIndex = null, string address = null,
            string mnemonic = null, int? index = null, string privateKey = null, string signatureId = null, long? nonce = null,
            CancellationToken ct = default)
        {
            var credits = 2;
            var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "supply", supply },
                { "description", description },
                { "basePair", basePair },
            };
            parameters.AddOptionalParameter("customer", customer);
            parameters.AddOptionalParameter("xpub", xpub);
            parameters.AddOptionalParameter("derivationIndex", derivationIndex);
            parameters.AddOptionalParameter("address", address);
            parameters.AddOptionalParameter("mnemonic", mnemonic);
            parameters.AddOptionalParameter("index", index);
            parameters.AddOptionalParameter("privateKey", privateKey);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("nonce", nonce);

            var url = GetUrl(string.Format(Endpoints_Offchain_Blockchain_DeployERC20Token));
            var result = await SendTatumRequest<OffchainAccountIdTxIdPair>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success/* || !result.Data.Completed*/) return WebCallResult<OffchainAccountIdTxIdPair>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<OffchainAccountIdTxIdPair>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Set ERC20 token contract address<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Set contract address of ERC20 token. This must be done in order to communicate with ERC20 smart contract. After creating and deploying ERC20 token to Ethereum blockchain, smart contract address is generated and must be set within Tatum. Otherwise Tatum platform will not be able to detect incoming deposits of ERC20 and do withdrawals from Tatum accounts to other blockchain addresses.
        /// </summary>
        /// <param name="address">ERC20 contract address</param>
        /// <param name="symbol">ERC20 symbol name.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> OffchainBlockchain_SetERC20TokenContractAddress(string address, string symbol, CancellationToken ct = default) => OffchainBlockchain_SetERC20TokenContractAddress_Async(address, symbol, ct).Result;
        /// <summary>
        /// <b>Title:</b> Set ERC20 token contract address<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Set contract address of ERC20 token. This must be done in order to communicate with ERC20 smart contract. After creating and deploying ERC20 token to Ethereum blockchain, smart contract address is generated and must be set within Tatum. Otherwise Tatum platform will not be able to detect incoming deposits of ERC20 and do withdrawals from Tatum accounts to other blockchain addresses.
        /// </summary>
        /// <param name="address">ERC20 contract address</param>
        /// <param name="symbol">ERC20 symbol name.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> OffchainBlockchain_SetERC20TokenContractAddress_Async(string address, string symbol, CancellationToken ct = default)
        {
            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Offchain_Blockchain_SetERC20TokenContractAddress, symbol, address));
            var result = await SendTatumRequest<string>(url, HttpMethod.Post, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Transfer Ethereum ERC20 from Tatum ledger to blockchain<br />
        /// <b>Credits:</b> 4 credits per API call.<br />
        /// <b>Description:</b>
        /// Transfer Ethereum ERC20 Smart Contract Tokens from Tatum account to blockchain address. This will create Tatum internal withdrawal request with ID. If every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If Ethereum server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="to_address">Blockchain address to send ERC20 token to</param>
        /// <param name="amount">Amount to be sent.</param>
        /// <param name="currency">ERC20 symbol. Required only for calls from Tatum Middleware.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="privateKey">Private key of sender address. Either mnemonic and index, privateKey or signature Id must be present - depends on the type of account and xpub.</param>
        /// <param name="signatureId">Identifier of the mnemonic / private key associated in signing application. When hash identifies mnemonic, index must be present to represent specific account to pay from. Private key, mnemonic or signature Id must be present.</param>
        /// <param name="index">Derivation index of sender address.</param>
        /// <param name="mnemonic">Mnemonic to generate private key for sender address. Either mnemonic and index, or privateKey must be present - depends on the type of account and xpub.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OffchainTransferResponse> OffchainBlockchain_SendERC20Token(
            string senderAccountId, string to_address, decimal amount, string currency = null, long? nonce = null,
            bool? compliant = null, string privateKey = null, string signatureId = null, int? index = null, string mnemonic = null,
            string paymentId = null, string senderNote = null,
            CancellationToken ct = default)
            => OffchainBlockchain_SendERC20Token_Async(
            senderAccountId, to_address, amount, currency, nonce,
            compliant, privateKey, signatureId, index, mnemonic,
            paymentId, senderNote, ct).Result;
        /// <summary>
        /// <b>Title:</b> Transfer Ethereum ERC20 from Tatum ledger to blockchain<br />
        /// <b>Credits:</b> 4 credits per API call.<br />
        /// <b>Description:</b>
        /// Transfer Ethereum ERC20 Smart Contract Tokens from Tatum account to blockchain address. This will create Tatum internal withdrawal request with ID. If every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If Ethereum server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="to_address">Blockchain address to send ERC20 token to</param>
        /// <param name="amount">Amount to be sent.</param>
        /// <param name="currency">ERC20 symbol. Required only for calls from Tatum Middleware.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="privateKey">Private key of sender address. Either mnemonic and index, privateKey or signature Id must be present - depends on the type of account and xpub.</param>
        /// <param name="signatureId">Identifier of the mnemonic / private key associated in signing application. When hash identifies mnemonic, index must be present to represent specific account to pay from. Private key, mnemonic or signature Id must be present.</param>
        /// <param name="index">Derivation index of sender address.</param>
        /// <param name="mnemonic">Mnemonic to generate private key for sender address. Either mnemonic and index, or privateKey must be present - depends on the type of account and xpub.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OffchainTransferResponse>> OffchainBlockchain_SendERC20Token_Async(
            string senderAccountId, string to_address, decimal amount, string currency = null, long? nonce = null,
            bool? compliant = null, string privateKey = null, string signatureId = null, int? index = null, string mnemonic = null,
            string paymentId = null, string senderNote = null,
            CancellationToken ct = default)
        {
            var credits = 4;
            var ci = CultureInfo.InvariantCulture;
            var parameters = new Dictionary<string, object>
            {
                { "senderAccountId", senderAccountId },
                { "address", to_address },
                { "amount", amount.ToString(ci) },
            };
            parameters.AddOptionalParameter("currency", currency);
            parameters.AddOptionalParameter("nonce", nonce);
            parameters.AddOptionalParameter("compliant", compliant);
            parameters.AddOptionalParameter("privateKey", privateKey);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("index", index);
            parameters.AddOptionalParameter("mnemonic", mnemonic);
            parameters.AddOptionalParameter("paymentId", paymentId);
            parameters.AddOptionalParameter("senderNote", senderNote);

            var url = GetUrl(string.Format(Endpoints_Offchain_Blockchain_TransferERC20Token));
            var result = await SendTatumRequest<OffchainTransferResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success/* || !result.Data.Completed*/) return WebCallResult<OffchainTransferResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<OffchainTransferResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Send XLM / Asset from Tatum ledger to blockchain<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send XLM or XLM-based Assets from account to account. This will create Tatum internal withdrawal request with ID. When every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If XLM server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="fromAccount">Blockchain account to send from</param>
        /// <param name="address">Blockchain address to send assets</param>
        /// <param name="amount">Amount to be sent, in XLM or XLM-based Asset.</param>
        /// <param name="secret">Secret for account. Secret, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the secret associated in signing application. Secret, or signature Id must be present.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="attr">Short message to recipient. Usually used as an account discriminator. It can be either 28 characters long ASCII text, 64 characters long HEX string or uint64 number. When using as an account disciminator in Tatum Offchain ledger, can be in format of destination_acc|source_acc.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account.</param>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets. Required only for calls from Tatum Middleware.</param>
        /// <param name="token">Asset name. Required only for calls from Tatum Middleware.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OffchainTransferResponse> OffchainBlockchain_SendStellar(
            string senderAccountId, string fromAccount, string address, decimal amount,
            string secret = null, string signatureId = null, bool? compliant = null, string attr = null, string paymentId = null,
            string senderNote = null, string issuerAccount = null, string token = null,
            CancellationToken ct = default)
            => OffchainBlockchain_SendStellar_Async(
            senderAccountId, fromAccount, address, amount,
            secret, signatureId, compliant, attr, paymentId,
            senderNote, issuerAccount, token, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send XLM / Asset from Tatum ledger to blockchain<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send XLM or XLM-based Assets from account to account. This will create Tatum internal withdrawal request with ID. When every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If XLM server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="fromAccount">Blockchain account to send from</param>
        /// <param name="address">Blockchain address to send assets</param>
        /// <param name="amount">Amount to be sent, in XLM or XLM-based Asset.</param>
        /// <param name="secret">Secret for account. Secret, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the secret associated in signing application. Secret, or signature Id must be present.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="attr">Short message to recipient. Usually used as an account discriminator. It can be either 28 characters long ASCII text, 64 characters long HEX string or uint64 number. When using as an account disciminator in Tatum Offchain ledger, can be in format of destination_acc|source_acc.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account.</param>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets. Required only for calls from Tatum Middleware.</param>
        /// <param name="token">Asset name. Required only for calls from Tatum Middleware.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OffchainTransferResponse>> OffchainBlockchain_SendStellar_Async(
            string senderAccountId, string fromAccount, string address, decimal amount,
            string secret = null, string signatureId = null, bool? compliant = null, string attr = null, string paymentId = null,
            string senderNote = null, string issuerAccount = null, string token = null,
            CancellationToken ct = default)
        {
            var credits = 10;
            var ci = CultureInfo.InvariantCulture;
            var parameters = new Dictionary<string, object>
            {
                { "senderAccountId", senderAccountId },
                { "fromAccount", fromAccount },
                { "address", address },
                { "amount", amount.ToString(ci) },
            };
            parameters.AddOptionalParameter("secret", secret);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("compliant", compliant);
            parameters.AddOptionalParameter("attr", attr);
            parameters.AddOptionalParameter("paymentId", paymentId);
            parameters.AddOptionalParameter("senderNote", senderNote);
            parameters.AddOptionalParameter("issuerAccount", issuerAccount);
            parameters.AddOptionalParameter("token", token);

            var url = GetUrl(string.Format(Endpoints_Offchain_Blockchain_StellarTransfer));
            var result = await SendTatumRequest<OffchainTransferResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success/* || !result.Data.Completed*/) return WebCallResult<OffchainTransferResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<OffchainTransferResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Create XLM based Asset<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create XLM-based Asset in Tatum Ledger. Asset must be created and configured on XLM blockhain before using Create trust line. This API call will create Tatum internal Virtual Currency. It is possible to create Tatum ledger accounts with off-chain support.
        /// </summary>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets.</param>
        /// <param name="token">Asset name.</param>
        /// <param name="basePair">Base pair for Asset. Transaction value will be calculated according to this base pair. e.g. 1 TOKEN123 is equal to 1 EUR, if basePair is set to EUR.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> OffchainBlockchain_CreateXLMAsset(string issuerAccount, string token, string basePair, CancellationToken ct = default) => OffchainBlockchain_CreateXLMAsset_Async(issuerAccount, token, basePair, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create XLM based Asset<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create XLM-based Asset in Tatum Ledger. Asset must be created and configured on XLM blockhain before using Create trust line. This API call will create Tatum internal Virtual Currency. It is possible to create Tatum ledger accounts with off-chain support.
        /// </summary>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets.</param>
        /// <param name="token">Asset name.</param>
        /// <param name="basePair">Base pair for Asset. Transaction value will be calculated according to this base pair. e.g. 1 TOKEN123 is equal to 1 EUR, if basePair is set to EUR.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> OffchainBlockchain_CreateXLMAsset_Async(string issuerAccount, string token, string basePair, CancellationToken ct = default)
        {
            var credits = 2;
            var parameters = new Dictionary<string, object>
            {
                { "issuerAccount", issuerAccount },
                { "token", token },
                { "basePair", basePair },
            };

            var url = GetUrl(string.Format(Endpoints_Offchain_Blockchain_CreateXLMAsset));
            var result = await SendTatumRequest<string>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Send XRP from Tatum ledger to blockchain<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send XRP from account to account. This will create Tatum internal withdrawal request with ID. When every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If XRP server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="account">XRP account address. Must be the one used for generating deposit tags.</param>
        /// <param name="address">Blockchain address to send assets</param>
        /// <param name="amount">Amount to be sent, in XRP.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="attr">Destination tag of the recipient account, if any. Must be stringified uint32.</param>
        /// <param name="sourceTag">Source tag of sender account, if any.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="secret">Secret for account. Secret, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the secret associated in signing application. Secret, or signature Id must be present.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account.</param>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets to create trust line for. Required only for calls from Tatum Middleware.</param>
        /// <param name="token">Asset name. Must be 160bit HEX string, e.g. SHA1. Required only for calls from Tatum Middleware.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OffchainTransferResponse> OffchainBlockchain_SendRipple(
            string senderAccountId, string account, string address, decimal amount,
            bool? compliant = null, string attr = null, int? sourceTag = null, string paymentId = null, string secret = null,
            string signatureId = null, string senderNote = null, string issuerAccount = null, string token = null,
            CancellationToken ct = default)
            => OffchainBlockchain_SendRipple_Async(
            senderAccountId, account, address, amount,
            compliant, attr, sourceTag, paymentId, secret,
            signatureId, senderNote, issuerAccount, token, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send XRP from Tatum ledger to blockchain<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send XRP from account to account. This will create Tatum internal withdrawal request with ID. When every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If XRP server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="account">XRP account address. Must be the one used for generating deposit tags.</param>
        /// <param name="address">Blockchain address to send assets</param>
        /// <param name="amount">Amount to be sent, in XRP.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="attr">Destination tag of the recipient account, if any. Must be stringified uint32.</param>
        /// <param name="sourceTag">Source tag of sender account, if any.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="secret">Secret for account. Secret, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the secret associated in signing application. Secret, or signature Id must be present.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account.</param>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets to create trust line for. Required only for calls from Tatum Middleware.</param>
        /// <param name="token">Asset name. Must be 160bit HEX string, e.g. SHA1. Required only for calls from Tatum Middleware.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OffchainTransferResponse>> OffchainBlockchain_SendRipple_Async(
            string senderAccountId, string account, string address, decimal amount,
            bool? compliant = null, string attr = null, int? sourceTag = null, string paymentId = null, string secret = null,
            string signatureId = null, string senderNote = null, string issuerAccount = null, string token = null,
            CancellationToken ct = default)
        {
            var credits = 10;
            var ci = CultureInfo.InvariantCulture;
            var parameters = new Dictionary<string, object>
            {
                { "senderAccountId", senderAccountId },
                { "account", account },
                { "address", address },
                { "amount", amount.ToString(ci) },
            };
            parameters.AddOptionalParameter("compliant", compliant);
            parameters.AddOptionalParameter("attr", attr);
            parameters.AddOptionalParameter("sourceTag", sourceTag);
            parameters.AddOptionalParameter("paymentId", paymentId);
            parameters.AddOptionalParameter("secret", secret);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("senderNote", senderNote);
            parameters.AddOptionalParameter("issuerAccount", issuerAccount);
            parameters.AddOptionalParameter("token", token);

            var url = GetUrl(string.Format(Endpoints_Offchain_Blockchain_RippleTransfer));
            var result = await SendTatumRequest<OffchainTransferResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success/* || !result.Data.Completed*/) return WebCallResult<OffchainTransferResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<OffchainTransferResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Create XRP based Asset<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create XRP-based Asset in Tatum Ledger. Asset must be created and configured on XRP blockhain before using Create trust line. This API call will create Tatum internal Virtual Currency. It is possible to create Tatum ledger accounts with off-chain support.
        /// </summary>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets.</param>
        /// <param name="token">Asset name.</param>
        /// <param name="basePair">Base pair for Asset. Transaction value will be calculated according to this base pair. e.g. 1 TOKEN123 is equal to 1 EUR, if basePair is set to EUR.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> OffchainBlockchain_CreateXRPAsset(string issuerAccount, string token, string basePair, CancellationToken ct = default) => OffchainBlockchain_CreateXRPAsset_Async(issuerAccount, token, basePair, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create XRP based Asset<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create XRP-based Asset in Tatum Ledger. Asset must be created and configured on XRP blockhain before using Create trust line. This API call will create Tatum internal Virtual Currency. It is possible to create Tatum ledger accounts with off-chain support.
        /// </summary>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets.</param>
        /// <param name="token">Asset name.</param>
        /// <param name="basePair">Base pair for Asset. Transaction value will be calculated according to this base pair. e.g. 1 TOKEN123 is equal to 1 EUR, if basePair is set to EUR.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> OffchainBlockchain_CreateXRPAsset_Async(string issuerAccount, string token, string basePair, CancellationToken ct = default)
        {
            var credits = 2;
            var parameters = new Dictionary<string, object>
            {
                { "issuerAccount", issuerAccount },
                { "token", token },
                { "basePair", basePair },
            };

            var url = GetUrl(string.Format(Endpoints_Offchain_Blockchain_CreateXRPAsset));
            var result = await SendTatumRequest<string>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Send BNB from Tatum ledger to blockchain<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send BNB or BNB Asset from account to account. This will create Tatum internal withdrawal request with ID. When every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If BNB server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="address">Blockchain address to send assets</param>
        /// <param name="amount">Amount to be sent, in BNB.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="attr">Memo of the recipient account, if any.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="privateKey">Private key of sender address.</param>
        /// <param name="signatureId">Identifier of the secret associated in signing application. Secret, or signature Id must be present.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OffchainTransferResponse> OffchainBlockchain_SendBNB(
            string senderAccountId, string address, decimal amount,
            bool? compliant = null, string attr = null, string paymentId = null,
            string privateKey = null, string signatureId = null, string senderNote = null,
            CancellationToken ct = default)
            => OffchainBlockchain_SendBNB_Async(
            senderAccountId, address, amount,
            compliant, attr, paymentId,
            privateKey, signatureId, senderNote, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send BNB from Tatum ledger to blockchain<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send BNB or BNB Asset from account to account. This will create Tatum internal withdrawal request with ID. When every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If BNB server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="address">Blockchain address to send assets</param>
        /// <param name="amount">Amount to be sent, in BNB.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="attr">Memo of the recipient account, if any.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="privateKey">Private key of sender address.</param>
        /// <param name="signatureId">Identifier of the secret associated in signing application. Secret, or signature Id must be present.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OffchainTransferResponse>> OffchainBlockchain_SendBNB_Async(
            string senderAccountId, string address, decimal amount,
            bool? compliant = null, string attr = null, string paymentId = null,
            string privateKey = null, string signatureId = null, string senderNote = null,
            CancellationToken ct = default)
        {
            var credits = 10;
            var ci = CultureInfo.InvariantCulture;
            var parameters = new Dictionary<string, object>
            {
                { "senderAccountId", senderAccountId },
                { "address", address },
                { "amount", amount.ToString(ci) },
            };
            parameters.AddOptionalParameter("compliant", compliant);
            parameters.AddOptionalParameter("attr", attr);
            parameters.AddOptionalParameter("paymentId", paymentId);
            parameters.AddOptionalParameter("privateKey", privateKey);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("senderNote", senderNote);

            var url = GetUrl(string.Format(Endpoints_Offchain_Blockchain_BinanceTransfer));
            var result = await SendTatumRequest<OffchainTransferResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success/* || !result.Data.Completed*/) return WebCallResult<OffchainTransferResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<OffchainTransferResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Create BNB based Asset<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create BNB-based Asset in Tatum Ledger. Asset must be created and configured on Binance blockhain before. Please see Create Asset. This API call will create Tatum internal Virtual Currency. It is possible to create Tatum ledger accounts with off-chain support.
        /// </summary>
        /// <param name="token">Asset name.</param>
        /// <param name="basePair">Base pair for Asset. Transaction value will be calculated according to this base pair. e.g. 1 TOKEN123 is equal to 1 EUR, if basePair is set to EUR.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> OffchainBlockchain_CreateBNBAsset(string token, string basePair, CancellationToken ct = default) => OffchainBlockchain_CreateBNBAsset_Async(token, basePair, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create BNB based Asset<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create BNB-based Asset in Tatum Ledger. Asset must be created and configured on Binance blockhain before. Please see Create Asset. This API call will create Tatum internal Virtual Currency. It is possible to create Tatum ledger accounts with off-chain support.
        /// </summary>
        /// <param name="token">Asset name.</param>
        /// <param name="basePair">Base pair for Asset. Transaction value will be calculated according to this base pair. e.g. 1 TOKEN123 is equal to 1 EUR, if basePair is set to EUR.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> OffchainBlockchain_CreateBNBAsset_Async(string token, string basePair, CancellationToken ct = default)
        {
            var credits = 2;
            var parameters = new Dictionary<string, object>
            {
                { "token", token },
                { "basePair", basePair },
            };

            var url = GetUrl(string.Format(Endpoints_Offchain_Blockchain_CreateBNBAsset));
            var result = await SendTatumRequest<string>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }
        #endregion

        #region Off-chain / Withdrawal
        /// <summary>
        /// <b>Title:</b> Store withdrawal<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create a withdrawal from Tatum Ledger account to the blockchain.<br />
        /// <b>BTC, LTC, BCH</b><br />
        /// When withdrawal from Tatum is executed, all deposits, which are not processed yet are used as an input and change is moved to pool address 0 of wallet for defined account's xpub. If there are no unspent deposits, only last pool address 0 UTXO is used. If balance of wallet is not sufficient, it is impossible to perform withdrawal from this account -> funds were transferred to another linked wallet within system or outside of Tatum visibility.<br />
        /// For the first time of withdrawal from wallet, there must be some deposit made and funds are obtained from that.Since there is no withdrawal, there was no transfer to pool address 0 and thus it is not present in vIn.Pool transfer is identified by missing data.address property in response.When last not cancelled withdrawal is not completed and thus there is no tx id of output transaction given, we cannot perform next withdrawal.<br />
        /// <b>ETH</b><br />
        /// Withdrawal from Tatum can be processed only from 1 account.In Ethereum Blockchain, each address is recognized as an account and only funds from that account can be sent in 1 transaction.Example: Account A has 0.5 ETH, Account B has 0.3 ETH.Account A is linked to Tatum Account 1, Account B is linked to Tatum Account 2. Tatum Account 1 has balance 0.7 Ethereum and Tatum Account 2 has 0.1 ETH.Withdrawal from Tatum Account 1 can be at most 0.5 ETH, even though balance in Tatum Private Ledger is 0.7 ETH.Because of this Ethereum Blockchain limitation, withdrawal request should always contain sourceAddress, from which withdrawal will be made. To get available balances for Ethereoum wallet accounts, use hint endpoint.<br />
        /// <b>XRP</b><br />
        /// XRP withdrawal can contain DestinationTag except of address, which is placed in attr parameter of withdrawal request.SourceTag of the blockchain transaction should be withdrawal ID for autocomplete purposes of withdrawals.<br />
        /// <b>XLM</b><br />
        /// XLM withdrawal can contain memo except of address, which is placed in attr parameter of withdrawal request.XLM blockchain does not have possibility to enter source account information.It is possible to create memo in format 'destination|source', which is supported way of memo in Tatum and also there is information about the sender account in the blockchain.<br />
        /// When withdrawal is created, all other withdrawals with the same currency are pending, unless the current one is marked as complete or cancelled.<br />
        /// Tatum ledger transaction is created for every withdrawal request with operation type WITHDRAWAL.The value of the transaction is the withdrawal amount + blockchain fee, which should be paid. In the situation, when there is withdrawal for ERC20, XLM, or XRP based custom assets, the fee is not included in the transaction because it is paid in different assets than the withdrawal itself.<br />
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="address">Blockchain address to send assets to. For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain addresses as a comma separated string.</param>
        /// <param name="amount">Amount to be withdrawn to blockchain.</param>
        /// <param name="attr">Used to parametrize withdrawal. Used for XRP withdrawal to define destination tag of recipient, or XLM memo of the recipient, if needed. For Bitcoin, Litecoin, Bitcoin Cash, used as a change address for left coins from transaction.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="fee">Fee to be submitted as a transaction fee to blockchain.</param>
        /// <param name="multipleAmounts">For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain amounts. List of recipient addresses must be present in the address field and total sum of amounts must be equal to the amount field.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OffchainWithdrawalResponse> OffchainWithdrawal_Request(
            string senderAccountId, string address, decimal amount, string attr = null,
            bool? compliant = null, decimal? fee = null, IEnumerable<string> multipleAmounts = null, string paymentId = null, string senderNote = null,
            CancellationToken ct = default)
            => OffchainWithdrawal_Request_Async(
            senderAccountId, address, amount, attr,
            compliant, fee, multipleAmounts, paymentId, senderNote,
            ct).Result;
        /// <summary>
        /// <b>Title:</b> Store withdrawal<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create a withdrawal from Tatum Ledger account to the blockchain.<br />
        /// <b>BTC, LTC, BCH</b><br />
        /// When withdrawal from Tatum is executed, all deposits, which are not processed yet are used as an input and change is moved to pool address 0 of wallet for defined account's xpub. If there are no unspent deposits, only last pool address 0 UTXO is used. If balance of wallet is not sufficient, it is impossible to perform withdrawal from this account -> funds were transferred to another linked wallet within system or outside of Tatum visibility.<br />
        /// For the first time of withdrawal from wallet, there must be some deposit made and funds are obtained from that.Since there is no withdrawal, there was no transfer to pool address 0 and thus it is not present in vIn.Pool transfer is identified by missing data.address property in response.When last not cancelled withdrawal is not completed and thus there is no tx id of output transaction given, we cannot perform next withdrawal.<br />
        /// <b>ETH</b><br />
        /// Withdrawal from Tatum can be processed only from 1 account.In Ethereum Blockchain, each address is recognized as an account and only funds from that account can be sent in 1 transaction.Example: Account A has 0.5 ETH, Account B has 0.3 ETH.Account A is linked to Tatum Account 1, Account B is linked to Tatum Account 2. Tatum Account 1 has balance 0.7 Ethereum and Tatum Account 2 has 0.1 ETH.Withdrawal from Tatum Account 1 can be at most 0.5 ETH, even though balance in Tatum Private Ledger is 0.7 ETH.Because of this Ethereum Blockchain limitation, withdrawal request should always contain sourceAddress, from which withdrawal will be made. To get available balances for Ethereoum wallet accounts, use hint endpoint.<br />
        /// <b>XRP</b><br />
        /// XRP withdrawal can contain DestinationTag except of address, which is placed in attr parameter of withdrawal request.SourceTag of the blockchain transaction should be withdrawal ID for autocomplete purposes of withdrawals.<br />
        /// <b>XLM</b><br />
        /// XLM withdrawal can contain memo except of address, which is placed in attr parameter of withdrawal request.XLM blockchain does not have possibility to enter source account information.It is possible to create memo in format 'destination|source', which is supported way of memo in Tatum and also there is information about the sender account in the blockchain.<br />
        /// When withdrawal is created, all other withdrawals with the same currency are pending, unless the current one is marked as complete or cancelled.<br />
        /// Tatum ledger transaction is created for every withdrawal request with operation type WITHDRAWAL.The value of the transaction is the withdrawal amount + blockchain fee, which should be paid. In the situation, when there is withdrawal for ERC20, XLM, or XRP based custom assets, the fee is not included in the transaction because it is paid in different assets than the withdrawal itself.<br />
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="address">Blockchain address to send assets to. For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain addresses as a comma separated string.</param>
        /// <param name="amount">Amount to be withdrawn to blockchain.</param>
        /// <param name="attr">Used to parametrize withdrawal. Used for XRP withdrawal to define destination tag of recipient, or XLM memo of the recipient, if needed. For Bitcoin, Litecoin, Bitcoin Cash, used as a change address for left coins from transaction.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="fee">Fee to be submitted as a transaction fee to blockchain.</param>
        /// <param name="multipleAmounts">For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain amounts. List of recipient addresses must be present in the address field and total sum of amounts must be equal to the amount field.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OffchainWithdrawalResponse>> OffchainWithdrawal_Request_Async(
            string senderAccountId, string address, decimal amount, string attr = null,
            bool? compliant = null, decimal? fee = null, IEnumerable<string> multipleAmounts = null, string paymentId = null, string senderNote = null,
            CancellationToken ct = default)
        {
            var credits = 4;
            var ci = CultureInfo.InvariantCulture;
            var parameters = new Dictionary<string, object>
            {
                { "senderAccountId", senderAccountId },
                { "address", address },
                { "amount", amount.ToString(ci) },
            };
            parameters.AddOptionalParameter("attr", attr);
            parameters.AddOptionalParameter("compliant", compliant);
            parameters.AddOptionalParameter("fee", fee?.ToString(ci));

            if (multipleAmounts != null)
            {
                var lst = new List<string>();
                foreach (var ma in multipleAmounts) lst.Add(ma.ToString(ci));
                parameters.AddOptionalParameter("multipleAmounts", lst);
            }

            parameters.AddOptionalParameter("paymentId", paymentId);
            parameters.AddOptionalParameter("senderNote", senderNote);

            var url = GetUrl(string.Format(Endpoints_Offchain_Withdrawal_Store));
            var result = await SendTatumRequest<OffchainWithdrawalResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<OffchainWithdrawalResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<OffchainWithdrawalResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Complete withdrawal<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Invoke complete withdrawal as soon as blockchain transaction ID is available. All other withdrawals for the same currency will be pending unless the last one is processed and marked as completed.
        /// </summary>
        /// <param name="id">ID of created withdrawal</param>
        /// <param name="txId">Blockchain transaction ID of created withdrawal</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> OffchainWithdrawal_CompleteRequest(string id, string txId, CancellationToken ct = default) => OffchainWithdrawal_CompleteRequest_Async(id, txId, ct).Result;
        /// <summary>
        /// <b>Title:</b> Complete withdrawal<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Invoke complete withdrawal as soon as blockchain transaction ID is available. All other withdrawals for the same currency will be pending unless the last one is processed and marked as completed.
        /// </summary>
        /// <param name="id">ID of created withdrawal</param>
        /// <param name="txId">Blockchain transaction ID of created withdrawal</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> OffchainWithdrawal_CompleteRequest_Async(string id, string txId, CancellationToken ct = default)
        {
            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Offchain_Withdrawal_Complete, id, txId));
            var result = await SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Cancel withdrawal<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// This method is helpful if you need to cancel the withdrawal if the blockchain transaction fails or is not yet processed. This does not cancel already broadcast blockchain transaction, only Tatum internal withdrawal, and the ledger transaction, that was linked to this withdrawal.
        /// By default, the transaction fee is included in the reverted transaction.There are situations, like sending ERC20 on ETH, or XLM or XRP based assets, when the fee should not be reverted, because the fee is in calculated in Ethereum and transaction was in ERC20 currency.In this situation, only the transaction amount should be reverted, not the fee.
        /// </summary>
        /// <param name="id">ID of created withdrawal</param>
        /// <param name="revert">Defines whether fee should be reverted to account balance as well as amount. Defaults to true. Revert true would be typically used when withdrawal was not broadcast to blockchain. False is used usually for Ethereum based currencies when gas was consumed but transaction was reverted.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> OffchainWithdrawal_Cancel(string id, bool revert = true, CancellationToken ct = default) => OffchainWithdrawal_Cancel_Async(id, revert, ct).Result;
        /// <summary>
        /// <b>Title:</b> Cancel withdrawal<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// This method is helpful if you need to cancel the withdrawal if the blockchain transaction fails or is not yet processed. This does not cancel already broadcast blockchain transaction, only Tatum internal withdrawal, and the ledger transaction, that was linked to this withdrawal.
        /// By default, the transaction fee is included in the reverted transaction.There are situations, like sending ERC20 on ETH, or XLM or XRP based assets, when the fee should not be reverted, because the fee is in calculated in Ethereum and transaction was in ERC20 currency.In this situation, only the transaction amount should be reverted, not the fee.
        /// </summary>
        /// <param name="id">ID of created withdrawal</param>
        /// <param name="revert">Defines whether fee should be reverted to account balance as well as amount. Defaults to true. Revert true would be typically used when withdrawal was not broadcast to blockchain. False is used usually for Ethereum based currencies when gas was consumed but transaction was reverted.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> OffchainWithdrawal_Cancel_Async(string id, bool revert = true, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Offchain_Withdrawal_Cancel, id));
            var result = await SendTatumRequest<string>(url, HttpMethod.Delete, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Broadcast signed transaction and complete withdrawal<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed raw transaction end complete withdrawal associated with it. When broadcast succeeded but it is impossible to complete withdrawal, transaction id of transaction is returned and withdrawal must be completed manually.
        /// </summary>
        /// <param name="currency">Currency of signed transaction to be broadcast, BTC, LTC, BCH, ETH, XRP, ERC20</param>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="withdrawalId">Withdrawal ID to be completed by transaction broadcast</param>
        /// <param name="signatureId">ID of prepared payment template to sign. This is should be stored on a client side to retrieve ID of the blockchain transaction, when signing application signs the transaction and broadcasts it to the blockchain.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OffchainResponse> OffchainWithdrawal_Broadcast(string currency, string txData, string withdrawalId, string signatureId, CancellationToken ct = default) => OffchainWithdrawal_Broadcast_Async(currency, txData, withdrawalId, signatureId, ct).Result;
        /// <summary>
        /// <b>Title:</b> Broadcast signed transaction and complete withdrawal<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed raw transaction end complete withdrawal associated with it. When broadcast succeeded but it is impossible to complete withdrawal, transaction id of transaction is returned and withdrawal must be completed manually.
        /// </summary>
        /// <param name="currency">Currency of signed transaction to be broadcast, BTC, LTC, BCH, ETH, XRP, ERC20</param>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="withdrawalId">Withdrawal ID to be completed by transaction broadcast</param>
        /// <param name="signatureId">ID of prepared payment template to sign. This is should be stored on a client side to retrieve ID of the blockchain transaction, when signing application signs the transaction and broadcasts it to the blockchain.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OffchainResponse>> OffchainWithdrawal_Broadcast_Async(string currency, string txData, string withdrawalId, string signatureId, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "currency", currency },
                { "txData", txData },
            };
            parameters.AddOptionalParameter("withdrawalId", withdrawalId);
            parameters.AddOptionalParameter("signatureId", signatureId);

            var credits = 2;
            var url = GetUrl(string.Format(Endpoints_Offchain_Withdrawal_Broadcast));
            var result = await SendTatumRequest<OffchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<OffchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<OffchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }
        #endregion

        #region Blockchain / Shared (Bitcoin, BitcoinCash, Ethereum, Litecoin, Scrypta, VeChain)
        protected virtual WebCallResult<BlockchainWallet> Blockchain_GenerateWallet(BlockchainType chain, string mnemonics = null, CancellationToken ct = default) => Blockchain_GenerateWallet_Async(chain, new List<string> { mnemonics }, ct).Result;
        protected virtual WebCallResult<BlockchainWallet> Blockchain_GenerateWallet(BlockchainType chain, IEnumerable<string> mnemonics = null, CancellationToken ct = default) => Blockchain_GenerateWallet_Async(chain, mnemonics, ct).Result;
        protected virtual async Task<WebCallResult<BlockchainWallet>> Blockchain_GenerateWallet_Async(BlockchainType chain, string mnemonics = null, CancellationToken ct = default) => await Blockchain_GenerateWallet_Async(chain, new List<string> { mnemonics }, ct);
        protected virtual async Task<WebCallResult<BlockchainWallet>> Blockchain_GenerateWallet_Async(BlockchainType chain, IEnumerable<string> mnemonics = null, CancellationToken ct = default)
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

        protected virtual WebCallResult<TatumAddress> Blockchain_GenerateDepositAddress(BlockchainType chain, string xpub, int index, CancellationToken ct = default) => Blockchain_GenerateDepositAddress_Async(chain, xpub, index, ct).Result;
        protected virtual async Task<WebCallResult<TatumAddress>> Blockchain_GenerateDepositAddress_Async(BlockchainType chain, string xpub, int index, CancellationToken ct = default)
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
                if (!result.Success) return WebCallResult<TatumAddress>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

                return new WebCallResult<TatumAddress>(result.ResponseStatusCode, result.ResponseHeaders, new TatumAddress { Address = result.Data }, null);
            }
            else
            {
                return await SendTatumRequest<TatumAddress>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            }
        }

        protected virtual WebCallResult<TatumKey> Blockchain_GeneratePrivateKey(BlockchainType chain, string mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKey_Async(chain, new List<string> { mnemonics }, index, ct).Result;
        protected virtual WebCallResult<TatumKey> Blockchain_GeneratePrivateKey(BlockchainType chain, IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKey_Async(chain, mnemonics, index, ct).Result;
        protected virtual async Task<WebCallResult<TatumKey>> Blockchain_GeneratePrivateKey_Async(BlockchainType chain, string mnemonics, int index, CancellationToken ct = default) => await Blockchain_GeneratePrivateKey_Async(chain, new List<string> { mnemonics }, index, default);
        protected virtual async Task<WebCallResult<TatumKey>> Blockchain_GeneratePrivateKey_Async(BlockchainType chain, IEnumerable<string> mnemonics, int index, CancellationToken ct = default)
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
            return await SendTatumRequest<TatumKey>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
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
        public virtual WebCallResult<BlockchainWallet> Bitcoin_GenerateWallet(string mnemonics, CancellationToken ct = default) => Blockchain_GenerateWallet_Async(BlockchainType.Bitcoin, new List<string> { mnemonics }, ct).Result;
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
        public virtual WebCallResult<BlockchainWallet> Bitcoin_GenerateWallet(IEnumerable<string> mnemonics = null, CancellationToken ct = default) => Blockchain_GenerateWallet_Async(BlockchainType.Bitcoin, mnemonics, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainWallet>> Bitcoin_GenerateWallet_Async(string mnemonics, CancellationToken ct = default) => await Blockchain_GenerateWallet_Async(BlockchainType.Bitcoin, new List<string> { mnemonics }, ct);
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
        public virtual async Task<WebCallResult<BlockchainWallet>> Bitcoin_GenerateWallet_Async(IEnumerable<string> mnemonics = null, CancellationToken ct = default) => await Blockchain_GenerateWallet_Async(BlockchainType.Bitcoin, mnemonics, ct);

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
        public virtual WebCallResult<TatumAddress> Bitcoin_GenerateDepositAddress(string xpub, int index, CancellationToken ct = default) => Blockchain_GenerateDepositAddress_Async(BlockchainType.Bitcoin, xpub, index, ct).Result;
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
        public virtual async Task<WebCallResult<TatumAddress>> Bitcoin_GenerateDepositAddress_Async(string xpub, int index, CancellationToken ct = default) => await Blockchain_GenerateDepositAddress_Async(BlockchainType.Bitcoin, xpub, index, ct);

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
        public virtual WebCallResult<TatumKey> Bitcoin_GeneratePrivateKey(string mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKey_Async(BlockchainType.Bitcoin, new List<string> { mnemonics }, index, ct).Result;
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
        public virtual WebCallResult<TatumKey> Bitcoin_GeneratePrivateKey(IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKey_Async(BlockchainType.Bitcoin, mnemonics, index, ct).Result;
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
        public virtual async Task<WebCallResult<TatumKey>> Bitcoin_GeneratePrivateKey_Async(string mnemonics, int index, CancellationToken ct = default) => await Blockchain_GeneratePrivateKey_Async(BlockchainType.Bitcoin, new List<string> { mnemonics }, index, default);
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
        public virtual async Task<WebCallResult<TatumKey>> Bitcoin_GeneratePrivateKey_Async(IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => await Blockchain_GeneratePrivateKey_Async(BlockchainType.Bitcoin, mnemonics, index, default);

        /// <summary>
        /// <b>Title:</b> Get Blockchain Information<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Blockchain Information. Obtain basic info like testnet / mainent version of the chain, current block number and it's hash.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BitcoinChainInfo> Bitcoin_GetBlockchainInformation(CancellationToken ct = default) => Bitcoin_GetBlockchainInformation_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Blockchain Information<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Blockchain Information. Obtain basic info like testnet / mainent version of the chain, current block number and it's hash.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BitcoinChainInfo>> Bitcoin_GetBlockchainInformation_Async(CancellationToken ct = default)
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
        public virtual WebCallResult<TatumHash> Bitcoin_GetBlockHash(long block_id, CancellationToken ct = default) => Bitcoin_GetBlockHash_Async(block_id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Block hash<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Block hash. Returns hash of the block to get the block detail.
        /// </summary>
        /// <param name="block_id">The number of blocks preceding a particular block on a block chain.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TatumHash>> Bitcoin_GetBlockHash_Async(long block_id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Bitcoin_GetBlockHash, block_id));
            return await SendTatumRequest<TatumHash>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
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
        public virtual WebCallResult<BitcoinBlock> Bitcoin_GetBlock(string hash_height, CancellationToken ct = default) => Bitcoin_GetBlock_Async(hash_height, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Block by hash or height<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Block detail by block hash or height.
        /// </summary>
        /// <param name="hash_height">Block hash or height.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BitcoinBlock>> Bitcoin_GetBlock_Async(string hash_height, CancellationToken ct = default)
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
        public virtual WebCallResult<BitcoinTransaction> Bitcoin_GetTransactionByHash(string hash, CancellationToken ct = default) => Bitcoin_GetTransactionByHash_Async(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Transaction by hash<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Transaction detail by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BitcoinTransaction>> Bitcoin_GetTransactionByHash_Async(string hash, CancellationToken ct = default)
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
        public virtual WebCallResult<IEnumerable<BitcoinTransaction>> Bitcoin_GetTransactionsByAddress(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default) => Bitcoin_GetTransactionsByAddress_Async(address, pageSize, offset, ct).Result;
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
        public virtual async Task<WebCallResult<IEnumerable<BitcoinTransaction>>> Bitcoin_GetTransactionsByAddress_Async(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default)
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
        public virtual WebCallResult<BitcoinBalance> Bitcoin_GetBalance(string address, CancellationToken ct = default) => Bitcoin_GetBalance_Async(address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Balance of the address<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Balance of the address.
        /// </summary>
        /// <param name="address">Address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BitcoinBalance>> Bitcoin_GetBalance_Async(string address, CancellationToken ct = default)
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
        public virtual WebCallResult<BitcoinUTXO> Bitcoin_GetTransactionUTXO(string txhash, long index, CancellationToken ct = default) => Bitcoin_GetTransactionUTXO_Async(txhash, index, ct).Result;
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
        public virtual async Task<WebCallResult<BitcoinUTXO>> Bitcoin_GetTransactionUTXO_Async(string txhash, long index, CancellationToken ct = default)
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
        public virtual WebCallResult<BlockchainResponse> Bitcoin_Send(IEnumerable<BitcoinSendOrderFromAddress> fromAddress, IEnumerable<BitcoinSendOrderFromUTXO> fromUTXO, IEnumerable<BitcoinSendOrderTo> to, CancellationToken ct = default) => Bitcoin_Send_Async(fromAddress, fromUTXO, to, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainResponse>> Bitcoin_Send_Async(IEnumerable<BitcoinSendOrderFromAddress> fromAddress, IEnumerable<BitcoinSendOrderFromUTXO> fromUTXO, IEnumerable<BitcoinSendOrderTo> to, CancellationToken ct = default)
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
        public virtual WebCallResult<BlockchainResponse> Bitcoin_Broadcast(string txData, string signatureId = null, CancellationToken ct = default) => Bitcoin_Broadcast_Async(txData, signatureId, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainResponse>> Bitcoin_Broadcast_Async(string txData, string signatureId = null, CancellationToken ct = default)
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
        public virtual WebCallResult<BlockchainWallet> Ethereum_GenerateWallet(string mnemonics, CancellationToken ct = default) => Blockchain_GenerateWallet_Async(BlockchainType.Ethereum, new List<string> { mnemonics }, ct).Result;
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
        public virtual WebCallResult<BlockchainWallet> Ethereum_GenerateWallet(IEnumerable<string> mnemonics = null, CancellationToken ct = default) => Blockchain_GenerateWallet_Async(BlockchainType.Ethereum, mnemonics, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainWallet>> Ethereum_GenerateWallet_Async(string mnemonics, CancellationToken ct = default) => await Blockchain_GenerateWallet_Async(BlockchainType.Ethereum, new List<string> { mnemonics }, ct);
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
        public virtual async Task<WebCallResult<BlockchainWallet>> Ethereum_GenerateWallet_Async(IEnumerable<string> mnemonics = null, CancellationToken ct = default) => await Blockchain_GenerateWallet_Async(BlockchainType.Ethereum, mnemonics, ct);

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
        public virtual WebCallResult<TatumAddress> Ethereum_GenerateDepositAddress(string xpub, int index, CancellationToken ct = default) => Blockchain_GenerateDepositAddress_Async(BlockchainType.Ethereum, xpub, index, ct).Result;
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
        public virtual async Task<WebCallResult<TatumAddress>> Ethereum_GenerateDepositAddress_Async(string xpub, int index, CancellationToken ct = default) => await Blockchain_GenerateDepositAddress_Async(BlockchainType.Ethereum, xpub, index, ct);

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
        public virtual WebCallResult<TatumKey> Ethereum_GeneratePrivateKey(string mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKey_Async(BlockchainType.Ethereum, new List<string> { mnemonics }, index, ct).Result;
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
        public virtual WebCallResult<TatumKey> Ethereum_GeneratePrivateKey(IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKey_Async(BlockchainType.Ethereum, mnemonics, index, ct).Result;
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
        public virtual async Task<WebCallResult<TatumKey>> Ethereum_GeneratePrivateKey_Async(string mnemonics, int index, CancellationToken ct = default) => await Blockchain_GeneratePrivateKey_Async(BlockchainType.Ethereum, new List<string> { mnemonics }, index, default);
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
        public virtual async Task<WebCallResult<TatumKey>> Ethereum_GeneratePrivateKey_Async(IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => await Blockchain_GeneratePrivateKey_Async(BlockchainType.Ethereum, mnemonics, index, default);

        /// <summary>
        /// <b>Title:</b> Web3 HTTP driver<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Use this endpoint URL as a http-based web3 driver to connect directly to the Ethereum node provided by Tatum. 
        /// To learn more about Ethereum Web3, please visit Ethereum developer's guide.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<EthereumDriver> Ethereum_Web3HttpDriver(CancellationToken ct = default) => Ethereum_Web3HttpDriver_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Web3 HTTP driver<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Use this endpoint URL as a http-based web3 driver to connect directly to the Ethereum node provided by Tatum. 
        /// To learn more about Ethereum Web3, please visit Ethereum developer's guide.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<EthereumDriver>> Ethereum_Web3HttpDriver_Async(CancellationToken ct = default)
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
        public virtual WebCallResult<long> Ethereum_GetCurrentBlockNumber(CancellationToken ct = default) => Ethereum_GetCurrentBlockNumber_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get current block number<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum current block number. This is the number of the latest block in the blockchain.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<long>> Ethereum_GetCurrentBlockNumber_Async(CancellationToken ct = default)
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
        public virtual WebCallResult<EthereumBlock> Ethereum_GetBlock(string hash_height, CancellationToken ct = default) => Ethereum_GetBlock_Async(hash_height, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Ethereum block by hash<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum block by block hash or block number.
        /// </summary>
        /// <param name="hash_height">Block hash or block number</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<EthereumBlock>> Ethereum_GetBlock_Async(string hash_height, CancellationToken ct = default)
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
        public virtual WebCallResult<decimal> Ethereum_ETH_GetBalance(string address, CancellationToken ct = default) => Ethereum_ETH_GetBalance_Async(address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Ethereum Account balance<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum account balance in ETH. This method does not prints any balance of the ERC20 or ERC721 tokens on the account.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<decimal>> Ethereum_ETH_GetBalance_Async(string address, CancellationToken ct = default)
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
        public virtual WebCallResult<decimal> Ethereum_ERC20_GetBalance(string address, string contractAddress, int decimals = 0, CancellationToken ct = default) => Ethereum_ERC20_GetBalance_Async(address, contractAddress, decimals, ct).Result;
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
        public virtual async Task<WebCallResult<decimal>> Ethereum_ERC20_GetBalance_Async(string address, string contractAddress, int decimals = 0, CancellationToken ct = default)
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
        public virtual WebCallResult<EthereumTransaction> Ethereum_GetTransactionByHash(string hash, CancellationToken ct = default) => Ethereum_GetTransactionByHash_Async(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Ethereum Transaction<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Ethereum transaction by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<EthereumTransaction>> Ethereum_GetTransactionByHash_Async(string hash, CancellationToken ct = default)
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
        public virtual WebCallResult<long> Ethereum_GetOutgoingTransactionsCount(string address, CancellationToken ct = default) => Ethereum_GetOutgoingTransactionsCount_Async(address, ct).Result;
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
        public virtual async Task<WebCallResult<long>> Ethereum_GetOutgoingTransactionsCount_Async(string address, CancellationToken ct = default)
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
        public virtual WebCallResult<IEnumerable<EthereumTransaction>> Ethereum_GetTransactionsByAddress(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default) => Ethereum_GetTransactionsByAddress_Async(address, pageSize, offset, ct).Result;
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
        public virtual async Task<WebCallResult<IEnumerable<EthereumTransaction>>> Ethereum_GetTransactionsByAddress_Async(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default)
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
        public virtual WebCallResult<BlockchainResponse> Ethereum_Send(
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
        public virtual async Task<WebCallResult<BlockchainResponse>> Ethereum_Send_Async(
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
        public virtual WebCallResult<BlockchainResponse> Ethereum_InvokeSmartContractMethod(
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
        public virtual async Task<WebCallResult<BlockchainResponse>> Ethereum_InvokeSmartContractMethod_Async(
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
        public virtual WebCallResult<BlockchainResponse> Ethereum_ERC20_DeploySmartContract(
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
        public virtual async Task<WebCallResult<BlockchainResponse>> Ethereum_ERC20_DeploySmartContract_Async(
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
        public virtual WebCallResult<BlockchainResponse> Ethereum_ERC20_Transfer(
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
        public virtual async Task<WebCallResult<BlockchainResponse>> Ethereum_ERC20_Transfer_Async(
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
        public virtual WebCallResult<BlockchainResponse> Ethereum_ERC721_DeploySmartContract(
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
        public virtual async Task<WebCallResult<BlockchainResponse>> Ethereum_ERC721_DeploySmartContract_Async(
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
        public virtual WebCallResult<BlockchainResponse> Ethereum_ERC721_Mint(
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
        public virtual async Task<WebCallResult<BlockchainResponse>> Ethereum_ERC721_Mint_Async(
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
        public virtual WebCallResult<BlockchainResponse> Ethereum_ERC721_Transfer(
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
        public virtual async Task<WebCallResult<BlockchainResponse>> Ethereum_ERC721_Transfer_Async(
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
        public virtual WebCallResult<BlockchainResponse> Ethereum_ERC721_MintMultiple(
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
        public virtual async Task<WebCallResult<BlockchainResponse>> Ethereum_ERC721_MintMultiple_Async(
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
        public virtual WebCallResult<BlockchainResponse> Ethereum_ERC721_Burn(
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
        public virtual async Task<WebCallResult<BlockchainResponse>> Ethereum_ERC721_Burn_Async(
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
        public virtual WebCallResult<decimal> Ethereum_ERC721_GetBalance(string address, string contractAddress, int decimals = 0, CancellationToken ct = default) => Ethereum_GetERC721Balance_Async(address, contractAddress, decimals, ct).Result;
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
        public virtual async Task<WebCallResult<decimal>> Ethereum_GetERC721Balance_Async(string address, string contractAddress, int decimals = 0, CancellationToken ct = default)
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
        public virtual WebCallResult<decimal> Ethereum_ERC721_GetToken(string address, int index, string contractAddress, int decimals = 0, CancellationToken ct = default) => Ethereum_ERC721_GetToken_Async(address, index, contractAddress, decimals, ct).Result;
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
        public virtual async Task<WebCallResult<decimal>> Ethereum_ERC721_GetToken_Async(string address, int index, string contractAddress, int decimals = 0, CancellationToken ct = default)
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
        public virtual WebCallResult<EthereumData> Ethereum_ERC721_GetTokenMetadata(string token, string contractAddress, decimal divider = 1, CancellationToken ct = default) => Ethereum_ERC721_GetTokenMetadata_Async(token, contractAddress, divider, ct).Result;
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
        public virtual async Task<WebCallResult<EthereumData>> Ethereum_ERC721_GetTokenMetadata_Async(string token, string contractAddress, decimal divider = 1, CancellationToken ct = default)
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
        public virtual WebCallResult<EthereumData> Ethereum_ERC721_GetTokenOwner(string token, string contractAddress, decimal divider = 1, CancellationToken ct = default) => Ethereum_ERC721_GetTokenOwner_Async(token, contractAddress, divider, ct).Result;
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
        public virtual async Task<WebCallResult<EthereumData>> Ethereum_ERC721_GetTokenOwner_Async(string token, string contractAddress, decimal divider = 1, CancellationToken ct = default)
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
        public virtual WebCallResult<BlockchainResponse> Ethereum_Broadcast(string txData, string signatureId = null, CancellationToken ct = default) => Ethereum_Broadcast_Async(txData, signatureId, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainResponse>> Ethereum_Broadcast_Async(string txData, string signatureId = null, CancellationToken ct = default)
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
        public virtual WebCallResult<BlockchainWallet> BitcoinCash_GenerateWallet(string mnemonics, CancellationToken ct = default) => Blockchain_GenerateWallet_Async(BlockchainType.BitcoinCash, new List<string> { mnemonics }, ct).Result;
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
        public virtual WebCallResult<BlockchainWallet> BitcoinCash_GenerateWallet(IEnumerable<string> mnemonics = null, CancellationToken ct = default) => Blockchain_GenerateWallet_Async(BlockchainType.BitcoinCash, mnemonics, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainWallet>> BitcoinCash_GenerateWallet_Async(string mnemonics, CancellationToken ct = default) => await Blockchain_GenerateWallet_Async(BlockchainType.BitcoinCash, new List<string> { mnemonics }, ct);
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
        public virtual async Task<WebCallResult<BlockchainWallet>> BitcoinCash_GenerateWallet_Async(IEnumerable<string> mnemonics = null, CancellationToken ct = default) => await Blockchain_GenerateWallet_Async(BlockchainType.BitcoinCash, mnemonics, ct);

        /// <summary>
        /// <b>Title:</b> Get Bitcoin Cash Blockchain Information<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Cash Blockchain Information. Obtain basic info like testnet / mainent version of the chain, current block number and it's hash.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BitcoinCashChainInfo> BitcoinCash_GetBlockchainInformation(CancellationToken ct = default) => BitcoinCash_GetBlockchainInformation_Async(ct).Result;
        /// <b>Title:</b> Get Bitcoin Cash Blockchain Information<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Cash Blockchain Information. Obtain basic info like testnet / mainent version of the chain, current block number and it's hash.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BitcoinCashChainInfo>> BitcoinCash_GetBlockchainInformation_Async(CancellationToken ct = default)
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
        public virtual WebCallResult<TatumHash> BitcoinCash_GetBlockHash(long block_id, CancellationToken ct = default) => BitcoinCash_GetBlockHash_Async(block_id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Bitcoin Cash Block hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Cash Block hash. Returns hash of the block to get the block detail.
        /// </summary>
        /// <param name="block_id">Block hash or height</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TatumHash>> BitcoinCash_GetBlockHash_Async(long block_id, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_BitcoinCash_GetBlockHash, block_id));
            return await SendTatumRequest<TatumHash>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
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
        public virtual WebCallResult<BitcoinCashBlock> BitcoinCash_GetBlock(string hash_height, CancellationToken ct = default) => BitcoinCash_GetBlock_Async(hash_height, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Bitcoin Cash Block by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Cash Block detail by block hash or height.
        /// </summary>
        /// <param name="hash_height"></param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BitcoinCashBlock>> BitcoinCash_GetBlock_Async(string hash_height, CancellationToken ct = default)
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
        public virtual WebCallResult<BitcoinCashTransaction> BitcoinCash_GetTransactionByHash(string hash, CancellationToken ct = default) => BitcoinCash_GetTransactionByHash_Async(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Bitcoin Cash Transaction by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Bitcoin Cash Transaction by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BitcoinCashTransaction>> BitcoinCash_GetTransactionByHash_Async(string hash, CancellationToken ct = default)
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
        public virtual WebCallResult<IEnumerable<BitcoinCashTransaction>> BitcoinCash_GetTransactionsByAddress(string address, int skip = 0, CancellationToken ct = default) => BitcoinCash_GetTransactionsByAddress_Async(address, skip, ct).Result;
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
        public virtual async Task<WebCallResult<IEnumerable<BitcoinCashTransaction>>> BitcoinCash_GetTransactionsByAddress_Async(string address, int skip = 0, CancellationToken ct = default)
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
        public virtual WebCallResult<TatumAddress> BitcoinCash_GenerateDepositAddress(string xpub, int index, CancellationToken ct = default) => Blockchain_GenerateDepositAddress_Async(BlockchainType.BitcoinCash, xpub, index, ct).Result;
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
        public virtual async Task<WebCallResult<TatumAddress>> BitcoinCash_GenerateDepositAddress_Async(string xpub, int index, CancellationToken ct = default) => await Blockchain_GenerateDepositAddress_Async(BlockchainType.BitcoinCash, xpub, index, ct);

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
        public virtual WebCallResult<TatumKey> BitcoinCash_GeneratePrivateKey(string mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKey_Async(BlockchainType.BitcoinCash, new List<string> { mnemonics }, index, ct).Result;
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
        public virtual WebCallResult<TatumKey> BitcoinCash_GeneratePrivateKey(IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKey_Async(BlockchainType.BitcoinCash, mnemonics, index, ct).Result;
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
        public virtual async Task<WebCallResult<TatumKey>> BitcoinCash_GeneratePrivateKey_Async(string mnemonics, int index, CancellationToken ct = default) => await Blockchain_GeneratePrivateKey_Async(BlockchainType.BitcoinCash, new List<string> { mnemonics }, index, default);
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
        public virtual async Task<WebCallResult<TatumKey>> BitcoinCash_GeneratePrivateKey_Async(IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => await Blockchain_GeneratePrivateKey_Async(BlockchainType.BitcoinCash, mnemonics, index, default);

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
        public virtual WebCallResult<BlockchainResponse> BitcoinCash_Send(IEnumerable<BitcoinCashSendOrderFromUTXO> fromUTXO, IEnumerable<BitcoinCashSendOrderTo> to, CancellationToken ct = default) => BitcoinCash_Send_Async(fromUTXO, to, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainResponse>> BitcoinCash_Send_Async(IEnumerable<BitcoinCashSendOrderFromUTXO> fromUTXO, IEnumerable<BitcoinCashSendOrderTo> to, CancellationToken ct = default)
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
        public virtual WebCallResult<BlockchainResponse> BitcoinCash_Broadcast(string txData, string signatureId = null, CancellationToken ct = default) => BitcoinCash_Broadcast_Async(txData, signatureId, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainResponse>> BitcoinCash_Broadcast_Async(string txData, string signatureId = null, CancellationToken ct = default)
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
        public virtual WebCallResult<BlockchainWallet> Litecoin_GenerateWallet(string mnemonics, CancellationToken ct = default) => Blockchain_GenerateWallet_Async(BlockchainType.Litecoin, new List<string> { mnemonics }, ct).Result;
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
        public virtual WebCallResult<BlockchainWallet> Litecoin_GenerateWallet(IEnumerable<string> mnemonics = null, CancellationToken ct = default) => Blockchain_GenerateWallet_Async(BlockchainType.Litecoin, mnemonics, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainWallet>> Litecoin_GenerateWallet_Async(string mnemonics, CancellationToken ct = default) => await Blockchain_GenerateWallet_Async(BlockchainType.Litecoin, new List<string> { mnemonics }, ct);
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
        public virtual async Task<WebCallResult<BlockchainWallet>> Litecoin_GenerateWallet_Async(IEnumerable<string> mnemonics = null, CancellationToken ct = default) => await Blockchain_GenerateWallet_Async(BlockchainType.Litecoin, mnemonics, ct);

        /// <summary>
        /// <b>Title:</b> Get Litecoin Blockchain Information<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Litecoin Blockchain Information. Obtain basic info like testnet / mainent version of the chain, current block number and it's hash.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<LitecoinChainInfo> Litecoin_GetBlockchainInformation(CancellationToken ct = default) => Litecoin_GetBlockchainInformation_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Litecoin Blockchain Information<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Litecoin Blockchain Information. Obtain basic info like testnet / mainent version of the chain, current block number and it's hash.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<LitecoinChainInfo>> Litecoin_GetBlockchainInformation_Async(CancellationToken ct = default)
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
        public virtual WebCallResult<TatumHash> Litecoin_GetBlockHash(long block_id, CancellationToken ct = default) => Litecoin_GetBlockHash_Async(block_id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Litecoin Block hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Litecoin Block hash. Returns hash of the block to get the block detail.
        /// </summary>
        /// <param name="block_id">The number of blocks preceding a particular block on a block chain.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TatumHash>> Litecoin_GetBlockHash_Async(long block_id, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_Litecoin_GetBlockHash, block_id));
            return await SendTatumRequest<TatumHash>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
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
        public virtual WebCallResult<LitecoinBlock> Litecoin_GetBlock(string hash_height, CancellationToken ct = default) => Litecoin_GetBlock_Async(hash_height, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Litecoin Block by hash or height<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Litecoin Block detail by block hash or height.
        /// </summary>
        /// <param name="hash_height">Block hash or height.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<LitecoinBlock>> Litecoin_GetBlock_Async(string hash_height, CancellationToken ct = default)
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
        public virtual WebCallResult<LitecoinTransaction> Litecoin_GetTransactionByHash(string hash, CancellationToken ct = default) => Litecoin_GetTransactionByHash_Async(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Litecoin Transaction by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Litecoin Transaction detail by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<LitecoinTransaction>> Litecoin_GetTransactionByHash_Async(string hash, CancellationToken ct = default)
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
        public virtual WebCallResult<IEnumerable<LitecoinTransaction>> Litecoin_GetTransactionsByAddress(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default) => Litecoin_GetTransactionsByAddress_Async(address, pageSize, offset, ct).Result;
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
        public virtual async Task<WebCallResult<IEnumerable<LitecoinTransaction>>> Litecoin_GetTransactionsByAddress_Async(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default)
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
        public virtual WebCallResult<LitecoinBalance> Litecoin_GetBalance(string address, CancellationToken ct = default) => Litecoin_GetBalance_Async(address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Litecoin Balance of the address<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Litecoin Balance of the address.
        /// </summary>
        /// <param name="address">Address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<LitecoinBalance>> Litecoin_GetBalance_Async(string address, CancellationToken ct = default)
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
        public virtual WebCallResult<LitecoinUTXO> Litecoin_GetTransactionUTXO(string txhash, long index, CancellationToken ct = default) => Litecoin_GetTransactionUTXO_Async(txhash, index, ct).Result;
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
        public virtual async Task<WebCallResult<LitecoinUTXO>> Litecoin_GetTransactionUTXO_Async(string txhash, long index, CancellationToken ct = default)
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
        public virtual WebCallResult<TatumAddress> Litecoin_GenerateDepositAddress(string xpub, int index, CancellationToken ct = default) => Blockchain_GenerateDepositAddress_Async(BlockchainType.Litecoin, xpub, index, ct).Result;
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
        public virtual async Task<WebCallResult<TatumAddress>> Litecoin_GenerateDepositAddress_Async(string xpub, int index, CancellationToken ct = default) => await Blockchain_GenerateDepositAddress_Async(BlockchainType.Litecoin, xpub, index, ct);

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
        public virtual WebCallResult<TatumKey> Litecoin_GeneratePrivateKey(string mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKey_Async(BlockchainType.Litecoin, new List<string> { mnemonics }, index, ct).Result;
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
        public virtual WebCallResult<TatumKey> Litecoin_GeneratePrivateKey(IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKey_Async(BlockchainType.Litecoin, mnemonics, index, ct).Result;
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
        public virtual async Task<WebCallResult<TatumKey>> Litecoin_GeneratePrivateKey_Async(string mnemonics, int index, CancellationToken ct = default) => await Blockchain_GeneratePrivateKey_Async(BlockchainType.Litecoin, new List<string> { mnemonics }, index, default);
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
        public virtual async Task<WebCallResult<TatumKey>> Litecoin_GeneratePrivateKey_Async(IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => await Blockchain_GeneratePrivateKey_Async(BlockchainType.Litecoin, mnemonics, index, default);

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
        public virtual WebCallResult<BlockchainResponse> Litecoin_Send(IEnumerable<LitecoinSendOrderFromAddress> fromAddress, IEnumerable<LitecoinSendOrderFromUTXO> fromUTXO, IEnumerable<LitecoinSendOrderTo> to, CancellationToken ct = default) => Litecoin_Send_Async(fromAddress, fromUTXO, to, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainResponse>> Litecoin_Send_Async(IEnumerable<LitecoinSendOrderFromAddress> fromAddress, IEnumerable<LitecoinSendOrderFromUTXO> fromUTXO, IEnumerable<LitecoinSendOrderTo> to, CancellationToken ct = default)
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
        public virtual WebCallResult<BlockchainResponse> Litecoin_Broadcast(string txData, string signatureId = null, CancellationToken ct = default) => Litecoin_Broadcast_Async(txData, signatureId, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainResponse>> Litecoin_Broadcast_Async(string txData, string signatureId = null, CancellationToken ct = default)
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
        public virtual WebCallResult<RippleAddressSecret> Ripple_GenerateAccount(CancellationToken ct = default) => Ripple_GenerateAccount_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate XRP account<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate XRP account. Tatum does not support HD wallet for XRP, only specific address and private key can be generated.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<RippleAddressSecret>> Ripple_GenerateAccount_Async(CancellationToken ct = default)
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
        public virtual WebCallResult<RippleChainInfo> Ripple_GetBlockchainInformation(CancellationToken ct = default) => Ripple_GetBlockchainInformation_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get XRP Blockchain Information<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XRP Blockchain last closed ledger index and hash.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<RippleChainInfo>> Ripple_GetBlockchainInformation_Async(CancellationToken ct = default)
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
        public virtual WebCallResult<RippleChainFee> Ripple_GetBlockchainFee(CancellationToken ct = default) => Ripple_GetBlockchainFee_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get actual Blockchain fee<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XRP Blockchain fee. Standard fee for the transaction is available in the drops.base_fee section and is 10 XRP drops by default. When there is a heavy traffic on the blockchain, fees are increasing according to current traffic.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<RippleChainFee>> Ripple_GetBlockchainFee_Async(CancellationToken ct = default)
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
        public virtual WebCallResult<RippleAccountTransactions> Ripple_GetTransactionsByAccount(string account, int? min = null, RippleMarker marker = null, CancellationToken ct = default) => Ripple_GetTransactionsByAccount_Async(account, min, marker, ct).Result;
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
        public virtual async Task<WebCallResult<RippleAccountTransactions>> Ripple_GetTransactionsByAccount_Async(string account, int? min = null, RippleMarker marker = null, CancellationToken ct = default)
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
        public virtual WebCallResult<RippleLedger> Ripple_GetLedger(long index, CancellationToken ct = default) => Ripple_GetLedger_Async(index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Ledger<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get ledger by sequence.
        /// </summary>
        /// <param name="index">Sequence of XRP ledger.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<RippleLedger>> Ripple_GetLedger_Async(long index, CancellationToken ct = default)
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
        public virtual WebCallResult<RippleTransactionData> Ripple_GetTransactionByHash(string hash, CancellationToken ct = default) => Ripple_GetTransactionByHash_Async(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get XRP Transaction by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XRP Transaction by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<RippleTransactionData>> Ripple_GetTransactionByHash_Async(string hash, CancellationToken ct = default)
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
        public virtual WebCallResult<RippleAccount> Ripple_GetAccountInfo(string account, CancellationToken ct = default) => Ripple_GetAccountInfo_Async(account, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Account info<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XRP Account info.
        /// </summary>
        /// <param name="account">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<RippleAccount>> Ripple_GetAccountInfo_Async(string account, CancellationToken ct = default)
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
        public virtual WebCallResult<RippleBalance> Ripple_GetBalance(string account, CancellationToken ct = default) => Ripple_GetBalance_Async(account, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Account Balance<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XRP Account Balance. Obtain balance of the XRP and other assets on the account.
        /// </summary>
        /// <param name="account">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<RippleBalance>> Ripple_GetBalance_Async(string account, CancellationToken ct = default)
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
        public virtual WebCallResult<BlockchainResponse> Ripple_Send(
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
        public virtual async Task<WebCallResult<BlockchainResponse>> Ripple_Send_Async(
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
        public virtual WebCallResult<BlockchainResponse> Ripple_TrustLine(
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
        public virtual async Task<WebCallResult<BlockchainResponse>> Ripple_TrustLine_Async(
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
        public virtual WebCallResult<BlockchainResponse> Ripple_ModifyAccountSettings(
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
        public virtual async Task<WebCallResult<BlockchainResponse>> Ripple_ModifyAccountSettings_Async(
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
        public virtual WebCallResult<BlockchainResponse> Ripple_Broadcast(string txData, string signatureId = null, CancellationToken ct = default) => Ripple_Broadcast_Async(txData, signatureId, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainResponse>> Ripple_Broadcast_Async(string txData, string signatureId = null, CancellationToken ct = default)
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
        public virtual WebCallResult<StellarAddressSecret> Stellar_GenerateAccount(CancellationToken ct = default) => Stellar_GenerateAccount_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate XLM account<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate XLM account. Tatum does not support HD wallet for XLM, only specific address and private key can be generated.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<StellarAddressSecret>> Stellar_GenerateAccount_Async(CancellationToken ct = default)
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
        public virtual WebCallResult<StellarChainInfo> Stellar_GetBlockchainInformation(CancellationToken ct = default) => Stellar_GetBlockchainInformation_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get XLM Blockchain Information<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Blockchain last closed ledger.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<StellarChainInfo>> Stellar_GetBlockchainInformation_Async(CancellationToken ct = default)
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
        public virtual WebCallResult<StellarChainInfo> Stellar_GetLedger(long sequence, CancellationToken ct = default) => Stellar_GetLedger_Async(sequence, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get XLM Blockchain Ledger by sequence<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Blockchain ledger for ledger sequence.
        /// </summary>
        /// <param name="sequence">Sequence of the ledger.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<StellarChainInfo>> Stellar_GetLedger_Async(long sequence, CancellationToken ct = default)
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
        public virtual WebCallResult<IEnumerable<StellarTransaction>> Stellar_GetTransactionsInLedger(long sequence, CancellationToken ct = default) => Stellar_GetTransactionsInLedger_Async(sequence, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get XLM Blockchain Transactions in Ledger<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Blockchain transactions in the ledger.
        /// </summary>
        /// <param name="sequence">Sequence of the ledger.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<StellarTransaction>>> Stellar_GetTransactionsInLedger_Async(long sequence, CancellationToken ct = default)
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
        public virtual WebCallResult<decimal> Stellar_GetBlockchainFee(CancellationToken ct = default) => Stellar_GetBlockchainFee_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get actual XLM fee<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Blockchain fee in 1/10000000 of XLM (stroop)
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<decimal>> Stellar_GetBlockchainFee_Async(CancellationToken ct = default)
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
        public virtual WebCallResult<IEnumerable<StellarTransaction>> Stellar_GetTransactionsByAccount(string account, CancellationToken ct = default) => Stellar_GetTransactionsByAccount_Async(account, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get XLM Account transactions<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// List all XLM account transactions.
        /// </summary>
        /// <param name="account">Address of XLM account.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<StellarTransaction>>> Stellar_GetTransactionsByAccount_Async(string account, CancellationToken ct = default)
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
        public virtual WebCallResult<StellarTransaction> Stellar_GetTransactionByHash(string hash, CancellationToken ct = default) => Stellar_GetTransactionByHash_Async(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get XLM Transaction by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Transaction by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<StellarTransaction>> Stellar_GetTransactionByHash_Async(string hash, CancellationToken ct = default)
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
        public virtual WebCallResult<StellarAccountInfo> Stellar_GetAccountInfo(string account, CancellationToken ct = default) => Stellar_GetAccountInfo_Async(account, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get XLM Account info<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get XLM Account detail.
        /// </summary>
        /// <param name="account">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<StellarAccountInfo>> Stellar_GetAccountInfo_Async(string account, CancellationToken ct = default)
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
        public virtual WebCallResult<BlockchainResponse> Stellar_Send(
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
        public virtual async Task<WebCallResult<BlockchainResponse>> Stellar_Send_Async(
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
        public virtual WebCallResult<BlockchainResponse> Stellar_TrustLine(
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
        public virtual async Task<WebCallResult<BlockchainResponse>> Stellar_TrustLine_Async(
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
        public virtual WebCallResult<BlockchainResponse> Stellar_Broadcast(string txData, string signatureId = null, CancellationToken ct = default) => Stellar_Broadcast_Async(txData, signatureId, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainResponse>> Stellar_Broadcast_Async(string txData, string signatureId = null, CancellationToken ct = default)
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
        public virtual WebCallResult<BlockchainResponse> Records_SetData(BlockchainType chain, string data, string fromPrivateKey = null, string to = null, long? nonce = null, CancellationToken ct = default) => Records_SetData_Async(chain, data, fromPrivateKey, to, nonce, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainResponse>> Records_SetData_Async(BlockchainType chain, string data, string fromPrivateKey = null, string to = null, long? nonce = null, CancellationToken ct = default)
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
        public virtual WebCallResult<TatumData> Records_GetData(BlockchainType chain, string id, CancellationToken ct = default) => Records_GetData_Async(chain, id, ct).Result;
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
        public virtual async Task<WebCallResult<TatumData>> Records_GetData_Async(BlockchainType chain, string id, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "chain", JsonConvert.SerializeObject(chain, new BlockchainTypeConverter(false)) },
                { "id", id },
            };
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Records_Log));
            return await SendTatumRequest<TatumData>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
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
        public virtual WebCallResult<BinanceAddress> Binance_GenerateAccount(CancellationToken ct = default) => Binance_GenerateAccount_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Binance wallet<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate BNB account. Tatum does not support HD wallet for BNB, only specific address and private key can be generated.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BinanceAddress>> Binance_GenerateAccount_Async(CancellationToken ct = default)
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
        public virtual WebCallResult<long> Binance_GetCurrentBlock(CancellationToken ct = default) => Binance_GetCurrentBlock_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Binance current block<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Binance current block number.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<long>> Binance_GetCurrentBlock_Async(CancellationToken ct = default)
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
        public virtual WebCallResult<BinanceBlockTransactions> Binance_GetTransactionsInBlock(long height, CancellationToken ct = default) => Binance_GetTransactionsInBlock_Async(height, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Binance Transactions in Block<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Transactions in block by block height.
        /// </summary>
        /// <param name="height">Block height</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BinanceBlockTransactions>> Binance_GetTransactionsInBlock_Async(long height, CancellationToken ct = default)
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
        public virtual WebCallResult<BinanceAccount> Binance_GetAccountInfo(string account, CancellationToken ct = default) => Binance_GetAccountInfo_Async(account, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Binance Account<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Binance Account Detail by address.
        /// </summary>
        /// <param name="account">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BinanceAccount>> Binance_GetAccountInfo_Async(string account, CancellationToken ct = default)
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
        public virtual WebCallResult<BinanceTransaction> Binance_GetTransaction(string hash, CancellationToken ct = default) => Binance_GetTransaction_Async(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Binance Transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Binance Transaction by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BinanceTransaction>> Binance_GetTransaction_Async(string hash, CancellationToken ct = default)
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
        public virtual WebCallResult<BlockchainResponse> Binance_Send(
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
        public virtual async Task<WebCallResult<BlockchainResponse>> Stellar_Send_Async(
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
        public virtual WebCallResult<BlockchainResponse> Binance_Broadcast(string txData, CancellationToken ct = default) => Binance_Broadcast_Async(txData, ct).Result;
        /// <summary>
        /// <b>Title:</b> Broadcast signed BNB transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed transaction to Binance blockchain. This method is used internally from Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> Binance_Broadcast_Async(string txData, CancellationToken ct = default)
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
        public virtual WebCallResult<BlockchainWallet> VeChain_GenerateWallet(string mnemonics, CancellationToken ct = default) => Blockchain_GenerateWallet_Async(BlockchainType.VeChain, new List<string> { mnemonics }, ct).Result;
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
        public virtual WebCallResult<BlockchainWallet> VeChain_GenerateWallet(IEnumerable<string> mnemonics = null, CancellationToken ct = default) => Blockchain_GenerateWallet_Async(BlockchainType.VeChain, mnemonics, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainWallet>> VeChain_GenerateWallet_Async(string mnemonics, CancellationToken ct = default) => await Blockchain_GenerateWallet_Async(BlockchainType.VeChain, new List<string> { mnemonics }, ct);
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
        public virtual async Task<WebCallResult<BlockchainWallet>> VeChain_GenerateWallet_Async(IEnumerable<string> mnemonics = null, CancellationToken ct = default) => await Blockchain_GenerateWallet_Async(BlockchainType.VeChain, mnemonics, ct);

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
        public virtual WebCallResult<TatumAddress> VeChain_GenerateDepositAddress(string xpub, int index, CancellationToken ct = default) => Blockchain_GenerateDepositAddress_Async(BlockchainType.VeChain, xpub, index, ct).Result;
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
        public virtual async Task<WebCallResult<TatumAddress>> VeChain_GenerateDepositAddress_Async(string xpub, int index, CancellationToken ct = default) => await Blockchain_GenerateDepositAddress_Async(BlockchainType.VeChain, xpub, index, ct);

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
        public virtual WebCallResult<TatumKey> VeChain_GeneratePrivateKey(string mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKey_Async(BlockchainType.VeChain, new List<string> { mnemonics }, index, ct).Result;
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
        public virtual WebCallResult<TatumKey> VeChain_GeneratePrivateKey(IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKey_Async(BlockchainType.VeChain, mnemonics, index, ct).Result;
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
        public virtual async Task<WebCallResult<TatumKey>> VeChain_GeneratePrivateKey_Async(string mnemonics, int index, CancellationToken ct = default) => await Blockchain_GeneratePrivateKey_Async(BlockchainType.VeChain, new List<string> { mnemonics }, index, default);
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
        public virtual async Task<WebCallResult<TatumKey>> VeChain_GeneratePrivateKey_Async(IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => await Blockchain_GeneratePrivateKey_Async(BlockchainType.VeChain, mnemonics, index, default);

        /// <summary>
        /// <b>Title:</b> Get VeChain current block<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get VeChain current block number.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<long> VeChain_GetCurrentBlock(CancellationToken ct = default) => VeChain_GetCurrentBlock_Async(ct).Result;
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
        public virtual async Task<WebCallResult<long>> VeChain_GetCurrentBlock_Async(CancellationToken ct = default)
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
        public virtual WebCallResult<VeChainBlock> VeChain_GetBlock(string hash_height, CancellationToken ct = default) => VeChain_GetBlock_Async(hash_height, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get VeChain Block by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get VeChain Block by block hash or block number.
        /// </summary>
        /// <param name="hash_height">Block hash or block number</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<VeChainBlock>> VeChain_GetBlock_Async(string hash_height, CancellationToken ct = default)
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
        public virtual WebCallResult<VeChainBalance> VeChain_GetBalance(string address, CancellationToken ct = default) => VeChain_GetBalance_Async(address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get VeChain Account balance<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get VeChain Account balance in VET.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<VeChainBalance>> VeChain_GetBalance_Async(string address, CancellationToken ct = default)
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
        public virtual WebCallResult<VeChainEnergy> VeChain_GetEnergy(string address, CancellationToken ct = default) => VeChain_GetEnergy_Async(address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get VeChain Account energy (VTHO)<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get VeChain Account energy in VTHO. VTHO is used for paying for the transaction fee.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<VeChainEnergy>> VeChain_GetEnergy_Async(string address, CancellationToken ct = default)
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
        public virtual WebCallResult<VeChainTransaction> VeChain_GetTransactionByHash(string hash, CancellationToken ct = default) => VeChain_GetTransactionByHash_Async(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get VeChain Transaction<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Get VeChain Transaction by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<VeChainTransaction>> VeChain_GetTransactionByHash_Async(string hash, CancellationToken ct = default)
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
        public virtual WebCallResult<VeChainTransactionReceipt> VeChain_GetTransactionReceipt(string hash, CancellationToken ct = default) => VeChain_GetTransactionReceipt_Async(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get VeChain Transaction Receipt<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get VeChain Transaction Receipt by transaction hash. Transaction receipt is available only after transaction is included in the block and contains information about paid fee or created contract address and much more.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<VeChainTransactionReceipt>> VeChain_GetTransactionReceipt_Async(string hash, CancellationToken ct = default)
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
        public virtual WebCallResult<BlockchainResponse> VeChain_Send(string to, decimal amount, string fromPrivateKey = null, string signatureId = null, string data = null, VeChainFee fee = null, CancellationToken ct = default) => VeChain_Send_Async(to, amount, fromPrivateKey, signatureId, data, fee, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainResponse>> VeChain_Send_Async(string to, decimal amount, string fromPrivateKey = null, string signatureId = null, string data = null, VeChainFee fee = null, CancellationToken ct = default)
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
        public virtual WebCallResult<decimal> VeChain_EstimateGasForTransaction(string from, string to, decimal value, string data = null, long? nonce = null, CancellationToken ct = default) => VeChain_EstimateGasForTransaction_Async(from, to, value, data, nonce, ct).Result;
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
        public virtual async Task<WebCallResult<decimal>> VeChain_EstimateGasForTransaction_Async(string from, string to, decimal value, string data = null, long? nonce = null, CancellationToken ct = default)
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
        public virtual WebCallResult<BlockchainResponse> VeChain_Broadcast(string txData, string signatureId = null, CancellationToken ct = default) => Bitcoin_Broadcast_Async(txData, signatureId, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainResponse>> VeChain_Broadcast_Async(string txData, string signatureId = null, CancellationToken ct = default)
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
        public virtual WebCallResult<NeoAccount> Neo_GenerateAccount(CancellationToken ct = default) => Neo_GenerateAccount_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate NEO account<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate NEO account. Tatum does not support HD wallet for NEO, only specific address and private key can be generated.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<NeoAccount>> Neo_GenerateAccount_Async(CancellationToken ct = default)
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
        public virtual WebCallResult<long> Neo_GetCurrentBlock(CancellationToken ct = default) => Neo_GetCurrentBlock_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get current NEO block<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get current NEO block.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<long>> Neo_GetCurrentBlock_Async(CancellationToken ct = default)
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
        public virtual WebCallResult<NeoBlock> Neo_GetBlock(string hash_height, CancellationToken ct = default) => Neo_GetBlock_Async(hash_height, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get NEO block<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get NEO block by hash or height.
        /// </summary>
        /// <param name="hash_height">Block hash or height.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<NeoBlock>> Neo_GetBlock_Async(string hash_height, CancellationToken ct = default)
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
        public virtual WebCallResult<NeoBalance> Neo_GetBalance(string address, CancellationToken ct = default) => Neo_GetBalance_Async(address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get NEO Account balance<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Balance of all assets (NEO, GAS, etc.) and tokens for the Account.
        /// </summary>
        /// <param name="address">Address to get balance</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<NeoBalance>> Neo_GetBalance_Async(string address, CancellationToken ct = default)
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
        public virtual WebCallResult<NeoAsset> Neo_GetAssetInfo(string asset, CancellationToken ct = default) => Neo_GetAssetInfo_Async(asset, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Neo Asset details<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get information about asset.
        /// </summary>
        /// <param name="asset">Asset ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<NeoAsset>> Neo_GetAssetInfo_Async(string asset, CancellationToken ct = default)
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
        public virtual WebCallResult<NeoTransactionOutput> Neo_GetUnspentTransactionOutputs(string txId, long index, CancellationToken ct = default) => Neo_GetUnspentTransactionOutputs_Async(txId, index, ct).Result;
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
        public virtual async Task<WebCallResult<NeoTransactionOutput>> Neo_GetUnspentTransactionOutputs_Async(string txId, long index, CancellationToken ct = default)
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
        public virtual WebCallResult<IEnumerable<NeoAccountTransaction>> Neo_GetTransactionsByAccount(string address, CancellationToken ct = default) => Neo_GetTransactionsByAccount_Async(address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get NEO Account transactions<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get NEO Account transactions.
        /// </summary>
        /// <param name="address">Example: AKL19WwiJ2fiTDkAnYQ7GJSTUBoJPTQKhn</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<NeoAccountTransaction>>> Neo_GetTransactionsByAccount_Async(string address, CancellationToken ct = default)
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
        public virtual WebCallResult<NeoContract> Neo_GetContractInfo(string scriptHash, CancellationToken ct = default) => Neo_GetContractInfo_Async(scriptHash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get NEO contract details<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get NEO contract details.
        /// </summary>
        /// <param name="scriptHash">Hash of smart contract</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<NeoContract>> Neo_GetContractInfo_Async(string scriptHash, CancellationToken ct = default)
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
        public virtual WebCallResult<NeoTransaction> Neo_GetTransactionByHash(string hash, CancellationToken ct = default) => Neo_GetTransactionByHash_Async(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get NEO transaction by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get NEO transaction by hash.
        /// </summary>
        /// <param name="hash">Transaction hash.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<NeoTransaction>> Neo_GetTransactionByHash_Async(string hash, CancellationToken ct = default)
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
        public virtual WebCallResult<BlockchainResponse> Neo_Send(string to, decimal NEO_Amount, decimal GAS_Amount, string fromPrivateKey, CancellationToken ct = default) => Neo_Send_Async(to, NEO_Amount, GAS_Amount, fromPrivateKey, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainResponse>> Neo_Send_Async(string to, decimal NEO_Amount, decimal GAS_Amount, string fromPrivateKey, CancellationToken ct = default)
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
        public virtual WebCallResult<BlockchainResponse> Neo_ClaimGAS(string privateKey, CancellationToken ct = default) => Neo_ClaimGAS_Async(privateKey, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainResponse>> Neo_ClaimGAS_Async(string privateKey, CancellationToken ct = default)
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
        public virtual WebCallResult<BlockchainResponse> Neo_SendToken(string scriptHash, decimal amount, int numOfDecimals, string fromPrivateKey, string to, decimal additionalInvocationGas = 0, CancellationToken ct = default) => Neo_SendToken_Async(scriptHash, amount, numOfDecimals, fromPrivateKey, to, additionalInvocationGas, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainResponse>> Neo_SendToken_Async(string scriptHash, decimal amount, int numOfDecimals, string fromPrivateKey, string to, decimal additionalInvocationGas = 0, CancellationToken ct = default)
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
        public virtual WebCallResult<BlockchainResponse> Neo_Broadcast(string txData, CancellationToken ct = default) => Neo_Broadcast_Async(txData, ct).Result;
        /// <summary>
        /// <b>Title:</b> Broadcast NEO transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast NEO transaction. This method is used internally from Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> Neo_Broadcast_Async(string txData, CancellationToken ct = default)
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

        #region Blockchain / Libra
        // N/A
        #endregion

        #region Blockchain / TRON
        /// <summary>
        /// <b>Title:</b> Generate Tron wallet<br />
        /// <b>Credits:</b> 5 credit per API call.<br />
        /// <b>Description:</b>
        /// Generate TRON address and private key.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TronWallet> Tron_GenerateAccount(CancellationToken ct = default) => Tron_GenerateAccount_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate Tron wallet<br />
        /// <b>Credits:</b> 5 credit per API call.<br />
        /// <b>Description:</b>
        /// Generate TRON address and private key.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TronWallet>> Tron_GenerateAccount_Async(CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_TRON_GenerateAccount));
            return await SendTatumRequest<TronWallet>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> XXXXXXXXXXXX<br />
        /// <b>Credits:</b> XXXXXXXXXXXX<br />
        /// <b>Description:</b>
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TronCurrentBlock> Tron_GetCurrentBlock(CancellationToken ct = default) => Tron_GetCurrentBlock_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> XXXXXXXXXXXX<br />
        /// <b>Credits:</b> XXXXXXXXXXXX<br />
        /// <b>Description:</b>
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TronCurrentBlock>> Tron_GetCurrentBlock_Async(CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_TRON_CurrentBlock));
            return await SendTatumRequest<TronCurrentBlock>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get current Tron block<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Tron block by hash or height.
        /// </summary>
        /// <param name="hash_height">Block hash or height.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TronBlock> Tron_GetBlock(string hash_height, CancellationToken ct = default) => Tron_GetBlock_Async(hash_height, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get current Tron block<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Tron block by hash or height.
        /// </summary>
        /// <param name="hash_height">Block hash or height.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TronBlock>> Tron_GetBlock_Async(string hash_height, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_TRON_GetBlock, hash_height));
            return await SendTatumRequest<TronBlock>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Tron Account transactions<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Tron Account transactions. Default page size is 200 transactions per request.
        /// </summary>
        /// <param name="address">Address to get transactions for.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TronAccountTransactions> Tron_GetTransactionsByAccount(string address, CancellationToken ct = default) => Tron_GetTransactionsByAccount_Async(address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Tron Account transactions<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Tron Account transactions. Default page size is 200 transactions per request.
        /// </summary>
        /// <param name="address">Address to get transactions for.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TronAccountTransactions>> Tron_GetTransactionsByAccount_Async(string address, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_TRON_GetTransactionsByAccount, address));
            return await SendTatumRequest<TronAccountTransactions>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Tron transaction by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Tron transaction by hash.
        /// </summary>
        /// <param name="hash">Transaction hash.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TronTransaction> Tron_GetTransactionByHash(string hash, CancellationToken ct = default) => Tron_GetTransactionByHash_Async(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Tron transaction by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Tron transaction by hash.
        /// </summary>
        /// <param name="hash">Transaction hash.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TronTransaction>> Tron_GetTransactionByHash_Async(string hash, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_TRON_GetTransactionByHash, hash));
            return await SendTatumRequest<TronTransaction>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Send Tron transaction<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Tron transaction from address to address.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="fromPrivateKey">Private key of the address, from which the TRX will be sent.</param>
        /// <param name="to">Recipient address of TRON account in Base58 format.</param>
        /// <param name="amount">Amount to be sent in TRX.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> Tron_Send(string fromPrivateKey, string to, decimal amount, CancellationToken ct = default) => Tron_Send_Async(fromPrivateKey, to, amount, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send Tron transaction<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Tron transaction from address to address.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="fromPrivateKey">Private key of the address, from which the TRX will be sent.</param>
        /// <param name="to">Recipient address of TRON account in Base58 format.</param>
        /// <param name="amount">Amount to be sent in TRX.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> Tron_Send_Async(string fromPrivateKey, string to, decimal amount, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "fromPrivateKey", fromPrivateKey },
                { "to", to },
                { "amount", amount.ToString(CultureInfo.InvariantCulture) },
            };

            var credits = 10;
            var url = GetUrl(string.Format(Endpoints_TRON_Send));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Broadcast Tron transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast Tron transaction. This method is used internally from Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> Tron_Broadcast(string txData, CancellationToken ct = default) => Tron_Broadcast_Async(txData, ct).Result;
        /// <summary>
        /// <b>Title:</b> Broadcast Tron transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast Tron transaction. This method is used internally from Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> Tron_Broadcast_Async(string txData, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "txData", txData },
            };

            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_TRON_Broadcast));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Freeze Tron balance<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Freeze Tron assets on the address. By freezing assets, you can obtain energy or bandwith to perform transactions.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="fromPrivateKey">Private key of the address, from which the TRX will be sent.</param>
        /// <param name="receiver">Recipient address of frozen BANDWIDTH or ENERGY.</param>
        /// <param name="duration">Duration of frozen funds, in days.</param>
        /// <param name="resource">Resource to obtain, BANDWIDTH or ENERGY.</param>
        /// <param name="amount">Amount to be frozen in TRX.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> Tron_FreezeBalance(string fromPrivateKey, string receiver, int duration, string resource, decimal amount, CancellationToken ct = default) => Tron_FreezeBalance_Async(fromPrivateKey, receiver, duration, resource, amount, ct).Result;
        /// <summary>
        /// <b>Title:</b> Freeze Tron balance<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Freeze Tron assets on the address. By freezing assets, you can obtain energy or bandwith to perform transactions.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="fromPrivateKey">Private key of the address, from which the TRX will be sent.</param>
        /// <param name="receiver">Recipient address of frozen BANDWIDTH or ENERGY.</param>
        /// <param name="duration">Duration of frozen funds, in days.</param>
        /// <param name="resource">Resource to obtain, BANDWIDTH or ENERGY.</param>
        /// <param name="amount">Amount to be frozen in TRX.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> Tron_FreezeBalance_Async(string fromPrivateKey, string receiver, int duration, string resource, decimal amount, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "fromPrivateKey", fromPrivateKey },
                { "receiver", receiver },
                { "duration", duration },
                { "resource", resource },
                { "amount", amount.ToString(CultureInfo.InvariantCulture) },
            };

            var credits = 10;
            var url = GetUrl(string.Format(Endpoints_TRON_Freeze));
            var result = await SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Get Tron TRC10 token detail<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Tron TRC10 token details.
        /// </summary>
        /// <param name="id">TRC10 token ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TronTRC10Token> Tron_TRC10GetTokenDetails(long id, CancellationToken ct = default) => Tron_TRC10GetTokenDetails_Async(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Tron TRC10 token detail<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Tron TRC10 token details.
        /// </summary>
        /// <param name="id">TRC10 token ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TronTRC10Token>> Tron_TRC10GetTokenDetails_Async(long id, CancellationToken ct = default)
        {
            var credits = 5;
            var url = GetUrl(string.Format(Endpoints_TRON_TRC10GetToken, id));
            return await SendTatumRequest<TronTRC10Token>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Create Tron TRC10 token<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Create Tron TRC10 token. 1 account can create only 1 token. All supply of the tokens are transfered to the issuer account 100 seconds after the creation.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="fromPrivateKey">Private key of the address, from which the TRX will be sent.</param>
        /// <param name="recipient">Recipient address of created TRC20 tokens.</param>
        /// <param name="name">Name of the token.</param>
        /// <param name="abbreviation">Abbreviation of the token.</param>
        /// <param name="description">Description of the token.</param>
        /// <param name="url">URL of the token.</param>
        /// <param name="totalSupply">Total supply of the tokens.</param>
        /// <param name="decimals">Number of decimal places of the token.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> Tron_TRC10CreateToken(string fromPrivateKey, string recipient, string name, string abbreviation, string description, string url, long totalSupply, int decimals, CancellationToken ct = default) => Tron_TRC10CreateToken_Async(fromPrivateKey, recipient, name, abbreviation, description, url, totalSupply, decimals, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create Tron TRC10 token<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Create Tron TRC10 token. 1 account can create only 1 token. All supply of the tokens are transfered to the issuer account 100 seconds after the creation.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="fromPrivateKey">Private key of the address, from which the TRX will be sent.</param>
        /// <param name="recipient">Recipient address of created TRC20 tokens.</param>
        /// <param name="name">Name of the token.</param>
        /// <param name="abbreviation">Abbreviation of the token.</param>
        /// <param name="description">Description of the token.</param>
        /// <param name="url">URL of the token.</param>
        /// <param name="totalSupply">Total supply of the tokens.</param>
        /// <param name="decimals">Number of decimal places of the token.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> Tron_TRC10CreateToken_Async(string fromPrivateKey, string recipient, string name, string abbreviation, string description, string url, long totalSupply, int decimals, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "fromPrivateKey", fromPrivateKey },
                { "recipient", recipient },
                { "name", name },
                { "abbreviation", abbreviation },
                { "description", description },
                { "url", url },
                { "totalSupply", totalSupply },
                { "decimals", decimals },
            };

            var credits = 10;
            var url_ = GetUrl(string.Format(Endpoints_TRON_TRC10CreateToken));
            var result = await SendTatumRequest<BlockchainResponse>(url_, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Send Tron TRC10 transaction<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Tron TRC10 transaction from address to address.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="fromPrivateKey">Private key of the address, from which the TRX will be sent.</param>
        /// <param name="to">Recipient address of TRON account in Base58 format.</param>
        /// <param name="tokenId">ID of the token to transfer.</param>
        /// <param name="amount">Amount to be sent in TRX.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> Tron_TRC10Send(string fromPrivateKey, string to, long tokenId, decimal amount, CancellationToken ct = default) => Tron_TRC10Send_Async(fromPrivateKey, to, tokenId, amount, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send Tron TRC10 transaction<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Tron TRC10 transaction from address to address.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="fromPrivateKey">Private key of the address, from which the TRX will be sent.</param>
        /// <param name="to">Recipient address of TRON account in Base58 format.</param>
        /// <param name="tokenId">ID of the token to transfer.</param>
        /// <param name="amount">Amount to be sent in TRX.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> Tron_TRC10Send_Async(string fromPrivateKey, string to, long tokenId, decimal amount, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "fromPrivateKey", fromPrivateKey },
                { "to", to },
                { "tokenId", tokenId.ToString(CultureInfo.InvariantCulture) },
                { "amount", amount.ToString(CultureInfo.InvariantCulture) },
            };

            var credits = 10;
            var url_ = GetUrl(string.Format(Endpoints_TRON_TRC10Send));
            var result = await SendTatumRequest<BlockchainResponse>(url_, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Create Tron TRC20 token<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Create Tron TRC20 token. 1 account can create only 1 token. All supply of the tokens are transfered to the issuer account 100 seconds after the creation.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="fromPrivateKey">Private key of the address, from which the TRX will be sent.</param>
        /// <param name="recipient">Recipient address of created TRC20 tokens.</param>
        /// <param name="name">Name of the token.</param>
        /// <param name="symbol">Symbol of the token.</param>
        /// <param name="totalSupply">Total supply of the tokens.</param>
        /// <param name="decimals">Number of decimal places of the token.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> Tron_TRC20CreateToken(string fromPrivateKey, string recipient, string name, string symbol, long totalSupply, int decimals, CancellationToken ct = default) => Tron_TRC20CreateToken_Async(fromPrivateKey, recipient, name, symbol, totalSupply, decimals, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create Tron TRC20 token<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Create Tron TRC20 token. 1 account can create only 1 token. All supply of the tokens are transfered to the issuer account 100 seconds after the creation.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="fromPrivateKey">Private key of the address, from which the TRX will be sent.</param>
        /// <param name="recipient">Recipient address of created TRC20 tokens.</param>
        /// <param name="name">Name of the token.</param>
        /// <param name="symbol">Symbol of the token.</param>
        /// <param name="totalSupply">Total supply of the tokens.</param>
        /// <param name="decimals">Number of decimal places of the token.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> Tron_TRC20CreateToken_Async(string fromPrivateKey, string recipient, string name, string symbol, long totalSupply, int decimals, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "fromPrivateKey", fromPrivateKey },
                { "recipient", recipient },
                { "name", name },
                { "symbol", symbol },
                { "totalSupply", totalSupply },
                { "decimals", decimals },
            };

            var credits = 10;
            var url_ = GetUrl(string.Format(Endpoints_TRON_TRC20CreateToken));
            var result = await SendTatumRequest<BlockchainResponse>(url_, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Send Tron TRC20 transaction<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Tron TRC20 transaction from address to address.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="fromPrivateKey">Private key of the address, from which the TRX will be sent.</param>
        /// <param name="to">Recipient address of TRON account in Base58 format.</param>
        /// <param name="tokenAddress">Address of the TRC20 token to transfer.</param>
        /// <param name="amount">Fee in TRX to be paid.</param>
        /// <param name="feeLimit">Amount to be sent in TRX.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> Tron_TRC20Send(string fromPrivateKey, string to, string tokenAddress, decimal amount, decimal feeLimit, CancellationToken ct = default) => Tron_TRC20Send_Async(fromPrivateKey, to, tokenAddress, amount, feeLimit, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send Tron TRC20 transaction<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Tron TRC20 transaction from address to address.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="fromPrivateKey">Private key of the address, from which the TRX will be sent.</param>
        /// <param name="to">Recipient address of TRON account in Base58 format.</param>
        /// <param name="tokenAddress">Address of the TRC20 token to transfer.</param>
        /// <param name="amount">Fee in TRX to be paid.</param>
        /// <param name="feeLimit">Amount to be sent in TRX.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> Tron_TRC20Send_Async(string fromPrivateKey, string to, string tokenAddress, decimal amount, decimal feeLimit, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "fromPrivateKey", fromPrivateKey },
                { "to", to },
                { "tokenAddress", tokenAddress },
                { "feeLimit", feeLimit },
                { "amount", amount.ToString(CultureInfo.InvariantCulture) },
            };

            var credits = 10;
            var url_ = GetUrl(string.Format(Endpoints_TRON_TRC20Send));
            var result = await SendTatumRequest<BlockchainResponse>(url_, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
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
        public virtual WebCallResult<BlockchainWallet> Scrypta_GenerateWallet(string mnemonics, CancellationToken ct = default) => Blockchain_GenerateWallet_Async(BlockchainType.Scrypta, new List<string> { mnemonics }, ct).Result;
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
        public virtual WebCallResult<BlockchainWallet> Scrypta_GenerateWallet(IEnumerable<string> mnemonics = null, CancellationToken ct = default) => Blockchain_GenerateWallet_Async(BlockchainType.Scrypta, mnemonics, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainWallet>> Scrypta_GenerateWallet_Async(string mnemonics, CancellationToken ct = default) => await Blockchain_GenerateWallet_Async(BlockchainType.Scrypta, new List<string> { mnemonics }, ct);
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
        public virtual async Task<WebCallResult<BlockchainWallet>> Scrypta_GenerateWallet_Async(IEnumerable<string> mnemonics = null, CancellationToken ct = default) => await Blockchain_GenerateWallet_Async(BlockchainType.Scrypta, mnemonics, ct);

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
        public virtual WebCallResult<TatumKey> Scrypta_GeneratePrivateKey(string mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKey_Async(BlockchainType.Scrypta, new List<string> { mnemonics }, index, ct).Result;
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
        public virtual WebCallResult<TatumKey> Scrypta_GeneratePrivateKey(IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKey_Async(BlockchainType.Scrypta, mnemonics, index, ct).Result;
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
        public virtual async Task<WebCallResult<TatumKey>> Scrypta_GeneratePrivateKey_Async(string mnemonics, int index, CancellationToken ct = default) => await Blockchain_GeneratePrivateKey_Async(BlockchainType.Scrypta, new List<string> { mnemonics }, index, default);
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
        public virtual async Task<WebCallResult<TatumKey>> Scrypta_GeneratePrivateKey_Async(IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => await Blockchain_GeneratePrivateKey_Async(BlockchainType.Scrypta, mnemonics, index, default);

        /// <summary>
        /// <b>Title:</b> Get Block hash<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Scrypta Block hash. Returns hash of the block to get the block detail.
        /// </summary>
        /// <param name="block_id">The number of blocks preceding a particular block on a block chain.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<string> Scrypta_GetBlockHash(long block_id, CancellationToken ct = default) => Scrypta_GetBlockHash_Async(block_id, ct).Result;
        public virtual async Task<WebCallResult<string>> Scrypta_GetBlockHash_Async(long block_id, CancellationToken ct = default)
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
        public virtual WebCallResult<ScryptaBlock> Scrypta_GetBlock(string hash_height, CancellationToken ct = default) => Scrypta_GetBlock_Async(hash_height, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Block by hash or height<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Scrypta Block detail by block hash or height.
        /// </summary>
        /// <param name="hash_height">Block hash or height.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<ScryptaBlock>> Scrypta_GetBlock_Async(string hash_height, CancellationToken ct = default)
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
        public virtual WebCallResult<BlockchainResponse> Scrypta_Send(IEnumerable<ScryptaSendOrderFromAddress> fromAddress, IEnumerable<ScryptaSendOrderFromUTXO> fromUTXO, IEnumerable<ScryptaSendOrderTo> to, CancellationToken ct = default) => Scrypta_Send_Async(fromAddress, fromUTXO, to, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainResponse>> Scrypta_Send_Async(IEnumerable<ScryptaSendOrderFromAddress> fromAddress, IEnumerable<ScryptaSendOrderFromUTXO> fromUTXO, IEnumerable<ScryptaSendOrderTo> to, CancellationToken ct = default)
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
        public virtual WebCallResult<ScryptaTransaction> Scrypta_GetTransactionByHash(string hash, CancellationToken ct = default) => Scrypta_GetTransactionByHash_Async(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Scrypta Transaction by hash<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Scrypta Transaction detail by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<ScryptaTransaction>> Scrypta_GetTransactionByHash_Async(string hash, CancellationToken ct = default)
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
        public virtual WebCallResult<IEnumerable<ScryptaTransaction>> Scrypta_GetTransactionsByAddress(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default) => Scrypta_GetTransactionsByAddress_Async(address, pageSize, offset, ct).Result;
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
        public virtual async Task<WebCallResult<IEnumerable<ScryptaTransaction>>> Scrypta_GetTransactionsByAddress_Async(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default)
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
        public virtual WebCallResult<IEnumerable<ScryptaUTXO>> Scrypta_GetSpendableUTXO(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default) => Scrypta_GetSpendableUTXO_Async(address, pageSize, offset, ct).Result;
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
        public virtual async Task<WebCallResult<IEnumerable<ScryptaUTXO>>> Scrypta_GetSpendableUTXO_Async(string address, int pageSize = 50, int offset = 0, CancellationToken ct = default)
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
        public virtual WebCallResult<ScryptaUTXO> Scrypta_GetTransactionUTXO(string txhash, long index, CancellationToken ct = default) => Scrypta_GetTransactionUTXO_Async(txhash, index, ct).Result;
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
        public virtual async Task<WebCallResult<ScryptaUTXO>> Scrypta_GetTransactionUTXO_Async(string txhash, long index, CancellationToken ct = default)
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
        public virtual WebCallResult<TatumAddress> Scrypta_GenerateDepositAddress(string xpub, int index, CancellationToken ct = default) => Blockchain_GenerateDepositAddress_Async(BlockchainType.Scrypta, xpub, index, ct).Result;
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
        public virtual async Task<WebCallResult<TatumAddress>> Scrypta_GenerateDepositAddress_Async(string xpub, int index, CancellationToken ct = default) => await Blockchain_GenerateDepositAddress_Async(BlockchainType.Scrypta, xpub, index, ct);

        /// <summary>
        /// <b>Title:</b> Get Blockchain Information<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Scrypta Blockchain Information. Obtain basic info like testnet / mainent version of the chain, current block number and it's hash.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<ScryptaChainInfo> Scrypta_GetBlockchainInformation(CancellationToken ct = default) => Scrypta_GetBlockchainInformation_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Blockchain Information<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get Scrypta Blockchain Information. Obtain basic info like testnet / mainent version of the chain, current block number and it's hash.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<ScryptaChainInfo>> Scrypta_GetBlockchainInformation_Async(CancellationToken ct = default)
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
        public virtual WebCallResult<BlockchainResponse> Scrypta_Broadcast(string txData, string signatureId = null, CancellationToken ct = default) => Scrypta_Broadcast_Async(txData, signatureId, ct).Result;
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
        public virtual async Task<WebCallResult<BlockchainResponse>> Scrypta_Broadcast_Async(string txData, string signatureId = null, CancellationToken ct = default)
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
        public virtual WebCallResult<IEnumerable<ServiceUsage>> Service_GetConsumptions(CancellationToken ct = default) => Service_GetConsumptions_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> List credit consumption for last month<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// List usage information of credits.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<ServiceUsage>>> Service_GetConsumptions_Async(CancellationToken ct = default)
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
        public virtual WebCallResult<ServiceExchangeRate> Service_GetExchangeRates(string currency, string basePair, CancellationToken ct = default) => Service_GetExchangeRates_Async(currency, basePair, ct).Result;
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
        public virtual async Task<WebCallResult<ServiceExchangeRate>> Service_GetExchangeRates_Async(string currency, string basePair, CancellationToken ct = default)
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
        public virtual WebCallResult<ServiceVersion> Service_GetVersion(CancellationToken ct = default) => Service_GetVersion_Async(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get API version<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get current version of the API.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<ServiceVersion>> Service_GetVersion_Async(CancellationToken ct = default)
        {
            var credits = 1;
            var url = GetUrl(string.Format(Endpoints_Service_Version));
            return await SendTatumRequest<ServiceVersion>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }
        #endregion

        #endregion

        #region Protected Methods
        protected virtual Uri GetUrl(string endpoint, int apiversion = Endpoints_Version)
        {
            return new Uri($"{BaseAddress.TrimEnd('/')}/v{apiversion}/{endpoint}");
        }

        protected virtual async Task<WebCallResult<T>> SendTatumRequest<T>(
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
            return await SendRequest<T>(uri, method, cancellationToken, parameters, signed, checkResult, postPosition, arraySerialization, credits);
        }

        protected override Error ParseErrorResponse(JToken error)
        {
            return this.TatumParseErrorResponse(error);
        }

        protected virtual Error TatumParseErrorResponse(JToken error)
        {
            if (error["statusCode"] == null || error["message"] == null)
                return new TatumError(error.ToString());

            return new TatumError((int)error["statusCode"], (string)error["message"]);
        }

        #endregion

    }
}