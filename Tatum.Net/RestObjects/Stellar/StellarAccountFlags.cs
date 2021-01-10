using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class StellarAccountFlags
    {
        [JsonProperty("auth_required")]
        public bool AuthRequired { get; set; }

        [JsonProperty("auth_revocable")]
        public bool AuthRevocable { get; set; }

        [JsonProperty("auth_immutable")]
        public bool AuthImmutable { get; set; }
    }
}
