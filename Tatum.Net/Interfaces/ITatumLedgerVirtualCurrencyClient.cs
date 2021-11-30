using CryptoExchange.Net.Objects;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumLedgerVirtualCurrencyClient
    {
        WebCallResult<LedgerReport> Create(string name, string supply, string basePair, decimal baseRate = 1.0M, LedgerCustomerOptions customer = null, string description = null, string accountCode = null, string accountNumber = null, string accountingCurrency = null, CancellationToken ct = default);
        Task<WebCallResult<LedgerReport>> CreateAsync(string name, string supply, string basePair, decimal baseRate = 1.0M, LedgerCustomerOptions customer = null, string description = null, string accountCode = null, string accountNumber = null, string accountingCurrency = null, CancellationToken ct = default);
        WebCallResult<TatumReference> Destroy(string accountId, decimal amount, string paymentId = null, string reference = null, string transactionCode = null, string recipientNote = null, string counterAccount = null, string senderNote = null, CancellationToken ct = default);
        Task<WebCallResult<TatumReference>> DestroyAsync(string accountId, decimal amount, string paymentId = null, string reference = null, string transactionCode = null, string recipientNote = null, string counterAccount = null, string senderNote = null, CancellationToken ct = default);
        WebCallResult<LedgerReport> Get(string name, CancellationToken ct = default);
        Task<WebCallResult<LedgerReport>> GetAsync(string name, CancellationToken ct = default);
        WebCallResult<TatumReference> Mint(string accountId, decimal amount, string paymentId = null, string reference = null, string transactionCode = null, string recipientNote = null, string counterAccount = null, string senderNote = null, CancellationToken ct = default);
        Task<WebCallResult<TatumReference>> MintAsync(string accountId, decimal amount, string paymentId = null, string reference = null, string transactionCode = null, string recipientNote = null, string counterAccount = null, string senderNote = null, CancellationToken ct = default);
        WebCallResult<bool> Update(string name, string basePair = null, decimal? baseRate = null, CancellationToken ct = default);
        Task<WebCallResult<bool>> UpdateAsync(string name, string basePair = null, decimal? baseRate = null, CancellationToken ct = default);
    }
}
