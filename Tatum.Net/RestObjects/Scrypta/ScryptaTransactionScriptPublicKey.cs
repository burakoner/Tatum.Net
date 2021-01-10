using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class ScryptaTransactionScriptPublicKey
    {
        [JsonProperty("asm")]
        public string Asm { get; set; }
        
        [JsonProperty("hex")]
        public string Hex { get; set; }
        
        [JsonProperty("reqSigs")]
        public long RequiredSignatures { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("addresses")]
        public IEnumerable<string> Addresses { get; set; }
    }
}
