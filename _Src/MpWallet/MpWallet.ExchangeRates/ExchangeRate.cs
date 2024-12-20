using MpWallet.Currencies;

namespace MpWallet.ExchangeRates;

public sealed class ExchangeRate
{
    public CurrencyRatio Ratio { get; }
    public decimal Value { get; }

    public ExchangeRate(CurrencyRatio ratio, decimal value)
    {
        ArgumentNullException.ThrowIfNull(ratio);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);
        
        Ratio = ratio;
        Value = value;
    }
}