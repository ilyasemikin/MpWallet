using System.Collections.Frozen;
using MpWallet.Currencies;
using MpWallet.ExchangeRates.Abstractions;
using MpWallet.ExchangeRates.Abstractions.Attributes;
using MpWallet.Results;

namespace MpWallet.ExchangeRates;

public class ExchangeRatesGetter
{
    private readonly FrozenDictionary<CurrencyRatio, IExchangeRateExternalSourceGetter> _getters;

    public ExchangeRatesGetter(IEnumerable<IExchangeRateExternalSourceGetter> getters)
    {
        ArgumentNullException.ThrowIfNull(getters);
        
        _getters = getters.ToFrozenDictionary(ExchangeRateExternalSourceGetterAttribute.Extract);
    }

    public async Task<Result<ExchangeRate, string>> GetAsync(CurrencyRatio ratio, CancellationToken cancellationToken)
    {
        if (!_getters.TryGetValue(ratio, out var getter))
            return Result<ExchangeRate, string>.Failure("Getter for ratio not found");

        try
        {
            var rateValue = await getter.GetAsync(cancellationToken);
            var rate = new ExchangeRate(ratio, rateValue);
            return Result<ExchangeRate, string>.Success(rate);
        }
        catch (Exception ex)
        {
            return Result<ExchangeRate, string>.Failure(ex.Message);
        }
    }
}