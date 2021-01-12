using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class TronContractParameterValue
    {
        [JsonProperty("contract_address")]
        public string ContractAddress { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("owner_address")]
        public string OwnerAddress { get; set; }

        [JsonProperty("to_address")]
        public string RecepientAddress { get; set; }

        [JsonProperty("votes")]
        public IEnumerable<TronContractVote> Votes { get; set; }
    }
}
