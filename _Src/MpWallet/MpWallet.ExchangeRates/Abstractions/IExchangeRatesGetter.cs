using MpWallet.Currencies;
using MpWallet.Results;

namespace MpWallet.ExchangeRates.Abstractions;

public interface IExchangeRatesGetter
{
    Task<Result<ExchangeRate, string>> GetAsync(CurrencyRatio ratio, CancellationToken cancellationToken = default);
}