using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tatum.Net.RestObjects
{
    public class NeoContract
    {
        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("code_version")]
        public string CodeVersion { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("parameters")]
        public IEnumerable<string> Parameters { get; set; }

        [JsonProperty("properties")]
        public NeoContractProperties Properties { get; set; }

        [JsonProperty("returntype")]
        public string Returntype { get; set; }

        [JsonProperty("script")]
        public string Script { get; set; }
    }
}
