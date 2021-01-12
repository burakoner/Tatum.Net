using Newtonsoft.Json;
using System.Collections.Generic;
using Tatum.Net.Converters;
using Tatum.Net.Enums;

namespace Tatum.Net.RestObjects
{
    public class KMSPendingTransaction
    {
        [JsonProperty("chain"), JsonConverter(typeof(BlockchainTypeConverter))]
        public BlockchainType Chain { get; set; }

        [JsonProperty("hashes")]
        public IEnumerable<string> Hashes { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("index")]
        public long Index { get; set; }

        [JsonProperty("serializedTransaction")]
        public string serializedTransaction { get; set; }

        [JsonProperty("txId")]
        public string TransactionId { get; set; }

        [JsonProperty("withdrawalId")]
        public string WithdrawalId { get; set; }

        [JsonProperty("withdrawalResponses")]
        public IEnumerable<KMSWithdrawalResponse> WithdrawalResponses { get; set; }
    }
}
