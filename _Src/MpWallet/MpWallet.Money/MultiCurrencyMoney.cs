using MpWallet.Currencies;
using MpWallet.ExchangeRates.Abstractions;
using MpWallet.Money.Abstractions;

namespace MpWallet.Money;

public sealed class MultiCurrencyMoney : IMoney
{
    public IReadOnlyList<Money> Parts { get; }
    
    public MultiCurrencyMoney(IEnumerable<Money> money)
    {
        ArgumentNullException.ThrowIfNull(money);
        
        Parts = CreateParts(money);
    }

    public async Task<Money> AsAsync(
        IExchangeRatesGetter exchangeRatesGetter, Currency newCurrency,
        CancellationToken cancellationToken = default)
    {
        if (Parts.Count == 0)
            return new Money(0, newCurrency);
        
        var result = Parts[0];

        for (var i = 1; i < Parts.Count; i++)
            result = await Money.AddAsync(result, Parts[i], exchangeRatesGetter, newCurrency, cancellationToken);

        return result;
    }

    public override string ToString()
    {
        var joined = string.Join(" + ", Parts);
        return $"{{{joined}}}";
    }

    private static Money[] CreateParts(IEnumerable<Money> money)
    {
        var amounts = new Dictionary<Currency, decimal>();
        foreach (var item in money)
        {
            if (!amounts.TryAdd(item.Currency, item.Amount))
                amounts[item.Currency] += item.Amount;
        }

        var array = new Money[amounts.Count];
        var i = 0;
        foreach (var (currency, amount) in amounts.OrderBy(p => p.Key.Code))
            array[i++] = new Money(amount, currency);

        return array;
    }
}