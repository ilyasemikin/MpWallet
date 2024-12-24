using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MpWallet.ExchangeRates.Implementations;

namespace MpWallet.ExchangeRates.DependencyInjection.Microsoft;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExchangeRatesServices(this IServiceCollection services)
    {
        services.TryAddTransient<ExchangeRatesGetter>();
        return services;
    }
}
