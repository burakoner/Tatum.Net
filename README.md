# ![Icon](https://github.com/burakoner/Tatum.Net/blob/master/Tatum.Net/Icon/icon.png?raw=true) Tatum.Net 

A .Net wrapper for the Tatum API as described on [Tatum](https://tatum.io/apidoc), including all features the API provides using clear and readable objects.

**If you think something is broken, something is missing or have any questions, please open an [Issue](https://github.com/burakoner/Tatum.Net/issues)**

## Donations
Donations are greatly appreciated and a motivation to keep improving.

**BTC**:  33WbRKqt7wXARVdAJSu1G1x3QnbyPtZ2bH
**ETH**:  0x65b02db9b67b73f5f1e983ae10796f91ded57b64

## Installation
![Nuget version](https://img.shields.io/nuget/v/Tatum.Net.svg)  ![Nuget downloads](https://img.shields.io/nuget/dt/Tatum.Net.svg)
Available on [Nuget](https://www.nuget.org/packages/Tatum.Net).
```
PM> Install-Package Tatum.Net
```
To get started with Tatum.Net first you will need to get the library itself. The easiest way to do this is to install the package into your project using  [NuGet](https://www.nuget.org/packages/Tatum.Net). Using Visual Studio this can be done in two ways.

### Using the package manager
In Visual Studio right click on your solution and select 'Manage NuGet Packages for solution...'. A screen will appear which initially shows the currently installed packages. In the top bit select 'Browse'. This will let you download net package from the NuGet server. In the search box type 'Tatum.Net' and hit enter. The Tatum.Net package should come up in the results. After selecting the package you can then on the right hand side select in which projects in your solution the package should install. After you've selected all project you wish to install and use Tatum.Net in hit 'Install' and the package will be downloaded and added to you projects.

### Using the package manager console
In Visual Studio in the top menu select 'Tools' -> 'NuGet Package Manager' -> 'Package Manager Console'. This should open up a command line interface. On top of the interface there is a dropdown menu where you can select the Default Project. This is the project that Tatum.Net will be installed in. After selecting the correct project type  `Install-Package Tatum.Net`  in the command line interface. This should install the latest version of the package in your project.

After doing either of above steps you should now be ready to actually start using Tatum.Net.
## Getting started
After installing it's time to actually use it. To get started we have to add the Tatum.Net namespace:  `using Tatum.Net;`.

Tatum.Net provides one main client to interact with the Tatum API. The  `TatumClient`  provides all rest API calls. TatumClient is disposable and as such can be used in a  `using`statement.

## Tatum Rest Api Examples
**Defining Api Client with Rate Limiters**
```C#
/* Tatum Api Client */
var cli_options = new TatumClientOptions
{
    RateLimiters = new List<IRateLimiter> { new RateLimiterCredit(5, TimeSpan.FromMilliseconds(1000)), },
    RateLimitingBehaviour = RateLimitingBehaviour.Wait,
};
var api = new TatumClient("XXXXXXXX-API-KEY-XXXXXXXX", cli_options);
```

**Ledger / Account Endpoints**
```C#
var led_01 = api.LedgerAccount_Create(BlockchainType.BitcoinCash);
var led_02 = api.LedgerAccount_GetAccounts();
var led_03 = api.LedgerAccount_CreateBatch(new List<LedgerAccountOptions> { });
var led_04 = api.LedgerAccount_GetByCustomerId("-----customer-id-----");
var led_05 = api.LedgerAccount_GetById("-----account-id-----");
var led_06 = api.LedgerAccount_Update("-----account-id-----", "-----account-code-----", "-----account-number-----");
var led_07 = api.LedgerAccount_GetBalance("-----account-id-----");
var led_08 = api.LedgerAccount_BlockAmount("-----account-id-----", 1000, "DEBIT_CARD_OP", "Description");
var led_09 = api.LedgerAccount_UnlockAmountAndPerformTransaction("-----blockage-id-----", "-----recipient-account-id-----", 1000.0m, false, false, "-----tx-code-----", "-----payment-id-----", "-----recipient-notes-----", "-----sender-notes-----", 1.0m);
var led_10 = api.LedgerAccount_UnblockAmount("-----blockage-id-----");
var led_11 = api.LedgerAccount_GetBlockedAmounts("-----account-id-----");
var led_12 = api.LedgerAccount_UnblockAllBlockedAmounts("-----account-id-----");
var led_13 = api.LedgerAccount_Activate("-----account-id-----");
var led_14 = api.LedgerAccount_Deactivate("-----account-id-----");
var led_15 = api.LedgerAccount_Freeze("-----account-id-----");
var led_16 = api.LedgerAccount_Unfreeze("-----account-id-----");
```

**Ledger / Transaction Endpoints**
```C#
var led_21 = api.LedgerTransaction_SendPayment("-----sender-account-id-----", "-----recipient-account-id-----", 10.0m);
var led_22 = api.LedgerTransaction_GetTransactionsByAccount("-----account-id-----");
var led_23 = api.LedgerTransaction_GetTransactionsByCustomer("-----customer-id-----");
var led_24 = api.LedgerTransaction_GetTransactionsByLedger();
var led_25 = api.LedgerTransaction_GetTransactionsByReference("-----reference-----");
```

**Ledger / Customer Endpoints**
```C#
var led_31 = api.LedgerCustomer_ListAll();
var led_32 = api.LedgerCustomer_Get("-----customer-id-----");
var led_33 = api.LedgerCustomer_Update("-----customer-internal-id-----", "-----customer-external-id-----");
var led_34 = api.LedgerCustomer_Activate("-----customer-id-----");
var led_35 = api.LedgerCustomer_Deactivate("-----customer-id-----");
var led_36 = api.LedgerCustomer_Enable("-----customer-id-----");
var led_37 = api.LedgerCustomer_Disable("-----customer-id-----");
```

**Ledger / Virtual Currency Endpoints**
```C#
var led_41 = api.LedgerVirtualCurrency_Create("-----name-----", "-----supply-----", "-----base-pair-----");
var led_42 = api.LedgerVirtualCurrency_Update("-----name-----", "-----base-pair-----");
var led_43 = api.LedgerVirtualCurrency_Get("-----name-----");
var led_44 = api.LedgerVirtualCurrency_Mint("-----account-id-----", 1000000, "-----payment-id-----", "-----reference-----", "-----transaction-code-----", "-----recipient-note-----", "-----counter-account-----", "-----sender-note-----");
var led_45 = api.LedgerVirtualCurrency_Destroy("-----account-id-----", 1000000, "-----payment-id-----", "-----reference-----", "-----transaction-code-----", "-----recipient-note-----", "-----counter-account-----", "-----sender-note-----");
```

**Ledger / Subscription Endpoints**
```C#
var led_51 = api.LedgerSubscription_Create(LedgerSubscriptionType.ACCOUNT_INCOMING_BLOCKCHAIN_TRANSACTION, account_id: "-----account-id-----", url: "https://www.google.com");
var led_52 = api.LedgerSubscription_List();
var led_53 = api.LedgerSubscription_Cancel("-----subscription-id-----");
var led_54 = api.LedgerSubscription_GetReport("-----subscription-id-----");
```

**Ledger / Order Book Endpoints**
```C#
var led_61 = api.LedgerOrderBook_GetHistoricalTrades("-----account-id-----","-----trade-pair-----");
var led_62 = api.LedgerOrderBook_GetBuyTrades("-----account-id-----");
var led_63 = api.LedgerOrderBook_GetSellTrades("-----account-id-----");
var led_64 = api.LedgerOrderBook_PlaceOrder_Async(LedgerTradeType.Buy, 1300.05m, 15.87m, "-----trade-pair-----", "-----currency1-account-id-----", "-----currency2-account-id-----");
var led_65 = api.LedgerOrderBook_GetTrade("-----trade-id-----");
var led_66 = api.LedgerOrderBook_CancelOrder("-----trade-id-----");
var led_67 = api.LedgerOrderBook_CancelAllOrders("-----account-id-----");
```

**Security / Key Management System Endpoints**
```C#
var kms_01 = api.KMS_GetPendingTransactions(BlockchainType.Bitcoin);
var kms_02 = api.KMS_GetTransaction("-----tx-id-----");
var kms_03 = api.KMS_CompletePendingTransaction("-----id-----", "-----tx-id-----");
var kms_04 = api.KMS_DeleteTransaction("-----id-----");
```

**Security / Address Endpoints**
```C#
var sec_01 = api.Security_CheckMalicousAddress("33WbRKqt7wXARVdAJSu1G1x3QnbyPtZ2bH");
```

**Off-chain / Blockchain Endpoints**
```C#
var off_priv_11 = api.Bitcoin_GeneratePrivateKey("-----mnemonics-----", 0);
var off_11 = api.OffchainBlockchain_SendBitcoin("-----account-id-----", "-----address-----", 0.01m, mnemonic: "-----mnemonics-----", xpub: "-----xpub-----");
var off_11_kms = api.KMS_CompletePendingTransaction("-----account-id-----", off_11.Data.Id);
            
var off_priv_12 = api.BitcoinCash_GeneratePrivateKey("-----mnemonics-----", 0);
var off_12 = api.OffchainBlockchain_SendBitcoinCash("-----account-id-----", "-----address-----", 0.01m, mnemonic: "-----mnemonics-----", xpub: "-----xpub-----");
var off_12_kms = api.KMS_CompletePendingTransaction("-----account-id-----", off_12.Data.Id);
            
var off_priv_13 = api.Litecoin_GeneratePrivateKey("-----mnemonics-----", 0);
var off_13 = api.OffchainBlockchain_SendLitecoin("-----account-id-----", "-----address-----", 0.01m, mnemonic: "-----mnemonics-----", xpub: "-----xpub-----");
var off_13_kms = api.KMS_CompletePendingTransaction("-----account-id-----", off_13.Data.Id);
            
var off_priv_14 = api.Ethereum_GeneratePrivateKey("-----mnemonics-----", 0);
var off_14 = api.OffchainBlockchain_SendEthereum("-----account-id-----", "-----address-----", 0.01m, mnemonic: "-----mnemonics-----");
var off_14_kms = api.KMS_CompletePendingTransaction("-----account-id-----", off_14.Data.Id);

var off_15 = api.OffchainBlockchain_CreateERC20Token("-----symbol-----", "-----supply-----", "-----description-----", "-----base-pair-----");
var off_16 = api.OffchainBlockchain_DeployERC20Token("-----symbol-----", "-----supply-----", "-----description-----", "-----base-pair-----");
var off_17 = api.OffchainBlockchain_SetERC20TokenContractAddress("-----address-----", "-----symbol-----");

var off_priv_18 = api.Ethereum_GeneratePrivateKey("-----mnemonics-----", 0);
var off_18 = api.OffchainBlockchain_SendERC20Token("-----account-id-----", "-----address-----", 0.01m, mnemonic: "-----mnemonics-----");
var off_18_kms = api.KMS_CompletePendingTransaction("-----account-id-----", off_18.Data.Id);

var off_19 = api.OffchainBlockchain_SendStellar("-----account-id-----", "-----account-----", "-----address-----", 0.01m, "-----secret-----");
var off_19_kms = api.KMS_CompletePendingTransaction("-----account-id-----", off_19.Data.Id);

var off_20 = api.OffchainBlockchain_CreateXLMAsset("-----issuer-account-----", "-----token-----", "-----base-pair-----");

var off_21 = api.OffchainBlockchain_SendRipple("-----account-id-----", "-----account-----", "-----address-----", 0.01m, secret: "-----secret-----");
var off_21_kms = api.KMS_CompletePendingTransaction("-----account-id-----", off_21.Data.Id);

var off_22 = api.OffchainBlockchain_CreateXRPAsset("-----issuer-account-----", "-----token-----", "-----base-pair-----");

var off_23 = api.OffchainBlockchain_SendBNB("-----account-id-----", "-----address-----", 0.01m, privateKey: "-----private-key-----");
var off_23_kms = api.KMS_CompletePendingTransaction("-----account-id-----", off_23.Data.Id);

var off_24 = api.OffchainBlockchain_CreateBNBAsset("-----token-----", "-----base-pair-----");
```

**Off-chain / Withdrawal Endpoints**
```C#
var off_31 = api.OffchainWithdrawal_Request("-----account-id-----", "-----address-----", 15.97m);
var off_32 = api.OffchainWithdrawal_CompleteRequest("-----withdrawal-id-----", "-----txid-----");
var off_33 = api.OffchainWithdrawal_Cancel("-----withdrawal-id-----");
var off_34 = api.OffchainWithdrawal_Broadcast("-----currency-----", "-----tx-data-----", "-----withdrawal-id-----", "-----signature-id-----");
```

**Blockchain / Bitcoin Endpoints**
```C#
var btc_mnemonics = Mnemonics.Generate(12);
var btc_01 = api.Bitcoin_GenerateWallet();
var btc_02 = api.Bitcoin_GenerateWallet(btc_mnemonics);
var btc_03 = api.Bitcoin_GenerateWallet(string.Join(" ", btc_mnemonics));
var btc_04 = api.Bitcoin_GenerateDepositAddress("-----xpub-----", 0);
var btc_05 = api.Bitcoin_GeneratePrivateKey(btc_mnemonics, 0);
var btc_06 = api.Bitcoin_GeneratePrivateKey(string.Join(" ", btc_mnemonics), 0);
var btc_07 = api.Bitcoin_GetBlockchainInformation();
var btc_08 = api.Bitcoin_GetBlockHash(664827);
var btc_09 = api.Bitcoin_GetBlock("664827");
var btc_10 = api.Bitcoin_GetBlock("0000000000000000000c2b526c01f6885dec2135214b7c742bf3ce1d386e6ffb");
var btc_11 = api.Bitcoin_GetTransactionByHash("8d05fbe88dc38ab6c339e422ea3682b48fb406fcbac0400064301b0ac2fdecdd");
var btc_12 = api.Bitcoin_GetTransactionsByAddress("33WbRKqt7wXARVdAJSu1G1x3QnbyPtZ2bH");
var btc_13 = api.Bitcoin_GetBalance("33WbRKqt7wXARVdAJSu1G1x3QnbyPtZ2bH");
var btc_14 = api.Bitcoin_GetTransactionUTXO("8d05fbe88dc38ab6c339e422ea3682b48fb406fcbac0400064301b0ac2fdecdd", 0);
var btc_fromAddress = new List<BitcoinSendOrderFromAddress> { new BitcoinSendOrderFromAddress { Address = "-----from-address-----", PrivateKey = "-----private-key-----", SignatureId = "-----signature-id-----" } };
var btc_fromUTXO = new List<BitcoinSendOrderFromUTXO> { new BitcoinSendOrderFromUTXO { Index = 0, PrivateKey = "-----private-key-----", SignatureId = "-----signature-id-----", TxHash = "-----tx-hash-----" } };
var btc_to = new List<BitcoinSendOrderTo> { new BitcoinSendOrderTo { Address = "-----to-address-----", Value = 0.02969944m } };
var btc_15 = api.Bitcoin_Send(btc_fromAddress, btc_fromUTXO, btc_to);
var btc_16 = api.Bitcoin_Broadcast("-----tx-data-----", "-----signature-id-----");
```

**Blockchain / Ethereum Endpoints**
```C#
var eth_mnemonics = Mnemonics.Generate(12);
var eth_01 = api.Ethereum_GenerateWallet();
var eth_02 = api.Ethereum_GenerateWallet(eth_mnemonics);
var eth_03 = api.Ethereum_GenerateWallet(string.Join(" ", eth_mnemonics));
var eth_04 = api.Ethereum_GenerateDepositAddress("-----xpub-----", 0);
var eth_05 = api.Ethereum_GeneratePrivateKey(eth_mnemonics, 0);
var eth_06 = api.Ethereum_GeneratePrivateKey(string.Join(" ", eth_mnemonics), 0);
var eth_07 = api.Ethereum_Web3HttpDriver();
var eth_08 = api.Ethereum_GetCurrentBlockNumber();
var eth_09 = api.Ethereum_GetBlock("11606998");
var eth_10 = api.Ethereum_GetBlock("0x91525011ef785e2c6d75540890c30fbcac8553ff4f1f75a2fcc5114e93db0ae0");
var eth_11 = api.Ethereum_ETH_GetBalance("0x65b02db9b67b73f5f1e983ae10796f91ded57b64");
var eth_12 = api.Ethereum_ERC20_GetBalance("0x65b02db9b67b73f5f1e983ae10796f91ded57b64", "0xdac17f958d2ee523a2206206994597c13d831ec7");
var eth_13 = api.Ethereum_ERC20_GetBalance("0x2AA04047B580C29b5bf676E963D8dF393733c7d5", "0x1a77f3c831af5c109f7269e4de735efd2c915aa6");
var eth_14 = api.Ethereum_ERC20_GetBalance("0x2AA04047B580C29b5bf676E963D8dF393733c7d5", "0x1a77f3c831af5c109f7269e4de735efd2c915aa6", 18);
var eth_15 = api.Ethereum_GetTransactionByHash("0x43484947e3f090d9a855bfdd0b36fc4d1a6d1daf3138095370cc342f8f241a80");
var eth_16 = api.Ethereum_GetOutgoingTransactionsCount("0x65b02db9b67b73f5f1e983ae10796f91ded57b64");
var eth_17 = api.Ethereum_GetTransactionsByAddress("0x65b02db9b67b73f5f1e983ae10796f91ded57b64");
var eth_18 = api.Ethereum_Send(EthereumPredefinedCurrency.ETH, "-----amount-----", "-----to-address-----", "-----data-----", "-----signature-id-----", "-----private-key-----");
var eth_19 = api.Ethereum_InvokeSmartContractMethod("-----contract-----", "-----method-name-----", "-----method-abi-----", new List<object> { }, "-----signature-id-----", "-----private-key-----");
var eth_20 = api.Ethereum_ERC20_DeploySmartContract("-----contract-name-----", "-----aymbol-----", "-----supply-----", 18, "-----signature-id-----", "-----private-key-----");
var eth_21 = api.Ethereum_ERC20_Transfer("-----contract-address-----", "-----recipient-address-----", "-----amount-----", 18, "-----signature-id-----", "-----private-key-----");
var eth_22 = api.Ethereum_ERC721_DeploySmartContract("-----contract-name-----", "-----contract-symbol-----", "-----signature-id-----", "-----private-key-----");
var eth_23 = api.Ethereum_ERC721_Mint("-----contract-address-----", "-----token-id-----", "-----recipient-address-----", "-----url-----", "-----signature-id-----", "-----private-key-----");
var eth_24 = api.Ethereum_ERC721_Transfer("-----contract-address-----", "-----token-id-----", "-----recipient-address-----", "-----signature-id-----", "-----private-key-----");
var eth_25 = api.Ethereum_ERC721_MintMultiple("-----contract-address-----", new List<string>(), new List<string>(), "-----signature-id-----", "-----private-key-----");
var eth_26 = api.Ethereum_ERC721_Burn("-----contract-address-----", "-----token-id-----", "-----signature-id-----", "-----private-key-----");
var eth_27 = api.Ethereum_ERC721_GetBalance("-----wallet-address-----", "-----contract-address-----");
var eth_28 = api.Ethereum_ERC721_GetToken("-----address-----", 0, "-----contract-address-----");
var eth_29 = api.Ethereum_ERC721_GetTokenMetadata("-----address-----", "-----contract-address-----");
var eth_30 = api.Ethereum_ERC721_GetTokenOwner("-----address-----", "-----contract-address-----");
var eth_31 = api.Ethereum_Broadcast("-----tx-data-----", "-----signature-id-----");
```

**Blockchain / BitcoinCash Endpoints**
```C#
var bch_mnemonics = Mnemonics.Generate(12);
var bch_01 = api.BitcoinCash_GenerateWallet();
var bch_02 = api.BitcoinCash_GenerateWallet(bch_mnemonics);
var bch_03 = api.BitcoinCash_GenerateWallet(string.Join(" ", bch_mnemonics));
var bch_04 = api.BitcoinCash_GenerateDepositAddress("-----xpub-----", 0);
var bch_05 = api.BitcoinCash_GeneratePrivateKey(bch_mnemonics, 0);
var bch_06 = api.BitcoinCash_GeneratePrivateKey(string.Join(" ", bch_mnemonics), 0);
var bch_07 = api.BitcoinCash_GetBlockchainInformation();
var bch_08 = api.BitcoinCash_GetBlockHash(669153);
var bch_09 = api.BitcoinCash_GetBlock("669153");
var bch_10 = api.BitcoinCash_GetBlock("000000000000000003876f9ec88c2848b62ed35ebc154bd7299c19f1b12a8988");
var bch_11 = api.BitcoinCash_GetTransactionByHash("df3fe80b5689dd707a4f670d3aaed493c39db6b14906cc4e51937c161fd1210a");
var bch_12 = api.BitcoinCash_GetTransactionsByAddress("qrzf7nwy52j98rs5aped95ah3l42884wavaqp6vv4t");
var bch_fromUTXO = new List<BitcoinCashSendOrderFromUTXO> { new BitcoinCashSendOrderFromUTXO { Index = 0, PrivateKey = "-----private-key-----", SignatureId = "-----signature-id-----", TxHash = "53fa-----tx-hash-----a103e8217e1520f5149a4e8c84aeb58e55bdab11164a95e69a8ca50f8fcc" } };
var bch_to = new List<BitcoinCashSendOrderTo> { new BitcoinCashSendOrderTo { Address = "-----to-address-----", Value = 0.02969944m } };
var bch_13 = api.BitcoinCash_Send(bch_fromUTXO, bch_to);
var bch_14 = api.BitcoinCash_Broadcast("-----tx-data-----", "-----signature-id-----");
```

**Blockchain / Litecoin Endpoints**
```C#
var ltc_mnemonics = Mnemonics.Generate(12);
var ltc_01 = api.Litecoin_GenerateWallet();
var ltc_02 = api.Litecoin_GenerateWallet(ltc_mnemonics);
var ltc_03 = api.Litecoin_GenerateWallet(string.Join(" ", ltc_mnemonics));
var ltc_04 = api.Bitcoin_GenerateDepositAddress("-----xpub-----", 0);
var ltc_05 = api.Bitcoin_GeneratePrivateKey(ltc_mnemonics, 0);
var ltc_06 = api.Bitcoin_GeneratePrivateKey(string.Join(" ", ltc_mnemonics), 0);
var ltc_07 = api.Litecoin_GetBlockchainInformation();
var ltc_08 = api.Litecoin_GetBlockHash(1979367);
var ltc_09 = api.Litecoin_GetBlock("1979367");
var ltc_10 = api.Litecoin_GetBlock("b7f6435c93a476b7d62bb573645f185af0a5b1e7c089c30b8dc3a5a14b766b51");
var ltc_11 = api.Litecoin_GetTransactionByHash("44c1fde6323370aca15a841cf5335e72720211f2d5a4eb0952bed6189de6c252");
var ltc_12 = api.Litecoin_GetTransactionsByAddress("LfmssDyX6iZvbVqHv6t9P6JWXia2JG7mdb");
var ltc_13 = api.Litecoin_GetBalance("LfmssDyX6iZvbVqHv6t9P6JWXia2JG7mdb");
var ltc_14 = api.Litecoin_GetTransactionUTXO("44c1fde6323370aca15a841cf5335e72720211f2d5a4eb0952bed6189de6c252", 0);
var ltc_fromAddress = new List<LitecoinSendOrderFromAddress> { new LitecoinSendOrderFromAddress { Address = "-----from-address-----", PrivateKey = "-----private-key-----", SignatureId = "-----signature-id-----" } };
var ltc_fromUTXO = new List<LitecoinSendOrderFromUTXO> { new LitecoinSendOrderFromUTXO { Index = 0, PrivateKey = "-----private-key-----", SignatureId = "-----signature-id-----", TxHash = "-----tx-hash-----" } };
var ltc_to = new List<LitecoinSendOrderTo> { new LitecoinSendOrderTo { Address = "-----to-address-----", Value = 0.02969944m } };
var ltc_15 = api.Litecoin_Send(ltc_fromAddress, ltc_fromUTXO, ltc_to);
var ltc_16 = api.Litecoin_Broadcast("-----tx-data-----", "-----signature-id-----");
```

**Blockchain / Ripple Endpoints**
```C#
var xrp_01 = api.Ripple_GenerateAccount();
var xrp_02 = api.Ripple_GetBlockchainInformation();
var xrp_03 = api.Ripple_GetBlockchainFee();
var xrp_04 = api.Ripple_GetTransactionsByAccount("rUTEn2jLLv4ESmrUqQmhZfEfDN3LorhgvZ");
var xrp_05 = api.Ripple_GetLedger(60769056);
var xrp_06 = api.Ripple_GetTransactionByHash("0F87C3613BECEC5A7D12E3C4F36C267C3C58A8A8100BAE5194006E43265554DE");
var xrp_07 = api.Ripple_GetAccountInfo("rf1BiGeXwwQoi8Z2ueFYTEXSwuJYfV2Jpn");
var xrp_08 = api.Ripple_GetBalance("rf1BiGeXwwQoi8Z2ueFYTEXSwuJYfV2Jpn");
var xrp_09 = api.Ripple_Send("-----from-account-----", "-----to-account-----", "-----amount-----", "-----from-secret-----", "-----signature-id-----", "-----fee-----", "-----source-tag-----", "-----destination-tag-----", "-----issuer-account-----", "-----token-----");
var xrp_10 = api.Ripple_ModifyAccountSettings("-----from-account-----", "-----from-secret-----", "-----signature-id-----", "-----fee-----", true, true);
var xrp_11 = api.Ripple_Broadcast("-----tx-data-----", "-----signature-id-----");
```

**Blockchain / Stellar Endpoints**
```C#
var xlm_01 = api.Stellar_GenerateAccount();
var xlm_02 = api.Stellar_GetBlockchainInformation();
var xlm_03 = api.Stellar_GetBlockchainFee();
var xlm_04 = api.Stellar_GetLedger(33458359);
var xlm_05 = api.Stellar_GetTransactionsInLedger(33458359);
var xlm_06 = api.Stellar_GetTransactionsByAccount("GADICAIA2LMERY6ETEEVYWFU2XZB7R3TBESSU7NGGW3IV7ETF4JW4SD3");
var xlm_07 = api.Stellar_GetTransactionByHash("81f7412e1f0a7483478c0fb945dff60e4e2ce8bdc532c35a85c4780bf3e66b74");
var xlm_08 = api.Stellar_GetAccountInfo("GADICAIA2LMERY6ETEEVYWFU2XZB7R3TBESSU7NGGW3IV7ETF4JW4SD3");
var xlm_09 = api.Stellar_Send("-----from-account-----", "-----to-account-----", "-----amount-----", "-----from-secret-----", "-----signature-id-----", "-----token-----", "-----issuer-account-----", "-----message-----", false);
var xlm_10 = api.Stellar_TrustLine("-----from-account-----", "-----issuer-account-----", "-----token-----", "-----from-secret-----", "-----signature-id-----", "-----limit-----");
var xlm_11 = api.Stellar_Broadcast("-----tx-data-----", "-----signature-id-----");
```

**Blockchain / Records Endpoints**
```C#
var rec_01 = api.Records_SetData(BlockchainType.Ethereum, "-----data-----");
var rec_02 = api.Records_GetData(BlockchainType.Ethereum, "-----id-----");
```

**Blockchain / Binance Endpoints**
```C#
var bnb_01 = api.Binance_GenerateAccount();
var bnb_02 = api.Binance_GetCurrentBlock();
var bnb_03 = api.Binance_GetTransactionsInBlock(137005994);
var bnb_04 = api.Binance_GetAccountInfo("tbnb185tqzq3j6y7yep85lncaz9qeectjxqe5054cgn");
var bnb_05 = api.Binance_GetTransaction("196530CC9B9C908631B8270E918EB72F530800849BEEDA4FB001C64C5CD4C6E1");
var bnb_06 = api.Binance_Send("-----to-address-----", "-----currency-----", "-----amount-----", "-----from-private-key-----", "-----signature-id-----", "-----message-----");
var bnb_07 = api.Binance_Broadcast("-----tx-data-----");
```

**Blockchain / VeChain Endpoints**
```C#
var vet_mnemonics = Mnemonics.Generate(12);
var vet_01 = api.VeChain_GenerateWallet();
var vet_02 = api.VeChain_GenerateWallet(vet_mnemonics);
var vet_03 = api.VeChain_GenerateWallet(string.Join(" ", vet_mnemonics));
var vet_04 = api.VeChain_GenerateDepositAddress("-----xpub-----", 0);
var vet_05 = api.VeChain_GeneratePrivateKey(vet_mnemonics, 0);
var vet_06 = api.VeChain_GeneratePrivateKey(string.Join(" ", vet_mnemonics), 0);
var vet_07 = api.VeChain_GetCurrentBlock();
var vet_09 = api.VeChain_GetBlock("7900534");
var vet_10 = api.VeChain_GetBlock("0x00788d76e966c1e4d51133ccc586a7d24b6fc05652ea9e18d712e98e8d3ca1f2");
var vet_11 = api.VeChain_GetBalance("0xF0237f7F76aF2a88Cbc2EFB2f5458C7E604dEfD3");
var vet_12 = api.VeChain_GetEnergy("0xF0237f7F76aF2a88Cbc2EFB2f5458C7E604dEfD3");
var vet_13 = api.VeChain_GetTransactionByHash("0xdee7980c5272edc018ccad81ee03de381ff95ba5adb9fffb96fc2f93b3c66abb");
var vet_14 = api.VeChain_GetTransactionReceipt("0xdee7980c5272edc018ccad81ee03de381ff95ba5adb9fffb96fc2f93b3c66abb");
var vet_15 = api.VeChain_GetTransactionReceipt("0xec2572d231d8e0defeebefd5edf500976d1cb5ce89c046e5a3e6fe6d23837ca5");
var vet_16 = api.VeChain_Send("-----to-address-----", 10.0m, "-----from-private-key-----", "-----signature-id-----", "-----data-----");
var vet_17 = api.VeChain_EstimateGasForTransaction("-----from-address-----", "-----to-address-----", 10.0m, "-----data-----");
var vet_18 = api.Bitcoin_Broadcast("-----tx-data-----", "-----signature-id-----");
```

**Blockchain / NEO Endpoints**
```C#
var neo_01 = api.Neo_GenerateAccount();
var neo_02 = api.Neo_GetCurrentBlock();
var neo_03 = api.Neo_GetBlock("6747208");
var neo_04 = api.Neo_GetBlock("ba25765688aa4834c8202a17eeb2e3e757c04a4d3e820e08fb7a3a6cb71a69da");
var neo_05 = api.Neo_GetBalance("ALvQkkRRexjoB4cJ3Tcg7NjVJqQpgc9fXZ");
var neo_06 = api.Neo_GetAssetInfo("0x602c79718b16e442de58778e148d0b1084e3b2dffd5de6b7b16cee7969282de7");
var neo_07 = api.Neo_GetUnspentTransactionOutputs("11dfd5514f0d4983984287d20ea7b8fc0c73b17a5d6a8b56db5882276a3d4eec", 0);
var neo_08 = api.Neo_GetTransactionsByAccount("ALvQkkRRexjoB4cJ3Tcg7NjVJqQpgc9fXZ");
var neo_09 = api.Neo_GetContractInfo("883ec9d1950b21f340cc195562c27b2ac0c94dd3");
var neo_10 = api.Neo_GetTransactionByHash("11dfd5514f0d4983984287d20ea7b8fc0c73b17a5d6a8b56db5882276a3d4eec");
var neo_11 = api.Neo_Send("-----to-address-----", 10.0m, 0.0m, "-----from-private-key-----");
var neo_12 = api.Neo_ClaimGAS("-----from-private-key-----");
var neo_13 = api.Neo_SendToken("-----script-hash-----", 10.019m, 3, "-----from-private-key-----", "-----to-address-----");
var neo_14 = api.Neo_Broadcast("-----tx-data-----");
```

**Blockchain / TRON Endpoints**
```C#
var trx_01 = api.Tron_GenerateAccount();
var trx_02 = api.Tron_GetCurrentBlock();
var trx_03 = api.Tron_GetBlock("26669070");
var trx_04 = api.Tron_GetBlock("000000000196f00e8d83c7bd0e294b5566e1f182d5b8d2434df8cad68d93ad8a");
var trx_05 = api.Tron_GetTransactionsByAccount("TY65QiDt4hLTMpf3WRzcX357BnmdxT2sw9");
var trx_06 = api.Tron_GetTransactionByHash("b821cf24dd81edb93b63cbd2e39e79f314c6505d3b728c4de9692fa7bcc6d894");
var trx_07 = api.Tron_Send("-----from-private-key-----", "-----to-address-----", 10.0m);
var trx_08 = api.Tron_Broadcast("-----tx-data-----");
```

**Blockchain / Scrypta Endpoints**
```C#
var lyra_mnemonics = Mnemonics.Generate(12);
var lyra_01 = api.Scrypta_GenerateWallet();
var lyra_02 = api.Scrypta_GenerateWallet(lyra_mnemonics);
var lyra_03 = api.Scrypta_GenerateWallet(string.Join(" ", lyra_mnemonics));
var lyra_04 = api.Scrypta_GenerateDepositAddress("-----xpub-----", 0);
var lyra_05 = api.Scrypta_GeneratePrivateKey(lyra_mnemonics, 0);
var lyra_06 = api.Scrypta_GeneratePrivateKey(string.Join(" ", lyra_mnemonics), 0);
var lyra_07 = api.Scrypta_GetBlockchainInformation();
var lyra_08 = api.Scrypta_GetBlockHash(664827);
var lyra_09 = api.Scrypta_GetBlock("1068234");
var lyra_10 = api.Scrypta_GetBlock("4abed48a6eba78a7a789d10d68b1c43e4057ce35b578867059d873f4917b493a");
var lyra_11 = api.Scrypta_GetTransactionByHash("92c402f3fe01ae17fd93607f6083cb550ea739237988b374e248ff4855fe3a11");
var lyra_12 = api.Scrypta_GetTransactionsByAddress("LRUGaXJQVTL6AZYMw98m1hjdWD7f2BDGN3");
var lyra_13 = api.Scrypta_GetSpendableUTXO("LRUGaXJQVTL6AZYMw98m1hjdWD7f2BDGN3");
var lyra_14 = api.Scrypta_GetTransactionUTXO("92c402f3fe01ae17fd93607f6083cb550ea739237988b374e248ff4855fe3a11", 0);
var lyra_fromAddress = new List<ScryptaSendOrderFromAddress> { new ScryptaSendOrderFromAddress { Address = "-----from-address-----", PrivateKey = "-----private-key-----", SignatureId = "-----signature-id-----" } };
var lyra_fromUTXO = new List<ScryptaSendOrderFromUTXO> { new ScryptaSendOrderFromUTXO { Index = 0, PrivateKey = "-----private-key-----", SignatureId = "-----signature-id-----", TxHash = "-----tx-hash-----" } };
var lyra_to = new List<ScryptaSendOrderTo> { new ScryptaSendOrderTo { Address = "-----to-address-----", Value = 0.02969944m } };
var lyra_15 = api.Scrypta_Send(lyra_fromAddress, lyra_fromUTXO, lyra_to);
var lyra_16 = api.Scrypta_Broadcast("-----tx-data-----", "-----signature-id-----");
```

**Tatum / Service Endpoints**
```C#
var service_01 = api.Service_GetConsumptions();
var service_02 = api.Service_GetExchangeRates("-----currency-----","-----base-pair-----");
var service_03 = api.Service_GetVersion();
```

## Release Notes
* Version 1.1.0 - 12 Jan 2021
    * Completed All Rest Api Endpoints

* Version 1.0.0 - 10 Jan 2021
    * First Release