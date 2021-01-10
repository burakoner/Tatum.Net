using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class VeChainTransactionTransfer
    {
        [JsonProperty("sender")]
        public string Sender { get; set; }

        [JsonProperty("recipient")]
        public string Recipient { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }
    }
}