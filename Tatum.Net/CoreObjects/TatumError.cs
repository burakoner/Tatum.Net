using CryptoExchange.Net.Objects;

namespace Tatum.Net.CoreObjects
{
    public class TatumError : Error
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        public TatumError(string message, object data = null) : base(null, message, data) { }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        public TatumError(int code, string message, object data = null) : base(code, message, data)
        {
        }
    }
}
