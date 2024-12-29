using MpWallet.Storage;
using MpWallet.Wallets.Configurations.Repositories.Models;

namespace MpWallet.Wallets.Configurations.Repositories;

public interface IWalletConfigurationsRepository
{
    Task CreateAsync(WalletConfiguration configuration, CancellationToken cancellationToken = default);
    Task UpdateAsync(WalletConfiguration configuration, CancellationToken cancellationToken = default);
    Task DeleteAsync(WalletIdentifier id, CancellationToken cancellationToken = default);

    Task<RepositoryBatchResult<WalletConfiguration>> GetAsync(
        ListWalletConfigurationRequest request, 
        CancellationToken cancellationToken = default);
}