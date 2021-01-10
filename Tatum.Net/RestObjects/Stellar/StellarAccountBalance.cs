using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class StellarAccountBalance
    {
        [JsonProperty("balance")]
        public string Balance { get; set; }

        [JsonProperty("limit")]
        public string Limit { get; set; }

        [JsonProperty("buying_liabilities")]
        public string BuyingLiabilities { get; set; }

        [JsonProperty("selling_liabilities")]
        public string SellingLiabilities { get; set; }

        [JsonProperty("last_modified_ledger")]
        public long LastModifiedLedger { get; set; }

        [JsonProperty("is_authorized")]
        public bool IsAuthorized { get; set; }

        [JsonProperty("is_authorized_to_maintain_liabilities")]
        public bool IsAuthorizedToMaintainLiabilities { get; set; }

        [JsonProperty("asset_type")]
        public string AssetType { get; set; }

        [JsonProperty("asset_code")]
        public string AssetCode { get; set; }

        [JsonProperty("asset_issuer")]
        public string AssetIssuer { get; set; }
    }
}
