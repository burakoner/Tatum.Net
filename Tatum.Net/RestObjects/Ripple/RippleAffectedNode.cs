using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class RippleAffectedNode
    {
        [JsonProperty("CreatedNode")]
        public RippleCreatedNode CreatedNode { get; set; }

        [JsonProperty("DeletedNode")]
        public RippleCreatedNode DeletedNode { get; set; }

        [JsonProperty("ModifiedNode")]
        public RippleCreatedNode ModifiedNode { get; set; }
    }
}
