using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class TronTransaction
    {
        [JsonProperty("rawData")]
        public TronTransactionRawData RawData { get; set; }

        [JsonProperty("ret")]
        public IEnumerable<TronTransactionRet> Ret { get; set; }

        [JsonProperty("signature")]
        public IEnumerable<string> Signatures { get; set; }

        [JsonProperty("txID")]
        public string TransactionId { get; set; }

        [JsonProperty("netFee")]
        public decimal NetFee { get; set; }

        [JsonProperty("netUsage")]
        public decimal NetUsage { get; set; }

        [JsonProperty("energyFee")]
        public decimal EnergyFee { get; set; }

        [JsonProperty("energyUsage")]
        public decimal EnergyUsage { get; set; }

        [JsonProperty("energyUsageTotal")]
        public decimal EnergyUsageTotal { get; set; }

        [JsonProperty("internalTransactions")]
        public IList<TronInternalTransaction> InternalTransactions { get; set; }
    }
}
