namespace Tatum.Net.WalletObjects
{
    public enum BlockchainManager
    {
        None,
        Tatum,
        External,
    }

    public enum AssetType
    {
        Fiat,
        Coin,
        Token,
    }

    public enum TokenType
    {
        None,
        BEP20,
        ERC20,
        ERC721,
    }
}
