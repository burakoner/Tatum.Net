using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;
using System;
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
    public class RecordsClient : ITatumBlockchainRecordsClient
    {
        public TatumClient Tatum { get; protected set; }

        #region API Endpoints

        #region Blockchain - Records
        protected const string Endpoints_Log = "record";
        #endregion

        #endregion

        public RecordsClient(TatumClient tatumClient)
        {
            Tatum = tatumClient;
        }


        #region Blockchain / Records
        /// <summary>
        /// <b>Title:</b> Store log record<br />
        /// <b>Credits:</b> 2 credits per API call. Additional credits are debited based on the size of the data, which are being stored and type of blockchain.<br />
        /// <b>Description:</b>
        /// Store record data on blockchain. Tatum currently supports Ethereum blockchain.
        /// Total cost of the transaction(in credits) on Ethereum blockchain is dependent on the size of the data.Data are stored as a HEX string and maximum data size is approximatelly 130 kB on mainnet, 30 kB on testnet.
        /// Each 5 characters of the data costs 1 credit, so API call with data of length 1 kB = 1024 characters would cost 205 credits.
        /// </summary>
        /// <param name="chain">Blockchain, where to store log data.</param>
        /// <param name="data">Log data to be stored on a blockchain.</param>
        /// <param name="fromPrivateKey">Private key of account, from which the transaction will be initiated. If not present, transaction fee will be debited from Tatum internal account and additional credits will be charged.</param>
        /// <param name="to">Blockchain address to store log on. If not defined, it will be stored on an address, from which the transaction was being made.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<BlockchainResponse> SetData(BlockchainType chain, string data, string fromPrivateKey = null, string to = null, long? nonce = null, CancellationToken ct = default) => SetDataAsync(chain, data, fromPrivateKey, to, nonce, ct).Result;
        /// <summary>
        /// <b>Title:</b> Store log record<br />
        /// <b>Credits:</b> 2 credits per API call. Additional credits are debited based on the size of the data, which are being stored and type of blockchain.<br />
        /// <b>Description:</b>
        /// Store record data on blockchain. Tatum currently supports Ethereum blockchain.
        /// Total cost of the transaction(in credits) on Ethereum blockchain is dependent on the size of the data.Data are stored as a HEX string and maximum data size is approximatelly 130 kB on mainnet, 30 kB on testnet.
        /// Each 5 characters of the data costs 1 credit, so API call with data of length 1 kB = 1024 characters would cost 205 credits.
        /// </summary>
        /// <param name="chain">Blockchain, where to store log data.</param>
        /// <param name="data">Log data to be stored on a blockchain.</param>
        /// <param name="fromPrivateKey">Private key of account, from which the transaction will be initiated. If not present, transaction fee will be debited from Tatum internal account and additional credits will be charged.</param>
        /// <param name="to">Blockchain address to store log on. If not defined, it will be stored on an address, from which the transaction was being made.</param>
        /// <param name="nonce">Nonce to be set to Ethereum transaction. If not present, last known nonce will be used.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<BlockchainResponse>> SetDataAsync(BlockchainType chain, string data, string fromPrivateKey = null, string to = null, long? nonce = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "chain", JsonConvert.SerializeObject(chain, new BlockchainTypeConverter(false)) },
                { "data", data },
            };
            parameters.AddOptionalParameter("fromPrivateKey", fromPrivateKey);
            parameters.AddOptionalParameter("nonce", nonce);
            parameters.AddOptionalParameter("to", to);

            var credits = 2 + Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(data.Length) / 5));
            var url = Tatum.GetUrl(string.Format(Endpoints_Log));
            var result = await Tatum.SendTatumRequest<BlockchainResponse>(url, HttpMethod.Post, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
            if (!result.Success || result.Data.Failed) return WebCallResult<BlockchainResponse>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            return new WebCallResult<BlockchainResponse>(result.ResponseStatusCode, result.ResponseHeaders, result.Data, null);
        }

        /// <summary>
        /// <b>Title:</b> Get log record<br />
        /// <b>Credits:</b> 1 credits per API call.<br />
        /// <b>Description:</b>
        /// Get log data from Ethereum blockchain.
        /// </summary>
        /// <param name="chain">Blockchain, from which to get log record</param>
        /// <param name="id">ID of log record / transaction on blockchain</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<TatumData> GetData(BlockchainType chain, string id, CancellationToken ct = default) => GetDataAsync(chain, id, ct).Result;
        /// <summary>
        /// <b>Title:</b> Get log record<br />
        /// <b>Credits:</b> 1 credits per API call.<br />
        /// <b>Description:</b>
        /// Get log data from Ethereum blockchain.
        /// </summary>
        /// <param name="chain">Blockchain, from which to get log record</param>
        /// <param name="id">ID of log record / transaction on blockchain</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<TatumData>> GetDataAsync(BlockchainType chain, string id, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "chain", JsonConvert.SerializeObject(chain, new BlockchainTypeConverter(false)) },
                { "id", id },
            };
            var credits = 1;
            var url = Tatum.GetUrl(string.Format(Endpoints_Log));
            return await Tatum.SendTatumRequest<TatumData>(url, HttpMethod.Get, ct, checkResult: false, signed: true, parameters: parameters, credits: credits).ConfigureAwait(false);
        }

        #endregion

    }
}
