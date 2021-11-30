using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.Enums;
using Tatum.Net.Helpers;
using Tatum.Net.Interfaces;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Clients
{
    public class OffChainAccountClient : ITatumOffchainAccountClient
    {
        public OffChainClient Offchain { get; protected set; }

        #region API Endpoints

        #region Off-chain Account
        protected const string Endpoints_DepositAddress = "offchain/account/{0}/address";
        protected const string Endpoints_DepositAddressBatch = "offchain/account/address/batch";
        protected const string Endpoints_CheckAddress = "offchain/account/address/{0}/{1}";
        protected const string Endpoints_RemoveAddress = "offchain/account/{0}/address/{1}";
        protected const string Endpoints_AssignAddress = "offchain/account/{0}/address/{1}";
        #endregion

        #endregion

        public OffChainAccountClient(OffChainClient offChainClient)
        {
            Offchain = offChainClient;
        }


        #region Off-chain / Account
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
        #endregion

    }
}
