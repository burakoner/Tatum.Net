using CryptoExchange.Net.Objects;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumLedgerVirtualCurrencyClient
    {
        WebCallResult<LedgerReport> LedgerVirtualCurrency_Create(string name, string supply, string basePair, decimal baseRate = 1.0M, LedgerCustomerOptions customer = null, string description = null, string accountCode = null, string accountNumber = null, string accountingCurrency = null, CancellationToken ct = default);
        Task<WebCallResult<LedgerReport>> LedgerVirtualCurrency_Create_Async(string name, string supply, string basePair, decimal baseRate = 1.0M, LedgerCustomerOptions customer = null, string description = null, string accountCode = null, string accountNumber = null, string accountingCurrency = null, CancellationToken ct = default);
        WebCallResult<TatumReference> LedgerVirtualCurrency_Destroy(string accountId, decimal amount, string paymentId = null, string reference = null, string transactionCode = null, string recipientNote = null, string counterAccount = null, string senderNote = null, CancellationToken ct = default);
        Task<WebCallResult<TatumReference>> LedgerVirtualCurrency_Destroy_Async(string accountId, decimal amount, string paymentId = null, string reference = null, string transactionCode = null, string recipientNote = null, string counterAccount = null, string senderNote = null, CancellationToken ct = default);
        WebCallResult<LedgerReport> LedgerVirtualCurrency_Get(string name, CancellationToken ct = default);
        Task<WebCallResult<LedgerReport>> LedgerVirtualCurrency_Get_Async(string name, CancellationToken ct = default);
        WebCallResult<TatumReference> LedgerVirtualCurrency_Mint(string accountId, decimal amount, string paymentId = null, string reference = null, string transactionCode = null, string recipientNote = null, string counterAccount = null, string senderNote = null, CancellationToken ct = default);
        Task<WebCallResult<TatumReference>> LedgerVirtualCurrency_Mint_Async(string accountId, decimal amount, string paymentId = null, string reference = null, string transactionCode = null, string recipientNote = null, string counterAccount = null, string senderNote = null, CancellationToken ct = default);
        WebCallResult<bool> LedgerVirtualCurrency_Update(string name, string basePair = null, decimal? baseRate = null, CancellationToken ct = default);
        Task<WebCallResult<bool>> LedgerVirtualCurrency_Update_Async(string name, string basePair = null, decimal? baseRate = null, CancellationToken ct = default);
    }
}
