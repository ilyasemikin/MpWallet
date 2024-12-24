using MpWallet.Currencies;
using MpWallet.ExchangeRates.Abstractions;
using MpWallet.ExchangeRates.Extensions;

namespace MpWallet.Money;

public sealed class Money : IEquatable<Money>
{
    public decimal Amount { get; }
    public Currency Currency { get; }

    public Money(decimal amount, Currency currency)
    {
        ArgumentNullException.ThrowIfNull(currency);
        
        Amount = amount;
        Currency = currency;
    }

    public async Task<Money> AsAsync(
        IExchangeRatesGetter exchangeRatesGetter, Currency newCurrency, 
        CancellationToken cancellationToken = default)
    {
        if (newCurrency == Currency)
            return this;
        if (Amount == 0)
            return new Money(0, newCurrency);
        
        var ratio = new CurrencyRatio(Currency, newCurrency);
        var rate = await exchangeRatesGetter.GetAsync(ratio, cancellationToken).Unwrap();

        return new Money(Amount * rate.Value, newCurrency);
    }
    
    public bool Equals(Money? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;
        return Amount == other.Amount && (Amount == 0 || Currency.Equals(other.Currency));
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Money other && Equals(other);
    }
    
    public override string ToString()
    {
        return $"{Amount}{Currency}";
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Amount, Currency);
    }

    public static async Task<Money> AddAsync(
        Money left, Money right, 
        IExchangeRatesGetter exchangeRatesGetter, Currency currency, 
        CancellationToken cancellationToken = default)
    {
        left = await left.AsAsync(exchangeRatesGetter, currency, cancellationToken);
        right = await right.AsAsync(exchangeRatesGetter, currency, cancellationToken);

        return new Money(left.Amount + right.Amount, currency);
    }

    public static async Task<Money> SubtractAsync(
        Money left, Money right,
        IExchangeRatesGetter exchangeRatesGetter, Currency currency,
        CancellationToken cancellationToken = default)
    {
        left = await left.AsAsync(exchangeRatesGetter, currency, cancellationToken);
        right = await right.AsAsync(exchangeRatesGetter, currency, cancellationToken);
        
        return new Money(left.Amount - right.Amount, currency);
    }

    public static Money operator *(Money money, decimal multiplier)
    {
        return new Money(money.Amount * multiplier, money.Currency);
    }

    public static Money operator *(decimal multiplier, Money money)
    {
        return new Money(money.Amount * multiplier, money.Currency);
    }

    public static Money operator /(Money money, decimal divider)
    {
        return new Money(money.Amount / divider, money.Currency);
    }
}
