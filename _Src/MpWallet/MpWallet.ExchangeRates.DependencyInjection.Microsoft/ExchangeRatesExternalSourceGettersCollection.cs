using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MpWallet.Currencies;
using MpWallet.ExchangeRates.Abstractions;
using MpWallet.ExchangeRates.Abstractions.Attributes;

namespace MpWallet.ExchangeRates.DependencyInjection.Microsoft;

public sealed class ExchangeRatesExternalSourceGettersCollection
{
    private readonly IServiceCollection _services;
    private readonly HashSet<CurrencyRatio> _ratios;

    internal ExchangeRatesExternalSourceGettersCollection(IServiceCollection services)
    {
        _services = services;
        _ratios = new HashSet<CurrencyRatio>();
    }
    
    public ExchangeRatesExternalSourceGettersCollection Add<T>()
        where T : class, IExchangeRateExternalSourceGetter, new()
    {
        var ratio = ExchangeRateExternalSourceGetterAttribute.Extract<T>();
        if (!_ratios.Add(ratio))
            throw new InvalidOperationException($"Getter for ratio {ratio} already added");

        _services.TryAddTransient<IExchangeRateExternalSourceGetter, T>();
        
        return this;
    }
}