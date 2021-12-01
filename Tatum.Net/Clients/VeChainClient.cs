using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.Enums;
using Tatum.Net.Helpers;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Clients
{
    public class VeChainClient
    {
        public TatumClient Tatum { get; protected set; }

        protected const string Endpoints_CurrentBlock = "vet/block/current";
        protected const string Endpoints_GetBlockByHash = "vet/block/{0}";
        protected const string Endpoints_GetBalance = "vet/account/balance/{0}";
        protected const string Endpoints_GetEnergy = "vet/account/energy/{0}";
        protected const string Endpoints_GetTransactionByHash = "vet/transaction/{0}";
        protected const string Endpoints_GetTransactionReceipt = "vet/transaction/{0}/receipt";
        protected const string Endpoints_Transaction = "vet/transaction";
        protected const string Endpoints_Gas = "vet/transaction/gas";
        protected const string Endpoints_Broadcast = "vet/broadcast";

        public VeChainClient(TatumClient tatumClient)
        {
            Tatum = tatumClient;
        }

        /// <summary>
        /// <b>Title:</b> Generate VeChain wallet<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// - Tatum follows BIP44 specification and generates for VeChain wallet with derivation path m'/44'/818'/0'/0. More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. Generate BIP44 compatible VeChain wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainWallet> GenerateWallet(string mnemonics, CancellationToken ct = default) => Tatum.Blockchain_GenerateWalletAsync(BlockchainType.VeChain, new List<string> { mnemonics }, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate VeChain wallet<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// - Tatum follows BIP44 specification and generates for VeChain wallet with derivation path m'/44'/818'/0'/0. More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. Generate BIP44 compatible VeChain wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainWallet> GenerateWallet(IEnumerable<string> mnemonics = null, CancellationToken ct = default) => Tatum.Blockchain_GenerateWalletAsync(BlockchainType.VeChain, mnemonics, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate VeChain wallet<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// - Tatum follows BIP44 specification and generates for VeChain wallet with derivation path m'/44'/818'/0'/0. More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. Generate BIP44 compatible VeChain wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainWallet>> GenerateWalletAsync(string mnemonics, CancellationToken ct = default) => await Tatum.Blockchain_GenerateWalletAsync(BlockchainType.VeChain, new List<string> { mnemonics }, ct);
        /// <summary>
        /// <b>Title:</b> Generate VeChain wallet<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// - Tatum follows BIP44 specification and generates for VeChain wallet with derivation path m'/44'/818'/0'/0. More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. Generate BIP44 compatible VeChain wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainWallet>> GenerateWalletAsync(IEnumerable<string> mnemonics = null, CancellationToken ct = default) => await Tatum.Blockchain_GenerateWalletAsync(BlockchainType.VeChain, mnemonics, ct);

        /// <summary>
        /// <b>Title:</b> Generate VeChain account address from Extended public key<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate VeChain account deposit address from Extended public key. Deposit address is generated for the specific index - each extended public key can generate up to 2^32 addresses starting from index 0 until 2^31.
        /// </summary>
        /// <param name="xpub">Extended public key of wallet.</param>
        /// <param name="index">Derivation index of desired address to be generated.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TatumAddress> GenerateDepositAddress(string xpub, int index, CancellationToken ct = default) => Tatum.Blockchain_GenerateDepositAddressAsync(BlockchainType.VeChain, xpub, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate VeChain wallet<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Tatum supports BIP44 HD wallets. It is very convenient and secure, since it can generate 2^31 addresses from 1 mnemonic phrase. Mnemonic phrase consists of 24 special words in defined order and can restore access to all generated addresses and private keys.
        /// Each address is identified by 3 main values:
        /// - Private Key - your secret value, which should never be revealed
        /// - Public Key - public address to be published
        /// - Derivation index - index of generated address
        /// - Tatum follows BIP44 specification and generates for VeChain wallet with derivation path m'/44'/818'/0'/0. More about BIP44 HD wallets can be found here - https://github.com/bitcoin/bips/blob/master/bip-0044.mediawiki. Generate BIP44 compatible VeChain wallet.
        /// </summary>
        /// <param name="mnemonics">Mnemonic to use for generation of extended public and private keys.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TatumAddress>> GenerateDepositAddressAsync(string xpub, int index, CancellationToken ct = default) => await Tatum.Blockchain_GenerateDepositAddressAsync(BlockchainType.VeChain, xpub, index, ct);

        /// <summary>
        /// <b>Title:</b> Generate VeChain private key<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate private key of address from mnemonic for given derivation path index. Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics"></param>
        /// <param name="index"></param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TatumKey> GeneratePrivateKey(string mnemonics, int index, CancellationToken ct = default) => Tatum.Blockchain_GeneratePrivateKeyAsync(BlockchainType.VeChain, new List<string> { mnemonics }, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate VeChain private key<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate private key of address from mnemonic for given derivation path index. Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics"></param>
        /// <param name="index"></param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TatumKey> GeneratePrivateKey(IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => Tatum.Blockchain_GeneratePrivateKeyAsync(BlockchainType.VeChain, mnemonics, index, ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate VeChain private key<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate private key of address from mnemonic for given derivation path index. Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics"></param>
        /// <param name="index"></param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TatumKey>> GeneratePrivateKeyAsync(string mnemonics, int index, CancellationToken ct = default) => await Tatum.Blockchain_GeneratePrivateKeyAsync(BlockchainType.VeChain, new List<string> { mnemonics }, index, default);
        /// <summary>
        /// <b>Title:</b> Generate VeChain private key<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate private key of address from mnemonic for given derivation path index. Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics"></param>
        /// <param name="index"></param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TatumKey>> GeneratePrivateKeyAsync(IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => await Tatum.Blockchain_GeneratePrivateKeyAsync(BlockchainType.VeChain, mnemonics, index, default);

        /// <summary>
        /// <b>Title:</b> Get VeChain current block<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get VeChain current block number.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<long> GetCurrentBlock(CancellationToken ct = default) => GetCurrentBlockAsync(ct).Result;
        /// <summary>
        /// <b>Title:</b> Generate VeChain private key<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Generate private key of address from mnemonic for given derivation path index. Private key is generated for the specific index - each mnemonic can generate up to 2^32 private keys starting from index 0 until 2^31.
        /// </summary>
        /// <param name="mnemonics"></param>
        /// <param name="index"></param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<long>> GetCurrentBlockAsync(CancellationToken ct = default)
        {
            var credits = 1;
            var url = Tatum.GetUrl(string.Format(Endpoints_CurrentBlock));
            var result = await Tatum.SendTatumRequest<string>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<long>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<long>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.ToLong(), null);
        }

        /// <summary>
        /// <b>Title:</b> Get VeChain Block by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get VeChain Block by block hash or block number.
        /// </summary>
        /// <param name="hash_height">Block hash or block number</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<VeChainBlock> GetBlock(string hash_height, CancellationToken ct = default) => GetBlockAsync(hash_height, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get VeChain Block by hash<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get VeChain Block by block hash or block number.
        /// </summary>
        /// <param name="hash_height">Block hash or block number</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<VeChainBlock>> GetBlockAsync(string hash_height, CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetBlockByHash, hash_height));
            return await Tatum.SendTatumRequest<VeChainBlock>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get VeChain Account balance<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get VeChain Account balance in VET.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<VeChainBalance> GetBalance(string address, CancellationToken ct = default) => GetBalanceAsync(address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get VeChain Account balance<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get VeChain Account balance in VET.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<VeChainBalance>> GetBalanceAsync(string address, CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetBalance, address));
            return await Tatum.SendTatumRequest<VeChainBalance>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get VeChain Account energy (VTHO)<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get VeChain Account energy in VTHO. VTHO is used for paying for the transaction fee.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<VeChainEnergy> GetEnergy(string address, CancellationToken ct = default) => GetEnergyAsync(address, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get VeChain Account energy (VTHO)<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get VeChain Account energy in VTHO. VTHO is used for paying for the transaction fee.
        /// </summary>
        /// <param name="address">Account address</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<VeChainEnergy>> GetEnergyAsync(string address, CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetEnergy, address));
            return await Tatum.SendTatumRequest<VeChainEnergy>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get VeChain Transaction<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Get VeChain Transaction by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<VeChainTransaction> GetTransactionByHash(string hash, CancellationToken ct = default) => GetTransactionByHashAsync(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get VeChain Transaction<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Get VeChain Transaction by transaction hash.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<VeChainTransaction>> GetTransactionByHashAsync(string hash, CancellationToken ct = default)
        {
            var credits = 10;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetTransactionByHash, hash));
            return await Tatum.SendTatumRequest<VeChainTransaction>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Get VeChain Transaction Receipt<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get VeChain Transaction Receipt by transaction hash. Transaction receipt is available only after transaction is included in the block and contains information about paid fee or created contract address and much more.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<VeChainTransactionReceipt> GetTransactionReceipt(string hash, CancellationToken ct = default) => GetTransactionReceiptAsync(hash, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get VeChain Transaction Receipt<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Get VeChain Transaction Receipt by transaction hash. Transaction receipt is available only after transaction is included in the block and contains information about paid fee or created contract address and much more.
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<VeChainTransactionReceipt>> GetTransactionReceiptAsync(string hash, CancellationToken ct = default)
        {
            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_GetTransactionReceipt, hash));
            return await Tatum.SendTatumRequest<VeChainTransactionReceipt>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
        }

        /// <summary>
        /// <b>Title:</b> Send VeChain from account to account<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send VET from account to account. Fee for the transaction is paid in VTHO.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="to">Blockchain address to send assets</param>
        /// <param name="amount">Amount to be sent in VET</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="data">Additinal data, that can be passed to blockchain transaction as data property.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> Send(string to, decimal amount, string fromPrivateKey = null, string signatureId = null, string data = null, VeChainFee fee = null, CancellationToken ct = default) => SendAsync(to, amount, fromPrivateKey, signatureId, data, fee, ct).Result;
        /// <summary>
        /// <b>Title:</b> Send VeChain from account to account<br />
        /// <b>Credits:</b> 10 credits per API call.<br />
        /// <b>Description:</b>
        /// Send VET from account to account. Fee for the transaction is paid in VTHO.
        /// This operation needs the private key of the blockchain address.Every time the funds are transferred, the transaction must be signed with the corresponding private key.No one should ever send it's own private keys to the internet because there is a strong possibility of stealing keys and loss of funds. In this method, it is possible to enter privateKey or signatureId. PrivateKey should be used only for quick development on testnet versions of blockchain when there is no risk of losing funds. In production, Tatum KMS should be used for the highest security standards, and signatureId should be present in the request. Alternatively, using the Tatum client library for supported languages or Tatum Middleware with a custom key management system is possible.
        /// </summary>
        /// <param name="to">Blockchain address to send assets</param>
        /// <param name="amount">Amount to be sent in VET</param>
        /// <param name="fromPrivateKey">Private key of sender address. Private key, or signature Id must be present.</param>
        /// <param name="signatureId">Identifier of the private key associated in signing application. Private key, or signature Id must be present.</param>
        /// <param name="data">Additinal data, that can be passed to blockchain transaction as data property.</param>
        /// <param name="fee">Custom defined fee. If not present, it will be calculated automatically.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> SendAsync(string to, decimal amount, string fromPrivateKey = null, string signatureId = null, string data = null, VeChainFee fee = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                 { "to", to },
                 { "amount", amount },
             };
            parameters.AddOptionalParameter("fromPrivateKey", fromPrivateKey);
            parameters.AddOptionalParameter("signatureId", signatureId);
            parameters.AddOptionalParameter("data", data);
            parameters.AddOptionalParameter("fee", fee);

            var credits = 10;
            var url = Tatum.GetUrl(string.Format(Endpoints_Transaction));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Estimate VeChain Gas for transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Estimate gas required for transaction.
        /// </summary>
        /// <param name="from">Sender account address.</param>
        /// <param name="to">Recipient account address.</param>
        /// <param name="value">Amount to send.</param>
        /// <param name="data">Data to send to Smart Contract</param>
        /// <param name="nonce">Nonce</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<decimal> EstimateGasForTransaction(string from, string to, decimal value, string data = null, long? nonce = null, CancellationToken ct = default) => EstimateGasForTransactionAsync(from, to, value, data, nonce, ct).Result;
        /// <summary>
        /// <b>Title:</b> Estimate VeChain Gas for transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Estimate gas required for transaction.
        /// </summary>
        /// <param name="from">Sender account address.</param>
        /// <param name="to">Recipient account address.</param>
        /// <param name="value">Amount to send.</param>
        /// <param name="data">Data to send to Smart Contract</param>
        /// <param name="nonce">Nonce</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<decimal>> EstimateGasForTransactionAsync(string from, string to, decimal value, string data = null, long? nonce = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                 { "from", from },
                 { "to", to },
                 { "value", value },
             };
            parameters.AddOptionalParameter("data", data);
            parameters.AddOptionalParameter("nonce", nonce);

            var credits = 5;
            var url = Tatum.GetUrl(string.Format(Endpoints_Gas));
            var result = await Tatum.SendTatumRequest<string>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<decimal>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<decimal>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.ToDecimal(), null);
        }

        /// <summary>
        /// <b>Title:</b> Broadcast signed VeChain transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed transaction to VeChain blockchain. This method is used internally from Tatum KMS, Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="signatureId">ID of prepared payment template to sign. Required only, when broadcasting transaction signed by Tatum KMS.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> Broadcast(string txData, string signatureId = null, CancellationToken ct = default) => BroadcastAsync(txData, signatureId, ct).Result;
        /// <summary>
        /// <b>Title:</b> Broadcast signed VeChain transaction<br />
        /// <b>Credits:</b> 5 credits per API call.<br />
        /// <b>Description:</b>
        /// Broadcast signed transaction to VeChain blockchain. This method is used internally from Tatum KMS, Tatum Middleware or Tatum client libraries. It is possible to create custom signing mechanism and use this method only for broadcasting data to the blockchian.
        /// </summary>
        /// <param name="txData">Raw signed transaction to be published to network.</param>
        /// <param name="signatureId">ID of prepared payment template to sign. Required only, when broadcasting transaction signed by Tatum KMS.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> BroadcastAsync(string txData, string signatureId = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                 { "txData", txData },
             };
            parameters.AddOptionalParameter("signatureId", signatureId);

            var credits = 2;
            var url = Tatum.GetUrl(string.Format(Endpoints_Broadcast));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }
    }
}
