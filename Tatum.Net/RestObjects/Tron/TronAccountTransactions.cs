using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class TronAccountTransactions
    {
        [JsonProperty("next")]
        public string Next { get; set; }

        [JsonProperty("transactions")]
        public IEnumerable<TronTransaction> Transactions { get; set; }
    }
}
