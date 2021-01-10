using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class ScryptaTransactionOutput
    {
        [JsonProperty("value")]
        public decimal Value { get; set; }
        
        [JsonProperty("n")]
        public long N { get; set; }

        [JsonProperty("scriptPubKey")]
        public ScryptaTransactionScriptPublicKey ScriptPublicKey { get; set; }
    }
}
