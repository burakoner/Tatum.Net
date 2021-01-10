using Newtonsoft.Json;
using System.Collections.Generic;
using Tatum.Net.Attributes;

namespace Tatum.Net.RestObjects
{
    public class NeoBalance
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("assets")]
        public NeoBalanceData Assets { get; set; }

        [JsonProperty("tokens")]
        public NeoBalanceData Tokens { get; set; }
    }

    [JsonConverter(typeof(TypedDataConverter<NeoBalanceData>))]
    public class NeoBalanceData
    {
        [TypedData]
        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
    }
}
