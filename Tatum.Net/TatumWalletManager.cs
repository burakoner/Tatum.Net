using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiter;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Tatum.Net.CoreObjects;
using Tatum.Net.Enums;
using Tatum.Net.Helpers;
using Tatum.Net.RestObjects;
using Tatum.Net.WalletObjects;

namespace Tatum.Net
{
    public class TatumWalletManager : TatumClient
    {
        protected CultureInfo ci = CultureInfo.InvariantCulture;
        public Dictionary<string, WalletAssetOptions> Assets { get; private set; }
        public TatumWalletManager() : this("") { }
        public TatumWalletManager(string apikey, int rateLimiterPerSecond = 5) : this(apikey, ConstructTatumClientOptions(rateLimiterPerSecond)) { }
        public TatumWalletManager(string apikey, TatumClientOptions options) : this(apikey, options, null) { }
        public TatumWalletManager(string apikey, TatumClientOptions options, IEnumerable<WalletAssetOptions> assets) : base(apikey, options)
        {
            Assets = new Dictionary<string, WalletAssetOptions>();
            if (assets != null) LoadAssets(assets);
        }

        protected static TatumClientOptions ConstructTatumClientOptions(int rateLimiterPerSecond = 5)
        {
            return new TatumClientOptions
            {
#if DEBUG
                LogLevel = LogLevel.Debug,
#endif
                RateLimiters = new List<IRateLimiter> { new RateLimiterCredit(rateLimiterPerSecond, TimeSpan.FromMilliseconds(1000)), },
                RateLimitingBehaviour = RateLimitingBehaviour.Wait,
            };
        }

        public virtual void AddAsset(WalletAssetOptions asset)
        {
            if (asset != null)
            {
                var ops = asset.BlockchainType.GetBlockchainOptions();
                var key = $"{asset.AssetCode}-{ops.Code}";
                Assets[key] = asset;
            }
        }
        public virtual void LoadAssets(IEnumerable<WalletAssetOptions> assets)
        {
            foreach (var asset in assets)
                AddAsset(asset);
        }

        public virtual List<string> GenerateMnemonics(int length)
        {
            var rnd = new Random();
            var list = new List<string>();
            for (var i = 0; i < length; i++)
            {
                while (true)
                {
                    var index = rnd.Next(Mnemonics.Words.Count());
                    var word = Mnemonics.Words.ElementAt(index);
                    if (!list.Contains(word))
                    {
                        list.Add(word);
                        break;
                    }
                }
            }
            return list;
        }

        public virtual WalletResponse<LedgerAccount> CreateLedgerAccount(string assetCode, LedgerAccountOptions accountOptions = null)
        {
            var assetOptions = Assets.Values.Where(x => x.AssetCode == assetCode).FirstOrDefault();
            if (assetOptions == null) return WalletResponse<LedgerAccount>.CreateErrorResult(new WalletError("Invalid asset code!"));

            if (assetOptions.BlockchainManager == BlockchainManager.Tatum)
            {
                var resp = Ledger.Account.Create(assetOptions.BlockchainType, accountOptions);
                if (resp.Success) new WalletResponse<LedgerAccount>(resp.Data);
                else new WalletResponse<LedgerAccount>(new WalletError(resp.Error));
            }
            else if (assetOptions.BlockchainManager == BlockchainManager.External)
            {
                return CreateLedgerAccountViaExternalManager(assetOptions, accountOptions);
            }

            return WalletResponse<LedgerAccount>.CreateErrorResult(new WalletError("Invalid Blockchain Manager!"));
        }
        protected virtual WalletResponse<LedgerAccount> CreateLedgerAccountViaExternalManager(WalletAssetOptions assetOptions, LedgerAccountOptions accountOptions = null)
        {
            throw new NotImplementedException();
        }

        public virtual WalletResponse<WalletDepositAddress> CreateDepositAddress(string assetCode, List<string> mnemonics, int index = 0) => CreateDepositAddress(assetCode, string.Join(" ", mnemonics), index);
        public virtual WalletResponse<WalletDepositAddress> CreateDepositAddress(string assetCode, string mnemonics, int index = 0)
        {
            var assetOptions = Assets.Values.Where(x => x.AssetCode == assetCode).FirstOrDefault();
            if (assetOptions == null) return WalletResponse<WalletDepositAddress>.CreateErrorResult(new WalletError("Invalid asset code!"));

            if (assetOptions.BlockchainManager == BlockchainManager.Tatum)
            {
                if (assetOptions.BlockchainType.IsOneOf(
                    BlockchainType.Bitcoin,
                    BlockchainType.BitcoinCash,
                    BlockchainType.Ethereum,
                    BlockchainType.Litecoin,
                    BlockchainType.Scrypta,
                    BlockchainType.VeChain))
                {
                    // Generate Wallet
                    var r01 = Blockchain_GenerateWallet(assetOptions.BlockchainType, mnemonics);
                    if (!r01.Success) return new WalletResponse<WalletDepositAddress>(new WalletError(r01.Error));

                    // Get Address
                    var r02 = Blockchain_GenerateDepositAddress(assetOptions.BlockchainType, r01.Data.ExtendedPublicKey, index);
                    if (!r02.Success) return new WalletResponse<WalletDepositAddress>(new WalletError(r02.Error));

                    // Generate Private Key
                    var r03 = Blockchain_GeneratePrivateKey(assetOptions.BlockchainType, r01.Data.Mnemonics, index);
                    if (!r03.Success) return new WalletResponse<WalletDepositAddress>(new WalletError(r02.Error));

                    // Return
                    var wda = new WalletDepositAddress();
                    wda.ExtendedPublicKey = r01.Data.ExtendedPublicKey;
                    wda.Mnemonics = r01.Data.Mnemonics;
                    wda.Address = r02.Data.Address;
                    wda.PrivateKey = r03.Data.Key;
                    wda.Index = index;
                    return new WalletResponse<WalletDepositAddress>(wda);
                }
                else if (assetOptions.BlockchainType == BlockchainType.Ripple)
                {
                    // Generate Account
                    var r01 = Ripple.GenerateAccount();
                    if (!r01.Success) return new WalletResponse<WalletDepositAddress>(new WalletError(r01.Error));

                    // Return
                    var wda = new WalletDepositAddress();
                    wda.Address = r01.Data.Address;
                    wda.PrivateKey = r01.Data.Secret;
                    return new WalletResponse<WalletDepositAddress>(wda);
                }
                else if (assetOptions.BlockchainType == BlockchainType.Stellar)
                {
                    // Generate Account
                    var r01 = Stellar.GenerateAccount();
                    if (!r01.Success) return new WalletResponse<WalletDepositAddress>(new WalletError(r01.Error));

                    // Return
                    var wda = new WalletDepositAddress();
                    wda.Address = r01.Data.Address;
                    wda.PrivateKey = r01.Data.Secret;
                    return new WalletResponse<WalletDepositAddress>(wda);
                }
                else if (assetOptions.BlockchainType == BlockchainType.BinanceChain)
                {
                    // Generate Account
                    var r01 = Binance.GenerateAccount();
                    if (!r01.Success) return new WalletResponse<WalletDepositAddress>(new WalletError(r01.Error));

                    // Return
                    var wda = new WalletDepositAddress();
                    wda.Address = r01.Data.Address;
                    wda.PrivateKey = r01.Data.PrivateKey;
                    return new WalletResponse<WalletDepositAddress>(wda);
                }
                else if (assetOptions.BlockchainType == BlockchainType.NEO)
                {
                    // Generate Account
                    var r01 = NEO.GenerateAccount();
                    if (!r01.Success) return new WalletResponse<WalletDepositAddress>(new WalletError(r01.Error));

                    // Return
                    var wda = new WalletDepositAddress();
                    wda.Address = r01.Data.Address;
                    wda.PrivateKey = r01.Data.PrivateKey;
                    return new WalletResponse<WalletDepositAddress>(wda);
                }
                else if (assetOptions.BlockchainType == BlockchainType.Libra)
                {
                    return WalletResponse<WalletDepositAddress>.CreateErrorResult(new WalletError($"{assetOptions.BlockchainType} Blockchain doesnt support to create deposit address!"));
                }
                else if (assetOptions.BlockchainType == BlockchainType.TRON)
                {
                    // Generate Account
                    var r01 = TRON.GenerateAccount();
                    if (!r01.Success) return new WalletResponse<WalletDepositAddress>(new WalletError(r01.Error));

                    // Return
                    var wda = new WalletDepositAddress();
                    wda.Address = r01.Data.Address;
                    wda.PrivateKey = r01.Data.PrivateKey;
                    return new WalletResponse<WalletDepositAddress>(wda);
                }
            }
            else if (assetOptions.BlockchainManager == BlockchainManager.External)
            {
                return CreateDepositAddressViaExternalManager(assetOptions, mnemonics, index);
            }

            return WalletResponse<WalletDepositAddress>.CreateErrorResult(new WalletError("Invalid Blockchain Manager!"));
        }
        protected virtual WalletResponse<WalletDepositAddress> CreateDepositAddressViaExternalManager(WalletAssetOptions assetOptions, string mnemonics, int index = 0)
        {
            throw new NotImplementedException();
        }

        public virtual WalletResponse<WalletBalance> GetBlockchainBalance(string assetCode, string address, string tagOrMemo = null)
        {
            var assetOptions = Assets.Values.Where(x => x.AssetCode == assetCode).FirstOrDefault();
            if (assetOptions == null) return WalletResponse<WalletBalance>.CreateErrorResult(new WalletError("Invalid asset code!"));

            if (assetOptions.BlockchainManager == BlockchainManager.Tatum)
            {
                if (assetOptions.BlockchainType == BlockchainType.Bitcoin)
                {
                    // Get Balance
                    var r01 = Bitcoin.GetBalance(address);
                    if (!r01.Success) return new WalletResponse<WalletBalance>(new WalletError(r01.Error));

                    // Return
                    var wb = new WalletBalance();
                    wb.IncomingBalance = r01.Data.Incoming;
                    wb.OutgoingBalance = r01.Data.Outgoing;
                    wb.CurrentBalance = r01.Data.Incoming - r01.Data.Outgoing;
                    return new WalletResponse<WalletBalance>(wb);
                }
                else if (assetOptions.BlockchainType == BlockchainType.BitcoinCash)
                {
                    return WalletResponse<WalletBalance>.CreateErrorResult(new WalletError($"{assetOptions.BlockchainType} Blockchain doesnt support to get balance!"));
                }
                else if (assetOptions.BlockchainType == BlockchainType.Ethereum)
                {
                    // ETH
                    if (assetOptions.AssetType == AssetType.Coin)
                    {
                        // Get Balance
                        var r01 = Ethereum.ETH_GetBalance(address);
                        if (!r01.Success) return new WalletResponse<WalletBalance>(new WalletError(r01.Error));

                        // Return
                        var wb = new WalletBalance();
                        wb.CurrentBalance = r01.Data;
                        return new WalletResponse<WalletBalance>(wb);
                    }

                    // ERC20, ERC721
                    else if (assetOptions.AssetType == AssetType.Token)
                    {
                        // ERC20
                        if (assetOptions.TokenType == TokenType.ERC20)
                        {
                            // Get Balance
                            var r01 = Ethereum.ERC20_GetBalance(address, assetOptions.TokenContract, assetOptions.ChainDecimals);
                            if (!r01.Success) return new WalletResponse<WalletBalance>(new WalletError(r01.Error));

                            // Return
                            var wb = new WalletBalance();
                            wb.CurrentBalance = r01.Data;
                            return new WalletResponse<WalletBalance>(wb);
                        }

                        // ERC721
                        else if (assetOptions.TokenType == TokenType.ERC721)
                        {
                            // Get Balance
                            var r01 = Ethereum.ERC721_GetBalance(address, assetOptions.TokenContract, assetOptions.ChainDecimals);
                            if (!r01.Success) return new WalletResponse<WalletBalance>(new WalletError(r01.Error));

                            // Return
                            var wb = new WalletBalance();
                            wb.CurrentBalance = r01.Data;
                            return new WalletResponse<WalletBalance>(wb);
                        }
                    }
                }
                else if (assetOptions.BlockchainType == BlockchainType.Litecoin)
                {
                    // Get Balance
                    var r01 = Litecoin.GetBalance(address);
                    if (!r01.Success) return new WalletResponse<WalletBalance>(new WalletError(r01.Error));

                    // Return
                    var wb = new WalletBalance();
                    wb.IncomingBalance = r01.Data.Incoming;
                    wb.OutgoingBalance = r01.Data.Outgoing;
                    wb.CurrentBalance = r01.Data.Incoming - r01.Data.Outgoing;
                    return new WalletResponse<WalletBalance>(wb);
                }
                else if (assetOptions.BlockchainType == BlockchainType.Scrypta)
                {
                    return WalletResponse<WalletBalance>.CreateErrorResult(new WalletError($"{assetOptions.BlockchainType} Blockchain doesnt support to get balance!"));
                }
                else if (assetOptions.BlockchainType == BlockchainType.VeChain)
                {
                    // Get Balance
                    var r01 = VeChain.GetBalance(address);
                    if (!r01.Success) return new WalletResponse<WalletBalance>(new WalletError(r01.Error));

                    // Return
                    var wb = new WalletBalance();
                    wb.CurrentBalance = r01.Data.Balance;
                    return new WalletResponse<WalletBalance>(wb);
                }
                else if (assetOptions.BlockchainType == BlockchainType.Ripple)
                {
                    // Get Balance
                    var r01 = Ripple.GetBalance(address);
                    if (!r01.Success) return new WalletResponse<WalletBalance>(new WalletError(r01.Error));

                    // Return
                    var wb = new WalletBalance();
                    wb.CurrentBalance = r01.Data.Balance.ToDecimalNullable();
                    return new WalletResponse<WalletBalance>(wb);
                }
                else if (assetOptions.BlockchainType == BlockchainType.Stellar)
                {
                    return WalletResponse<WalletBalance>.CreateErrorResult(new WalletError($"{assetOptions.BlockchainType} Blockchain doesnt support to get balance!"));
                }
                else if (assetOptions.BlockchainType == BlockchainType.BinanceChain)
                {
                    return WalletResponse<WalletBalance>.CreateErrorResult(new WalletError($"{assetOptions.BlockchainType} Blockchain doesnt support to get balance!"));
                }
                else if (assetOptions.BlockchainType == BlockchainType.NEO)
                {
                    // Get Balance
                    var r01 = NEO.GetBalance(address);
                    if (!r01.Success) return new WalletResponse<WalletBalance>(new WalletError(r01.Error));
                    if (r01.Data.Assets == null ||
                        r01.Data.Assets.Data == null ||
                        r01.Data.Assets.Data.ContainsKey("NEO") == false)
                        return new WalletResponse<WalletBalance>(new WalletError("Couldnt get NEO balance"));

                    // Return
                    var wb = new WalletBalance();
                    wb.CurrentBalance = r01.Data.Assets.Data["NEO"].ToDecimalNullable();
                    return new WalletResponse<WalletBalance>(wb);
                }
                else if (assetOptions.BlockchainType == BlockchainType.Libra)
                {
                    return WalletResponse<WalletBalance>.CreateErrorResult(new WalletError($"{assetOptions.BlockchainType} Blockchain doesnt support to get balance!"));
                }
                else if (assetOptions.BlockchainType == BlockchainType.TRON)
                {
                    return WalletResponse<WalletBalance>.CreateErrorResult(new WalletError($"{assetOptions.BlockchainType} Blockchain doesnt support to get balance!"));
                }
            }
            else if (assetOptions.BlockchainManager == BlockchainManager.External)
            {
                return GetBlockchainBalanceViaExternalManager(assetOptions, address, tagOrMemo);
            }

            return WalletResponse<WalletBalance>.CreateErrorResult(new WalletError("Invalid Blockchain Manager!"));
        }
        protected virtual WalletResponse<WalletBalance> GetBlockchainBalanceViaExternalManager(WalletAssetOptions assetOptions, string address, string tagOrMemo = null)
        {
            throw new NotImplementedException();
        }

        public virtual WalletResponse<WalletWithdrawResponse> WithdrawAndBroadcast(string assetCode, WalletDepositAddress wallet, decimal amount, string recepientAddress, string recepientTagOrMemo = null) => Withdraw(assetCode, wallet, amount, recepientAddress, recepientTagOrMemo, true);
        public virtual WalletResponse<WalletWithdrawResponse> Withdraw(string assetCode, WalletDepositAddress wallet, decimal amount, string recepientAddress, string recepientTagOrMemo = null, bool triggerBroadcasting = false)
        {
            var assetOptions = Assets.Values.Where(x => x.AssetCode == assetCode).FirstOrDefault();
            if (assetOptions == null) return WalletResponse<WalletWithdrawResponse>.CreateErrorResult(new WalletError("Invalid asset code!"));

            if (assetOptions.BlockchainManager == BlockchainManager.Tatum)
            {
                if (assetOptions.BlockchainType == BlockchainType.Bitcoin)
                {
                    // Send
                    var btc_fromAddress = new List<BitcoinSendOrderFromAddress> { new BitcoinSendOrderFromAddress { Address = wallet.Address, PrivateKey = wallet.PrivateKey } };
                    var btc_to = new List<BitcoinSendOrderTo> { new BitcoinSendOrderTo { Address = recepientAddress, Value = amount } };
                    var r01 = Bitcoin.Send(btc_fromAddress, null, btc_to);
                    if (!r01.Success || r01.Data.Failed) return new WalletResponse<WalletWithdrawResponse>(new WalletError(r01.Error));

                    // Return Data
                    var wwr = new WalletWithdrawResponse();
                    wwr.Withdraw_TransactionId = r01.Data.TransactionId;
                    wwr.Withdraw_Success = !r01.Data.Failed;

                    // Broadcast
                    if (triggerBroadcasting)
                    {
                        var r02 = Bitcoin.Broadcast(r01.Data.TransactionId);
                        if (!r02.Success || r02.Data.Failed) return new WalletResponse<WalletWithdrawResponse>(new WalletError(r02.Error));

                        // Return Data
                        wwr.Broadcast_TransactionId = r02.Data.TransactionId;
                        wwr.Broadcast_Success = !r02.Data.Failed;
                    }

                    // Return
                    return new WalletResponse<WalletWithdrawResponse>(wwr);
                }
                else if (assetOptions.BlockchainType == BlockchainType.BitcoinCash)
                {
                    throw new NotImplementedException();
                }
                else if (assetOptions.BlockchainType == BlockchainType.Ethereum)
                {
                    // ETH
                    if (assetOptions.AssetType == AssetType.Coin)
                    {
                        // Send
                        var btc_fromAddress = new List<BitcoinSendOrderFromAddress> { new BitcoinSendOrderFromAddress { Address = wallet.Address, PrivateKey = wallet.PrivateKey } };
                        var btc_to = new List<BitcoinSendOrderTo> { new BitcoinSendOrderTo { Address = recepientAddress, Value = amount } };
                        var r01 = Ethereum.Send(EthereumPredefinedCurrency.ETH, amount.ToString(ci), recepientAddress, fromPrivateKey: wallet.PrivateKey);
                        if (!r01.Success || r01.Data.Failed) return new WalletResponse<WalletWithdrawResponse>(new WalletError(r01.Error));

                        // Return Data
                        var wwr = new WalletWithdrawResponse();
                        wwr.Withdraw_TransactionId = r01.Data.TransactionId;
                        wwr.Withdraw_Success = !r01.Data.Failed;

                        // Broadcast
                        if (triggerBroadcasting)
                        {
                            var r02 = Ethereum.Broadcast(r01.Data.TransactionId);
                            if (!r02.Success || r02.Data.Failed) return new WalletResponse<WalletWithdrawResponse>(new WalletError(r02.Error));

                            // Return Data
                            wwr.Broadcast_TransactionId = r02.Data.TransactionId;
                            wwr.Broadcast_Success = !r02.Data.Failed;
                        }

                        // Return
                        return new WalletResponse<WalletWithdrawResponse>(wwr);
                    }

                    // ERC20, ERC721
                    else if (assetOptions.AssetType == AssetType.Token)
                    {
                        // ERC20
                        if (assetOptions.TokenType == TokenType.ERC20)
                        {
                            // Send
                            var btc_fromAddress = new List<BitcoinSendOrderFromAddress> { new BitcoinSendOrderFromAddress { Address = wallet.Address, PrivateKey = wallet.PrivateKey } };
                            var btc_to = new List<BitcoinSendOrderTo> { new BitcoinSendOrderTo { Address = recepientAddress, Value = amount } };
                            var r01 = Ethereum.ERC20_Transfer(assetOptions.TokenContract, recepientAddress, amount.ToString(ci), assetOptions.ChainDecimals, fromPrivateKey: wallet.PrivateKey);
                            if (!r01.Success || r01.Data.Failed) return new WalletResponse<WalletWithdrawResponse>(new WalletError(r01.Error));

                            // Return Data
                            var wwr = new WalletWithdrawResponse();
                            wwr.Withdraw_TransactionId = r01.Data.TransactionId;
                            wwr.Withdraw_Success = !r01.Data.Failed;

                            // Broadcast
                            if (triggerBroadcasting)
                            {
                                var r02 = Ethereum.Broadcast(r01.Data.TransactionId);
                                if (!r02.Success || r02.Data.Failed) return new WalletResponse<WalletWithdrawResponse>(new WalletError(r02.Error));

                                // Return Data
                                wwr.Broadcast_TransactionId = r02.Data.TransactionId;
                                wwr.Broadcast_Success = !r02.Data.Failed;
                            }

                            // Return
                            return new WalletResponse<WalletWithdrawResponse>(wwr);
                        }

                        // ERC721
                        else if (assetOptions.TokenType == TokenType.ERC721)
                        {
                            // Send
                            var btc_fromAddress = new List<BitcoinSendOrderFromAddress> { new BitcoinSendOrderFromAddress { Address = wallet.Address, PrivateKey = wallet.PrivateKey } };
                            var btc_to = new List<BitcoinSendOrderTo> { new BitcoinSendOrderTo { Address = recepientAddress, Value = amount } };
                            var r01 = Ethereum.ERC721_Transfer(assetOptions.TokenContract, assetOptions.TokenId, recepientAddress, fromPrivateKey: wallet.PrivateKey);
                            if (!r01.Success || r01.Data.Failed) return new WalletResponse<WalletWithdrawResponse>(new WalletError(r01.Error));

                            // Return Data
                            var wwr = new WalletWithdrawResponse();
                            wwr.Withdraw_TransactionId = r01.Data.TransactionId;
                            wwr.Withdraw_Success = !r01.Data.Failed;

                            // Broadcast
                            if (triggerBroadcasting)
                            {
                                var r02 = Ethereum.Broadcast(r01.Data.TransactionId);
                                if (!r02.Success || r02.Data.Failed) return new WalletResponse<WalletWithdrawResponse>(new WalletError(r02.Error));

                                // Return Data
                                wwr.Broadcast_TransactionId = r02.Data.TransactionId;
                                wwr.Broadcast_Success = !r02.Data.Failed;
                            }

                            // Return
                            return new WalletResponse<WalletWithdrawResponse>(wwr);
                        }
                    }
                }
                else if (assetOptions.BlockchainType == BlockchainType.Litecoin)
                {
                    // Send
                    var ltc_fromAddress = new List<LitecoinSendOrderFromAddress> { new LitecoinSendOrderFromAddress { Address = wallet.Address, PrivateKey = wallet.PrivateKey } };
                    var ltc_to = new List<LitecoinSendOrderTo> { new LitecoinSendOrderTo { Address = recepientAddress, Value = amount } };
                    var r01 = Litecoin.Send(ltc_fromAddress, null, ltc_to);
                    if (!r01.Success || r01.Data.Failed) return new WalletResponse<WalletWithdrawResponse>(new WalletError(r01.Error));

                    // Return Data
                    var wwr = new WalletWithdrawResponse();
                    wwr.Withdraw_TransactionId = r01.Data.TransactionId;
                    wwr.Withdraw_Success = !r01.Data.Failed;

                    // Broadcast
                    if (triggerBroadcasting)
                    {
                        var r02 = Litecoin.Broadcast(r01.Data.TransactionId);
                        if (!r02.Success || r02.Data.Failed) return new WalletResponse<WalletWithdrawResponse>(new WalletError(r02.Error));

                        // Return Data
                        wwr.Broadcast_TransactionId = r02.Data.TransactionId;
                        wwr.Broadcast_Success = !r02.Data.Failed;
                    }

                    // Return
                    return new WalletResponse<WalletWithdrawResponse>(wwr);
                }
                else if (assetOptions.BlockchainType == BlockchainType.Scrypta)
                {
                    // Send
                    var lyra_fromAddress = new List<ScryptaSendOrderFromAddress> { new ScryptaSendOrderFromAddress { Address = wallet.Address, PrivateKey = wallet.PrivateKey } };
                    var lyra_to = new List<ScryptaSendOrderTo> { new ScryptaSendOrderTo { Address = recepientAddress, Value = amount } };
                    var r01 = Scrypta.Send(lyra_fromAddress, null, lyra_to);
                    if (!r01.Success || r01.Data.Failed) return new WalletResponse<WalletWithdrawResponse>(new WalletError(r01.Error));

                    // Return Data
                    var wwr = new WalletWithdrawResponse();
                    wwr.Withdraw_TransactionId = r01.Data.TransactionId;
                    wwr.Withdraw_Success = !r01.Data.Failed;

                    // Broadcast
                    if (triggerBroadcasting)
                    {
                        var r02 = Scrypta.Broadcast(r01.Data.TransactionId);
                        if (!r02.Success || r02.Data.Failed) return new WalletResponse<WalletWithdrawResponse>(new WalletError(r02.Error));

                        // Return Data
                        wwr.Broadcast_TransactionId = r02.Data.TransactionId;
                        wwr.Broadcast_Success = !r02.Data.Failed;
                    }

                    // Return
                    return new WalletResponse<WalletWithdrawResponse>(wwr);
                }
                else if (assetOptions.BlockchainType == BlockchainType.VeChain)
                {
                    // Send
                    var r01 = VeChain.Send(recepientAddress, amount, fromPrivateKey: wallet.PrivateKey);
                    if (!r01.Success || r01.Data.Failed) return new WalletResponse<WalletWithdrawResponse>(new WalletError(r01.Error));

                    // Return Data
                    var wwr = new WalletWithdrawResponse();
                    wwr.Withdraw_TransactionId = r01.Data.TransactionId;
                    wwr.Withdraw_Success = !r01.Data.Failed;

                    // Broadcast
                    if (triggerBroadcasting)
                    {
                        var r02 = VeChain.Broadcast(r01.Data.TransactionId);
                        if (!r02.Success || r02.Data.Failed) return new WalletResponse<WalletWithdrawResponse>(new WalletError(r02.Error));

                        // Return Data
                        wwr.Broadcast_TransactionId = r02.Data.TransactionId;
                        wwr.Broadcast_Success = !r02.Data.Failed;
                    }

                    // Return
                    return new WalletResponse<WalletWithdrawResponse>(wwr);
                }
                else if (assetOptions.BlockchainType == BlockchainType.Ripple)
                {
                    // Send
                    var r01 = Ripple.Send(wallet.Address, recepientAddress, amount.ToString(ci), fromSecret: wallet.PrivateKey, sourceTag: wallet.Tag, destinationTag: recepientTagOrMemo);
                    if (!r01.Success || r01.Data.Failed) return new WalletResponse<WalletWithdrawResponse>(new WalletError(r01.Error));

                    // Return Data
                    var wwr = new WalletWithdrawResponse();
                    wwr.Withdraw_TransactionId = r01.Data.TransactionId;
                    wwr.Withdraw_Success = !r01.Data.Failed;

                    // Broadcast
                    if (triggerBroadcasting)
                    {
                        var r02 = Ripple.Broadcast(r01.Data.TransactionId);
                        if (!r02.Success || r02.Data.Failed) return new WalletResponse<WalletWithdrawResponse>(new WalletError(r02.Error));

                        // Return Data
                        wwr.Broadcast_TransactionId = r02.Data.TransactionId;
                        wwr.Broadcast_Success = !r02.Data.Failed;
                    }

                    // Return
                    return new WalletResponse<WalletWithdrawResponse>(wwr);
                }
                else if (assetOptions.BlockchainType == BlockchainType.Stellar)
                {
                    // Send
                    var r01 = Stellar.Send(wallet.Address, recepientAddress, amount.ToString(ci), fromSecret: wallet.PrivateKey);
                    if (!r01.Success || r01.Data.Failed) return new WalletResponse<WalletWithdrawResponse>(new WalletError(r01.Error));

                    // Return Data
                    var wwr = new WalletWithdrawResponse();
                    wwr.Withdraw_TransactionId = r01.Data.TransactionId;
                    wwr.Withdraw_Success = !r01.Data.Failed;

                    // Broadcast
                    if (triggerBroadcasting)
                    {
                        var r02 = Stellar.Broadcast(r01.Data.TransactionId);
                        if (!r02.Success || r02.Data.Failed) return new WalletResponse<WalletWithdrawResponse>(new WalletError(r02.Error));

                        // Return Data
                        wwr.Broadcast_TransactionId = r02.Data.TransactionId;
                        wwr.Broadcast_Success = !r02.Data.Failed;
                    }

                    // Return
                    return new WalletResponse<WalletWithdrawResponse>(wwr);
                }
                else if (assetOptions.BlockchainType == BlockchainType.BinanceChain)
                {
                    // Send
                    var r01 = Binance.Send(recepientAddress, "BNB", amount.ToString(ci), fromPrivateKey: wallet.PrivateKey);
                    if (!r01.Success || r01.Data.Failed) return new WalletResponse<WalletWithdrawResponse>(new WalletError(r01.Error));

                    // Return Data
                    var wwr = new WalletWithdrawResponse();
                    wwr.Withdraw_TransactionId = r01.Data.TransactionId;
                    wwr.Withdraw_Success = !r01.Data.Failed;

                    // Broadcast
                    if (triggerBroadcasting)
                    {
                        var r02 = Binance.Broadcast(r01.Data.TransactionId);
                        if (!r02.Success || r02.Data.Failed) return new WalletResponse<WalletWithdrawResponse>(new WalletError(r02.Error));

                        // Return Data
                        wwr.Broadcast_TransactionId = r02.Data.TransactionId;
                        wwr.Broadcast_Success = !r02.Data.Failed;
                    }

                    // Return
                    return new WalletResponse<WalletWithdrawResponse>(wwr);
                }
                else if (assetOptions.BlockchainType == BlockchainType.NEO)
                {
                    // Send
                    var r01 = NEO.Send(recepientAddress, amount, 0.0m, fromPrivateKey: wallet.PrivateKey);
                    if (!r01.Success || r01.Data.Failed) return new WalletResponse<WalletWithdrawResponse>(new WalletError(r01.Error));

                    // Return Data
                    var wwr = new WalletWithdrawResponse();
                    wwr.Withdraw_TransactionId = r01.Data.TransactionId;
                    wwr.Withdraw_Success = !r01.Data.Failed;

                    // Broadcast
                    if (triggerBroadcasting)
                    {
                        var r02 = NEO.Broadcast(r01.Data.TransactionId);
                        if (!r02.Success || r02.Data.Failed) return new WalletResponse<WalletWithdrawResponse>(new WalletError(r02.Error));

                        // Return Data
                        wwr.Broadcast_TransactionId = r02.Data.TransactionId;
                        wwr.Broadcast_Success = !r02.Data.Failed;
                    }

                    // Return
                    return new WalletResponse<WalletWithdrawResponse>(wwr);
                }
                else if (assetOptions.BlockchainType == BlockchainType.Libra)
                {
                    return WalletResponse<WalletWithdrawResponse>.CreateErrorResult(new WalletError($"{assetOptions.BlockchainType} Blockchain doesnt support to send transaction!"));
                }
                else if (assetOptions.BlockchainType == BlockchainType.TRON)
                {
                    // Send
                    var r01 = TRON.Send(wallet.PrivateKey, recepientAddress, amount);
                    if (!r01.Success || r01.Data.Failed) return new WalletResponse<WalletWithdrawResponse>(new WalletError(r01.Error));

                    // Return Data
                    var wwr = new WalletWithdrawResponse();
                    wwr.Withdraw_TransactionId = r01.Data.TransactionId;
                    wwr.Withdraw_Success = !r01.Data.Failed;

                    // Broadcast
                    if (triggerBroadcasting)
                    {
                        var r02 = TRON.Broadcast(r01.Data.TransactionId);
                        if (!r02.Success || r02.Data.Failed) return new WalletResponse<WalletWithdrawResponse>(new WalletError(r02.Error));

                        // Return Data
                        wwr.Broadcast_TransactionId = r02.Data.TransactionId;
                        wwr.Broadcast_Success = !r02.Data.Failed;
                    }

                    // Return
                    return new WalletResponse<WalletWithdrawResponse>(wwr);
                }
            }
            else if (assetOptions.BlockchainManager == BlockchainManager.External)
            {
                return WithdrawViaExternalManager(assetOptions, wallet, amount, recepientAddress, recepientTagOrMemo, triggerBroadcasting);
            }

            return WalletResponse<WalletWithdrawResponse>.CreateErrorResult(new WalletError("Invalid Blockchain Manager!"));
        }
        protected virtual WalletResponse<WalletWithdrawResponse> WithdrawViaExternalManager(WalletAssetOptions assetOptions, WalletDepositAddress wallet, decimal amount, string recepientAddress, string recepientTagOrMemo = null, bool triggerBroadcasting = false)
        {
            throw new NotImplementedException();
        }

    }
}