using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class BitcoinCashTransaction
    {
        [JsonProperty("hex")]
        public string Hex { get; set; }

        [JsonProperty("txid")]
        public string TxId { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("size")]
        public long? Size { get; set; }

        [JsonProperty("vsize")]
        public long? VSize { get; set; }

        [JsonProperty("version")]
        public long Version { get; set; }

        [JsonProperty("locktime")]
        public long LockTime { get; set; }

        [JsonProperty("blockhash")]
        public string BlockHash { get; set; }

        [JsonProperty("confirmations")]
        public long Confirmations { get; set; }

        [JsonProperty("time"), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime Time { get; set; }

        [JsonProperty("blocktime"), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime BlockTime { get; set; }

        [JsonProperty("vin")]
        public IEnumerable<BitcoinCashTransactionInput> Inputs { get; set; }

        [JsonProperty("vout")]
        public IEnumerable<BitcoinCashTransactionOutput> Outputs { get; set; }
    }

}
