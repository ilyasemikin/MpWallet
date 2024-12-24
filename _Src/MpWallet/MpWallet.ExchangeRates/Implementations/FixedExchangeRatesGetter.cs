using MpWallet.Currencies;
using MpWallet.ExchangeRates.Abstractions;
using MpWallet.Results;

namespace MpWallet.ExchangeRates.Implementations;

public class FixedExchangeRatesGetter : IExchangeRatesGetter
{
    public readonly IReadOnlyDictionary<CurrencyRatio, ExchangeRate> Rates;
    
    public FixedExchangeRatesGetter(params IEnumerable<ExchangeRate> rates)
    {
        Rates = rates.ToDictionary(rate => rate.Ratio);
    }
    
    public async Task<Result<ExchangeRate, string>> GetAsync(CurrencyRatio ratio, CancellationToken cancellationToken = default)
    {
        var result = Rates.TryGetValue(ratio, out var rate)
            ? Result<ExchangeRate, string>.Success(rate)
            : Result<ExchangeRate, string>.Failure($"Exchange rate for ratio `{ratio}` not found");
        
        return await Task.FromResult(result);
    }
}