using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class TronTransactionRet
    {
        [JsonProperty("contractRet")]
        public string ContractRet { get; set; }

        //[JsonProperty("fee")]
        //public object Fee { get; set; }
    }

}
