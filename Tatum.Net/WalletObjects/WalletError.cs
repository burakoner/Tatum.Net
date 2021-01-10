using CryptoExchange.Net.Objects;

namespace Tatum.Net.WalletObjects
{
    public class WalletError : Error
    {
        public WalletError(Error error) : base(error.Code, error.Message, error.Data) { }

        public WalletError(string message, object data = null) : base(null, message, data) { }

        public WalletError(int code, string message, object data = null) : base(code, message, data) { }
    }
}
