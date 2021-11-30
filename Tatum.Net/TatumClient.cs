using CryptoExchange.Net;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.Clients;
using Tatum.Net.CoreObjects;
using Tatum.Net.Enums;
using Tatum.Net.Helpers;
using Tatum.Net.Interfaces;
using Tatum.Net.RestObjects;

namespace Tatum.Net
{
    public class TatumClient : RestClient, IRestClient, ITatumClient
    {
        #region Core Fields
        protected static TatumClientOptions defaultOptions = new TatumClientOptions();
        protected static TatumClientOptions DefaultOptions => defaultOptions.Copy();
        #endregion

        #region Clients
        public BinanceClient Binance { get; protected set; }
        public BitcoinCashClient BitcoinCash { get; protected set; }
        public BitcoinClient Bitcoin { get; protected set; }
        public EthereumClient Ethereum { get; protected set; }
        public KmsClient KMS { get; protected set; }
        public LedgerClient Ledger { get; protected set; }
        public LibraClient Libra { get; protected set; }
        public LitecoinClient Litecoin { get; protected set; }
        public NeoClient NEO { get; protected set; }
        public OffChainClient OffChain { get; protected set; }
        public RecordsClient Records { get; protected set; }
        public RippleClient Ripple { get; protected set; }
        public ScryptaClient Scrypta { get; protected set; }
        public SecurityClient Security { get; protected set; }
        public ServiceClient Service { get; protected set; }
        public StellarClient Stellar { get; protected set; }
        public TronClient TRON { get; protected set; }
        public VeChainClient VeChain { get; protected set; }
        #endregion

        #region API Endpoints

        #region Version
        protected const int Endpoints_Version = 3;
        #endregion

        #region Blockchain - Shared
        protected const string Endpoints_Blockchain_GenerateWallet = "{0}/wallet";
        protected const string Endpoints_Blockchain_GenerateDepositAddress = "{0}/address/{1}/{2}";
        protected const string Endpoints_Blockchain_GenerateWalletPrivateKey = "{0}/wallet/priv";
        #endregion

        #endregion

        #region Constructor / Destructor
        /// <summary>
        /// Create a new instance of TatumClient using the default options
        /// </summary>
        public TatumClient() : this("", DefaultOptions)
        {
        }

        /// <summary>
        /// Create a new instance of TatumClient using the default options
        /// </summary>
        public TatumClient(string apiKey) : this(apiKey, DefaultOptions)
        {
        }

        /// <summary>
        /// Create a new instance of the TatumClient with the provided options
        /// </summary>
        public TatumClient(TatumClientOptions options) : this(options.ApiCredentials.Key.GetString(), options)
        {
        }

        /// <summary>
        /// Create a new instance of the TatumClient with the provided options
        /// </summary>
        public TatumClient(string apiKey, TatumClientOptions options) : base("Tatum", options, new TatumAuthenticationProvider(apiKey))
        {
            requestBodyFormat = RequestBodyFormat.Json;

            Binance = new BinanceClient(this);
            BitcoinCash = new BitcoinCashClient(this);
            Bitcoin = new BitcoinClient(this);
            Ethereum = new EthereumClient(this, authProvider);
            KMS = new KmsClient(this);
            Ledger = new LedgerClient(this);
            Libra = new LibraClient(this);
            Litecoin = new LitecoinClient(this);
            NEO = new NeoClient(this);
            OffChain = new OffChainClient(this);
            Records = new RecordsClient(this);
            Ripple = new RippleClient(this);
            Scrypta = new ScryptaClient(this);
            Security = new SecurityClient(this);
            Service = new ServiceClient(this);
            Stellar = new StellarClient(this);
            TRON = new TronClient(this);
            VeChain = new VeChainClient(this);
        }

        /// <summary>
        /// Sets the default options to use for new clients
        /// </summary>
        /// <param name="options">The options to use for new clients</param>
        public static void SetDefaultOptions(TatumClientOptions options)
        {
            defaultOptions = options;
        }

        /// <summary>
        /// Set API Key
        /// </summary>
        /// <param name="apiKey">The api key</param>
        public void SetApiCredentials(string apiKey)
        {
            SetAuthenticationProvider(new TatumAuthenticationProvider(apiKey));
        }
        #endregion

        #region Api Methods

        #region Blockchain / Shared (Bitcoin, BitcoinCash, Ethereum, Litecoin, Scrypta, VeChain)
        protected internal virtual WebCallResult<BlockchainWallet> Blockchain_GenerateWallet(BlockchainType chain, string mnemonics = null, CancellationToken ct = default) => Blockchain_GenerateWalletAsync(chain, new List<string> { mnemonics }, ct).Result;
        protected internal virtual WebCallResult<BlockchainWallet> Blockchain_GenerateWallet(BlockchainType chain, IEnumerable<string> mnemonics = null, CancellationToken ct = default) => Blockchain_GenerateWalletAsync(chain, mnemonics, ct).Result;
        protected internal virtual async Task<WebCallResult<BlockchainWallet>> Blockchain_GenerateWalletAsync(BlockchainType chain, string mnemonics = null, CancellationToken ct = default) => await Blockchain_GenerateWalletAsync(chain, new List<string> { mnemonics }, ct);
        protected internal virtual async Task<WebCallResult<BlockchainWallet>> Blockchain_GenerateWalletAsync(BlockchainType chain, IEnumerable<string> mnemonics = null, CancellationToken ct = default)
        {
            if (!chain.IsOneOf(
                BlockchainType.Bitcoin,
                BlockchainType.BitcoinCash,
                BlockchainType.Ethereum,
                BlockchainType.Litecoin,
                BlockchainType.Scrypta,
                BlockchainType.VeChain))
                throw new ArgumentException("Wrong BlockchainType");

            var credict = new Dictionary<BlockchainType, int>
            {
                { BlockchainType.Bitcoin, 1 },
                { BlockchainType.BitcoinCash, 5 },
                { BlockchainType.Ethereum, 1 },
                { BlockchainType.Litecoin, 5 },
                { BlockchainType.Scrypta, 1 },
                { BlockchainType.VeChain, 5 },
            };

            var credits = credict[chain];
            var parameters = new Dictionary<string, object>();
            if (mnemonics != null && mnemonics.Count() > 0) parameters.Add("mnemonic", string.Join(" ", mnemonics));

            var ops = chain.GetBlockchainOptions();
            var url = GetUrl(string.Format(Endpoints_Blockchain_GenerateWallet, ops.ChainSlug));
            return await SendTatumRequest<BlockchainWallet>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        protected internal virtual WebCallResult<TatumAddress> Blockchain_GenerateDepositAddress(BlockchainType chain, string xpub, int index, CancellationToken ct = default) => Blockchain_GenerateDepositAddressAsync(chain, xpub, index, ct).Result;
        protected internal virtual async Task<WebCallResult<TatumAddress>> Blockchain_GenerateDepositAddressAsync(BlockchainType chain, string xpub, int index, CancellationToken ct = default)
        {
            if (!chain.IsOneOf(
                BlockchainType.Bitcoin,
                BlockchainType.BitcoinCash,
                BlockchainType.Ethereum,
                BlockchainType.Litecoin,
                BlockchainType.Scrypta,
                BlockchainType.VeChain))
                throw new ArgumentException("Wrong BlockchainType");

            var credict = new Dictionary<BlockchainType, int>
            {
                { BlockchainType.Bitcoin, 1 },
                { BlockchainType.BitcoinCash, 5 },
                { BlockchainType.Ethereum, 1 },
                { BlockchainType.Litecoin, 5 },
                { BlockchainType.Scrypta, 1 },
                { BlockchainType.VeChain, 5 },
            };

            var credits = credict[chain];
            var ops = chain.GetBlockchainOptions();
            var url = GetUrl(string.Format(Endpoints_Blockchain_GenerateDepositAddress, ops.ChainSlug, xpub, index));

            if (chain == BlockchainType.Scrypta)
            {
                var result = await SendTatumRequest<string>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
                if (!result.Success) return WebCallResult<TatumAddress>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

                return new WebCallResult<TatumAddress>(result.ResponseStatusCode, result.ResponseHeaders, new TatumAddress { Address = result.Data }, null);
            }
            else
            {
                return await SendTatumRequest<TatumAddress>(url, HttpMethod.Get, ct, checkResult: false, signed: true, credits: credits).ConfigureAwait(false);
            }
        }

        protected internal virtual WebCallResult<TatumKey> Blockchain_GeneratePrivateKey(BlockchainType chain, string mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKeyAsync(chain, new List<string> { mnemonics }, index, ct).Result;
        protected internal virtual WebCallResult<TatumKey> Blockchain_GeneratePrivateKey(BlockchainType chain, IEnumerable<string> mnemonics, int index, CancellationToken ct = default) => Blockchain_GeneratePrivateKeyAsync(chain, mnemonics, index, ct).Result;
        protected internal virtual async Task<WebCallResult<TatumKey>> Blockchain_GeneratePrivateKeyAsync(BlockchainType chain, string mnemonics, int index, CancellationToken ct = default) => await Blockchain_GeneratePrivateKeyAsync(chain, new List<string> { mnemonics }, index, default);
        protected internal virtual async Task<WebCallResult<TatumKey>> Blockchain_GeneratePrivateKeyAsync(BlockchainType chain, IEnumerable<string> mnemonics, int index, CancellationToken ct = default)
        {
            if (!chain.IsOneOf(
                BlockchainType.Bitcoin,
                BlockchainType.BitcoinCash,
                BlockchainType.Ethereum,
                BlockchainType.Litecoin,
                BlockchainType.Scrypta,
                BlockchainType.VeChain))
                throw new ArgumentException("Wrong BlockchainType");

            var credict = new Dictionary<BlockchainType, int>
            {
                { BlockchainType.Bitcoin, 1 },
                { BlockchainType.Ethereum, 1 },
                { BlockchainType.BitcoinCash, 5 },
                { BlockchainType.Litecoin, 5 },
                { BlockchainType.Scrypta, 1 },
                { BlockchainType.VeChain, 5 },
            };

            var credits = credict[chain];
            var parameters = new Dictionary<string, object> {
                { "index", index },
                { "mnemonic", string.Join(" ", mnemonics) },
            };

            var ops = chain.GetBlockchainOptions();
            var url = GetUrl(string.Format(Endpoints_Blockchain_GenerateWalletPrivateKey, ops.ChainSlug));
            return await SendTatumRequest<TatumKey>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }
        #endregion

        #endregion

        #region Protected Methods
        protected internal virtual Uri GetUrl(string endpoint, int apiversion = Endpoints_Version)
        {
            return new Uri($"{BaseAddress.TrimEnd('/')}/v{apiversion}/{endpoint}");
        }

        protected internal virtual async Task<WebCallResult<T>> SendTatumRequest<T>(
            Uri uri,
            HttpMethod method,
            CancellationToken cancellationToken,
            Dictionary<string, object> parameters = null,
            bool signed = false,
            bool checkResult = true,
            HttpMethodParameterPosition? postPosition = null,
            ArrayParametersSerialization? arraySerialization = null,
            int credits = 1,
            JsonSerializer deserializer = null,
            Dictionary<string, string> additionalHeaders = null
            ) where T : class
        {
            return await SendRequestAsync<T>(uri, method, cancellationToken, parameters, signed, checkResult, postPosition, arraySerialization, credits, deserializer, additionalHeaders);
        }

        protected override Error ParseErrorResponse(JToken error)
        {
            return TatumParseErrorResponse(error);
        }

        protected virtual Error TatumParseErrorResponse(JToken error)
        {
            if (error["statusCode"] == null || error["message"] == null)
                return new TatumError(error.ToString());

            return new TatumError((int)error["statusCode"], (string)error["message"]);
        }

        #endregion

    }
}