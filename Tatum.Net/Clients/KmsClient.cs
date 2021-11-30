using CryptoExchange.Net.Objects;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.Converters;
using Tatum.Net.Enums;
using Tatum.Net.Interfaces;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Clients
{
    public class KmsClient : ITatumSecurityKMSClient
    {
        public TatumClient Tatum { get; protected set; }

        #region API Endpoints

        #region Security Key Management System
        protected const string Endpoints_GetPendingTransactions = "kms/pending/{0}";
        protected const string Endpoints_CompletePendingTransaction = "kms/{0}/{1}";
        protected const string Endpoints_Transaction = "kms/{0}";
        #endregion

        #endregion

        public KmsClient(TatumClient tatumClient)
        {
            Tatum = tatumClient;
        }

        #region Security / Key Management System
        /// <summary>
        /// <b>Title:</b> Get pending transactions to sign<br />
        /// <b>Credits:</b> 1 credits per API call.<br />
        /// <b>Description:</b>
        /// Get list of pending transaction to be signed and broadcast using Tatum KMS.
        /// </summary>
        /// <param name="chain">Blockchain to get pending transactions for.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<KMSPendingTransaction>> GetPendingTransactions(BlockchainType chain, CancellationToken ct = default) => GetPendingTransactionsAsync(chain, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get pending transactions to sign<br />
        /// <b>Credits:</b> 1 credits per API call.<br />
        /// <b>Description:</b>
        /// Get list of pending transaction to be signed and broadcast using Tatum KMS.
        /// </summary>
        /// <param name="chain">Blockchain to get pending transactions for.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<KMSPendingTransaction>>> GetPendingTransactionsAsync(BlockchainType chain, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetPendingTransactions, JsonConvert.SerializeObject(chain, new BlockchainTypeConverter(false))));
            return await Tatum.SendTatumRequest<IEnumerable<KMSPendingTransaction>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Complete pending transaction to sign<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Mark pending transaction to sign as a complete and update it with a transactionID from the blockchain.
        /// </summary>
        /// <param name="id">ID of pending transaction</param>
        /// <param name="txId">transaction ID of blockchain transaction</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> CompletePendingTransaction(string id, string txId, CancellationToken ct = default) => CompletePendingTransactionAsync(id, txId, ct).Result;
        /// <summary>
        /// <b>Title:</b> Complete pending transaction to sign<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Mark pending transaction to sign as a complete and update it with a transactionID from the blockchain.
        /// </summary>
        /// <param name="id">ID of pending transaction</param>
        /// <param name="txId">transaction ID of blockchain transaction</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> CompletePendingTransactionAsync(string id, string txId, CancellationToken ct = default)
        {
            var credits = 2;
            var url = Tatum.GetUrl(string.Format(Endpoints_CompletePendingTransaction, id, txId));
            var result = await Tatum.SendTatumRequest<string>(url, HttpMethod.Delete, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Get transaction details<br />
        /// <b>Credits:</b> 1 credits per API call.<br />
        /// <b>Description:</b>
        /// Get detail of transaction to be signed / that was already signed and contains transactionId.
        /// </summary>
        /// <param name="id">ID of transaction</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<KMSPendingTransaction> GetTransaction(string id, CancellationToken ct = default) => GetTransactionAsync(id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get transaction details<br />
        /// <b>Credits:</b> 1 credits per API call.<br />
        /// <b>Description:</b>
        /// Get detail of transaction to be signed / that was already signed and contains transactionId.
        /// </summary>
        /// <param name="id">ID of transaction</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<KMSPendingTransaction>> GetTransactionAsync(string id, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Tatum.GetUrl(string.Format(Endpoints_Transaction, id));
            return await Tatum.SendTatumRequest<KMSPendingTransaction>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Delete transaction<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Delete transaction to be signed. When deleting offchain transaction, linked withdrawal will be cancelled automatically.
        /// </summary>
        /// <param name="id">ID of transaction,</param>
        /// <param name="revert">Defines whether fee should be reverted to account balance as well as amount. Defaults to true. Revert true would be typically used when withdrawal was not broadcast to blockchain. False is used usually for Ethereum ERC20 based currencies.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> DeleteTransaction(string id, bool revert = true, CancellationToken ct = default) => DeleteTransactionAsync(id, revert, ct).Result;
        /// <summary>
        /// <b>Title:</b> Delete transaction<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Delete transaction to be signed. When deleting offchain transaction, linked withdrawal will be cancelled automatically.
        /// </summary>
        /// <param name="id">ID of transaction,</param>
        /// <param name="revert">Defines whether fee should be reverted to account balance as well as amount. Defaults to true. Revert true would be typically used when withdrawal was not broadcast to blockchain. False is used usually for Ethereum ERC20 based currencies.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> DeleteTransactionAsync(string id, bool revert = true, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "revert", revert }
            };

            var credits = 2;
            var url = Tatum.GetUrl(string.Format(Endpoints_Transaction, id));
            var result = await Tatum.SendTatumRequest<string>(url, HttpMethod.Delete, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }
        #endregion

    }
}
