namespace MpWallet.ExchangeRates.Abstractions;

public interface IExchangeRateExternalSourceGetter
{
    Task<decimal> GetAsync(CancellationToken cancellationToken = default);
}