using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpWallet.Currencies.Services.Abstractions.Extensions;

public static class CurrencyRatioProviderExtensions
{
    public static bool TryGet(
        this ICurrencyRatioProvider currencyRatioProvider, 
        Currency antecedent, Currency consequent, 
        [NotNullWhen(true)] out decimal? value)
    {
        var ratio = new CurrencyRatio(antecedent, consequent);
        return currencyRatioProvider.TryGet(ratio, out value);
    }

    public static decimal Get(this ICurrencyRatioProvider currencyRatioProvider, CurrencyRatio ratio)
    {
        if (!currencyRatioProvider.TryGet(ratio, out var ratioValue))
            throw new InvalidOperationException();

        return ratioValue.Value;
    }

    public static decimal Get(this ICurrencyRatioProvider currencyRatioProvider, Currency antecedent, Currency consequent)
    {
        var ratio = new CurrencyRatio(antecedent, consequent);
        return Get(currencyRatioProvider, ratio);
    }
}
