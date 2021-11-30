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
        WebCallResult<IEnumerable<OffchainDepositAddress>> AssignAddressToAccount(string account, string address, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<OffchainDepositAddress>>> AssignAddressToAccountAsync(string account, string address, CancellationToken ct = default);
        WebCallResult<OffchainDepositAddressState> CheckAddress(BlockchainType chain, string address, int? index = null, CancellationToken ct = default);
        Task<WebCallResult<OffchainDepositAddressState>> CheckAddressAsync(BlockchainType chain, string address, int? index = null, CancellationToken ct = default);
        WebCallResult<OffchainDepositAddress> GenerateDepositAddress(string account, int? index = null, CancellationToken ct = default);
        Task<WebCallResult<OffchainDepositAddress>> GenerateDepositAddressAsync(string account, int? index = null, CancellationToken ct = default);
        WebCallResult<IEnumerable<OffchainDepositAddress>> GenerateMultipleDepositAddresses(IEnumerable<OffchainDepositAddressRequest> addresses, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<OffchainDepositAddress>>> GenerateMultipleDepositAddressesAsync(IEnumerable<OffchainDepositAddressRequest> addresses, CancellationToken ct = default);
        WebCallResult<IEnumerable<OffchainDepositAddress>> GetAllDepositAddresses(string account, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<OffchainDepositAddress>>> GetAllDepositAddressesAsync(string account, CancellationToken ct = default);
        WebCallResult<bool> RemoveAddressFromAccount(string account, string address, CancellationToken ct = default);
        Task<WebCallResult<bool>> RemoveAddressFromAccountAsync(string account, string address, CancellationToken ct = default);
    }
}
