using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class EthereumTransactionLog
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("blockNumber")]
        public long BlockNumber { get; set; }

        [JsonProperty("transactionHash")]
        public string TransactionHash { get; set; }

        [JsonProperty("transactionIndex")]
        public long TransactionIndex { get; set; }

        [JsonProperty("blockHash")]
        public string BlockHash { get; set; }

        [JsonProperty("logIndex")]
        public long LogIndex { get; set; }

        [JsonProperty("removed")]
        public bool Removed { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("topics")]
        public IEnumerable<string> Topics { get; set; }
    }
}