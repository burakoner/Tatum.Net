namespace Tatum.Net.WalletObjects
{
    public class WalletResponse<T>
    {
        public T Data { get; set; }
        public WalletError Error { get; set; }
        public bool Success { get { return Error == null; } }

        public WalletResponse()
        {
        }

        public WalletResponse(T data)
        {
            Data = data;
        }

        public WalletResponse(WalletError error)
        {
            Error = error;
        }

        public WalletResponse(T data, WalletError error = null)
        {
            Data = data;
            Error = error;
        }

        public static WalletResponse<T> CreateSuccessResult(T data)
        {
            return new WalletResponse<T>(data);
        }

        public static WalletResponse<T> CreateErrorResult(WalletError error)
        {
            return new WalletResponse<T>(error);
        }
    }
}
