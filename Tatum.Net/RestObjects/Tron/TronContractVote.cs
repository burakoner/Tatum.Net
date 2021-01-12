using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class TronContractVote
    {
        [JsonProperty("vote_address")]
        public string VoteAddress { get; set; }

        [JsonProperty("vote_count")]
        public long VoteCount { get; set; }
    }
}
