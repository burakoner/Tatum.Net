using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class EthereumBlock
    {
        [JsonProperty("difficulty")]
        public string Difficulty { get; set; }

        [JsonProperty("extraData")]
        public string ExtraData { get; set; }

        [JsonProperty("gasLimit")]
        public long GasLimit { get; set; }

        [JsonProperty("gasUsed")]
        public long GasUsed { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("logsBloom")]
        public string LogsBloom { get; set; }

        [JsonProperty("miner")]
        public string Miner { get; set; }

        [JsonProperty("nonce")]
        public string Nonce { get; set; }

        [JsonProperty("number")]
        public long Number { get; set; }

        [JsonProperty("parentHash")]
        public string ParentHash { get; set; }

        [JsonProperty("sha3Uncles")]
        public string Sha3Uncles { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("stateRoot")]
        public string StateRoot { get; set; }

        [JsonProperty("timestamp"), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime Timestamp { get; set; }

        [JsonProperty("totalDifficulty")]
        public string TotalDifficulty { get; set; }

        [JsonProperty("transactionsRoot")]
        public string TransactionsRoot { get; set; }

        //[JsonProperty("uncles")]
        //public object Uncles { get; set; }

        [JsonProperty("transactions")]
        public IEnumerable<EthereumTransaction> Transactions { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }
    }

}