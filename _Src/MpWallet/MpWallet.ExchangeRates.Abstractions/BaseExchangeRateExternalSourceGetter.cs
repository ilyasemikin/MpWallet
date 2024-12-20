using MpWallet.Currencies;

namespace MpWallet.ExchangeRates.Abstractions;

public abstract class BaseExchangeRateExternalSourceGetter
{
    public CurrencyRatio ProvidedRatio { get; }

    protected BaseExchangeRateExternalSourceGetter(Currency antecedent, Currency consequent)
    {
        ArgumentNullException.ThrowIfNull(antecedent);
        ArgumentNullException.ThrowIfNull(consequent);
        
        ProvidedRatio = new CurrencyRatio(antecedent, consequent);
    }
    
    public abstract Task<decimal> GetAsync(CancellationToken cancellationToken = default);
}