using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class ScryptaScriptSignature
    {
        [JsonProperty("asm")]
        public string Asm { get; set; }

        [JsonProperty("hex")]
        public string Hex { get; set; }
    }
}
