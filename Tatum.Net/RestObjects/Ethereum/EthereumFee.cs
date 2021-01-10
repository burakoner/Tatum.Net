using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class EthereumFee
    {
        /// <summary>
        /// Gas limit for transaction in gas price.
        /// </summary>
        [JsonProperty("gasLimit")]
        public decimal GasLimit { get; set; }

        /// <summary>
        /// Gas price in Gwei.
        /// </summary>
        [JsonProperty("gasPrice")]
        public decimal GasPrice { get; set; }
    }
}
