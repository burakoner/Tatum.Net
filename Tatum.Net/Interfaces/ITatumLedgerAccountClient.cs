using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.Enums;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumLedgerAccountClient
    {
        WebCallResult<bool> Activate(string id, CancellationToken ct = default);
        Task<WebCallResult<bool>> ActivateAsync(string id, CancellationToken ct = default);
        WebCallResult<TatumId> BlockAmount(string id, decimal amount, string type, string description, CancellationToken ct = default);
        Task<WebCallResult<TatumId>> BlockAmountAsync(string id, decimal amount, string type, string description, CancellationToken ct = default);
        WebCallResult<LedgerAccount> Create(BlockchainType chain, LedgerAccountOptions options = null, CancellationToken ct = default);
        WebCallResult<IEnumerable<LedgerAccount>> CreateBatch(IEnumerable<LedgerAccountOptions> accounts, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<LedgerAccount>>> CreateBatchAsync(IEnumerable<LedgerAccountOptions> accounts, CancellationToken ct = default);
        Task<WebCallResult<LedgerAccount>> CreateAsync(BlockchainType chain, LedgerAccountOptions options = null, CancellationToken ct = default);
        WebCallResult<bool> Deactivate(string id, CancellationToken ct = default);
        Task<WebCallResult<bool>> DeactivateAsync(string id, CancellationToken ct = default);
        WebCallResult<bool> Freeze(string id, CancellationToken ct = default);
        Task<WebCallResult<bool>> FreezeAsync(string id, CancellationToken ct = default);
        WebCallResult<IEnumerable<LedgerAccount>> GetAccounts(int pageSize = 50, int offset = 0, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<LedgerAccount>>> GetAccountsAsync(int pageSize = 50, int offset = 0, CancellationToken ct = default);
        WebCallResult<LedgerBalance> GetBalance(string id, CancellationToken ct = default);
        Task<WebCallResult<LedgerBalance>> GetBalanceAsync(string id, CancellationToken ct = default);
        WebCallResult<IEnumerable<LedgerBlockedAmount>> GetBlockedAmounts(string id, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<LedgerBlockedAmount>>> GetBlockedAmountsAsync(string id, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        WebCallResult<IEnumerable<LedgerAccount>> GetByCustomerId(string customer_id, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<LedgerAccount>>> GetByCustomerIdAsync(string customer_id, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        WebCallResult<LedgerAccount> GetById(string id, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        Task<WebCallResult<LedgerAccount>> GetByIdAsync(string id, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        WebCallResult<bool> UnblockAllBlockedAmounts(string id, CancellationToken ct = default);
        Task<WebCallResult<bool>> UnblockAllBlockedAmountsAsync(string id, CancellationToken ct = default);
        WebCallResult<bool> UnblockAmount(string blockage_id, CancellationToken ct = default);
        Task<WebCallResult<bool>> UnblockAmountAsync(string blockage_id, CancellationToken ct = default);
        WebCallResult<bool> Unfreeze(string id, CancellationToken ct = default);
        Task<WebCallResult<bool>> UnfreezeAsync(string id, CancellationToken ct = default);
        WebCallResult<TatumReference> UnlockAmountAndPerformTransaction(string blockage_id, string recipientAccountId, decimal amount, bool anonymous, bool compliant, string transactionCode, string paymentId, string recipientNote, string senderNote, decimal baseRate = 1, CancellationToken ct = default);
        Task<WebCallResult<TatumReference>> UnlockAmountAndPerformTransactionAsync(string blockage_id, string recipientAccountId, decimal amount, bool anonymous, bool compliant, string transactionCode, string paymentId, string recipientNote, string senderNote, decimal baseRate = 1, CancellationToken ct = default);
        WebCallResult<bool> Update(string id, string accountCode, string accountNumber, CancellationToken ct = default);
        Task<WebCallResult<bool>> UpdateAsync(string id, string accountCode, string accountNumber, CancellationToken ct = default);
    }
}
