using Newtonsoft.Json;
using System.Collections.Generic;
using Tatum.Net.Attributes;

namespace Tatum.Net.RestObjects
{
    public class NeoAccountTransaction
    {
        [JsonProperty("txid")]
        public string TransactionId { get; set; }

        [JsonProperty("blockHeight")]
        public long BlockHeight { get; set; }

        [JsonProperty("change")]
        public NeoAccountChange Change { get; set; }
    }

    [JsonConverter(typeof(TypedDataConverter<NeoAccountChange>))]
    public class NeoAccountChange
    {
        [TypedData]
        public Dictionary<string, string> Assets { get; set; } = new Dictionary<string, string>();
    }
}
