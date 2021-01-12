using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class OffchainWithdrawalResponse
    {
        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("data")]
        public IEnumerable<OffchainWithdrawalData> Data { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
