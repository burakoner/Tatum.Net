using CryptoExchange.Net.Objects;
using Tatum.Net.Enums;

namespace Tatum.Net.CoreObjects
{
    public class TatumClientOptions : RestClientOptions
    {
        public ServerRegion Region { get; set; }

        public TatumClientOptions() : this(ServerRegion.Europe)
        {
        }

        public TatumClientOptions(ServerRegion region) : base(region == ServerRegion.Europe ? "https://api-eu1.tatum.io" : "https://api-us-west1.tatum.io")
        {
            this.Region = region;
        }

        public TatumClientOptions Copy()
        {
            return Copy<TatumClientOptions>();
        }
    }
}
