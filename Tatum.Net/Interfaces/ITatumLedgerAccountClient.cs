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
        WebCallResult<bool> LedgerAccount_Activate(string account_id, CancellationToken ct = default);
        Task<WebCallResult<bool>> LedgerAccount_Activate_Async(string account_id, CancellationToken ct = default);
        WebCallResult<TatumId> LedgerAccount_BlockAmount(string account_id, decimal amount, string type, string description, CancellationToken ct = default);
        Task<WebCallResult<TatumId>> LedgerAccount_BlockAmount_Async(string account_id, decimal amount, string type, string description, CancellationToken ct = default);
        WebCallResult<LedgerAccount> LedgerAccount_Create(BlockchainType chain, LedgerAccountOptions options = null, CancellationToken ct = default);
        WebCallResult<IEnumerable<LedgerAccount>> LedgerAccount_CreateBatch(IEnumerable<LedgerAccountOptions> accounts, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<LedgerAccount>>> LedgerAccount_CreateBatch_Async(IEnumerable<LedgerAccountOptions> accounts, CancellationToken ct = default);
        Task<WebCallResult<LedgerAccount>> LedgerAccount_Create_Async(BlockchainType chain, LedgerAccountOptions options = null, CancellationToken ct = default);
        WebCallResult<bool> LedgerAccount_Deactivate(string account_id, CancellationToken ct = default);
        Task<WebCallResult<bool>> LedgerAccount_Deactivate_Async(string account_id, CancellationToken ct = default);
        WebCallResult<bool> LedgerAccount_Freeze(string account_id, CancellationToken ct = default);
        Task<WebCallResult<bool>> LedgerAccount_Freeze_Async(string account_id, CancellationToken ct = default);
        WebCallResult<IEnumerable<LedgerAccount>> LedgerAccount_GetAccounts(int pageSize = 50, int offset = 0, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<LedgerAccount>>> LedgerAccount_GetAccounts_Async(int pageSize = 50, int offset = 0, CancellationToken ct = default);
        WebCallResult<LedgerBalance> LedgerAccount_GetBalance(string account_id, CancellationToken ct = default);
        Task<WebCallResult<LedgerBalance>> LedgerAccount_GetBalance_Async(string account_id, CancellationToken ct = default);
        WebCallResult<IEnumerable<LedgerBlockedAmount>> LedgerAccount_GetBlockedAmounts(string account_id, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<LedgerBlockedAmount>>> LedgerAccount_GetBlockedAmounts_Async(string account_id, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        WebCallResult<IEnumerable<LedgerAccount>> LedgerAccount_GetByCustomerId(string customer_id, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<LedgerAccount>>> LedgerAccount_GetByCustomerId_Async(string customer_id, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        WebCallResult<LedgerAccount> LedgerAccount_GetById(string account_id, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        Task<WebCallResult<LedgerAccount>> LedgerAccount_GetById_Async(string account_id, int pageSize = 50, int offset = 0, CancellationToken ct = default);
        WebCallResult<bool> LedgerAccount_UnblockAllBlockedAmounts(string account_id, CancellationToken ct = default);
        Task<WebCallResult<bool>> LedgerAccount_UnblockAllBlockedAmounts_Async(string account_id, CancellationToken ct = default);
        WebCallResult<bool> LedgerAccount_UnblockAmount(string blockage_id, CancellationToken ct = default);
        Task<WebCallResult<bool>> LedgerAccount_UnblockAmount_Async(string blockage_id, CancellationToken ct = default);
        WebCallResult<bool> LedgerAccount_Unfreeze(string account_id, CancellationToken ct = default);
        Task<WebCallResult<bool>> LedgerAccount_Unfreeze_Async(string account_id, CancellationToken ct = default);
        WebCallResult<TatumReference> LedgerAccount_UnlockAmountAndPerformTransaction(string blockage_id, string recipientAccountId, decimal amount, bool anonymous, bool compliant, string transactionCode, string paymentId, string recipientNote, string senderNote, decimal baseRate = 1, CancellationToken ct = default);
        Task<WebCallResult<TatumReference>> LedgerAccount_UnlockAmountAndPerformTransaction_Async(string blockage_id, string recipientAccountId, decimal amount, bool anonymous, bool compliant, string transactionCode, string paymentId, string recipientNote, string senderNote, decimal baseRate = 1, CancellationToken ct = default);
        WebCallResult<bool> LedgerAccount_Update(string account_id, string accountCode, string accountNumber, CancellationToken ct = default);
        Task<WebCallResult<bool>> LedgerAccount_Update_Async(string account_id, string accountCode, string accountNumber, CancellationToken ct = default);
    }
}
