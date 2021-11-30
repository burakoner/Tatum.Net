using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.Enums;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumLedgerTransactionClient
    {
        WebCallResult<IEnumerable<LedgerTransaction>> GetTransactionsByAccount(string id, string counterAccount = null, DateTime? from = null, DateTime? to = null, string currency = null, LedgerTransactionType? transactionType = null, LedgerOperationType? opType = null, string transactionCode = null, string paymentId = null, string recipientNote = null, string senderNote = null, int pageSize = 50, int offset = 0, bool? count = null, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<LedgerTransaction>>> GetTransactionsByAccountAsync(string id, string counterAccount = null, DateTime? from = null, DateTime? to = null, string currency = null, LedgerTransactionType? transactionType = null, LedgerOperationType? opType = null, string transactionCode = null, string paymentId = null, string recipientNote = null, string senderNote = null, int pageSize = 50, int offset = 0, bool? count = null, CancellationToken ct = default);
        WebCallResult<IEnumerable<LedgerTransaction>> GetTransactionsByCustomer(string id, string account = null, string counterAccount = null, DateTime? from = null, DateTime? to = null, string currency = null, LedgerTransactionType? transactionType = null, LedgerOperationType? opType = null, string transactionCode = null, string paymentId = null, string recipientNote = null, string senderNote = null, int pageSize = 50, int offset = 0, bool? count = null, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<LedgerTransaction>>> GetTransactionsByCustomerAsync(string id, string account = null, string counterAccount = null, DateTime? from = null, DateTime? to = null, string currency = null, LedgerTransactionType? transactionType = null, LedgerOperationType? opType = null, string transactionCode = null, string paymentId = null, string recipientNote = null, string senderNote = null, int pageSize = 50, int offset = 0, bool? count = null, CancellationToken ct = default);
        WebCallResult<IEnumerable<LedgerTransaction>> GetTransactionsByLedger(string account = null, string counterAccount = null, DateTime? from = null, DateTime? to = null, string currency = null, LedgerTransactionType? transactionType = null, LedgerOperationType? opType = null, string transactionCode = null, string paymentId = null, string recipientNote = null, string senderNote = null, int pageSize = 50, int offset = 0, bool? count = null, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<LedgerTransaction>>> GetTransactionsByLedgerAsync(string account = null, string counterAccount = null, DateTime? from = null, DateTime? to = null, string currency = null, LedgerTransactionType? transactionType = null, LedgerOperationType? opType = null, string transactionCode = null, string paymentId = null, string recipientNote = null, string senderNote = null, int pageSize = 50, int offset = 0, bool? count = null, CancellationToken ct = default);
        WebCallResult<IEnumerable<LedgerTransaction>> GetTransactionsByReference(string reference, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<LedgerTransaction>>> GetTransactionsByReferenceAsync(string reference, CancellationToken ct = default);
        WebCallResult<TatumReference> SendPayment(string senderAccountId, string recipientAccountId, decimal amount, bool anonymous = false, bool? compliant = null, string transactionCode = null, string paymentId = null, string recipientNote = null, decimal baseRate = 1.0M, string senderNote = null, CancellationToken ct = default);
        Task<WebCallResult<TatumReference>> SendPaymentAsync(string senderAccountId, string recipientAccountId, decimal amount, bool anonymous = false, bool? compliant = null, string transactionCode = null, string paymentId = null, string recipientNote = null, decimal baseRate = 1.0M, string senderNote = null, CancellationToken ct = default);
    }
}
