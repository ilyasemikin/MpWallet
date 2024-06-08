using MpWallet.Currencies;
using MpWallet.Currencies.Services.Abstractions;
using MpWallet.Currencies.Services.Abstractions.Extensions;
using MpWallet.Values.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpWallet.Values;

public sealed record Money(decimal Value, Currency Currency) : Value
{
    public bool TryConvertCurrency(ICurrencyRatioProvider ratioProvider, Currency currency, [NotNullWhen(true)] out Money? result)
    {
        if (!ratioProvider.TryGet(Currency, currency, out var ratio))
        {
            result = null;
            return false;
        }

        result = new Money(ratio.Value * Value, currency);
        return true;
    }

    public override string ToString()
    {
        return $"{Value}{Currency.Symbol}";
    }

    public override string ToString(string? format, IFormatProvider? formatProvider)
    {
        formatProvider ??= CultureInfo.CurrentCulture;

        return Value.ToString(format, formatProvider) + Currency.Symbol;
    }
}
