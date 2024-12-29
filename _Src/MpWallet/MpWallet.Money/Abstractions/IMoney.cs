using MpWallet.Currencies;
using MpWallet.ExchangeRates.Abstractions;

namespace MpWallet.Money.Abstractions;

public interface IMoney
{
    Task<Money> AsAsync(
        IExchangeRatesGetter exchangeRatesGetter, Currency newCurrency,
        CancellationToken cancellationToken = default);
}