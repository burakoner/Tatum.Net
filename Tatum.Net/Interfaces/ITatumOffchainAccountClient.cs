using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tatum.Net.Enums;
using Tatum.Net.RestObjects;

namespace Tatum.Net.Interfaces
{
    public interface ITatumOffchainAccountClient
    {
        WebCallResult<IEnumerable<OffchainDepositAddress>> OffchainAccount_AssignAddressToAccount(string account, string address, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<OffchainDepositAddress>>> OffchainAccount_AssignAddressToAccount_Async(string account, string address, CancellationToken ct = default);
        WebCallResult<OffchainDepositAddressState> OffchainAccount_CheckAddress(BlockchainType chain, string address, int? index = null, CancellationToken ct = default);
        Task<WebCallResult<OffchainDepositAddressState>> OffchainAccount_CheckAddress_Async(BlockchainType chain, string address, int? index = null, CancellationToken ct = default);
        WebCallResult<OffchainDepositAddress> OffchainAccount_GenerateDepositAddress(string account, int? index = null, CancellationToken ct = default);
        Task<WebCallResult<OffchainDepositAddress>> OffchainAccount_GenerateDepositAddress_Async(string account, int? index = null, CancellationToken ct = default);
        WebCallResult<IEnumerable<OffchainDepositAddress>> OffchainAccount_GenerateMultipleDepositAddresses(IEnumerable<OffchainDepositAddressRequest> addresses, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<OffchainDepositAddress>>> OffchainAccount_GenerateMultipleDepositAddresses_Async(IEnumerable<OffchainDepositAddressRequest> addresses, CancellationToken ct = default);
        WebCallResult<IEnumerable<OffchainDepositAddress>> OffchainAccount_GetAllDepositAddresses(string account, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<OffchainDepositAddress>>> OffchainAccount_GetAllDepositAddresses_Async(string account, CancellationToken ct = default);
        WebCallResult<bool> OffchainAccount_RemoveAddressFromAccount(string account, string address, CancellationToken ct = default);
        Task<WebCallResult<bool>> OffchainAccount_RemoveAddressFromAccount_Async(string account, string address, CancellationToken ct = default);
    }
}
