using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.Enums;
using Tatum.Net.Helpers;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Clients
{
    public class OffChainClient
    {
        public TatumClient Tatum { get; protected set; }
        public OffChainAccountClient Account { get; protected set; }
        public OffChainBlockchainClient Blockchain { get; protected set; }
        public OffChainWithdrawalClient Withdrawal { get; protected set; }

        public OffChainClient(TatumClient tatumClient)
        {
            Tatum = tatumClient;
            Account = new OffChainAccountClient(this);
            Blockchain = new OffChainBlockchainClient(this);
            Withdrawal = new OffChainWithdrawalClient(this);
        }
    }

    public class OffChainAccountClient
    {
        public OffChainClient Offchain { get; protected set; }

        protected const string Endpoints_DepositAddress = "offchain/account/{0}/address";
        protected const string Endpoints_DepositAddressBatch = "offchain/account/address/batch";
        protected const string Endpoints_CheckAddress = "offchain/account/address/{0}/{1}";
        protected const string Endpoints_RemoveAddress = "offchain/account/{0}/address/{1}";
        protected const string Endpoints_AssignAddress = "offchain/account/{0}/address/{1}";

        public OffChainAccountClient(OffChainClient offChainClient)
        {
            Offchain = offChainClient;
        }

        /// <summary>
        /// <b>Title:</b> Create new deposit address<br />
        /// <b>Credits:</b> 2 credits per API call and 5 credits for each address registered for scanning every day.<br />
        /// <b>Description:</b>
        /// Create a new deposit address for the account. This method associates public blockchain's ledger address with the account on Tatum's private ledger.
        /// It is possible to generate multiple blockchain addresses for the same ledger account.By this, it is possible to aggregate various blockchain transactions from different addresses into the same account.Depending on the currency of an account, this method will either generate a public address for Bitcoin, Bitcoin Cash, Litecoin or Ethereum, DestinationTag in case of XRP or message in case of XLM.More information about supported blockchains and address types can be found here.
        /// Addresses are generated in the natural order of the Extended public key provided in the account.Derivation index is the representation of that order - starts from 0 and ends at 2^31. When a new address is generated, the last not used index is used to generate an address.It is possible to skip some of the addresses to the different index, which means all the skipped addresses will no longer be used.
        /// </summary>
        /// <param name="account">Account ID</param>
        /// <param name="index">Derivation path index for specific address. If not present, last used index for given xpub of account + 1 is used. We recommend not to pass this value manually, since when some of the indexes are skipped, it is not possible to use them lately to generate address from it.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OffchainDepositAddress> GenerateDepositAddress(string account, int? index = null, CancellationToken ct = default) => GenerateDepositAddressAsync(account, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create new deposit address<br />
        /// <b>Credits:</b> 2 credits per API call and 5 credits for each address registered for scanning every day.<br />
        /// <b>Description:</b>
        /// Create a new deposit address for the account. This method associates public blockchain's ledger address with the account on Tatum's private ledger.
        /// It is possible to generate multiple blockchain addresses for the same ledger account.By this, it is possible to aggregate various blockchain transactions from different addresses into the same account.Depending on the currency of an account, this method will either generate a public address for Bitcoin, Bitcoin Cash, Litecoin or Ethereum, DestinationTag in case of XRP or message in case of XLM.More information about supported blockchains and address types can be found here.
        /// Addresses are generated in the natural order of the Extended public key provided in the account.Derivation index is the representation of that order - starts from 0 and ends at 2^31. When a new address is generated, the last not used index is used to generate an address.It is possible to skip some of the addresses to the different index, which means all the skipped addresses will no longer be used.
        /// </summary>
        /// <param name="account">Account ID</param>
        /// <param name="index">Derivation path index for specific address. If not present, last used index for given xpub of account + 1 is used. We recommend not to pass this value manually, since when some of the indexes are skipped, it is not possible to use them lately to generate address from it.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OffchainDepositAddress>> GenerateDepositAddressAsync(string account, int? index = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("index", index);

            var credits = 2;
            var url = Offchain.Tatum.GetUrl(string.Format(Endpoints_DepositAddress, account));
            return await Offchain.Tatum.SendTatumRequest<OffchainDepositAddress>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get all deposit addresses for account<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get all deposit addresses generated for account. It is possible to deposit funds from another blockchain address to any of associated addresses and they will be credited on the Tatum Ledger account connected to the address.
        /// </summary>
        /// <param name="account">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OffchainDepositAddress>> GetAllDepositAddresses(string account, CancellationToken ct = default) => GetAllDepositAddressesAsync(account, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get all deposit addresses for account<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Get all deposit addresses generated for account. It is possible to deposit funds from another blockchain address to any of associated addresses and they will be credited on the Tatum Ledger account connected to the address.
        /// </summary>
        /// <param name="account">Account ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OffchainDepositAddress>>> GetAllDepositAddressesAsync(string account, CancellationToken ct = default)
        {
            var credits = 2;
            var url = Offchain.Tatum.GetUrl(string.Format(Endpoints_DepositAddress, account));
            return await Offchain.Tatum.SendTatumRequest<IEnumerable<OffchainDepositAddress>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Create new deposit addresses in a batch call<br />
        /// <b>Credits:</b> 2 credits per API call, 1 credit for every address created and 5 credits for each address registered for scanning every day.<br />
        /// <b>Description:</b>
        /// Create new deposit addressess for the account. This method associates public blockchain's ledger address with the account on Tatum's private ledger.
        /// It is possible to generate multiple blockchain addresses for the same ledger account.By this, it is possible to aggregate various blockchain transactions from different addresses into the same account.Depending on the currency of an account, this method will either generate a public address for Bitcoin, Bitcoin Cash, Litecoin or Ethereum, DestinationTag in case of XRP or message in case of XLM.More information about supported blockchains and address types can be found here.
        /// Addresses are generated in the natural order of the Extended public key provided in the account.Derivation index is the representation of that order - starts from 0 and ends at 2^31. When a new address is generated, the last not used index is used to generate an address.It is possible to skip some of the addresses to the different index, which means all the skipped addresses will no longer be used.
        /// </summary>
        /// <param name="addresses"></param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OffchainDepositAddress>> GenerateMultipleDepositAddresses(IEnumerable<OffchainDepositAddressRequest> addresses, CancellationToken ct = default) => GenerateMultipleDepositAddressesAsync(addresses, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create new deposit addresses in a batch call<br />
        /// <b>Credits:</b> 2 credits per API call, 1 credit for every address created and 5 credits for each address registered for scanning every day.<br />
        /// <b>Description:</b>
        /// Create new deposit addressess for the account. This method associates public blockchain's ledger address with the account on Tatum's private ledger.
        /// It is possible to generate multiple blockchain addresses for the same ledger account.By this, it is possible to aggregate various blockchain transactions from different addresses into the same account.Depending on the currency of an account, this method will either generate a public address for Bitcoin, Bitcoin Cash, Litecoin or Ethereum, DestinationTag in case of XRP or message in case of XLM.More information about supported blockchains and address types can be found here.
        /// Addresses are generated in the natural order of the Extended public key provided in the account.Derivation index is the representation of that order - starts from 0 and ends at 2^31. When a new address is generated, the last not used index is used to generate an address.It is possible to skip some of the addresses to the different index, which means all the skipped addresses will no longer be used.
        /// </summary>
        /// <param name="addresses"></param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OffchainDepositAddress>>> GenerateMultipleDepositAddressesAsync(IEnumerable<OffchainDepositAddressRequest> addresses, CancellationToken ct = default)
        {
            if (addresses == null || addresses.Count() == 0)
                throw new ArgumentException("addresses parameter must contain one element at least");

            var parameters = new Dictionary<string, object>
            {
                { "addresses", addresses }
            };

            var credits = 2;
            var url = Offchain.Tatum.GetUrl(string.Format(Endpoints_DepositAddressBatch, addresses));
            return await Offchain.Tatum.SendTatumRequest<IEnumerable<OffchainDepositAddress>>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Check, if deposit address is assigned<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Check, whether blockchain address for given currency is registered within Tatum and assigned to Tatum Account. Returns account this address belongs to, otherwise throws an error.
        /// </summary>
        /// <param name="chain">Blockchain Type</param>
        /// <param name="address">Blockchain Address to check</param>
        /// <param name="index">In case of XLM or XRP, this is a memo or DestinationTag to search for.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OffchainDepositAddressState> CheckAddress(BlockchainType chain, string address, int? index = null, CancellationToken ct = default) => CheckAddressAsync(chain, address, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Check, if deposit address is assigned<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Check, whether blockchain address for given currency is registered within Tatum and assigned to Tatum Account. Returns account this address belongs to, otherwise throws an error.
        /// </summary>
        /// <param name="chain">Blockchain Type</param>
        /// <param name="address">Blockchain Address to check</param>
        /// <param name="index">In case of XLM or XRP, this is a memo or DestinationTag to search for.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OffchainDepositAddressState>> CheckAddressAsync(BlockchainType chain, string address, int? index = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("index", index);

            var credits = 1;
            var ops = chain.GetBlockchainOptions();
            var url = Offchain.Tatum.GetUrl(string.Format(Endpoints_CheckAddress, address, ops.Code));
            return await Offchain.Tatum.SendTatumRequest<OffchainDepositAddressState>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Remove address for account<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Remove blockchain address from the Ledger account. Tatum will not check for any incoming deposits on this address for this account. It will not be possible to generate the address in the future anymore.
        /// </summary>
        /// <param name="account">Account ID</param>
        /// <param name="address">Blockchain address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> RemoveAddressFromAccount(string account, string address, CancellationToken ct = default) => RemoveAddressFromAccountAsync(account, address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Remove address for account<br />
        /// <b>Credits:</b> 1 credit per API call.<br />
        /// <b>Description:</b>
        /// Remove blockchain address from the Ledger account. Tatum will not check for any incoming deposits on this address for this account. It will not be possible to generate the address in the future anymore.
        /// </summary>
        /// <param name="account">Account ID</param>
        /// <param name="address">Blockchain address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> RemoveAddressFromAccountAsync(string account, string address, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Offchain.Tatum.GetUrl(string.Format(Endpoints_RemoveAddress, account, address));
            var result = await Offchain.Tatum.SendTatumRequest<string>(url, HttpMethod.Delete, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Assign address for account<br />
        /// <b>Credits:</b> 2 credits for API call and 5 credits for each address registered for scanning every day.<br />
        /// <b>Description:</b>
        /// This method is used when the account has no default xpub assigned, and addresses are handled manually. It is possible to pair any number of blockchain address to the account.
        /// </summary>
        /// <param name="account">Account ID</param>
        /// <param name="address">Blockchain address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OffchainDepositAddress>> AssignAddressToAccount(string account, string address, CancellationToken ct = default) => AssignAddressToAccountAsync(account, address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Assign address for account<br />
        /// <b>Credits:</b> 2 credits for API call and 5 credits for each address registered for scanning every day.<br />
        /// <b>Description:</b>
        /// This method is used when the account has no default xpub assigned, and addresses are handled manually. It is possible to pair any number of blockchain address to the account.
        /// </summary>
        /// <param name="account">Account ID</param>
        /// <param name="address">Blockchain address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OffchainDepositAddress>>> AssignAddressToAccountAsync(string account, string address, CancellationToken ct = default)
        {
            var credits = 1;
            var url = Offchain.Tatum.GetUrl(string.Format(Endpoints_AssignAddress, account, address));
            return await Offchain.Tatum.SendTatumRequest<IEnumerable<OffchainDepositAddress>>(url, HttpMethod.Post, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }
    }

    public class OffChainBlockchainClient
    {
        public OffChainClient Offchain { get; protected set; }

        protected const string Endpoints_Transfer = "offchain/{0}/transfer";
        protected const string Endpoints_BitcoinTransfer = "offchain/bitcoin/transfer";
        protected const string Endpoints_BitcoinCashTransfer = "offchain/bcash/transfer";
        protected const string Endpoints_LitecoinTransfer = "offchain/litecoin/transfer";
        protected const string Endpoints_EthereumTransfer = "offchain/ethereum/transfer";
        protected const string Endpoints_CreateERC20Token = "offchain/ethereum/erc20";
        protected const string Endpoints_DeployERC20Token = "offchain/ethereum/erc20/deploy";
        protected const string Endpoints_SetERC20TokenContractAddress = "offchain/ethereum/erc20/{0}/{1}";
        protected const string Endpoints_TransferERC20Token = "offchain/ethereum/erc20/transfer";
        protected const string Endpoints_StellarTransfer = "offchain/xlm/transfer";
        protected const string Endpoints_CreateXLMAsset = "offchain/xlm/asset";
        protected const string Endpoints_RippleTransfer = "offchain/xrp/transfer";
        protected const string Endpoints_CreateXRPAsset = "offchain/xrp/asset";
        protected const string Endpoints_BinanceTransfer = "offchain/bnb/transfer";
        protected const string Endpoints_CreateBNBAsset = "offchain/bnb/asset";

        public OffChainBlockchainClient(OffChainClient offChainClient)
        {
            Offchain = offChainClient;
        }

        protected virtual async Task<WebCallResult<OffchainTransferResponse>> SendAsync(
            BlockchainType chain,
            string senderAccountId, string to_address, decimal amount, bool? compliant = null, decimal? fee = null,
            IEnumerable<decimal> multipleAmounts = null,
            IEnumerable<OffchainAddressPrivateKeyPair> keyPairs = null,
            string attr = null, string mnemonic = null, string signatureId = null,
            string xpub = null, string paymentId = null, string senderNote = null,
            CancellationToken ct = default)
        {
            if (!chain.IsOneOf(
                BlockchainType.Bitcoin,
                BlockchainType.BitcoinCash,
                BlockchainType.Litecoin))
                throw new ArgumentException("Wrong BlockchainType");

            var credict = new Dictionary<BlockchainType, int>
            {
                { BlockchainType.Bitcoin, 2 },
                { BlockchainType.BitcoinCash, 10 },
                { BlockchainType.Litecoin, 10 },
            };

            var ci = CultureInfo.InvariantCulture;
            var credits = credict[chain];
            var parameters = new Dictionary<string, object>
            {
                { "senderAccountId", senderAccountId },
                { "address", to_address },
                { "amount", amount.ToString(ci) },
            };
            parameters.AddOptionalParameter("compliant", compliant);
            parameters.AddOptionalParameter("fee", fee?.ToString(ci));
            if (multipleAmounts != null)
            {
                var lst = new List<string>();
                foreach (var ma in multipleAmounts) lst.Add(ma.ToString(ci));
                parameters.AddOptionalParameter("multipleAmounts", lst);
            }
            parameters.AddOptionalParameter("keyPair", keyPairs);
            parameters.AddOptionalParameter("attr", attr);
            parameters.AddOptionalParameter("mnemonic", mnemonic);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("xpub", xpub);
            parameters.AddOptionalParameter("paymentId", paymentId);
            parameters.AddOptionalParameter("senderNote", senderNote);

            var ops = chain.GetBlockchainOptions();
            var url = Offchain.Tatum.GetUrl(string.Format(Endpoints_Transfer, ops.ChainSlug));
            var result = await Offchain.Tatum.SendTatumRequest<OffchainTransferResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success/* || !result.Data.Completed*/) return WebCallResult<OffchainTransferResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<OffchainTransferResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Send Bitcoin from Tatum account to address<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Bitcoin from Tatum account to address. This will create Tatum internal withdrawal request with ID. If every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If Bitcoin server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// There are two possibilites how the transaction on the blockchain can be created:
        /// - Using mnemonic - all of the addresses, that are generated from the mnemonic are scanned for the incoming deposits which are used as a source of the transaction.Assets, which are not used in a transaction are moved to the system address wih the derivation index 0. Address with index 0 cannot be assigned automatically to any account and is used for custodial wallet use cases. For non-custodial wallets, field attr should be present and it should be address with the index 1 of the connected wallet.
        /// - Using keyPair - addresses which are used as a source of the transaction are entered manually
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This method is a helper method, which internally wraps these steps:
        /// 1. Store withdrawal - create a ledger transaction, which debits the assets on the sender account.
        /// 2. Perform blockchain transaction -
        /// 3. Complete withdrawal - move the withdrawal to the completed state, when all of the previous steps were successful.
        /// When some of the steps fails, Cancel withdrawal operation is used, which cancels withdrawal and creates refund transaction to the sender account.This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="to_address">Blockchain address to send assets to. For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain addresses as a comma separated string.</param>
        /// <param name="amount">Amount to be withdrawn to blockchain.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="fee">Fee to be submitted as a transaction fee to blockchain. If none is set, default value of 0.0005 BTC is used.</param>
        /// <param name="multipleAmounts">For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain amounts. List of recipient addresses must be present in the address field and total sum of amounts must be equal to the amount field.</param>
        /// <param name="keyPairs">Array of assigned blockchain addresses with their private keys. Either mnemonic, keyPair or signature Id must be present - depends on the type of account and xpub. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="attr">Used to parametrize withdrawal as a change address for left coins from transaction. XPub or attr must be used.</param>
        /// <param name="mnemonic">Mnemonic seed - usually 12-24 words with access to whole wallet. Either mnemonic, keyPair or signature Id must be present - depends on the type of account and xpub. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="signatureId">Signature hash of the mnemonic, which will be used to sign transactions locally. All signature Ids should be present, which might be used to sign transaction. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="xpub">Extended public key (xpub) of the wallet associated with the accounts. Should be present, when mnemonic is used.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OffchainTransferResponse> SendBitcoin(
            string senderAccountId, string to_address, decimal amount, bool? compliant = null, decimal? fee = null,
            IEnumerable<decimal> multipleAmounts = null, IEnumerable<OffchainAddressPrivateKeyPair> keyPairs = null,
            string attr = null, string mnemonic = null, string signatureId = null,
            string xpub = null, string paymentId = null, string senderNote = null,
            CancellationToken ct = default)
            => SendAsync(BlockchainType.Bitcoin, senderAccountId, to_address, amount, compliant, fee, multipleAmounts, keyPairs, attr, mnemonic, signatureId, xpub, paymentId, senderNote, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send Bitcoin from Tatum account to address<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Bitcoin from Tatum account to address. This will create Tatum internal withdrawal request with ID. If every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If Bitcoin server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// There are two possibilites how the transaction on the blockchain can be created:
        /// - Using mnemonic - all of the addresses, that are generated from the mnemonic are scanned for the incoming deposits which are used as a source of the transaction.Assets, which are not used in a transaction are moved to the system address wih the derivation index 0. Address with index 0 cannot be assigned automatically to any account and is used for custodial wallet use cases. For non-custodial wallets, field attr should be present and it should be address with the index 1 of the connected wallet.
        /// - Using keyPair - addresses which are used as a source of the transaction are entered manually
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This method is a helper method, which internally wraps these steps:
        /// 1. Store withdrawal - create a ledger transaction, which debits the assets on the sender account.
        /// 2. Perform blockchain transaction -
        /// 3. Complete withdrawal - move the withdrawal to the completed state, when all of the previous steps were successful.
        /// When some of the steps fails, Cancel withdrawal operation is used, which cancels withdrawal and creates refund transaction to the sender account.This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="to_address">Blockchain address to send assets to. For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain addresses as a comma separated string.</param>
        /// <param name="amount">Amount to be withdrawn to blockchain.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="fee">Fee to be submitted as a transaction fee to blockchain. If none is set, default value of 0.0005 BTC is used.</param>
        /// <param name="multipleAmounts">For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain amounts. List of recipient addresses must be present in the address field and total sum of amounts must be equal to the amount field.</param>
        /// <param name="keyPairs">Array of assigned blockchain addresses with their private keys. Either mnemonic, keyPair or signature Id must be present - depends on the type of account and xpub. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="attr">Used to parametrize withdrawal as a change address for left coins from transaction. XPub or attr must be used.</param>
        /// <param name="mnemonic">Mnemonic seed - usually 12-24 words with access to whole wallet. Either mnemonic, keyPair or signature Id must be present - depends on the type of account and xpub. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="signatureId">Signature hash of the mnemonic, which will be used to sign transactions locally. All signature Ids should be present, which might be used to sign transaction. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="xpub">Extended public key (xpub) of the wallet associated with the accounts. Should be present, when mnemonic is used.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OffchainTransferResponse>> SendBitcoinAsync(
            string senderAccountId, string to_address, decimal amount, bool? compliant = null, decimal? fee = null,
            IEnumerable<decimal> multipleAmounts = null, IEnumerable<OffchainAddressPrivateKeyPair> keyPairs = null,
            string attr = null, string mnemonic = null, string signatureId = null,
            string xpub = null, string paymentId = null, string senderNote = null,
            CancellationToken ct = default)
            => await SendAsync(BlockchainType.Bitcoin, senderAccountId, to_address, amount, compliant, fee, multipleAmounts, keyPairs, attr, mnemonic, signatureId, xpub, paymentId, senderNote, ct);

        /// <summary>
        /// <b>Title:</b> Send Bitcoin Cash from Tatum account to address<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Bitcoin Cash from Tatum account to address. This will create Tatum internal withdrawal request with ID. If every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If Bitcoin Cash server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// There are two possibilites how the transaction on the blockchain can be created:
        /// - Using mnemonic - all of the addresses, that are generated from the mnemonic are scanned for the incoming deposits which are used as a source of the transaction.Assets, which are not used in a transaction are moved to the system address wih the derivation index 0. Address with index 0 cannot be assigned automatically to any account and is used for custodial wallet use cases. For non-custodial wallets, field attr should be present and it should be address with the index 1 of the connected wallet.
        /// - Using keyPair - addresses which are used as a source of the transaction are entered manually
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This method is a helper method, which internally wraps these steps:
        /// 1. Store withdrawal - create a ledger transaction, which debits the assets on the sender account.
        /// 2. Perform blockchain transaction -
        /// 3. Complete withdrawal - move the withdrawal to the completed state, when all of the previous steps were successful.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="to_address">Blockchain address to send assets to. For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain addresses as a comma separated string.</param>
        /// <param name="amount">Amount to be withdrawn to blockchain.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="fee">Fee to be submitted as a transaction fee to blockchain. If none is set, default value of 0.0005 BTC is used.</param>
        /// <param name="multipleAmounts">For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain amounts. List of recipient addresses must be present in the address field and total sum of amounts must be equal to the amount field.</param>
        /// <param name="keyPairs">Array of assigned blockchain addresses with their private keys. Either mnemonic, keyPair or signature Id must be present - depends on the type of account and xpub. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="attr">Used to parametrize withdrawal as a change address for left coins from transaction. XPub or attr must be used.</param>
        /// <param name="mnemonic">Mnemonic seed - usually 12-24 words with access to whole wallet. Either mnemonic, keyPair or signature Id must be present - depends on the type of account and xpub. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="signatureId">Signature hash of the mnemonic, which will be used to sign transactions locally. All signature Ids should be present, which might be used to sign transaction. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="xpub">Extended public key (xpub) of the wallet associated with the accounts. Should be present, when mnemonic is used.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OffchainTransferResponse> SendBitcoinCash(
            string senderAccountId, string to_address, decimal amount, bool? compliant = null, decimal? fee = null,
            IEnumerable<decimal> multipleAmounts = null, IEnumerable<OffchainAddressPrivateKeyPair> keyPairs = null,
            string attr = null, string mnemonic = null, string signatureId = null,
            string xpub = null, string paymentId = null, string senderNote = null,
            CancellationToken ct = default)
            => SendAsync(BlockchainType.BitcoinCash, senderAccountId, to_address, amount, compliant, fee, multipleAmounts, keyPairs, attr, mnemonic, signatureId, xpub, paymentId, senderNote, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send Bitcoin Cash from Tatum account to address<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Bitcoin Cash from Tatum account to address. This will create Tatum internal withdrawal request with ID. If every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If Bitcoin Cash server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// There are two possibilites how the transaction on the blockchain can be created:
        /// - Using mnemonic - all of the addresses, that are generated from the mnemonic are scanned for the incoming deposits which are used as a source of the transaction.Assets, which are not used in a transaction are moved to the system address wih the derivation index 0. Address with index 0 cannot be assigned automatically to any account and is used for custodial wallet use cases. For non-custodial wallets, field attr should be present and it should be address with the index 1 of the connected wallet.
        /// - Using keyPair - addresses which are used as a source of the transaction are entered manually
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This method is a helper method, which internally wraps these steps:
        /// 1. Store withdrawal - create a ledger transaction, which debits the assets on the sender account.
        /// 2. Perform blockchain transaction -
        /// 3. Complete withdrawal - move the withdrawal to the completed state, when all of the previous steps were successful.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="to_address">Blockchain address to send assets to. For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain addresses as a comma separated string.</param>
        /// <param name="amount">Amount to be withdrawn to blockchain.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="fee">Fee to be submitted as a transaction fee to blockchain. If none is set, default value of 0.0005 BTC is used.</param>
        /// <param name="multipleAmounts">For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain amounts. List of recipient addresses must be present in the address field and total sum of amounts must be equal to the amount field.</param>
        /// <param name="keyPairs">Array of assigned blockchain addresses with their private keys. Either mnemonic, keyPair or signature Id must be present - depends on the type of account and xpub. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="attr">Used to parametrize withdrawal as a change address for left coins from transaction. XPub or attr must be used.</param>
        /// <param name="mnemonic">Mnemonic seed - usually 12-24 words with access to whole wallet. Either mnemonic, keyPair or signature Id must be present - depends on the type of account and xpub. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="signatureId">Signature hash of the mnemonic, which will be used to sign transactions locally. All signature Ids should be present, which might be used to sign transaction. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="xpub">Extended public key (xpub) of the wallet associated with the accounts. Should be present, when mnemonic is used.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OffchainTransferResponse>> SendBitcoinCashAsync(
            string senderAccountId, string to_address, decimal amount, bool? compliant = null, decimal? fee = null,
            IEnumerable<decimal> multipleAmounts = null, IEnumerable<OffchainAddressPrivateKeyPair> keyPairs = null,
            string attr = null, string mnemonic = null, string signatureId = null,
            string xpub = null, string paymentId = null, string senderNote = null,
            CancellationToken ct = default)
            => await SendAsync(BlockchainType.BitcoinCash, senderAccountId, to_address, amount, compliant, fee, multipleAmounts, keyPairs, attr, mnemonic, signatureId, xpub, paymentId, senderNote, ct);

        /// <summary>
        /// <b>Title:</b> Send Litecoin from Tatum account to address<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Litecoin from Tatum account to address. This will create Tatum internal withdrawal request with ID. If every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If Litecoin server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// There are two possibilites how the transaction on the blockchain can be created:
        /// - Using mnemonic - all of the addresses, that are generated from the mnemonic are scanned for the incoming deposits which are used as a source of the transaction.Assets, which are not used in a transaction are moved to the system address wih the derivation index 0. Address with index 0 cannot be assigned automatically to any account and is used for custodial wallet use cases. For non-custodial wallets, field attr should be present and it should be address with the index 1 of the connected wallet.
        /// - Using keyPair - addresses which are used as a source of the transaction are entered manually
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This method is a helper method, which internally wraps these steps:
        /// 1. Store withdrawal - create a ledger transaction, which debits the assets on the sender account.
        /// 2. Perform blockchain transaction -
        /// 3. Complete withdrawal - move the withdrawal to the completed state, when all of the previous steps were successful.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="to_address">Blockchain address to send assets to. For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain addresses as a comma separated string.</param>
        /// <param name="amount">Amount to be withdrawn to blockchain.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="fee">Fee to be submitted as a transaction fee to blockchain. If none is set, default value of 0.0005 BTC is used.</param>
        /// <param name="multipleAmounts">For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain amounts. List of recipient addresses must be present in the address field and total sum of amounts must be equal to the amount field.</param>
        /// <param name="keyPairs">Array of assigned blockchain addresses with their private keys. Either mnemonic, keyPair or signature Id must be present - depends on the type of account and xpub. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="attr">Used to parametrize withdrawal as a change address for left coins from transaction. XPub or attr must be used.</param>
        /// <param name="mnemonic">Mnemonic seed - usually 12-24 words with access to whole wallet. Either mnemonic, keyPair or signature Id must be present - depends on the type of account and xpub. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="signatureId">Signature hash of the mnemonic, which will be used to sign transactions locally. All signature Ids should be present, which might be used to sign transaction. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="xpub">Extended public key (xpub) of the wallet associated with the accounts. Should be present, when mnemonic is used.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OffchainTransferResponse> SendLitecoin(
            string senderAccountId, string to_address, decimal amount, bool? compliant = null, decimal? fee = null,
            IEnumerable<decimal> multipleAmounts = null, IEnumerable<OffchainAddressPrivateKeyPair> keyPairs = null,
            string attr = null, string mnemonic = null, string signatureId = null,
            string xpub = null, string paymentId = null, string senderNote = null,
            CancellationToken ct = default)
            => SendAsync(BlockchainType.Litecoin, senderAccountId, to_address, amount, compliant, fee, multipleAmounts, keyPairs, attr, mnemonic, signatureId, xpub, paymentId, senderNote, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send Litecoin from Tatum account to address<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Litecoin from Tatum account to address. This will create Tatum internal withdrawal request with ID. If every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If Litecoin server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// There are two possibilites how the transaction on the blockchain can be created:
        /// - Using mnemonic - all of the addresses, that are generated from the mnemonic are scanned for the incoming deposits which are used as a source of the transaction.Assets, which are not used in a transaction are moved to the system address wih the derivation index 0. Address with index 0 cannot be assigned automatically to any account and is used for custodial wallet use cases. For non-custodial wallets, field attr should be present and it should be address with the index 1 of the connected wallet.
        /// - Using keyPair - addresses which are used as a source of the transaction are entered manually
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This method is a helper method, which internally wraps these steps:
        /// 1. Store withdrawal - create a ledger transaction, which debits the assets on the sender account.
        /// 2. Perform blockchain transaction -
        /// 3. Complete withdrawal - move the withdrawal to the completed state, when all of the previous steps were successful.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="to_address">Blockchain address to send assets to. For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain addresses as a comma separated string.</param>
        /// <param name="amount">Amount to be withdrawn to blockchain.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="fee">Fee to be submitted as a transaction fee to blockchain. If none is set, default value of 0.0005 BTC is used.</param>
        /// <param name="multipleAmounts">For BTC, LTC and BCH, it is possible to enter list of multiple recipient blockchain amounts. List of recipient addresses must be present in the address field and total sum of amounts must be equal to the amount field.</param>
        /// <param name="keyPairs">Array of assigned blockchain addresses with their private keys. Either mnemonic, keyPair or signature Id must be present - depends on the type of account and xpub. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="attr">Used to parametrize withdrawal as a change address for left coins from transaction. XPub or attr must be used.</param>
        /// <param name="mnemonic">Mnemonic seed - usually 12-24 words with access to whole wallet. Either mnemonic, keyPair or signature Id must be present - depends on the type of account and xpub. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="signatureId">Signature hash of the mnemonic, which will be used to sign transactions locally. All signature Ids should be present, which might be used to sign transaction. Tatum KMS does not support keyPair type of off-chain transaction, only mnemonic based.</param>
        /// <param name="xpub">Extended public key (xpub) of the wallet associated with the accounts. Should be present, when mnemonic is used.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OffchainTransferResponse>> SendLitecoinAsync(
            string senderAccountId, string to_address, decimal amount, bool? compliant = null, decimal? fee = null,
            IEnumerable<decimal> multipleAmounts = null, IEnumerable<OffchainAddressPrivateKeyPair> keyPairs = null,
            string attr = null, string mnemonic = null, string signatureId = null,
            string xpub = null, string paymentId = null, string senderNote = null,
            CancellationToken ct = default)
            => await SendAsync(BlockchainType.Litecoin, senderAccountId, to_address, amount, compliant, fee, multipleAmounts, keyPairs, attr, mnemonic, signatureId, xpub, paymentId, senderNote, ct);

        /// <summary>
        /// <b>Title:</b> Send Ethereum from Tatum ledger to blockchain<br />
        /// <b>Credits:</b> 4 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Ethereum from Tatum Ledger to account. This will create Tatum internal withdrawal request with ID. If every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If Ethereum server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="to_address">Blockchain address to send assets</param>
        /// <param name="amount">Amount to be sent in Ether.</param>
        /// <param name="currency">Currency to transfer from Ethereum Blockchain Account. Required only for calls from Tatum Middleware.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="privateKey">Private key of sender address. Either mnemonic and index, privateKey or signature Id must be present - depends on the type of account and xpub.</param>
        /// <param name="signatureId">Identifier of the mnemonic / private key associated in signing application. When hash identifies mnemonic, index must be present to represent specific account to pay from. Private key, mnemonic or signature Id must be present.</param>
        /// <param name="index">Derivation index of sender address.</param>
        /// <param name="mnemonic">Mnemonic to generate private key for sender address. Either mnemonic and index, privateKey or signature Id must be present - depends on the type of account and xpub.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OffchainTransferResponse> SendEthereum(
            string senderAccountId, string to_address, decimal amount, string currency = null, long? nonce = null,
            bool? compliant = null, string privateKey = null, string signatureId = null, int? index = null, string mnemonic = null,
            string paymentId = null, string senderNote = null,
            CancellationToken ct = default)
            => SendEthereumAsync(
            senderAccountId, to_address, amount, currency, nonce,
            compliant, privateKey, signatureId, index, mnemonic,
            paymentId, senderNote, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send Ethereum from Tatum ledger to blockchain<br />
        /// <b>Credits:</b> 4 credits per API call.<br />
        /// <b>Description:</b>
        /// Send Ethereum from Tatum Ledger to account. This will create Tatum internal withdrawal request with ID. If every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If Ethereum server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="to_address">Blockchain address to send assets</param>
        /// <param name="amount">Amount to be sent in Ether.</param>
        /// <param name="currency">Currency to transfer from Ethereum Blockchain Account. Required only for calls from Tatum Middleware.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="privateKey">Private key of sender address. Either mnemonic and index, privateKey or signature Id must be present - depends on the type of account and xpub.</param>
        /// <param name="signatureId">Identifier of the mnemonic / private key associated in signing application. When hash identifies mnemonic, index must be present to represent specific account to pay from. Private key, mnemonic or signature Id must be present.</param>
        /// <param name="index">Derivation index of sender address.</param>
        /// <param name="mnemonic">Mnemonic to generate private key for sender address. Either mnemonic and index, privateKey or signature Id must be present - depends on the type of account and xpub.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OffchainTransferResponse>> SendEthereumAsync(
            string senderAccountId, string to_address, decimal amount, string currency = null, long? nonce = null,
            bool? compliant = null, string privateKey = null, string signatureId = null, int? index = null, string mnemonic = null,
            string paymentId = null, string senderNote = null,
            CancellationToken ct = default)
        {
            var credits = 4;
            var ci = CultureInfo.InvariantCulture;
            var parameters = new Dictionary<string, object>
            {
                { "senderAccountId", senderAccountId },
                { "address", to_address },
                { "amount", amount.ToString(ci) },
            };
            parameters.AddOptionalParameter("currency", currency);
            parameters.AddOptionalParameter("nonce", nonce);
            parameters.AddOptionalParameter("compliant", compliant);
            parameters.AddOptionalParameter("privateKey", privateKey);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("index", index);
            parameters.AddOptionalParameter("mnemonic", mnemonic);
            parameters.AddOptionalParameter("paymentId", paymentId);
            parameters.AddOptionalParameter("senderNote", senderNote);

            var url = Offchain.Tatum.GetUrl(string.Format(Endpoints_EthereumTransfer));
            var result = await Offchain.Tatum.SendTatumRequest<OffchainTransferResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success/* || !result.Data.Completed*/) return WebCallResult<OffchainTransferResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<OffchainTransferResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Create new ERC20 token<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// First step to create new ERC20 token with given supply on Ethereum blockchain with support of Tatum's private ledger.
        /// This method only creates Tatum Private ledger virtual currency with predefined parameters.It will not generate any blockchain smart contract.
        /// The whole supply of ERC20 token is stored in the customer's newly created account. Then it is possible to create new Tatum accounts with ERC20 token name as account's currency.
        /// Newly created account is frozen until the specific ERC20 smart contract address is linked with the Tatum virtual currency, representing the token.
        /// Order of the steps to create ERC20 smart contract with Tatum private ledger support:
        /// 1. Create ERC20 token - creates a virtual currency within Tatum
        /// 2. Deploy ERC20 smart contract - create new ERC20 smart contract on the blockchain
        /// 3. Store ERC20 smart contract address - link newly created ERC20 smart contract address with Tatum virtual currency - this operation enables frozen account and enables offchain synchronization for ERC20 Tatum accounts
        /// There is a helper method Deploy Ethereum ERC20 Smart Contract Off-chain, which wraps first 2 steps into 1 method.
        /// Address on the blockchain, where all initial supply will be transferred, can be defined via the address or xpub and derivationIndex.When xpub is present, the account connected to this virtualCurrency will be set as the account's xpub.
        /// </summary>
        /// <param name="symbol">ERC20 token name. Used as a identifier within Tatum system and also in blockchain as a currency symbol.</param>
        /// <param name="supply">Supply of ERC20 token.</param>
        /// <param name="description">Used as a description within Tatum system and in blockchain as a currency name.</param>
        /// <param name="basePair">Base pair for ERC20 token. Transaction value will be calculated according to this base pair.</param>
        /// <param name="customer">If customer is filled then is created or updated.</param>
        /// <param name="accountingCurrency">All transaction will be billed in this currency for created account associated with this currency. If not set, EUR is used. ISO-4217</param>
        /// <param name="derivationIndex">Derivation index for xpub to generate specific deposit address.</param>
        /// <param name="xpub">Extended public key (xpub), from which address, where all initial supply will be stored, will be generated. Either xpub and derivationIndex, or address must be present, not both.</param>
        /// <param name="address">Address on Ethereum blockchain, where all initial supply will be stored. Either xpub and derivationIndex, or address must be present, not both.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OffchainAccountIdAddressPair> CreateERC20Token(
            string symbol, string supply, string description, string basePair,
            LedgerCustomerOptions customer = null, string accountingCurrency = null, int? derivationIndex = null,
            string xpub = null, string address = null,
            CancellationToken ct = default)
            => CreateERC20TokenAsync(
            symbol, supply, description, basePair,
            customer, accountingCurrency, derivationIndex,
            xpub, address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create new ERC20 token<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// First step to create new ERC20 token with given supply on Ethereum blockchain with support of Tatum's private ledger.
        /// This method only creates Tatum Private ledger virtual currency with predefined parameters.It will not generate any blockchain smart contract.
        /// The whole supply of ERC20 token is stored in the customer's newly created account. Then it is possible to create new Tatum accounts with ERC20 token name as account's currency.
        /// Newly created account is frozen until the specific ERC20 smart contract address is linked with the Tatum virtual currency, representing the token.
        /// Order of the steps to create ERC20 smart contract with Tatum private ledger support:
        /// 1. Create ERC20 token - creates a virtual currency within Tatum
        /// 2. Deploy ERC20 smart contract - create new ERC20 smart contract on the blockchain
        /// 3. Store ERC20 smart contract address - link newly created ERC20 smart contract address with Tatum virtual currency - this operation enables frozen account and enables offchain synchronization for ERC20 Tatum accounts
        /// There is a helper method Deploy Ethereum ERC20 Smart Contract Off-chain, which wraps first 2 steps into 1 method.
        /// Address on the blockchain, where all initial supply will be transferred, can be defined via the address or xpub and derivationIndex.When xpub is present, the account connected to this virtualCurrency will be set as the account's xpub.
        /// </summary>
        /// <param name="symbol">ERC20 token name. Used as a identifier within Tatum system and also in blockchain as a currency symbol.</param>
        /// <param name="supply">Supply of ERC20 token.</param>
        /// <param name="description">Used as a description within Tatum system and in blockchain as a currency name.</param>
        /// <param name="basePair">Base pair for ERC20 token. Transaction value will be calculated according to this base pair.</param>
        /// <param name="customer">If customer is filled then is created or updated.</param>
        /// <param name="accountingCurrency">All transaction will be billed in this currency for created account associated with this currency. If not set, EUR is used. ISO-4217</param>
        /// <param name="derivationIndex">Derivation index for xpub to generate specific deposit address.</param>
        /// <param name="xpub">Extended public key (xpub), from which address, where all initial supply will be stored, will be generated. Either xpub and derivationIndex, or address must be present, not both.</param>
        /// <param name="address">Address on Ethereum blockchain, where all initial supply will be stored. Either xpub and derivationIndex, or address must be present, not both.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OffchainAccountIdAddressPair>> CreateERC20TokenAsync(
            string symbol, string supply, string description, string basePair,
            LedgerCustomerOptions customer = null, string accountingCurrency = null, int? derivationIndex = null,
            string xpub = null, string address = null,
            CancellationToken ct = default)
        {
            var credits = 2;
            var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "supply", supply },
                { "description", description },
                { "basePair", basePair },
            };
            parameters.AddOptionalParameter("customer", customer);
            parameters.AddOptionalParameter("accountingCurrency", accountingCurrency);
            parameters.AddOptionalParameter("derivationIndex", derivationIndex);
            parameters.AddOptionalParameter("xpub", xpub);
            parameters.AddOptionalParameter("address", address);

            var url = Offchain.Tatum.GetUrl(string.Format(Endpoints_CreateERC20Token));
            var result = await Offchain.Tatum.SendTatumRequest<OffchainAccountIdAddressPair>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success/* || !result.Data.Completed*/) return WebCallResult<OffchainAccountIdAddressPair>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<OffchainAccountIdAddressPair>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Deploy Ethereum ERC20 Smart Contract Off-chain<br />
        /// <b>Credits:</b> 4 credits per API call.<br />
        /// <b>Description:</b>
        /// Deploy Ethereum ERC20 Smart Contract. This is a helper method, which is combination of Create new ERC20 token and Deploy blockchain ERC20.
        /// After deploying a contract to blockchain, the contract address will become available and must be stored within Tatum.Otherwise, it will not be possible to interact with it and starts automatic blockchain synchronization.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="symbol">Name of the ERC20 token - stored as a symbol on Blockchain</param>
        /// <param name="supply">max supply of ERC20 token.</param>
        /// <param name="description">Description of the ERC20 token</param>
        /// <param name="basePair">Base pair for ERC20 token. 1 token will be equal to 1 unit of base pair. Transaction value will be calculated according to this base pair.</param>
        /// <param name="customer">If customer is filled then is created or updated.</param>
        /// <param name="xpub">Extended public key (xpub), from which address, where all initial supply will be stored, will be generated. Either xpub and derivationIndex, or address must be present, not both.</param>
        /// <param name="derivationIndex">Derivation index for xpub to generate specific deposit address.</param>
        /// <param name="address">Address on Ethereum blockchain, where all initial supply will be stored. Either xpub and derivationIndex, or address must be present, not both.</param>
        /// <param name="mnemonic">Mnemonic to generate private key for the deploy account of ERC20, from which the gas will be paid (index will be used). If address is not present, mnemonic is used to generate xpub and index is set to 1. Either mnemonic and index or privateKey and address must be present, not both.</param>
        /// <param name="index">derivation index of address to pay for deployment of ERC20</param>
        /// <param name="privateKey">Private key of Ethereum account address, from which gas for deployment of ERC20 will be paid. Private key, mnemonic or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the mnemonic / private key associated in signing application. When hash identifies mnemonic, index must be present to represent specific account to pay from. Private key, mnemonic or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OffchainAccountIdTxIdPair> DeployERC20Token(
            string symbol, string supply, string description, string basePair,
            LedgerCustomerOptions customer = null, string xpub = null, int? derivationIndex = null, string address = null,
            string mnemonic = null, int? index = null, string privateKey = null, string signatureId = null, long? nonce = null,
            CancellationToken ct = default)
            => DeployERC20TokenAsync(
            symbol, supply, description, basePair,
            customer, xpub, derivationIndex, address,
            mnemonic, index, privateKey, signatureId, nonce, ct).Result;
        /// <summary>
        /// <b>Title:</b> Deploy Ethereum ERC20 Smart Contract Off-chain<br />
        /// <b>Credits:</b> 4 credits per API call.<br />
        /// <b>Description:</b>
        /// Deploy Ethereum ERC20 Smart Contract. This is a helper method, which is combination of Create new ERC20 token and Deploy blockchain ERC20.
        /// After deploying a contract to blockchain, the contract address will become available and must be stored within Tatum.Otherwise, it will not be possible to interact with it and starts automatic blockchain synchronization.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="symbol">Name of the ERC20 token - stored as a symbol on Blockchain</param>
        /// <param name="supply">max supply of ERC20 token.</param>
        /// <param name="description">Description of the ERC20 token</param>
        /// <param name="basePair">Base pair for ERC20 token. 1 token will be equal to 1 unit of base pair. Transaction value will be calculated according to this base pair.</param>
        /// <param name="customer">If customer is filled then is created or updated.</param>
        /// <param name="xpub">Extended public key (xpub), from which address, where all initial supply will be stored, will be generated. Either xpub and derivationIndex, or address must be present, not both.</param>
        /// <param name="derivationIndex">Derivation index for xpub to generate specific deposit address.</param>
        /// <param name="address">Address on Ethereum blockchain, where all initial supply will be stored. Either xpub and derivationIndex, or address must be present, not both.</param>
        /// <param name="mnemonic">Mnemonic to generate private key for the deploy account of ERC20, from which the gas will be paid (index will be used). If address is not present, mnemonic is used to generate xpub and index is set to 1. Either mnemonic and index or privateKey and address must be present, not both.</param>
        /// <param name="index">derivation index of address to pay for deployment of ERC20</param>
        /// <param name="privateKey">Private key of Ethereum account address, from which gas for deployment of ERC20 will be paid. Private key, mnemonic or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the mnemonic / private key associated in signing application. When hash identifies mnemonic, index must be present to represent specific account to pay from. Private key, mnemonic or signature Id must be present.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OffchainAccountIdTxIdPair>> DeployERC20TokenAsync(
            string symbol, string supply, string description, string basePair,
            LedgerCustomerOptions customer = null, string xpub = null, int? derivationIndex = null, string address = null,
            string mnemonic = null, int? index = null, string privateKey = null, string signatureId = null, long? nonce = null,
            CancellationToken ct = default)
        {
            var credits = 2;
            var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "supply", supply },
                { "description", description },
                { "basePair", basePair },
            };
            parameters.AddOptionalParameter("customer", customer);
            parameters.AddOptionalParameter("xpub", xpub);
            parameters.AddOptionalParameter("derivationIndex", derivationIndex);
            parameters.AddOptionalParameter("address", address);
            parameters.AddOptionalParameter("mnemonic", mnemonic);
            parameters.AddOptionalParameter("index", index);
            parameters.AddOptionalParameter("privateKey", privateKey);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("nonce", nonce);

            var url = Offchain.Tatum.GetUrl(string.Format(Endpoints_DeployERC20Token));
            var result = await Offchain.Tatum.SendTatumRequest<OffchainAccountIdTxIdPair>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success/* || !result.Data.Completed*/) return WebCallResult<OffchainAccountIdTxIdPair>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<OffchainAccountIdTxIdPair>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Set ERC20 token contract address<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Set contract address of ERC20 token. This must be done in order to communicate with ERC20 smart contract. After creating and deploying ERC20 token to Ethereum blockchain, smart contract address is generated and must be set within Tatum. Otherwise Tatum platform will not be able to detect incoming deposits of ERC20 and do withdrawals from Tatum accounts to other blockchain addresses.
        /// </summary>
        /// <param name="address">ERC20 contract address</param>
        /// <param name="symbol">ERC20 symbol name.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> SetERC20TokenContractAddress(string address, string symbol, CancellationToken ct = default) => SetERC20TokenContractAddressAsync(address, symbol, ct).Result;
        /// <summary>
        /// <b>Title:</b> Set ERC20 token contract address<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Set contract address of ERC20 token. This must be done in order to communicate with ERC20 smart contract. After creating and deploying ERC20 token to Ethereum blockchain, smart contract address is generated and must be set within Tatum. Otherwise Tatum platform will not be able to detect incoming deposits of ERC20 and do withdrawals from Tatum accounts to other blockchain addresses.
        /// </summary>
        /// <param name="address">ERC20 contract address</param>
        /// <param name="symbol">ERC20 symbol name.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> SetERC20TokenContractAddressAsync(string address, string symbol, CancellationToken ct = default)
        {
            var credits = 2;
            var url = Offchain.Tatum.GetUrl(string.Format(Endpoints_SetERC20TokenContractAddress, symbol, address));
            var result = await Offchain.Tatum.SendTatumRequest<string>(url, HttpMethod.Post, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Transfer Ethereum ERC20 from Tatum ledger to blockchain<br />
        /// <b>Credits:</b> 4 credits per API call.<br />
        /// <b>Description:</b>
        /// Transfer Ethereum ERC20 Smart Contract Tokens from Tatum account to blockchain address. This will create Tatum internal withdrawal request with ID. If every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If Ethereum server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="to_address">Blockchain address to send ERC20 token to</param>
        /// <param name="amount">Amount to be sent.</param>
        /// <param name="currency">ERC20 symbol. Required only for calls from Tatum Middleware.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="privateKey">Private key of sender address. Either mnemonic and index, privateKey or signature Id must be present - depends on the type of account and xpub.</param>
        /// <param name="signatureId">Identifier of the mnemonic / private key associated in signing application. When hash identifies mnemonic, index must be present to represent specific account to pay from. Private key, mnemonic or signature Id must be present.</param>
        /// <param name="index">Derivation index of sender address.</param>
        /// <param name="mnemonic">Mnemonic to generate private key for sender address. Either mnemonic and index, or privateKey must be present - depends on the type of account and xpub.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OffchainTransferResponse> SendERC20Token(
            string senderAccountId, string to_address, decimal amount, string currency = null, long? nonce = null,
            bool? compliant = null, string privateKey = null, string signatureId = null, int? index = null, string mnemonic = null,
            string paymentId = null, string senderNote = null,
            CancellationToken ct = default)
            => SendERC20TokenAsync(
            senderAccountId, to_address, amount, currency, nonce,
            compliant, privateKey, signatureId, index, mnemonic,
            paymentId, senderNote, ct).Result;
        /// <summary>
        /// <b>Title:</b> Transfer Ethereum ERC20 from Tatum ledger to blockchain<br />
        /// <b>Credits:</b> 4 credits per API call.<br />
        /// <b>Description:</b>
        /// Transfer Ethereum ERC20 Smart Contract Tokens from Tatum account to blockchain address. This will create Tatum internal withdrawal request with ID. If every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If Ethereum server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="to_address">Blockchain address to send ERC20 token to</param>
        /// <param name="amount">Amount to be sent.</param>
        /// <param name="currency">ERC20 symbol. Required only for calls from Tatum Middleware.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="privateKey">Private key of sender address. Either mnemonic and index, privateKey or signature Id must be present - depends on the type of account and xpub.</param>
        /// <param name="signatureId">Identifier of the mnemonic / private key associated in signing application. When hash identifies mnemonic, index must be present to represent specific account to pay from. Private key, mnemonic or signature Id must be present.</param>
        /// <param name="index">Derivation index of sender address.</param>
        /// <param name="mnemonic">Mnemonic to generate private key for sender address. Either mnemonic and index, or privateKey must be present - depends on the type of account and xpub.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OffchainTransferResponse>> SendERC20TokenAsync(
            string senderAccountId, string to_address, decimal amount, string currency = null, long? nonce = null,
            bool? compliant = null, string privateKey = null, string signatureId = null, int? index = null, string mnemonic = null,
            string paymentId = null, string senderNote = null,
            CancellationToken ct = default)
        {
            var credits = 4;
            var ci = CultureInfo.InvariantCulture;
            var parameters = new Dictionary<string, object>
            {
                { "senderAccountId", senderAccountId },
                { "address", to_address },
                { "amount", amount.ToString(ci) },
            };
            parameters.AddOptionalParameter("currency", currency);
            parameters.AddOptionalParameter("nonce", nonce);
            parameters.AddOptionalParameter("compliant", compliant);
            parameters.AddOptionalParameter("privateKey", privateKey);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("index", index);
            parameters.AddOptionalParameter("mnemonic", mnemonic);
            parameters.AddOptionalParameter("paymentId", paymentId);
            parameters.AddOptionalParameter("senderNote", senderNote);

            var url = Offchain.Tatum.GetUrl(string.Format(Endpoints_TransferERC20Token));
            var result = await Offchain.Tatum.SendTatumRequest<OffchainTransferResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success/* || !result.Data.Completed*/) return WebCallResult<OffchainTransferResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<OffchainTransferResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Send XLM / Asset from Tatum ledger to blockchain<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send XLM or XLM-based Assets from account to account. This will create Tatum internal withdrawal request with ID. When every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If XLM server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="fromAccount">Blockchain account to send from</param>
        /// <param name="address">Blockchain address to send assets</param>
        /// <param name="amount">Amount to be sent, in XLM or XLM-based Asset.</param>
        /// <param name="secret">Secret for account. Secret, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the secret associated in signing application. Secret, or signature Id must be present.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="attr">Short message to recipient. Usually used as an account discriminator. It can be either 28 characters long ASCII text, 64 characters long HEX string or uint64 number. When using as an account disciminator in Tatum Offchain ledger, can be in format of destination_acc|source_acc.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account.</param>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets. Required only for calls from Tatum Middleware.</param>
        /// <param name="token">Asset name. Required only for calls from Tatum Middleware.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OffchainTransferResponse> SendStellar(
            string senderAccountId, string fromAccount, string address, decimal amount,
            string secret = null, string signatureId = null, bool? compliant = null, string attr = null, string paymentId = null,
            string senderNote = null, string issuerAccount = null, string token = null,
            CancellationToken ct = default)
            => SendStellarAsync(
            senderAccountId, fromAccount, address, amount,
            secret, signatureId, compliant, attr, paymentId,
            senderNote, issuerAccount, token, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send XLM / Asset from Tatum ledger to blockchain<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send XLM or XLM-based Assets from account to account. This will create Tatum internal withdrawal request with ID. When every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If XLM server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="fromAccount">Blockchain account to send from</param>
        /// <param name="address">Blockchain address to send assets</param>
        /// <param name="amount">Amount to be sent, in XLM or XLM-based Asset.</param>
        /// <param name="secret">Secret for account. Secret, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the secret associated in signing application. Secret, or signature Id must be present.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="attr">Short message to recipient. Usually used as an account discriminator. It can be either 28 characters long ASCII text, 64 characters long HEX string or uint64 number. When using as an account disciminator in Tatum Offchain ledger, can be in format of destination_acc|source_acc.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account.</param>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets. Required only for calls from Tatum Middleware.</param>
        /// <param name="token">Asset name. Required only for calls from Tatum Middleware.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OffchainTransferResponse>> SendStellarAsync(
            string senderAccountId, string fromAccount, string address, decimal amount,
            string secret = null, string signatureId = null, bool? compliant = null, string attr = null, string paymentId = null,
            string senderNote = null, string issuerAccount = null, string token = null,
            CancellationToken ct = default)
        {
            var credits = 10;
            var ci = CultureInfo.InvariantCulture;
            var parameters = new Dictionary<string, object>
            {
                { "senderAccountId", senderAccountId },
                { "fromAccount", fromAccount },
                { "address", address },
                { "amount", amount.ToString(ci) },
            };
            parameters.AddOptionalParameter("secret", secret);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("compliant", compliant);
            parameters.AddOptionalParameter("attr", attr);
            parameters.AddOptionalParameter("paymentId", paymentId);
            parameters.AddOptionalParameter("senderNote", senderNote);
            parameters.AddOptionalParameter("issuerAccount", issuerAccount);
            parameters.AddOptionalParameter("token", token);

            var url = Offchain.Tatum.GetUrl(string.Format(Endpoints_StellarTransfer));
            var result = await Offchain.Tatum.SendTatumRequest<OffchainTransferResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success/* || !result.Data.Completed*/) return WebCallResult<OffchainTransferResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<OffchainTransferResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Create XLM based Asset<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create XLM-based Asset in Tatum Ledger. Asset must be created and configured on XLM blockhain before using Create trust line. This API call will create Tatum internal Virtual Currency. It is possible to create Tatum ledger accounts with off-chain support.
        /// </summary>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets.</param>
        /// <param name="token">Asset name.</param>
        /// <param name="basePair">Base pair for Asset. Transaction value will be calculated according to this base pair. e.g. 1 TOKEN123 is equal to 1 EUR, if basePair is set to EUR.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> CreateXLMAsset(string issuerAccount, string token, string basePair, CancellationToken ct = default) => CreateXLMAssetAsync(issuerAccount, token, basePair, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create XLM based Asset<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create XLM-based Asset in Tatum Ledger. Asset must be created and configured on XLM blockhain before using Create trust line. This API call will create Tatum internal Virtual Currency. It is possible to create Tatum ledger accounts with off-chain support.
        /// </summary>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets.</param>
        /// <param name="token">Asset name.</param>
        /// <param name="basePair">Base pair for Asset. Transaction value will be calculated according to this base pair. e.g. 1 TOKEN123 is equal to 1 EUR, if basePair is set to EUR.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> CreateXLMAssetAsync(string issuerAccount, string token, string basePair, CancellationToken ct = default)
        {
            var credits = 2;
            var parameters = new Dictionary<string, object>
            {
                { "issuerAccount", issuerAccount },
                { "token", token },
                { "basePair", basePair },
            };

            var url = Offchain.Tatum.GetUrl(string.Format(Endpoints_CreateXLMAsset));
            var result = await Offchain.Tatum.SendTatumRequest<string>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Send XRP from Tatum ledger to blockchain<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send XRP from account to account. This will create Tatum internal withdrawal request with ID. When every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If XRP server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="account">XRP account address. Must be the one used for generating deposit tags.</param>
        /// <param name="address">Blockchain address to send assets</param>
        /// <param name="amount">Amount to be sent, in XRP.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="attr">Destination tag of the recipient account, if any. Must be stringified uint32.</param>
        /// <param name="sourceTag">Source tag of sender account, if any.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="secret">Secret for account. Secret, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the secret associated in signing application. Secret, or signature Id must be present.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account.</param>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets to create trust line for. Required only for calls from Tatum Middleware.</param>
        /// <param name="token">Asset name. Must be 160bit HEX string, e.g. SHA1. Required only for calls from Tatum Middleware.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OffchainTransferResponse> SendRipple(
            string senderAccountId, string account, string address, decimal amount,
            bool? compliant = null, string attr = null, int? sourceTag = null, string paymentId = null, string secret = null,
            string signatureId = null, string senderNote = null, string issuerAccount = null, string token = null,
            CancellationToken ct = default)
            => SendRippleAsync(
            senderAccountId, account, address, amount,
            compliant, attr, sourceTag, paymentId, secret,
            signatureId, senderNote, issuerAccount, token, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send XRP from Tatum ledger to blockchain<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send XRP from account to account. This will create Tatum internal withdrawal request with ID. When every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If XRP server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="account">XRP account address. Must be the one used for generating deposit tags.</param>
        /// <param name="address">Blockchain address to send assets</param>
        /// <param name="amount">Amount to be sent, in XRP.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="attr">Destination tag of the recipient account, if any. Must be stringified uint32.</param>
        /// <param name="sourceTag">Source tag of sender account, if any.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="secret">Secret for account. Secret, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the secret associated in signing application. Secret, or signature Id must be present.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account.</param>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets to create trust line for. Required only for calls from Tatum Middleware.</param>
        /// <param name="token">Asset name. Must be 160bit HEX string, e.g. SHA1. Required only for calls from Tatum Middleware.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OffchainTransferResponse>> SendRippleAsync(
            string senderAccountId, string account, string address, decimal amount,
            bool? compliant = null, string attr = null, int? sourceTag = null, string paymentId = null, string secret = null,
            string signatureId = null, string senderNote = null, string issuerAccount = null, string token = null,
            CancellationToken ct = default)
        {
            var credits = 10;
            var ci = CultureInfo.InvariantCulture;
            var parameters = new Dictionary<string, object>
            {
                { "senderAccountId", senderAccountId },
                { "account", account },
                { "address", address },
                { "amount", amount.ToString(ci) },
            };
            parameters.AddOptionalParameter("compliant", compliant);
            parameters.AddOptionalParameter("attr", attr);
            parameters.AddOptionalParameter("sourceTag", sourceTag);
            parameters.AddOptionalParameter("paymentId", paymentId);
            parameters.AddOptionalParameter("secret", secret);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("senderNote", senderNote);
            parameters.AddOptionalParameter("issuerAccount", issuerAccount);
            parameters.AddOptionalParameter("token", token);

            var url = Offchain.Tatum.GetUrl(string.Format(Endpoints_RippleTransfer));
            var result = await Offchain.Tatum.SendTatumRequest<OffchainTransferResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success/* || !result.Data.Completed*/) return WebCallResult<OffchainTransferResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<OffchainTransferResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Create XRP based Asset<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create XRP-based Asset in Tatum Ledger. Asset must be created and configured on XRP blockhain before using Create trust line. This API call will create Tatum internal Virtual Currency. It is possible to create Tatum ledger accounts with off-chain support.
        /// </summary>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets.</param>
        /// <param name="token">Asset name.</param>
        /// <param name="basePair">Base pair for Asset. Transaction value will be calculated according to this base pair. e.g. 1 TOKEN123 is equal to 1 EUR, if basePair is set to EUR.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> CreateXRPAsset(string issuerAccount, string token, string basePair, CancellationToken ct = default) => CreateXRPAssetAsync(issuerAccount, token, basePair, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create XRP based Asset<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create XRP-based Asset in Tatum Ledger. Asset must be created and configured on XRP blockhain before using Create trust line. This API call will create Tatum internal Virtual Currency. It is possible to create Tatum ledger accounts with off-chain support.
        /// </summary>
        /// <param name="issuerAccount">Blockchain address of the issuer of the assets.</param>
        /// <param name="token">Asset name.</param>
        /// <param name="basePair">Base pair for Asset. Transaction value will be calculated according to this base pair. e.g. 1 TOKEN123 is equal to 1 EUR, if basePair is set to EUR.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> CreateXRPAssetAsync(string issuerAccount, string token, string basePair, CancellationToken ct = default)
        {
            var credits = 2;
            var parameters = new Dictionary<string, object>
            {
                { "issuerAccount", issuerAccount },
                { "token", token },
                { "basePair", basePair },
            };

            var url = Offchain.Tatum.GetUrl(string.Format(Endpoints_CreateXRPAsset));
            var result = await Offchain.Tatum.SendTatumRequest<string>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }

        /// <summary>
        /// <b>Title:</b> Send BNB from Tatum ledger to blockchain<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send BNB or BNB Asset from account to account. This will create Tatum internal withdrawal request with ID. When every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If BNB server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="address">Blockchain address to send assets</param>
        /// <param name="amount">Amount to be sent, in BNB.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="attr">Memo of the recipient account, if any.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="privateKey">Private key of sender address.</param>
        /// <param name="signatureId">Identifier of the secret associated in signing application. Secret, or signature Id must be present.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OffchainTransferResponse> SendBNB(
            string senderAccountId, string address, decimal amount,
            bool? compliant = null, string attr = null, string paymentId = null,
            string privateKey = null, string signatureId = null, string senderNote = null,
            CancellationToken ct = default)
            => SendBNBAsync(
            senderAccountId, address, amount,
            compliant, attr, paymentId,
            privateKey, signatureId, senderNote, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send BNB from Tatum ledger to blockchain<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send BNB or BNB Asset from account to account. This will create Tatum internal withdrawal request with ID. When every system works as expected, withdrawal request is marked as complete and transaction id is assigned to it.
        /// - If BNB server connection is unavailable, withdrawal request is cancelled.
        /// - If blockchain transfer is successful, but is it not possible to reach Tatum, transaction id of blockchain transaction is returned and withdrawal request must be completed manually, otherwise all other withdrawals will be pending.
        /// It is possible to perform offchain to blockchain transaction for ledger accounts without blockchain address assigned to them.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and losing funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="senderAccountId">Sender account ID</param>
        /// <param name="address">Blockchain address to send assets</param>
        /// <param name="amount">Amount to be sent, in BNB.</param>
        /// <param name="compliant">Compliance check, if withdrawal is not compliant, it will not be processed.</param>
        /// <param name="attr">Memo of the recipient account, if any.</param>
        /// <param name="paymentId">Identifier of the payment, shown for created Transaction within Tatum sender account.</param>
        /// <param name="privateKey">Private key of sender address.</param>
        /// <param name="signatureId">Identifier of the secret associated in signing application. Secret, or signature Id must be present.</param>
        /// <param name="senderNote">Note visible to owner of withdrawing account.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OffchainTransferResponse>> SendBNBAsync(
            string senderAccountId, string address, decimal amount,
            bool? compliant = null, string attr = null, string paymentId = null,
            string privateKey = null, string signatureId = null, string senderNote = null,
            CancellationToken ct = default)
        {
            var credits = 10;
            var ci = CultureInfo.InvariantCulture;
            var parameters = new Dictionary<string, object>
            {
                { "senderAccountId", senderAccountId },
                { "address", address },
                { "amount", amount.ToString(ci) },
            };
            parameters.AddOptionalParameter("compliant", compliant);
            parameters.AddOptionalParameter("attr", attr);
            parameters.AddOptionalParameter("paymentId", paymentId);
            parameters.AddOptionalParameter("privateKey", privateKey);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("senderNote", senderNote);

            var url = Offchain.Tatum.GetUrl(string.Format(Endpoints_BinanceTransfer));
            var result = await Offchain.Tatum.SendTatumRequest<OffchainTransferResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success/* || !result.Data.Completed*/) return WebCallResult<OffchainTransferResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<OffchainTransferResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Create BNB based Asset<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create BNB-based Asset in Tatum Ledger. Asset must be created and configured on Binance blockhain before. Please see Create Asset. This API call will create Tatum internal Virtual Currency. It is possible to create Tatum ledger accounts with off-chain support.
        /// </summary>
        /// <param name="token">Asset name.</param>
        /// <param name="basePair">Base pair for Asset. Transaction value will be calculated according to this base pair. e.g. 1 TOKEN123 is equal to 1 EUR, if basePair is set to EUR.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> CreateBNBAsset(string token, string basePair, CancellationToken ct = default) => CreateBNBAssetAsync(token, basePair, ct).Result;
        /// <summary>
        /// <b>Title:</b> Create BNB based Asset<br />
        /// <b>Credits:</b> 2 credits per API call.<br />
        /// <b>Description:</b>
        /// Create BNB-based Asset in Tatum Ledger. Asset must be created and configured on Binance blockhain before. Please see Create Asset. This API call will create Tatum internal Virtual Currency. It is possible to create Tatum ledger accounts with off-chain support.
        /// </summary>
        /// <param name="token">Asset name.</param>
        /// <param name="basePair">Base pair for Asset. Transaction value will be calculated according to this base pair. e.g. 1 TOKEN123 is equal to 1 EUR, if basePair is set to EUR.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> CreateBNBAssetAsync(string token, string basePair, CancellationToken ct = default)
        {
            var credits = 2;
            var parameters = new Dictionary<string, object>
            {
                { "token", token },
                { "basePair", basePair },
            };

            var url = Offchain.Tatum.GetUrl(string.Format(Endpoints_CreateBNBAsset));
            var result = await Offchain.Tatum.SendTatumRequest<string>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, true, null);
        }
    }

    public class OffChainWithdrawalClient
    {
        public OffChainClient Offchain { get; protected set; }

        protected const string Endpoints_Store = "offchain/withdrawal";
        protected const string Endpoints_Complete = "offchain/withdrawal/{0}/{1}";
        protected const string Endpoints_Cancel = "offchain/withdrawal/{0";
        protected const string Endpoints_Broadcast = "offchain/withdrawal/broadcast";

        public OffChainWithdrawalClient(OffChainClient offChainClient)
        {
            Offchain = offChainClient;
        }

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
    }
}
