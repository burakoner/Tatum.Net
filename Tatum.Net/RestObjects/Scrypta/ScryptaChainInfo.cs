using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class ScryptaChainInfo
    {
        [JsonProperty("balance")]
        public decimal Balance { get; set; }

        [JsonProperty("blocks")]
        public long Blocks { get; set; }

        [JsonProperty("checksum")]
        public string Checksum { get; set; }

        [JsonProperty("connections")]
        public long Connections { get; set; }

        [JsonProperty("difficulty")]
        public decimal Difficulty { get; set; }

        [JsonProperty("errors")]
        public object Errors { get; set; }

        [JsonProperty("indexed")]
        public long Indexed { get; set; }

        [JsonProperty("keypoololdest")]
        public long KeyPoolOldest { get; set; }

        [JsonProperty("keypoolsize")]
        public long KeyPoolSize { get; set; }

        [JsonProperty("node")]
        public string Node { get; set; }

        [JsonProperty("obfuscation_balance")]
        public decimal ObfuscationBalance { get; set; }

        [JsonProperty("paytxfee")]
        public decimal PayTransactionFee { get; set; }

        [JsonProperty("protocolversion")]
        public long ProtocolVersion { get; set; }

        [JsonProperty("proxy")]
        public object Proxy { get; set; }

        [JsonProperty("relayfee")]
        public decimal RelayFee { get; set; }

        [JsonProperty("staking status")]
        public string StakingStatus { get; set; }

        [JsonProperty("testnet")]
        public bool TestNet { get; set; }

        [JsonProperty("timeoffset")]
        public long TimeOffset { get; set; }

        [JsonProperty("toindex")]
        public long ToOndex { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("walletversion")]
        public long WalletVersion { get; set; }
    }
}
