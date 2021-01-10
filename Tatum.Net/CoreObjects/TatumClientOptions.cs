using CryptoExchange.Net.Objects;

namespace Tatum.Net.CoreObjects
{
    public class TatumClientOptions : RestClientOptions
    {
        public TatumClientOptions():base("https://api-eu1.tatum.io")
        {
        }

        public TatumClientOptions Copy()
        {
            return Copy<TatumClientOptions>();
        }
    }
}
