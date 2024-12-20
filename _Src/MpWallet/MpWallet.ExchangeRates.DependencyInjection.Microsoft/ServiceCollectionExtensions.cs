using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MpWallet.ExchangeRates.DependencyInjection.Microsoft;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExchangeRatesServices(
        this IServiceCollection services, 
        Action<ExchangeRatesExternalSourceGettersCollection> gettersFiller)
    {
        services.TryAddTransient<ExchangeRatesGetter>();
        
        var collection = new ExchangeRatesExternalSourceGettersCollection(services);
        gettersFiller.Invoke(collection);
        
        return services;
    }
}
