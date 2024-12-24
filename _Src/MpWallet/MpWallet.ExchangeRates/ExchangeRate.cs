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

    public ExchangeRate(Currency antecedent, Currency consequent, decimal value)
        : this(new CurrencyRatio(antecedent, consequent), value)
    {
    }
}