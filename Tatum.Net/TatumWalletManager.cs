using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiter;
using System;
using System.Collections.Generic;
using System.Linq;
using Tatum.Net.CoreObjects;
using Tatum.Net.Enums;
using Tatum.Net.Helpers;
using Tatum.Net.RestObjects;
using Tatum.Net.WalletObjects;

namespace Tatum.Net
{
    public class TatumWalletManager
    {
        public TatumClient TatumApi { get; private set; }
        public Dictionary<string, WalletAssetOptions> Assets { get; private set; }
        public TatumWalletManager(string apikey, int rateLimiterPerSecond = 5)
        {
            var cli_options = new TatumClientOptions
            {
#if DEBUG
                LogVerbosity = LogVerbosity.Debug,
#endif
                RateLimiters = new List<IRateLimiter>
                {
                    new RateLimiterCredit(rateLimiterPerSecond, TimeSpan.FromMilliseconds(1000)),
                },
                RateLimitingBehaviour = RateLimitingBehaviour.Wait,
            };
            TatumApi = new TatumClient(apikey, cli_options);
            Assets = new Dictionary<string, WalletAssetOptions>();
        }

        public virtual void LoadAssets(IEnumerable<WalletAssetOptions> assets)
        {
            foreach (var asset in assets)
                AddAsset(asset);
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

        public virtual WalletResponse<LedgerAccount> CreateLedgerAccount(string assetCode, LedgerAccountOptions accountOptions = null)
        {
            var assetOptions = Assets.Values.Where(x => x.AssetCode == assetCode).FirstOrDefault();
            if (assetOptions == null) return WalletResponse<LedgerAccount>.CreateErrorResult(new WalletError("Invalid asset code!"));

            if (assetOptions.BlockchainManager == BlockchainManager.Tatum)
            {
                var resp = TatumApi.LedgerAccount_Create(assetOptions.BlockchainType, accountOptions);
                if (resp.Success) new WalletResponse<LedgerAccount>(resp.Data);
                else new WalletResponse<LedgerAccount>(new WalletError(resp.Error));
            }

            if (assetOptions.BlockchainManager == BlockchainManager.External)
                return CreateLedgerAccountViaExternalManager(assetOptions, accountOptions);

            return WalletResponse<LedgerAccount>.CreateErrorResult(new WalletError("Invalid Blockchain Manager!"));
        }

        protected virtual WalletResponse<LedgerAccount> CreateLedgerAccountViaExternalManager(WalletAssetOptions assetOptions, LedgerAccountOptions accountOptions = null)
        {
            throw new NotImplementedException();
        }

        public virtual WalletResponse<WalletDepositAddress> CreateDepositAddress(string assetCode, List<string> mnemonics, int index = 0)
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
                    var r01 = TatumApi.Blockchain_GenerateWallet(assetOptions.BlockchainType, mnemonics);
                    if (!r01.Success) return new WalletResponse<WalletDepositAddress>(new WalletError(r01.Error));

                    // Get Address
                    var r02 = TatumApi.Blockchain_GenerateDepositAddress(assetOptions.BlockchainType, r01.Data.ExtendedPublicKey, index);
                    if (!r02.Success) return new WalletResponse<WalletDepositAddress>(new WalletError(r02.Error));

                    // Return
                    var wda = new WalletDepositAddress();
                    wda.ExtendedPublicKey = r01.Data.ExtendedPublicKey;
                    wda.Mnemonics = string.Join(" ", mnemonics);
                    wda.Address = r02.Data.Address;
                    wda.Index = index;
                    return new WalletResponse<WalletDepositAddress>(wda);
                }
                else if (assetOptions.BlockchainType == BlockchainType.Ripple)
                {

                }
            }

            if (assetOptions.BlockchainManager == BlockchainManager.External)
                return CreateDepositAddressViaExternalManager(assetOptions, mnemonics);

            return WalletResponse<WalletDepositAddress>.CreateErrorResult(new WalletError("Invalid Blockchain Manager!"));
        }

        protected virtual WalletResponse<WalletDepositAddress> CreateDepositAddressViaExternalManager(WalletAssetOptions assetOptions, List<string> mnemonics)
        {
            throw new NotImplementedException();
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

    }
}
