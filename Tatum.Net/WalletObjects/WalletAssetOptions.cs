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

        /// <summary>
        /// Token Id for ERC721
        /// </summary>
        public string TokenId { get; set; }

        /// <summary>
        /// Token Contract Address for ERC20, ERC721
        /// </summary>
        public string TokenContract { get; set; }
        public int ChainDecimals { get; set; }
        public int DisplayDecimals { get; set; }
        public int DepositConfirmations { get; set; }
        public bool IsCrypto => AssetType.IsOneOf(AssetType.Coin, AssetType.Token);
        public string ExternalBlockchainManager { get; set; }
    }
}
