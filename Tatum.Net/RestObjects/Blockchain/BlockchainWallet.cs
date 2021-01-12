using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class BlockchainWallet
    {
        [JsonProperty("mnemonic")]
        public string Mnemonics { get; set; }

        [JsonProperty("xpub")]
        public string ExtendedPublicKey { get; set; }
    }
}
