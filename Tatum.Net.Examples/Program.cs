using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiter;
using System;
using System.Collections.Generic;
using Tatum.Net.CoreObjects;
using Tatum.Net.Enums;
using Tatum.Net.Helpers;
using Tatum.Net.RestObjects;
using Tatum.Net.WalletObjects;

namespace Tatum.Net.Examples
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            /* Examples Console */
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Select examples to run");
                Console.WriteLine("1: TatumApiClient");
                Console.WriteLine("2: TatumWalletManager");
                Console.WriteLine("9: Exit");
                var selection = Console.ReadLine();
                if (selection.ToInt() == 1) TatumApiClientExamples();
                else if (selection.ToInt() == 2) TatumWalletManagerExamples();
                else if (selection.ToInt() == 9) break;
                else { Console.WriteLine("Invalid selection! Press <ENTER> to try again"); Console.ReadLine(); }
            }

            /* Done */
            Console.WriteLine("Finished");
            Console.ReadLine();
        }

        private static void TatumApiClientExamples()
        {
            /* Tatum Api Client */
            var cli_options = new TatumClientOptions
            {
                RateLimiters = new List<IRateLimiter> { new RateLimiterCredit(5, TimeSpan.FromMilliseconds(1000)), },
                RateLimitingBehaviour = RateLimitingBehaviour.Wait,
            };
            var api = new TatumClient("XXXXXXXX-API-KEY-XXXXXXXX", cli_options);

            /* Ledger / Account */
            var led_01 = api.Ledger.Account.Create(BlockchainType.BitcoinCash);
            var led_02 = api.Ledger.Account.GetAccounts();
            var led_03 = api.Ledger.Account.CreateBatch(new List<LedgerAccountOptions> { });
            var led_04 = api.Ledger.Account.GetByCustomerId("-----customer-id-----");
            var led_05 = api.Ledger.Account.GetById("-----account-id-----");
            var led_06 = api.Ledger.Account.Update("-----account-id-----", "-----account-code-----", "-----account-number-----");
            var led_07 = api.Ledger.Account.GetBalance("-----account-id-----");
            var led_08 = api.Ledger.Account.BlockAmount("-----account-id-----", 1000, "DEBIT_CARD_OP", "Description");
            var led_09 = api.Ledger.Account.UnlockAmountAndPerformTransaction("-----blockage-id-----", "-----recipient-account-id-----", 1000.0m, false, false, "-----tx-code-----", "-----payment-id-----", "-----recipient-notes-----", "-----sender-notes-----", 1.0m);
            var led_10 = api.Ledger.Account.UnblockAmount("-----blockage-id-----");
            var led_11 = api.Ledger.Account.GetBlockedAmounts("-----account-id-----");
            var led_12 = api.Ledger.Account.UnblockAllBlockedAmounts("-----account-id-----");
            var led_13 = api.Ledger.Account.Activate("-----account-id-----");
            var led_14 = api.Ledger.Account.Deactivate("-----account-id-----");
            var led_15 = api.Ledger.Account.Freeze("-----account-id-----");
            var led_16 = api.Ledger.Account.Unfreeze("-----account-id-----");

            /* Ledger / Transaction */
            var led_21 = api.Ledger.Transaction.SendPayment("-----sender-account-id-----", "-----recipient-account-id-----", 10.0m);
            var led_22 = api.Ledger.Transaction.GetTransactionsByAccount("-----account-id-----");
            var led_23 = api.Ledger.Transaction.GetTransactionsByCustomer("-----customer-id-----");
            var led_24 = api.Ledger.Transaction.GetTransactionsByLedger();
            var led_25 = api.Ledger.Transaction.GetTransactionsByReference("-----reference-----");

            /* Ledger / Customer */
            var led_31 = api.Ledger.Customer.ListAll();
            var led_32 = api.Ledger.Customer.Get("-----customer-id-----");
            var led_33 = api.Ledger.Customer.Update("-----customer-internal-id-----", "-----customer-external-id-----");
            var led_34 = api.Ledger.Customer.Activate("-----customer-id-----");
            var led_35 = api.Ledger.Customer.Deactivate("-----customer-id-----");
            var led_36 = api.Ledger.Customer.Enable("-----customer-id-----");
            var led_37 = api.Ledger.Customer.Disable("-----customer-id-----");

            /* Ledger / Virtual Currency */
            var led_41 = api.Ledger.VirtualCurrency.Create("-----name-----", "-----supply-----", "-----base-pair-----");
            var led_42 = api.Ledger.VirtualCurrency.Update("-----name-----", "-----base-pair-----");
            var led_43 = api.Ledger.VirtualCurrency.Get("-----name-----");
            var led_44 = api.Ledger.VirtualCurrency.Mint("-----account-id-----", 1000000, "-----payment-id-----", "-----reference-----", "-----transaction-code-----", "-----recipient-note-----", "-----counter-account-----", "-----sender-note-----");
            var led_45 = api.Ledger.VirtualCurrency.Destroy("-----account-id-----", 1000000, "-----payment-id-----", "-----reference-----", "-----transaction-code-----", "-----recipient-note-----", "-----counter-account-----", "-----sender-note-----");

            /* Ledger / Subscription */
            var led_51 = api.Ledger.Subscription.Create(LedgerSubscriptionType.ACCOUNT_INCOMING_BLOCKCHAIN_TRANSACTION, account_id: "-----account-id-----", url: "https://www.google.com");
            var led_52 = api.Ledger.Subscription.List();
            var led_53 = api.Ledger.Subscription.Cancel("-----subscription-id-----");
            var led_54 = api.Ledger.Subscription.GetReport("-----subscription-id-----");

            /* Ledger / Order Book */
            var led_61 = api.Ledger.OrderBook.GetHistoricalTrades("-----account-id-----", "-----trade-pair-----");
            var led_62 = api.Ledger.OrderBook.GetBuyTrades("-----account-id-----");
            var led_63 = api.Ledger.OrderBook.GetSellTrades("-----account-id-----");
            var led_64 = api.Ledger.OrderBook.PlaceOrder(LedgerTradeType.Buy, 1300.05m, 15.87m, "-----trade-pair-----", "-----currency1-account-id-----", "-----currency2-account-id-----");
            var led_65 = api.Ledger.OrderBook.GetTrade("-----trade-id-----");
            var led_66 = api.Ledger.OrderBook.CancelOrder("-----trade-id-----");
            var led_67 = api.Ledger.OrderBook.CancelAllOrders("-----account-id-----");

            /* Security / Key Management System */
            var kms_01 = api.KMS.GetPendingTransactions(BlockchainType.Bitcoin);
            var kms_02 = api.KMS.GetTransaction("-----tx-id-----");
            var kms_03 = api.KMS.CompletePendingTransaction("-----id-----", "-----tx-id-----");
            var kms_04 = api.KMS.DeleteTransaction("-----id-----");

            /* Security / Address */
            var sec_01 = api.Security.CheckMalicousAddress("33WbRKqt7wXARVdAJSu1G1x3QnbyPtZ2bH");

            /* Off-Chain / Account */
            var off_btc = api.Bitcoin.GenerateWallet();
            var off_led = api.Ledger.Account.Create(BlockchainType.Bitcoin, new LedgerAccountOptions { ExtendedPublicKey = off_btc.Data.ExtendedPublicKey, AccountingCurrency = FiatCurrency.USD });
            var off_01 = api.OffChain.Account.GenerateDepositAddress(off_led.Data.Id, 0);
            var off_02 = api.OffChain.Account.GetAllDepositAddresses("-----account-id-----");
            var off_03 = api.OffChain.Account.GenerateMultipleDepositAddresses(new List<OffchainDepositAddressRequest> { });
            var off_04 = api.OffChain.Account.CheckAddress(BlockchainType.Bitcoin, "1HeYDxSrPahLfD1dbfxaKWEgAq4uV3hDox");
            var off_05 = api.OffChain.Account.RemoveAddressFromAccount("-----account-id-----", "1HeYDxSrPahLfD1dbfxaKWEgAq4uV3hDox");
            var off_06 = api.OffChain.Account.AssignAddressToAccount("-----account-id-----", "1HeYDxSrPahLfD1dbfxaKWEgAq4uV3hDox");

            /* Off-Chain / Blockchain */
            var off_priv_11 = api.Bitcoin.GeneratePrivateKey("-----mnemonics-----", 0);
            var off_11 = api.OffChain.Blockchain.SendBitcoin("-----account-id-----", "-----address-----", 0.01m, mnemonic: "-----mnemonics-----", xpub: "-----xpub-----");
            var off_11_kms = api.KMS.CompletePendingTransaction("-----account-id-----", off_11.Data.Id);

            var off_priv_12 = api.BitcoinCash.GeneratePrivateKey("-----mnemonics-----", 0);
            var off_12 = api.OffChain.Blockchain.SendBitcoinCash("-----account-id-----", "-----address-----", 0.01m, mnemonic: "-----mnemonics-----", xpub: "-----xpub-----");
            var off_12_kms = api.KMS.CompletePendingTransaction("-----account-id-----", off_12.Data.Id);

            var off_priv_13 = api.Litecoin.GeneratePrivateKey("-----mnemonics-----", 0);
            var off_13 = api.OffChain.Blockchain.SendLitecoin("-----account-id-----", "-----address-----", 0.01m, mnemonic: "-----mnemonics-----", xpub: "-----xpub-----");
            var off_13_kms = api.KMS.CompletePendingTransaction("-----account-id-----", off_13.Data.Id);

            var off_priv_14 = api.Ethereum.GeneratePrivateKey("-----mnemonics-----", 0);
            var off_14 = api.OffChain.Blockchain.SendEthereum("-----account-id-----", "-----address-----", 0.01m, mnemonic: "-----mnemonics-----");
            var off_14_kms = api.KMS.CompletePendingTransaction("-----account-id-----", off_14.Data.Id);

            var off_15 = api.OffChain.Blockchain.CreateERC20Token("-----symbol-----", "-----supply-----", "-----description-----", "-----base-pair-----");
            var off_16 = api.OffChain.Blockchain.DeployERC20Token("-----symbol-----", "-----supply-----", "-----description-----", "-----base-pair-----");
            var off_17 = api.OffChain.Blockchain.SetERC20TokenContractAddress("-----address-----", "-----symbol-----");

            var off_priv_18 = api.Ethereum.GeneratePrivateKey("-----mnemonics-----", 0);
            var off_18 = api.OffChain.Blockchain.SendERC20Token("-----account-id-----", "-----address-----", 0.01m, mnemonic: "-----mnemonics-----");
            var off_18_kms = api.KMS.CompletePendingTransaction("-----account-id-----", off_18.Data.Id);

            var off_19 = api.OffChain.Blockchain.SendStellar("-----account-id-----", "-----account-----", "-----address-----", 0.01m, "-----secret-----");
            var off_19_kms = api.KMS.CompletePendingTransaction("-----account-id-----", off_19.Data.Id);

            var off_20 = api.OffChain.Blockchain.CreateXLMAsset("-----issuer-account-----", "-----token-----", "-----base-pair-----");

            var off_21 = api.OffChain.Blockchain.SendRipple("-----account-id-----", "-----account-----", "-----address-----", 0.01m, secret: "-----secret-----");
            var off_21_kms = api.KMS.CompletePendingTransaction("-----account-id-----", off_21.Data.Id);

            var off_22 = api.OffChain.Blockchain.CreateXRPAsset("-----issuer-account-----", "-----token-----", "-----base-pair-----");

            var off_23 = api.OffChain.Blockchain.SendBNB("-----account-id-----", "-----address-----", 0.01m, privateKey: "-----private-key-----");
            var off_23_kms = api.KMS.CompletePendingTransaction("-----account-id-----", off_23.Data.Id);

            var off_24 = api.OffChain.Blockchain.CreateBNBAsset("-----token-----", "-----base-pair-----");

            /* Off-Chain / Withdrawal */
            var off_31 = api.OffChain.Withdrawal.Request("-----account-id-----", "-----address-----", 15.97m);
            var off_32 = api.OffChain.Withdrawal.CompleteRequest("-----withdrawal-id-----", "-----txid-----");
            var off_33 = api.OffChain.Withdrawal.Cancel("-----withdrawal-id-----");
            var off_34 = api.OffChain.Withdrawal.Broadcast("-----currency-----", "-----tx-data-----", "-----withdrawal-id-----", "-----signature-id-----");

            /* Blockchain / Bitcoin */
            var btc_mnemonics = Mnemonics.Generate(12);
            var btc_01 = api.Bitcoin.GenerateWallet();
            var btc_02 = api.Bitcoin.GenerateWallet(btc_mnemonics);
            var btc_03 = api.Bitcoin.GenerateWallet(string.Join(" ", btc_mnemonics));
            var btc_04 = api.Bitcoin.GenerateDepositAddress("-----xpub-----", 0);
            var btc_05 = api.Bitcoin.GeneratePrivateKey(btc_mnemonics, 0);
            var btc_06 = api.Bitcoin.GeneratePrivateKey(string.Join(" ", btc_mnemonics), 0);
            var btc_07 = api.Bitcoin.GetBlockchainInformation();
            var btc_08 = api.Bitcoin.GetBlockHash(664827);
            var btc_09 = api.Bitcoin.GetBlock("664827");
            var btc_10 = api.Bitcoin.GetBlock("0000000000000000000c2b526c01f6885dec2135214b7c742bf3ce1d386e6ffb");
            var btc_11 = api.Bitcoin.GetTransactionByHash("8d05fbe88dc38ab6c339e422ea3682b48fb406fcbac0400064301b0ac2fdecdd");
            var btc_12 = api.Bitcoin.GetTransactionsByAddress("33WbRKqt7wXARVdAJSu1G1x3QnbyPtZ2bH");
            var btc_13 = api.Bitcoin.GetBalance("33WbRKqt7wXARVdAJSu1G1x3QnbyPtZ2bH");
            var btc_14 = api.Bitcoin.GetTransactionUTXO("8d05fbe88dc38ab6c339e422ea3682b48fb406fcbac0400064301b0ac2fdecdd", 0);
            var btc_fromAddress = new List<BitcoinSendOrderFromAddress> { new BitcoinSendOrderFromAddress { Address = "-----from-address-----", PrivateKey = "-----private-key-----", SignatureId = "-----signature-id-----" } };
            var btc_fromUTXO = new List<BitcoinSendOrderFromUTXO> { new BitcoinSendOrderFromUTXO { Index = 0, PrivateKey = "-----private-key-----", SignatureId = "-----signature-id-----", TxHash = "-----tx-hash-----" } };
            var btc_to = new List<BitcoinSendOrderTo> { new BitcoinSendOrderTo { Address = "-----to-address-----", Value = 0.02969944m } };
            var btc_15 = api.Bitcoin.Send(btc_fromAddress, btc_fromUTXO, btc_to);
            var btc_16 = api.Bitcoin.Broadcast("-----tx-data-----", "-----signature-id-----");

            /* Blockchain / Ethereum */
            var eth_mnemonics = Mnemonics.Generate(12);
            var eth_01 = api.Ethereum.GenerateWallet();
            var eth_02 = api.Ethereum.GenerateWallet(eth_mnemonics);
            var eth_03 = api.Ethereum.GenerateWallet(string.Join(" ", eth_mnemonics));
            var eth_04 = api.Ethereum.GenerateDepositAddress("-----xpub-----", 0);
            var eth_05 = api.Ethereum.GeneratePrivateKey(eth_mnemonics, 0);
            var eth_06 = api.Ethereum.GeneratePrivateKey(string.Join(" ", eth_mnemonics), 0);
            var eth_07 = api.Ethereum.Web3HttpDriver();
            var eth_08 = api.Ethereum.GetCurrentBlockNumber();
            var eth_09 = api.Ethereum.GetBlock("11606998");
            var eth_10 = api.Ethereum.GetBlock("0x91525011ef785e2c6d75540890c30fbcac8553ff4f1f75a2fcc5114e93db0ae0");
            var eth_11 = api.Ethereum.ETH_GetBalance("0x65b02db9b67b73f5f1e983ae10796f91ded57b64");
            var eth_12 = api.Ethereum.ERC20_GetBalance("0x65b02db9b67b73f5f1e983ae10796f91ded57b64", "0xdac17f958d2ee523a2206206994597c13d831ec7");
            var eth_13 = api.Ethereum.ERC20_GetBalance("0x2AA04047B580C29b5bf676E963D8dF393733c7d5", "0x1a77f3c831af5c109f7269e4de735efd2c915aa6");
            var eth_14 = api.Ethereum.ERC20_GetBalance("0x2AA04047B580C29b5bf676E963D8dF393733c7d5", "0x1a77f3c831af5c109f7269e4de735efd2c915aa6", 18);
            var eth_15 = api.Ethereum.GetTransactionByHash("0x43484947e3f090d9a855bfdd0b36fc4d1a6d1daf3138095370cc342f8f241a80");
            var eth_16 = api.Ethereum.GetOutgoingTransactionsCount("0x65b02db9b67b73f5f1e983ae10796f91ded57b64");
            var eth_17 = api.Ethereum.GetTransactionsByAddress("0x65b02db9b67b73f5f1e983ae10796f91ded57b64");
            var eth_18 = api.Ethereum.Send(EthereumPredefinedCurrency.ETH, "-----amount-----", "-----to-address-----", "-----data-----", "-----signature-id-----", "-----private-key-----");
            var eth_19 = api.Ethereum.InvokeSmartContractMethod("-----contract-----", "-----method-name-----", "-----method-abi-----", new List<object> { }, "-----signature-id-----", "-----private-key-----");
            var eth_20 = api.Ethereum.ERC20_DeploySmartContract("-----contract-name-----", "-----aymbol-----", "-----supply-----", 18, "-----signature-id-----", "-----private-key-----");
            var eth_21 = api.Ethereum.ERC20_Transfer("-----contract-address-----", "-----recipient-address-----", "-----amount-----", 18, "-----signature-id-----", "-----private-key-----");
            var eth_22 = api.Ethereum.ERC721_DeploySmartContract("-----contract-name-----", "-----contract-symbol-----", "-----signature-id-----", "-----private-key-----");
            var eth_23 = api.Ethereum.ERC721_Mint("-----contract-address-----", "-----token-id-----", "-----recipient-address-----", "-----url-----", "-----signature-id-----", "-----private-key-----");
            var eth_24 = api.Ethereum.ERC721_Transfer("-----contract-address-----", "-----token-id-----", "-----recipient-address-----", "-----signature-id-----", "-----private-key-----");
            var eth_25 = api.Ethereum.ERC721_MintMultiple("-----contract-address-----", new List<string>(), new List<string>(), "-----signature-id-----", "-----private-key-----");
            var eth_26 = api.Ethereum.ERC721_Burn("-----contract-address-----", "-----token-id-----", "-----signature-id-----", "-----private-key-----");
            var eth_27 = api.Ethereum.ERC721_GetBalance("-----wallet-address-----", "-----contract-address-----");
            var eth_28 = api.Ethereum.ERC721_GetToken("-----address-----", 0, "-----contract-address-----");
            var eth_29 = api.Ethereum.ERC721_GetTokenMetadata("-----address-----", "-----contract-address-----");
            var eth_30 = api.Ethereum.ERC721_GetTokenOwner("-----address-----", "-----contract-address-----");
            var eth_31 = api.Ethereum.Broadcast("-----tx-data-----", "-----signature-id-----");

            /* Blockchain / BitcoinCash */
            var bch_mnemonics = Mnemonics.Generate(12);
            var bch_01 = api.BitcoinCash.GenerateWallet();
            var bch_02 = api.BitcoinCash.GenerateWallet(bch_mnemonics);
            var bch_03 = api.BitcoinCash.GenerateWallet(string.Join(" ", bch_mnemonics));
            var bch_04 = api.BitcoinCash.GenerateDepositAddress("-----xpub-----", 0);
            var bch_05 = api.BitcoinCash.GeneratePrivateKey(bch_mnemonics, 0);
            var bch_06 = api.BitcoinCash.GeneratePrivateKey(string.Join(" ", bch_mnemonics), 0);
            var bch_07 = api.BitcoinCash.GetBlockchainInformation();
            var bch_08 = api.BitcoinCash.GetBlockHash(669153);
            var bch_09 = api.BitcoinCash.GetBlock("669153");
            var bch_10 = api.BitcoinCash.GetBlock("000000000000000003876f9ec88c2848b62ed35ebc154bd7299c19f1b12a8988");
            var bch_11 = api.BitcoinCash.GetTransactionByHash("df3fe80b5689dd707a4f670d3aaed493c39db6b14906cc4e51937c161fd1210a");
            var bch_12 = api.BitcoinCash.GetTransactionsByAddress("qrzf7nwy52j98rs5aped95ah3l42884wavaqp6vv4t");
            var bch_fromUTXO = new List<BitcoinCashSendOrderFromUTXO> { new BitcoinCashSendOrderFromUTXO { Index = 0, PrivateKey = "-----private-key-----", SignatureId = "-----signature-id-----", TxHash = "53fa-----tx-hash-----a103e8217e1520f5149a4e8c84aeb58e55bdab11164a95e69a8ca50f8fcc" } };
            var bch_to = new List<BitcoinCashSendOrderTo> { new BitcoinCashSendOrderTo { Address = "-----to-address-----", Value = 0.02969944m } };
            var bch_13 = api.BitcoinCash.Send(bch_fromUTXO, bch_to);
            var bch_14 = api.BitcoinCash.Broadcast("-----tx-data-----", "-----signature-id-----");

            /* Blockchain / Litecoin */
            var ltc_mnemonics = Mnemonics.Generate(12);
            var ltc_01 = api.Litecoin.GenerateWallet();
            var ltc_02 = api.Litecoin.GenerateWallet(ltc_mnemonics);
            var ltc_03 = api.Litecoin.GenerateWallet(string.Join(" ", ltc_mnemonics));
            var ltc_04 = api.Litecoin.GenerateDepositAddress("-----xpub-----", 0);
            var ltc_05 = api.Litecoin.GeneratePrivateKey(ltc_mnemonics, 0);
            var ltc_06 = api.Litecoin.GeneratePrivateKey(string.Join(" ", ltc_mnemonics), 0);
            var ltc_07 = api.Litecoin.GetBlockchainInformation();
            var ltc_08 = api.Litecoin.GetBlockHash(1979367);
            var ltc_09 = api.Litecoin.GetBlock("1979367");
            var ltc_10 = api.Litecoin.GetBlock("b7f6435c93a476b7d62bb573645f185af0a5b1e7c089c30b8dc3a5a14b766b51");
            var ltc_11 = api.Litecoin.GetTransactionByHash("44c1fde6323370aca15a841cf5335e72720211f2d5a4eb0952bed6189de6c252");
            var ltc_12 = api.Litecoin.GetTransactionsByAddress("LfmssDyX6iZvbVqHv6t9P6JWXia2JG7mdb");
            var ltc_13 = api.Litecoin.GetBalance("LfmssDyX6iZvbVqHv6t9P6JWXia2JG7mdb");
            var ltc_14 = api.Litecoin.GetTransactionUTXO("44c1fde6323370aca15a841cf5335e72720211f2d5a4eb0952bed6189de6c252", 0);
            var ltc_fromAddress = new List<LitecoinSendOrderFromAddress> { new LitecoinSendOrderFromAddress { Address = "-----from-address-----", PrivateKey = "-----private-key-----", SignatureId = "-----signature-id-----" } };
            var ltc_fromUTXO = new List<LitecoinSendOrderFromUTXO> { new LitecoinSendOrderFromUTXO { Index = 0, PrivateKey = "-----private-key-----", SignatureId = "-----signature-id-----", TxHash = "-----tx-hash-----" } };
            var ltc_to = new List<LitecoinSendOrderTo> { new LitecoinSendOrderTo { Address = "-----to-address-----", Value = 0.02969944m } };
            var ltc_15 = api.Litecoin.Send(ltc_fromAddress, ltc_fromUTXO, ltc_to);
            var ltc_16 = api.Litecoin.Broadcast("-----tx-data-----", "-----signature-id-----");

            /* Blockchain / Ripple */
            var xrp_01 = api.Ripple.GenerateAccount();
            var xrp_02 = api.Ripple.GetBlockchainInformation();
            var xrp_03 = api.Ripple.GetBlockchainFee();
            var xrp_04 = api.Ripple.GetTransactionsByAccount("rUTEn2jLLv4ESmrUqQmhZfEfDN3LorhgvZ");
            var xrp_05 = api.Ripple.GetLedger(60769056);
            var xrp_06 = api.Ripple.GetTransactionByHash("0F87C3613BECEC5A7D12E3C4F36C267C3C58A8A8100BAE5194006E43265554DE");
            var xrp_07 = api.Ripple.GetAccountInfo("rf1BiGeXwwQoi8Z2ueFYTEXSwuJYfV2Jpn");
            var xrp_08 = api.Ripple.GetBalance("rf1BiGeXwwQoi8Z2ueFYTEXSwuJYfV2Jpn");
            var xrp_09 = api.Ripple.Send("-----from-account-----", "-----to-account-----", "-----amount-----", "-----from-secret-----", "-----signature-id-----", "-----fee-----", "-----source-tag-----", "-----destination-tag-----", "-----issuer-account-----", "-----token-----");
            var xrp_10 = api.Ripple.ModifyAccountSettings("-----from-account-----", "-----from-secret-----", "-----signature-id-----", "-----fee-----", true, true);
            var xrp_11 = api.Ripple.Broadcast("-----tx-data-----", "-----signature-id-----");

            /* Blockchain / Stellar */
            var xlm_01 = api.Stellar.GenerateAccount();
            var xlm_02 = api.Stellar.GetBlockchainInformation();
            var xlm_03 = api.Stellar.GetBlockchainFee();
            var xlm_04 = api.Stellar.GetLedger(33458359);
            var xlm_05 = api.Stellar.GetTransactionsInLedger(33458359);
            var xlm_06 = api.Stellar.GetTransactionsByAccount("GADICAIA2LMERY6ETEEVYWFU2XZB7R3TBESSU7NGGW3IV7ETF4JW4SD3");
            var xlm_07 = api.Stellar.GetTransactionByHash("81f7412e1f0a7483478c0fb945dff60e4e2ce8bdc532c35a85c4780bf3e66b74");
            var xlm_08 = api.Stellar.GetAccountInfo("GADICAIA2LMERY6ETEEVYWFU2XZB7R3TBESSU7NGGW3IV7ETF4JW4SD3");
            var xlm_09 = api.Stellar.Send("-----from-account-----", "-----to-account-----", "-----amount-----", "-----from-secret-----", "-----signature-id-----", "-----token-----", "-----issuer-account-----", "-----message-----", false);
            var xlm_10 = api.Stellar.TrustLine("-----from-account-----", "-----issuer-account-----", "-----token-----", "-----from-secret-----", "-----signature-id-----", "-----limit-----");
            var xlm_11 = api.Stellar.Broadcast("-----tx-data-----", "-----signature-id-----");

            /* Blockchain / Records */
            var rec_01 = api.Records.SetData(BlockchainType.Ethereum, "-----data-----");
            var rec_02 = api.Records.GetData(BlockchainType.Ethereum, "-----id-----");

            /* Blockchain / Binance */
            var bnb_01 = api.Binance.GenerateAccount();
            var bnb_02 = api.Binance.GetCurrentBlock();
            var bnb_03 = api.Binance.GetTransactionsInBlock(137005994);
            var bnb_04 = api.Binance.GetAccountInfo("tbnb185tqzq3j6y7yep85lncaz9qeectjxqe5054cgn");
            var bnb_05 = api.Binance.GetTransaction("196530CC9B9C908631B8270E918EB72F530800849BEEDA4FB001C64C5CD4C6E1");
            var bnb_06 = api.Binance.Send("-----to-address-----", "-----currency-----", "-----amount-----", "-----from-private-key-----", "-----signature-id-----", "-----message-----");
            var bnb_07 = api.Binance.Broadcast("-----tx-data-----");

            /* Blockchain / VeChain */
            var vet_mnemonics = Mnemonics.Generate(12);
            var vet_01 = api.VeChain.GenerateWallet();
            var vet_02 = api.VeChain.GenerateWallet(vet_mnemonics);
            var vet_03 = api.VeChain.GenerateWallet(string.Join(" ", vet_mnemonics));
            var vet_04 = api.VeChain.GenerateDepositAddress("-----xpub-----", 0);
            var vet_05 = api.VeChain.GeneratePrivateKey(vet_mnemonics, 0);
            var vet_06 = api.VeChain.GeneratePrivateKey(string.Join(" ", vet_mnemonics), 0);
            var vet_07 = api.VeChain.GetCurrentBlock();
            var vet_09 = api.VeChain.GetBlock("7900534");
            var vet_10 = api.VeChain.GetBlock("0x00788d76e966c1e4d51133ccc586a7d24b6fc05652ea9e18d712e98e8d3ca1f2");
            var vet_11 = api.VeChain.GetBalance("0xF0237f7F76aF2a88Cbc2EFB2f5458C7E604dEfD3");
            var vet_12 = api.VeChain.GetEnergy("0xF0237f7F76aF2a88Cbc2EFB2f5458C7E604dEfD3");
            var vet_13 = api.VeChain.GetTransactionByHash("0xdee7980c5272edc018ccad81ee03de381ff95ba5adb9fffb96fc2f93b3c66abb");
            var vet_14 = api.VeChain.GetTransactionReceipt("0xdee7980c5272edc018ccad81ee03de381ff95ba5adb9fffb96fc2f93b3c66abb");
            var vet_15 = api.VeChain.GetTransactionReceipt("0xec2572d231d8e0defeebefd5edf500976d1cb5ce89c046e5a3e6fe6d23837ca5");
            var vet_16 = api.VeChain.Send("-----to-address-----", 10.0m, "-----from-private-key-----", "-----signature-id-----", "-----data-----");
            var vet_17 = api.VeChain.EstimateGasForTransaction("-----from-address-----", "-----to-address-----", 10.0m, "-----data-----");
            var vet_18 = api.Bitcoin.Broadcast("-----tx-data-----", "-----signature-id-----");

            /* Blockchain / NEO */
            var neo_01 = api.NEO.GenerateAccount();
            var neo_02 = api.NEO.GetCurrentBlock();
            var neo_03 = api.NEO.GetBlock("6747208");
            var neo_04 = api.NEO.GetBlock("ba25765688aa4834c8202a17eeb2e3e757c04a4d3e820e08fb7a3a6cb71a69da");
            var neo_05 = api.NEO.GetBalance("ALvQkkRRexjoB4cJ3Tcg7NjVJqQpgc9fXZ");
            var neo_06 = api.NEO.GetAssetInfo("0x602c79718b16e442de58778e148d0b1084e3b2dffd5de6b7b16cee7969282de7");
            var neo_07 = api.NEO.GetUnspentTransactionOutputs("11dfd5514f0d4983984287d20ea7b8fc0c73b17a5d6a8b56db5882276a3d4eec", 0);
            var neo_08 = api.NEO.GetTransactionsByAccount("ALvQkkRRexjoB4cJ3Tcg7NjVJqQpgc9fXZ");
            var neo_09 = api.NEO.GetContractInfo("883ec9d1950b21f340cc195562c27b2ac0c94dd3");
            var neo_10 = api.NEO.GetTransactionByHash("11dfd5514f0d4983984287d20ea7b8fc0c73b17a5d6a8b56db5882276a3d4eec");
            var neo_11 = api.NEO.Send("-----to-address-----", 10.0m, 0.0m, "-----from-private-key-----");
            var neo_12 = api.NEO.ClaimGAS("-----from-private-key-----");
            var neo_13 = api.NEO.SendToken("-----script-hash-----", 10.019m, 3, "-----from-private-key-----", "-----to-address-----");
            var neo_14 = api.NEO.Broadcast("-----tx-data-----");

            /* Blockchain / TRON */
            var trx_01 = api.Tron.GenerateAccount();
            var trx_02 = api.Tron.GetCurrentBlock();
            var trx_03 = api.Tron.GetBlock("26669070");
            var trx_04 = api.Tron.GetBlock("000000000196f00e8d83c7bd0e294b5566e1f182d5b8d2434df8cad68d93ad8a");
            var trx_05 = api.Tron.GetTransactionsByAccount("TY65QiDt4hLTMpf3WRzcX357BnmdxT2sw9");
            var trx_06 = api.Tron.GetTransactionByHash("b821cf24dd81edb93b63cbd2e39e79f314c6505d3b728c4de9692fa7bcc6d894");
            var trx_07 = api.Tron.Send("-----from-private-key-----", "-----to-address-----", 10.0m);
            var trx_08 = api.Tron.Broadcast("-----tx-data-----");
            var trx_09 = api.Tron.FreezeBalance("-----from-private-key-----", "-----receiver-----", 3, "-----resource-----", 15.0m);
            var trx_10 = api.Tron.TRC10GetTokenDetails(1000540);
            var trx_11 = api.Tron.TRC10CreateToken("-----from-private-key-----", "-----recipient-----", "-----name-----", "-----abbreviation-----", "-----description-----", "-----url-----", 300000000, 2);
            var trx_12 = api.Tron.TRC10Send("-----from-private-key-----", "-----to-----", 1000540, 15.0m);
            var trx_13 = api.Tron.TRC20CreateToken("-----from-private-key-----", "-----recipient-----", "-----name-----", "-----symbol-----", 300000000, 2);
            var trx_14 = api.Tron.TRC20Send("-----from-private-key-----", "-----to-----", "-----token-address-----", 15.0m, 0.01m);

            /* Blockchain / Scrypta */
            var scr_mnemonics = Mnemonics.Generate(12);
            var scr_01 = api.Scrypta.GenerateWallet();
            var scr_02 = api.Scrypta.GenerateWallet(scr_mnemonics);
            var scr_03 = api.Scrypta.GenerateWallet(string.Join(" ", scr_mnemonics));
            var scr_04 = api.Scrypta.GenerateDepositAddress("-----xpub-----", 0);
            var scr_05 = api.Scrypta.GeneratePrivateKey(scr_mnemonics, 0);
            var scr_06 = api.Scrypta.GeneratePrivateKey(string.Join(" ", scr_mnemonics), 0);
            var scr_07 = api.Scrypta.GetBlockchainInformation();
            var scr_08 = api.Scrypta.GetBlockHash(664827);
            var scr_09 = api.Scrypta.GetBlock("1068234");
            var scr_10 = api.Scrypta.GetBlock("4abed48a6eba78a7a789d10d68b1c43e4057ce35b578867059d873f4917b493a");
            var scr_11 = api.Scrypta.GetTransactionByHash("92c402f3fe01ae17fd93607f6083cb550ea739237988b374e248ff4855fe3a11");
            var scr_12 = api.Scrypta.GetTransactionsByAddress("LRUGaXJQVTL6AZYMw98m1hjdWD7f2BDGN3");
            var scr_13 = api.Scrypta.GetSpendableUTXO("LRUGaXJQVTL6AZYMw98m1hjdWD7f2BDGN3");
            var scr_14 = api.Scrypta.GetTransactionUTXO("92c402f3fe01ae17fd93607f6083cb550ea739237988b374e248ff4855fe3a11", 0);
            var scr_fromAddress = new List<ScryptaSendOrderFromAddress> { new ScryptaSendOrderFromAddress { Address = "-----from-address-----", PrivateKey = "-----private-key-----", SignatureId = "-----signature-id-----" } };
            var scr_fromUTXO = new List<ScryptaSendOrderFromUTXO> { new ScryptaSendOrderFromUTXO { Index = 0, PrivateKey = "-----private-key-----", SignatureId = "-----signature-id-----", TxHash = "-----tx-hash-----" } };
            var scr_to = new List<ScryptaSendOrderTo> { new ScryptaSendOrderTo { Address = "-----to-address-----", Value = 0.02969944m } };
            var scr_15 = api.Scrypta.Send(scr_fromAddress, scr_fromUTXO, scr_to);
            var scr_16 = api.Scrypta.Broadcast("-----tx-data-----", "-----signature-id-----");

            /* Tatum / Service */
            var srv_01 = api.Service.GetConsumptions();
            var srv_02 = api.Service.GetExchangeRates("-----currency-----", "-----base-pair-----");
            var srv_03 = api.Service.GetVersion();
        }

        private static void TatumWalletManagerExamples()
        {
            /* Wallet Manager */
            var wm = new TatumWalletManager("XXXXXXXX-API-KEY-XXXXXXXX", 5);

            // Coins
            wm.AddAsset(new WalletAssetOptions { AssetCode = "BNB", AssetName = "Binance BNB", AssetType = AssetType.Coin, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.BinanceChain });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "BTC", AssetName = "Bitcoin", AssetType = AssetType.Coin, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Bitcoin });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "BCH", AssetName = "BitcoinCash", AssetType = AssetType.Coin, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.BitcoinCash });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "ETH", AssetName = "Ethereum", AssetType = AssetType.Coin, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "LIBRA", AssetName = "Libra", AssetType = AssetType.Coin, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Libra });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "LTC", AssetName = "Litecoin", AssetType = AssetType.Coin, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Litecoin });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "LYRA", AssetName = "Scrypta", AssetType = AssetType.Coin, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Scrypta });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "NEO", AssetName = "NEO", AssetType = AssetType.Coin, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.NEO });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "XLM", AssetName = "Stellar", AssetType = AssetType.Coin, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Stellar });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "XRP", AssetName = "Ripple", AssetType = AssetType.Coin, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ripple });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "VET", AssetName = "VeChain", AssetType = AssetType.Coin, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.VeChain });

            // CoinMarketCap Top ERC20 Tokens
            wm.AddAsset(new WalletAssetOptions { AssetCode = "USDT", AssetName = "Tether", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0xdac17f958d2ee523a2206206994597c13d831ec7" });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "LINK", AssetName = "ChainLink Token", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x514910771af9ca656af840dff83e8264ecf986ca", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "WBTC", AssetName = "Wrapped BTC", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x2260fac5e5542a773aa44fbcfedf7c193bc2c599", ChainDecimals = 8 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "USDC", AssetName = "USD Coin", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0xa0b86991c6218b36c1d19d4a2e9eb0ce3606eb48" });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "CRO", AssetName = "Crypto.com Coin", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0xa0b73e1ff0b80914ab6fe0444e65848c4c34450b" });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "UNI", AssetName = "Uniswap", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x1f9840a85d5af5bf1d1762f925bdaddc4201f984", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "AAVE", AssetName = "Aave Token", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x0000000000000", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "SNX", AssetName = "Synthetix", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0xc011a73ee8576fb46f5e1c5751ca3b9fe0af2a6f" });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "DAI", AssetName = "Dai Stablecoin", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x6b175474e89094c44da98b954eedeac495271d0f", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "CEL", AssetName = "Celsius", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0xaaaebe6fe48e54f431b0c390cfaf0b017d09d42d", ChainDecimals = 4 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "LEO", AssetName = "Bitfinex LEO Token", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x2af5d2ad76741191d15dfe7bf6ac92d4bd912ca3", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "MKR", AssetName = "Maker", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x9f8f72aa9304c8b593d555f12ef6589cc3a579a2", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "BUSD", AssetName = "Binance USD", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x4fabb145d64652a948d72533023f6e7a623c7c53" });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "REV", AssetName = "Revain", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x2ef52Ed7De8c5ce03a4eF0efbe9B7450F2D7Edc9" });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "YFI", AssetName = "yearn.finance", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x0bc529c00c6401aef6d220be8c6ea1667f6ad93e", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "HT", AssetName = "Huobi Token", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x6f259637dcd74c767781e37bc6133cd6a68aa161", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "FTT", AssetName = "FTX Token", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x50d1c9771902476076ecfc8b2a83ad6b9355a4c9", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "COMP", AssetName = "Compound", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0xc00e94cb662c3520282e6f5717214004a7f26888", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "UMA", AssetName = "UMA Voting Token", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x04Fa0d235C4abf4BcF4787aF4CF447DE572eF828", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "SUSHI", AssetName = "Sushi Token", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x6b3595068778dd592e39a122f4f5a5cf09c90fe2", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "RENBTC", AssetName = "renBTC", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0xeb4c2781e4eba804ce9a9803c67d0893436bb27d" });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "OMG", AssetName = "OMG Token", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0xd26114cd6EE289AccF82350c8d8487fedB8A0C07", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "LRC", AssetName = "Loopring", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0xbbbbca6a901c926f240b89eacb641d8aec7aeafd", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "GRT", AssetName = "The Graph", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0xc944e90c64b2c07662a292be6244bdf05cda44a7", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "OKB", AssetName = "OKB", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x75231f58b43240c9718dd58b4967c5114342a86c" });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "BAT", AssetName = "Basic Attention Token", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x0d8775f648430679a709e98d2b0cb6250d2887ef", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "RSR", AssetName = "Reserve Rights", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x8762db106b2c2a0bccb3a80d1ed41273552616e8", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "ZRX", AssetName = "0x", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0xe41d2489571d322189246dafa5ebde1f4699f498", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "NEXO", AssetName = "Nexo", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0xb62132e35a6c13ee1ee0f84dc5d40bad8d815206", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "REN", AssetName = "Republic Token", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x408e41876cccdc0f92210600ef50372656052a38", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "CHSB", AssetName = "SwissBorg", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0xba9d4199fab4f26efe3551d490e3821486f135ba", });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "HUSD", AssetName = "HUSD", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0xdf574c24545e5ffecb9a659c229253d4111d87e1" });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "TUSD", AssetName = "TrueUSD", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x0000000000085d4780B73119b644AE5ecd22b376" });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "AMPL", AssetName = "Ampleforth", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0xd46ba6d942050d489dbd938a2c909a5d5039a161" });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "PAX", AssetName = "Paxos Standard Token", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x8e870d67f660d95d5be530380d0ec0bd388289e1" });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "SNT", AssetName = "Status Network Token", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x744d70fdbe2ba4cf95131626614a1763df805b9e", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "KNC", AssetName = "Kyber Network", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0xdd974d5c2e2928dea5f71b9825b8b646686bd200", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "REP", AssetName = "Augur", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x1985365e9f78359a9b6ad760e32412f4a445e862" });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "ENJ", AssetName = "Enjin Coin", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0xf629cbd94d3791c9250152bd8dfbdf380e2a3b9c", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "MANA", AssetName = "Decentraland", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x0f5d2fb29fb7d3cfee444a200298f468908cc942", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "QNT", AssetName = "Quant", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x4a220e6096b25eadb88358cb44068a3248254675", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "BAND", AssetName = "Band Protocol", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0xba11d00c5f74255f56a5e366f4f77f5a186d7f55", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "OCEAN", AssetName = "Ocean Protocol", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x967da4048cd07ab37855c090aaf366e4ce1b9f48", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "HEDG", AssetName = "HedgeTrade", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0xF1290473E210b2108A85237fbCd7b6eb42Cc654F", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "NXM", AssetName = "NXM", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0xd7c49cee7e9188cca6ad8ff264c1da2e69d4cf3b", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "GNO", AssetName = "Gnosis", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x6810e776880c02933d47db1b9fc05908e5386b96", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "MATIC", AssetName = "Matic Network", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x7D1AfA7B718fb893dB30A3aBc0Cfc608AaCfeBB0" });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "BNT", AssetName = "Bancor", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x1f573d6fb3f13d689ff844b4ce37794d79a7ff1c" });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "HOT", AssetName = "Holo Token", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x6c6ee5e31d828de241282b9606c8e98ea48526e2", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "BNB", AssetName = "BNB Token", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0xB8c77482e45F1F44dE1745F52C74426C631bDD52" });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "CHZ", AssetName = "Chiliz", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x3506424f91fd33084466f402d5d97f05f8e3b4af", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "HBTC", AssetName = "Huobi BTC", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x0316EB71485b0Ab14103307bf65a021042c6d380", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "THETA", AssetName = "Theta Token", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x3883f5e181fccaf8410fa61e12b59bad963fb645", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "VEN", AssetName = "VeChain Token", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0xd850942ef8811f2a866692a623011bde52a462c1", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "WAVES", AssetName = "WAVES", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x1cf4592ebffd730c7dc92c1bdffdfc3b9efcf29a", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "ZIL", AssetName = "Zilliqa Token ", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x05f4a42e251f2d52b8ed15e9fedaacfcef1fad27", ChainDecimals = 12 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "NPXS", AssetName = "Pundi X Token", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0xa15c7ebe1f07caf6bff097d8a589fb8ac49ae5b3", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "IOST", AssetName = "IOSToken", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0xfa1a856cfa3409cfa145fa4e20eb270df3eb21ab", ChainDecimals = 18 });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "PAXG", AssetName = "PAX Gold", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x45804880de22913dafe09f4980848ece6ecbaf78" });
            wm.AddAsset(new WalletAssetOptions { AssetCode = "AAVE", AssetName = "Aave", AssetType = AssetType.Token, BlockchainManager = BlockchainManager.Tatum, BlockchainType = BlockchainType.Ethereum, TokenType = TokenType.ERC20, TokenContract = "0x7Fc66500c84A76Ad7e9c93437bFc5Ac33E2DDaE9" });

            var wm_01 = wm.CreateLedgerAccount("BTC");
            var wm_02 = wm.CreateLedgerAccount("ETH");
            var wm_03 = wm.CreateLedgerAccount("XRP");
            var wm_04 = wm.CreateLedgerAccount("USDT");
            var wm_05 = wm.CreateLedgerAccount("WAVES");

            var mnemo = wm.GenerateMnemonics(12);
            var wm_11 = wm.CreateDepositAddress("BTC", mnemo);
            var wm_12 = wm.CreateDepositAddress("ETH", mnemo);
            var wm_13 = wm.CreateDepositAddress("XRP", mnemo);
            var wm_14 = wm.CreateDepositAddress("USDT", mnemo);
            var wm_15 = wm.CreateDepositAddress("WAVES", mnemo);

            var wm_21 = wm.GetBlockchainBalance("BTC", "-----address-----");
            var wm_22 = wm.GetBlockchainBalance("ETH", "-----address-----");
            var wm_23 = wm.GetBlockchainBalance("XRP", "-----address-----");
            var wm_24 = wm.GetBlockchainBalance("USDT", "-----address-----");
            var wm_25 = wm.GetBlockchainBalance("WAVES", "-----address-----");

            var userWallet = new WalletDepositAddress(); // Please full fields with user specific data
            var wm_31 = wm.Withdraw("BTC", userWallet, 0.1m, "-----address-----", "-----tag-or-memo-----", triggerBroadcasting: true);
            var wm_32 = wm.Withdraw("ETH", userWallet, 0.1m, "-----address-----", "-----tag-or-memo-----", triggerBroadcasting: true);
            var wm_33 = wm.Withdraw("XRP", userWallet, 0.1m, "-----address-----", "-----tag-or-memo-----", triggerBroadcasting: true);
            var wm_34 = wm.Withdraw("USDT", userWallet, 0.1m, "-----address-----", "-----tag-or-memo-----", triggerBroadcasting: true);
            var wm_35 = wm.Withdraw("WAVES", userWallet, 0.1m, "-----address-----", "-----tag-or-memo-----", triggerBroadcasting: true);
        }
    }
}