using Tatum.Net.Enums;
using Tatum.Net.Helpers;

namespace Tatum.Net.WalletObjects
{
    public class WalletAssetOptions
    {
        public string AssetCode { get; set; }
        public string AssetName { get; set; }
        public AssetType AssetType { get; set; }
        public BlockchainType BlockchainType { get; set; }
        public BlockchainManager BlockchainManager { get; set; }
        public TokenType TokenType { get; set; }
        public string TokenContract { get; set; }
        public int ChainDecimals { get; set; }
        public int DisplayDecimals { get; set; }
        public int DepositConfirmations { get; set; }
        public bool IsCrypto => AssetType.IsOneOf(AssetType.Coin, AssetType.Token);
    }
}
