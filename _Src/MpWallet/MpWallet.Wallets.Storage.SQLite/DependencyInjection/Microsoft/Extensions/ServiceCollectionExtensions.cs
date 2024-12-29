using Microsoft.Extensions.DependencyInjection;
using MpWallet.Wallets.Storage.SQLite.Implementations;

namespace MpWallet.Wallets.Storage.SQLite.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSqliteStorage(this IServiceCollection services)
    {
        var context = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(DatabaseContext));
        if (context is not null)
            return services;

        return services
            .AddDbContextFactory<DatabaseContext>()
            .AddSingleton<WalletConfigurationsRepository>();
    }
}