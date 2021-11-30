using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.Helpers;
using Tatum.Net.Interfaces;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Clients
{
    public class NeoClient : ITatumBlockchainNeoClient
    {
        public TatumClient Tatum { get; protected set; }

        #region API Endpoints

        #region Blockchain - NEO
        protected const string Endpoints_GenerateAccount = "neo/wallet";
        protected const string Endpoints_CurrentBlock = "neo/block/current";
        protected const string Endpoints_GetBlock = "neo/block/{0}";
        protected const string Endpoints_GetBalance = "neo/account/balance/{0}";
        protected const string Endpoints_GetAssetInfo = "neo/asset/{0}";
        protected const string Endpoints_GetUnspentTransactionOutputs = "neo/transaction/out/{0}/{1}";
        protected const string Endpoints_GetTransactionsByAccount = "neo/account/tx/{0}";
        protected const string Endpoints_GetContractInfo = "neo/contract/{0}";
        protected const string Endpoints_GetTransactionByHash = "neo/transaction/{0}";
        protected const string Endpoints_Send = "neo/transaction";
        protected const string Endpoints_ClaimGAS = "neo/claim";
        protected const string Endpoints_Invoke = "neo/invoke";
        protected const string Endpoints_Broadcast = "neo/broadcast";
        #endregion

        #endregion

        public NeoClient(TatumClient tatumClient)
        {
            Tatum = tatumClient;
        }


        #region Blockchain / NEO
        /// <summary>
        /// <b>Title:</b> Generate NEO account<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate NEO account. Tatum does not support HD wallet for NEO, only specific address and private key can be generated.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<NeoAccount> GenerateAccount(CancellationToken ct = default) => GenerateAccountAsync(ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate NEO account<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate NEO account. Tatum does not support HD wallet for NEO, only specific address and private key can be generated.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<NeoAccount>> GenerateAccountAsync(CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_GenerateAccount));
            return await Tatum.SendTatumRequest<NeoAccount>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get current NEO block<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get current NEO block.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<long> GetCurrentBlock(CancellationToken ct = default) => GetCurrentBlockAsync(ct).Result;
        /// <summary>
        /// <b>Title:</b> Get current NEO block<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get current NEO block.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<long>> GetCurrentBlockAsync(CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_CurrentBlock));
            var result = await Tatum.SendTatumRequest<string>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<long>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<long>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.ToLong(), null);
        }

        /// <summary>
        /// <b>Title:</b> Get NEO block<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get NEO block by hash or height.
        /// </summary>
        /// <param name="hash_height">Block hash or height.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<NeoBlock> GetBlock(string hash_height, CancellationToken ct = default) => GetBlockAsync(hash_height, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get NEO block<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get NEO block by hash or height.
        /// </summary>
        /// <param name="hash_height">Block hash or height.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<NeoBlock>> GetBlockAsync(string hash_height, CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetBlock, hash_height));
            return await Tatum.SendTatumRequest<NeoBlock>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get NEO Account balance<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Balance of all assets (NEO, GAS, etc.) and tokens for the Account.
        /// </summary>
        /// <param name="address">Address to get balance</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<NeoBalance> GetBalance(string address, CancellationToken ct = default) => GetBalanceAsync(address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get NEO Account balance<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get Balance of all assets (NEO, GAS, etc.) and tokens for the Account.
        /// </summary>
        /// <param name="address">Address to get balance</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<NeoBalance>> GetBalanceAsync(string address, CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetBalance, address));
            return await Tatum.SendTatumRequest<NeoBalance>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get Neo Asset details<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get information about asset.
        /// </summary>
        /// <param name="asset">Asset ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<NeoAsset> GetAssetInfo(string asset, CancellationToken ct = default) => GetAssetInfoAsync(asset, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get Neo Asset details<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get information about asset.
        /// </summary>
        /// <param name="asset">Asset ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<NeoAsset>> GetAssetInfoAsync(string asset, CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetAssetInfo, asset));
            return await Tatum.SendTatumRequest<NeoAsset>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get NEO unspent transaction outputs<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get NEO unspent transaction outputs.
        /// </summary>
        /// <param name="txId">Transaction ID.</param>
        /// <param name="index">Index of output.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<NeoTransactionOutput> GetUnspentTransactionOutputs(string txId, long index, CancellationToken ct = default) => GetUnspentTransactionOutputsAsync(txId, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get NEO unspent transaction outputs<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get NEO unspent transaction outputs.
        /// </summary>
        /// <param name="txId">Transaction ID.</param>
        /// <param name="index">Index of output.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<NeoTransactionOutput>> GetUnspentTransactionOutputsAsync(string txId, long index, CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetUnspentTransactionOutputs, txId, index));
            return await Tatum.SendTatumRequest<NeoTransactionOutput>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get NEO Account transactions<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get NEO Account transactions.
        /// </summary>
        /// <param name="address">Example: AKL19WwiJ2fiTDkAnYQ7GJSTUBoJPTQKhn</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<NeoAccountTransaction>> GetTransactionsByAccount(string address, CancellationToken ct = default) => GetTransactionsByAccountAsync(address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get NEO Account transactions<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get NEO Account transactions.
        /// </summary>
        /// <param name="address">Example: AKL19WwiJ2fiTDkAnYQ7GJSTUBoJPTQKhn</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<NeoAccountTransaction>>> GetTransactionsByAccountAsync(string address, CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetTransactionsByAccount, address));
            return await Tatum.SendTatumRequest<IEnumerable<NeoAccountTransaction>>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get NEO contract details<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get NEO contract details.
        /// </summary>
        /// <param name="scriptHash">Hash of smart contract</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<NeoContract> GetContractInfo(string scriptHash, CancellationToken ct = default) => GetContractInfoAsync(scriptHash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get NEO contract details<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get NEO contract details.
        /// </summary>
        /// <param name="scriptHash">Hash of smart contract</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<NeoContract>> GetContractInfoAsync(string scriptHash, CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetContractInfo, scriptHash));
            return await Tatum.SendTatumRequest<NeoContract>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get NEO transaction by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get NEO transaction by hash.
        /// </summary>
        /// <param name="hash">Transaction hash.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<NeoTransaction> GetTransactionByHash(string hash, CancellationToken ct = default) => GetTransactionByHashAsync(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get NEO transaction by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get NEO transaction by hash.
        /// </summary>
        /// <param name="hash">Transaction hash.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<NeoTransaction>> GetTransactionByHashAsync(string hash, CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetTransactionByHash, hash));
            return await Tatum.SendTatumRequest<NeoTransaction>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Send NEO assets<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Send NEO assets from address to address. It is possible to send NEO and GAS in the same transaction.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="to">Recipient address.</param>
        /// <param name="Amount">Assets to send.</param>
        /// <param name="GAS_Amount">Assets to send.</param>
        /// <param name="fromPrivateKey">Private key of address to send assets from.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> Send(string to, decimal Amount, decimal GAS_Amount, string fromPrivateKey, CancellationToken ct = default) => SendAsync(to, Amount, GAS_Amount, fromPrivateKey, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send NEO assets<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Send NEO assets from address to address. It is possible to send NEO and GAS in the same transaction.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="to">Recipient address.</param>
        /// <param name="Amount">Assets to send.</param>
        /// <param name="GAS_Amount">Assets to send.</param>
        /// <param name="fromPrivateKey">Private key of address to send assets from.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> SendAsync(string to, decimal Amount, decimal GAS_Amount, string fromPrivateKey, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "to", to },
                { "initialize", fromPrivateKey },
                { "assets", new NeoGasAssetCouple { NEO = Amount, GAS = GAS_Amount } },
            };

            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_Send));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Claim GAS<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Claim GAS for NEO account. Every account owner can claim for the GAS, which is produced for owning NEO on the address.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="privateKey">Private key of address to claim for GAS.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> ClaimGAS(string privateKey, CancellationToken ct = default) => ClaimGASAsync(privateKey, ct).Result;
        /// <summary>
        /// <b>Title:</b> Claim GAS<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Claim GAS for NEO account. Every account owner can claim for the GAS, which is produced for owning NEO on the address.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="privateKey">Private key of address to claim for GAS.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> ClaimGASAsync(string privateKey, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "privateKey", privateKey },
            };

            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_ClaimGAS));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Send NEO smart contract tokens<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Send NEO smart contract tokens. It is possible to transfer custom NEO-based tokens to another account.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="scriptHash">Function to be invoked on Smart Contract.</param>
        /// <param name="amount">Amount to be sent.</param>
        /// <param name="numOfDecimals">Number of decimals of asset being transferred.</param>
        /// <param name="fromPrivateKey">Private key of address to invoke smart contract.</param>
        /// <param name="to">Recipient address.</param>
        /// <param name="additionalInvocationGas">Additional GAS to be paid for smart contract invocation.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> SendToken(string scriptHash, decimal amount, int numOfDecimals, string fromPrivateKey, string to, decimal additionalInvocationGas = 0, CancellationToken ct = default) => SendTokenAsync(scriptHash, amount, numOfDecimals, fromPrivateKey, to, additionalInvocationGas, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send NEO smart contract tokens<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Send NEO smart contract tokens. It is possible to transfer custom NEO-based tokens to another account.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, it is possible to use the Tatum client library for supported languages or Tatum Middleware with a custom key management system.
        /// </summary>
        /// <param name="scriptHash">Function to be invoked on Smart Contract.</param>
        /// <param name="amount">Amount to be sent.</param>
        /// <param name="numOfDecimals">Number of decimals of asset being transferred.</param>
        /// <param name="fromPrivateKey">Private key of address to invoke smart contract.</param>
        /// <param name="to">Recipient address.</param>
        /// <param name="additionalInvocationGas">Additional GAS to be paid for smart contract invocation.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> SendTokenAsync(string scriptHash, decimal amount, int numOfDecimals, string fromPrivateKey, string to, decimal additionalInvocationGas = 0, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "scriptHash", scriptHash },
                { "amount", amount },
                { "numOfDecimals", numOfDecimals },
                { "fromPrivateKey", fromPrivateKey },
                { "to", to },
                { "additionalInvocationGas", additionalInvocationGas },
            };

            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_Invoke));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Broadcast NEO transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast NEO transaction. This method is used internally from Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> Broadcast(string txData, CancellationToken ct = default) => BroadcastAsync(txData, ct).Result;
        /// <summary>
        /// <b>Title:</b> Broadcast NEO transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast NEO transaction. This method is used internally from Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> BroadcastAsync(string txData, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "txData", txData },
            };

            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_Broadcast));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }
        #endregion


    }
}
