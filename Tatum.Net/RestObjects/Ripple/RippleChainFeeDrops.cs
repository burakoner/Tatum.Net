using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class RippleChainFeeDrops
    {
        [JsonProperty("base_fee")]
        public string BaseFee { get; set; }

        [JsonProperty("median_fee")]
        public string MedianFee { get; set; }

        [JsonProperty("minimum_fee")]
        public string MinimumFee { get; set; }

        [JsonProperty("open_ledger_fee")]
        public string OpenLedgerFee { get; set; }
    }
}
