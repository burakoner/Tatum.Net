using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class VeChainTransactionOutput
    {
        //[JsonProperty("contractAddress")]
        //public string ContractAddress { get; set; }

        [JsonProperty("events")]
        public IEnumerable<VeChainTransactionEvent> Events { get; set; }

        [JsonProperty("transfers")]
        public IEnumerable<VeChainTransactionTransfer> Transfers { get; set; }
    }
}