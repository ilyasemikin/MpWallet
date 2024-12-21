using MpWallet.Currencies;

namespace MpWallet.ExchangeRates.Abstractions;

public abstract class BaseExchangeRateExternalSourceGetter
{
    public CurrencyRatio ProvidedRatio { get; }

    protected BaseExchangeRateExternalSourceGetter(CurrencyRatio providedRatio)
    {
        ArgumentNullException.ThrowIfNull(providedRatio);
        
        ProvidedRatio = providedRatio;
    }
    
    public abstract Task<decimal> GetAsync(CancellationToken cancellationToken = default);
}