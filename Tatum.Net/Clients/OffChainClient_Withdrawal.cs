using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.Interfaces;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Clients
{
    public class OffChainWithdrawalClient : ITatumOffchainWithdrawalClient
    {
        public OffChainClient Offchain { get; protected set; }

        #region API Endpoints

        #region Off-chain Withdrawal
        protected const string Endpoints_Store = "offchain/withdrawal";
        protected const string Endpoints_Complete = "offchain/withdrawal/{0}/{1}";
        protected const string Endpoints_Cancel = "offchain/withdrawal/{0";
        protected const string Endpoints_Broadcast = "offchain/withdrawal/broadcast";
        #endregion

        #endregion

        public OffChainWithdrawalClient(OffChainClient offChainClient)
        {
            Offchain = offChainClient;
        }


        #region Off-chain / Withdrawal
        /// <summary>
        /// <b>Title:</b> Store withdrawal<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create a withdrawal from Tatum Ledger account to the blockchain.<br />
        /// <b>BTC, LTC, BCH</b><br />
        /// When withdrawal from Tatum is executed, all deposits, which are not processed yet are used as an input and change is moved to pool address 0 of wallet for defined account's xpub. If there are no unspent deposits, only last pool address 0 UTXO is used. If balance of wallet is not sufficient, it is impossible to perform withdrawal from this account -> funds were transferred to another linked wallet within system or outside of Tatum visibility.<br />
        /// For the first time of withdrawal from wallet, there must be some deposit made and funds are obtained from that.Since there is no withdrawal, there was no transfer to pool address 0 and thus it is not present in vIn.Pool transfer is identified by missing data.address property in response.When last not cancelled withdrawal is not completed and thus there is no tx id of output transaction given, we cannot perform next withdrawal.<br />
        /// <b>ETH</b><br />
        /// Withdrawal from Tatum can be processed only from 1 account.In Ethereum Blockchain, each address is recognized as an account and only funds from that account can be sent in 1 transaction.Example: Account A has 0.5 ETH, Account B has 0.3 ETH.Account A is linked to Tatum Account 1, Account B is linked to Tatum Account 2. Tatum Account 1 has balance 0.7 Ethereum and Tatum Account 2 has 0.1 ETH.Withdrawal from Tatum Account 1 can be at most 0.5 ETH, even though balance in Tatum Private Ledger is 0.7 ETH.Because of this Ethereum Blockchain limitation, withdrawal request should always contain sourceAddress, from which withdrawal will be made. To get available balances for Ethereoum wallet accounts, use hint endpoint.<br />
        /// <b>XRP</b><br />
        /// XRP withdrawal can contain DestinationTag except of address, which is placed in attr parameter of withdrawal request.SourceTag of the blockchain transaction should be withdrawal ID for autocomplete purposes of withdrawals.<br />
        /// <b>XLM</b><br />
        /// XLM withdrawal can contain memo except of address, which is placed in attr parameter of withdrawal request.XLM blockchain does not have possibility to enter source account information.It is possible to create memo in format 'destination|source', which is supported way of memo in Tatum and also there is information about the sender account in the blockchain.<br />
        /// When withdrawal is created, all other withdrawals with the same currency are pending, unless the current one is marked as complete or cancelled.<br />
        /// Tatum ledger transaction is created for every withdrawal request with operation type WITHDRAWAL.The value of the transaction is the withdrawal amount + blockchain fee, which should be paid. In the situation, when there is withdrawal for ERC20, XLM, or XRP based custom assets, the fee is not included in the transaction because it is paid in different assets than the withdrawal itself.<br />
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="address">Blockchain address to send assets to. For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain addresses as a comma separated string.</param>
        /// <param name="amount">Amount to be withdrawn to blockchain.</param>
        /// <param name="attr">Used to parametrize withdrawal. Used for XRP withdrawal to define destination tag of recipient, or XLM memo of the recipient, if needed. For Bitcoin, Litecoin, Bitcoin Cash, used as a change address for left coins from transaction.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="fee">Fee to be submitted as a transaction fee to blockchain.</param>
        /// <param name="multipleAmounts">For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain amounts. List of recipient addresses must be present in the address field and total sum of amounts must be equal to the amount field.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OffchainWithdrawalResponse> Request(
            string senderAccountId, string address, decimal amount, string attr = null,
            bool? compliant = null, decimal? fee = null, IEnumerable<string> multipleAmounts = null, string paymentId = null, string senderNote = null,
            CancellationToken ct = default)
            => RequestAsync(
            senderAccountId, address, amount, attr,
            compliant, fee, multipleAmounts, paymentId, senderNote,
            ct).Result;
        /// <summary>
        /// <b>Title:</b> Store withdrawal<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create a withdrawal from Tatum Ledger account to the blockchain.<br />
        /// <b>BTC, LTC, BCH</b><br />
        /// When withdrawal from Tatum is executed, all deposits, which are not processed yet are used as an input and change is moved to pool address 0 of wallet for defined account's xpub. If there are no unspent deposits, only last pool address 0 UTXO is used. If balance of wallet is not sufficient, it is impossible to perform withdrawal from this account -> funds were transferred to another linked wallet within system or outside of Tatum visibility.<br />
        /// For the first time of withdrawal from wallet, there must be some deposit made and funds are obtained from that.Since there is no withdrawal, there was no transfer to pool address 0 and thus it is not present in vIn.Pool transfer is identified by missing data.address property in response.When last not cancelled withdrawal is not completed and thus there is no tx id of output transaction given, we cannot perform next withdrawal.<br />
        /// <b>ETH</b><br />
        /// Withdrawal from Tatum can be processed only from 1 account.In Ethereum Blockchain, each address is recognized as an account and only funds from that account can be sent in 1 transaction.Example: Account A has 0.5 ETH, Account B has 0.3 ETH.Account A is linked to Tatum Account 1, Account B is linked to Tatum Account 2. Tatum Account 1 has balance 0.7 Ethereum and Tatum Account 2 has 0.1 ETH.Withdrawal from Tatum Account 1 can be at most 0.5 ETH, even though balance in Tatum Private Ledger is 0.7 ETH.Because of this Ethereum Blockchain limitation, withdrawal request should always contain sourceAddress, from which withdrawal will be made. To get available balances for Ethereoum wallet accounts, use hint endpoint.<br />
        /// <b>XRP</b><br />
        /// XRP withdrawal can contain DestinationTag except of address, which is placed in attr parameter of withdrawal request.SourceTag of the blockchain transaction should be withdrawal ID for autocomplete purposes of withdrawals.<br />
        /// <b>XLM</b><br />
        /// XLM withdrawal can contain memo except of address, which is placed in attr parameter of withdrawal request.XLM blockchain does not have possibility to enter source account information.It is possible to create memo in format 'destination|source', which is supported way of memo in Tatum and also there is information about the sender account in the blockchain.<br />
        /// When withdrawal is created, all other withdrawals with the same currency are pending, unless the current one is marked as complete or cancelled.<br />
        /// Tatum ledger transaction is created for every withdrawal request with operation type WITHDRAWAL.The value of the transaction is the withdrawal amount + blockchain fee, which should be paid. In the situation, when there is withdrawal for ERC20, XLM, or XRP based custom assets, the fee is not included in the transaction because it is paid in different assets than the withdrawal itself.<br />
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="address">Blockchain address to send assets to. For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain addresses as a comma separated string.</param>
        /// <param name="amount">Amount to be withdrawn to blockchain.</param>
        /// <param name="attr">Used to parametrize withdrawal. Used for XRP withdrawal to define destination tag of recipient, or XLM memo of the recipient, if needed. For Bitcoin, Litecoin, Bitcoin Cash, used as a change address for left coins from transaction.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="fee">Fee to be submitted as a transaction fee to blockchain.</param>
        /// <param name="multipleAmounts">For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain amounts. List of recipient addresses must be present in the address field and total sum of amounts must be equal to the amount field.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OffchainWithdrawalResponse>> RequestAsync(
            string senderAccountId, string address, decimal amount, string attr = null,
            bool? compliant = null, decimal? fee = null, IEnumerable<string> multipleAmounts = null, string paymentId = null, string senderNote = null,
            CancellationToken ct = default)
        {
            var credits = 4;
            var ci = CultureInfo.InvariantCulture;
            var parameters = new Dictionary<string, object>
            {
                { "senderAccountId", senderAccountId },
                { "address", address },
                { "amount", amount.ToString(ci) },
            };
            parameters.AddOptionalParameter("attr", attr);
            parameters.AddOptionalParameter("compliant", compliant);
            parameters.AddOptionalParameter("fee", fee?.ToString(ci));

            if (multipleAmounts != null)
            {
                var lst = new List<string>();
                foreach (var ma in multipleAmounts) lst.Add(ma.ToString(ci));
                parameters.AddOptionalParameter("multipleAmounts", lst);
            }

            parameters.AddOptionalParameter("paymentId", paymentId);
            parameters.AddOptionalParameter("senderNote", senderNote);

            var url = Offchain.Tatum.GetUrl(string.Format(Endpoints_Store));
            var result = await Offchain.Tatum.SendTatumRequest<OffchainWithdrawalResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<OffchainWithdrawalResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<OffchainWithdrawalResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Complete withdrawal<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Invoke complete withdrawal as soon as blockchain transaction ID is available. All other withdrawals for the same currency will be pending unless the last one is processed and marked as completed.
        /// </summary>
        /// <param name="id">ID of created withdrawal</param>
        /// <param name="txId">Blockchain transaction ID of created withdrawal</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> CompleteRequest(string id, string txId, CancellationToken ct = default) => CompleteRequestAsync(id, txId, ct).Result;
        /// <summary>
        /// <b>Title:</b> Complete withdrawal<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Invoke complete withdrawal as soon as blockchain transaction ID is available. All other withdrawals for the same currency will be pending unless the last one is processed and marked as completed.
        /// </summary>
        /// <param name="id">ID of created withdrawal</param>
        /// <param name="txId">Blockchain transaction ID of created withdrawal</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> CompleteRequestAsync(string id, string txId, CancellationToken ct = default)
        {
            var credits = 2;
            var url = Offchain.Tatum.GetUrl(string.Format(Endpoints_Complete, id, txId));
            var result = await Offchain.Tatum.SendTatumRequest<string>(url, HttpMethod.Put, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Cancel withdrawal<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// This method is helpful if you need to cancel the withdrawal if the blockchain transaction fails or is not yet processed. This does not cancel already broadcast blockchain transaction, only Tatum internal withdrawal, and the ledger transaction, that was linked to this withdrawal.
        /// By default, the transaction fee is included in the reverted transaction.There are situations, like sending ERC20 on ETH, or XLM or XRP based assets, when the fee should not be reverted, because the fee is in calculated in Ethereum and transaction was in ERC20 currency.In this situation, only the transaction amount should be reverted, not the fee.
        /// </summary>
        /// <param name="id">ID of created withdrawal</param>
        /// <param name="revert">Defines whether fee should be reverted to account balance as well as amount. Defaults to true. Revert true would be typically used when withdrawal was not broadcast to blockchain. False is used usually for Ethereum based currencies when gas was consumed but transaction was reverted.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> Cancel(string id, bool revert = true, CancellationToken ct = default) => CancelAsync(id, revert, ct).Result;
        /// <summary>
        /// <b>Title:</b> Cancel withdrawal<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// This method is helpful if you need to cancel the withdrawal if the blockchain transaction fails or is not yet processed. This does not cancel already broadcast blockchain transaction, only Tatum internal withdrawal, and the ledger transaction, that was linked to this withdrawal.
        /// By default, the transaction fee is included in the reverted transaction.There are situations, like sending ERC20 on ETH, or XLM or XRP based assets, when the fee should not be reverted, because the fee is in calculated in Ethereum and transaction was in ERC20 currency.In this situation, only the transaction amount should be reverted, not the fee.
        /// </summary>
        /// <param name="id">ID of created withdrawal</param>
        /// <param name="revert">Defines whether fee should be reverted to account balance as well as amount. Defaults to true. Revert true would be typically used when withdrawal was not broadcast to blockchain. False is used usually for Ethereum based currencies when gas was consumed but transaction was reverted.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> CancelAsync(string id, bool revert = true, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Offchain.Tatum.GetUrl(string.Format(Endpoints_Cancel, id));
            var result = await Offchain.Tatum.SendTatumRequest<string>(url, HttpMethod.Delete, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Broadcast signed transaction and complete withdrawal<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed raw transaction end complete withdrawal associated with it. When broadcast succeeded but it is impossible to complete withdrawal, transaction id of transaction is returned and withdrawal must be completed manually.
        /// </summary>
        /// <param name="currency">Currency of signed transaction to be broadcast, BTC, LTC, BCH, ETH, XRP, ERC20</param>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="withdrawalId">Withdrawal ID to be completed by transaction broadcast</param>
        /// <param name="signatureId">ID of prepared payment template to sign. This is should be stored on a client side to retrieve ID of the blockchain transaction, when signing application signs the transaction and broadcasts it to the blockchain.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OffchainResponse> Broadcast(string currency, string txData, string withdrawalId, string signatureId, CancellationToken ct = default) => BroadcastAsync(currency, txData, withdrawalId, signatureId, ct).Result;
        /// <summary>
        /// <b>Title:</b> Broadcast signed transaction and complete withdrawal<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed raw transaction end complete withdrawal associated with it. When broadcast succeeded but it is impossible to complete withdrawal, transaction id of transaction is returned and withdrawal must be completed manually.
        /// </summary>
        /// <param name="currency">Currency of signed transaction to be broadcast, BTC, LTC, BCH, ETH, XRP, ERC20</param>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="withdrawalId">Withdrawal ID to be completed by transaction broadcast</param>
        /// <param name="signatureId">ID of prepared payment template to sign. This is should be stored on a client side to retrieve ID of the blockchain transaction, when signing application signs the transaction and broadcasts it to the blockchain.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OffchainResponse>> BroadcastAsync(string currency, string txData, string withdrawalId, string signatureId, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "currency", currency },
                { "txData", txData },
            };
            parameters.AddOptionalParameter("withdrawalId", withdrawalId);
            parameters.AddOptionalParameter("signatureId", signatureId);

            var credits = 2;
            var url = Offchain.Tatum.GetUrl(string.Format(Endpoints_Broadcast));
            var result = await Offchain.Tatum.SendTatumRequest<OffchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<OffchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<OffchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }
        #endregion

    }
}
