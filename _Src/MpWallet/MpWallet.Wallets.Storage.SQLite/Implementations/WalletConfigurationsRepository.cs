using Microsoft.EntityFrameworkCore;
using MpWallet.Storage;
using MpWallet.Wallets.Configurations;
using MpWallet.Wallets.Configurations.Repositories;
using MpWallet.Wallets.Configurations.Repositories.Models;
using MpWallet.Wallets.Storage.SQLite.Implementations.Mappers;

namespace MpWallet.Wallets.Storage.SQLite.Implementations;

internal sealed class WalletConfigurationsRepository : IWalletConfigurationsRepository
{
    private readonly IDbContextFactory<DatabaseContext> _factory;

    public WalletConfigurationsRepository(IDbContextFactory<DatabaseContext> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        
        _factory = factory;
    }

    public async Task CreateAsync(WalletConfiguration configuration, CancellationToken cancellationToken)
    {
        await using var context = await _factory.CreateDbContextAsync(cancellationToken);

        var entity = configuration.MapToEntity();
        await context.WalletConfigurations.AddAsync(entity, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(WalletConfiguration configuration, CancellationToken cancellationToken)
    {
        await using var context = await _factory.CreateDbContextAsync(cancellationToken);
        
        var entity = configuration.MapToEntity();
        context.WalletConfigurations.Update(entity);
        
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(WalletIdentifier id, CancellationToken cancellationToken)
    {
        await using var context = await _factory.CreateDbContextAsync(cancellationToken);

        await context.WalletConfigurations
            .Where(e => e.Id == id.ToString())
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<RepositoryBatchResult<WalletConfiguration>> GetAsync(
        ListWalletConfigurationRequest request, CancellationToken cancellationToken)
    {
        await using var context = await _factory.CreateDbContextAsync(cancellationToken);

        var totalCount = await context.WalletConfigurations.CountAsync(cancellationToken);
        var queryable = context.WalletConfigurations
            .OrderBy(e => e.Id)
            .Skip(request.Offset)
            .Take(request.Limit);

        var count = await queryable.CountAsync(cancellationToken);
        var entities = queryable.AsAsyncEnumerable();

        return await RepositoryBatchResult<WalletConfiguration>.CreateAsync(
            totalCount, count, entities, 
            WalletConfigurationMapper.MapToDomain);
    }
}