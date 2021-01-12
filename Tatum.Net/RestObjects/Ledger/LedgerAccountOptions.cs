using Newtonsoft.Json;
using Tatum.Net.Converters;
using Tatum.Net.Enums;

namespace Tatum.Net.RestObjects
{
    public class LedgerAccountOptions
    {
        [JsonProperty("currency"), JsonConverter(typeof(BlockchainTypeConverter))]
        public BlockchainType Blockchain { get; set; }

        /// <summary>
        /// Extended public key to generate addresses from.
        /// </summary>
        [JsonProperty("xpub")]
        public string ExtendedPublicKey { get; set; }

        /// <summary>
        /// If customer is filled then is created or updated.
        /// </summary>
        [JsonProperty("customer")]
        public LedgerCustomerOptions Customer { get; set; }

        /// <summary>
        /// Enable compliant checks. 
        /// If this is enabled, it is impossible to create account if compliant check fails.
        /// </summary>
        [JsonProperty("compliant")]
        public bool Compliant { get; set; }

        /// <summary>
        /// For bookkeeping to distinct account purpose.
        /// </summary>
        [JsonProperty("accountCode")]
        public string AccountCode { get; set; }

        /// <summary>
        /// All transaction will be accounted in this currency for all accounts. 
        /// Currency can be overridden per account level. 
        /// If not set, customer accountingCurrency is used or EUR by default. ISO-4217
        /// </summary>
        [JsonProperty("accountingCurrency"), JsonConverter(typeof(FiatCurrencyConverter))]
        public FiatCurrency AccountingCurrency { get; set; }

        /// <summary>
        /// Account number from external system.
        /// </summary>
        [JsonProperty("accountNumber")]
        public string AccountNumber { get; set; }
    }
}
