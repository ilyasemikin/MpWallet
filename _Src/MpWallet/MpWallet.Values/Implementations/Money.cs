using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;
using MpWallet.Currencies;
using MpWallet.Currencies.Extensions;
using MpWallet.Currencies.Services.Abstractions;
using MpWallet.Currencies.Services.Abstractions.Extensions;
using MpWallet.Values.Abstractions;

namespace MpWallet.Values.Implementations;

public sealed record Money(decimal Value, Currency Currency) : Value
{
    public static Regex PatternRegex { get; }

    public static decimal Min => decimal.MinValue;
    public static decimal Max => decimal.MaxValue;

    static Money()
    {
        var symbols = Currency.All.Select(currency => Regex.Escape(currency.Symbol));
        var codes = Currency.All.Select(currency => Regex.Escape(currency.Code));
        var currencies = string.Join("|", codes.Concat(symbols));

        PatternRegex = new Regex($@"(?<VALUE>-?\d+([\.,]\d+)?)\s*(?<CURRENCY>{currencies})", RegexOptions.Compiled);
    }
    
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
        return ToString(null, null);
    }

    public override string ToString(string? format, IFormatProvider? formatProvider)
    {
        formatProvider ??= CultureInfo.CurrentCulture;

        return Value.ToString(format, formatProvider) + Currency.Symbol;
    }

    public static bool TryParse(string input, [NotNullWhen(true)] out Money? value)
    {
        return TryParse(input, null, out value);
    }
    
    public static bool TryParse(string input, IFormatProvider? formatProvider, [NotNullWhen(true)] out Money? value)
    {
        formatProvider ??= CultureInfo.InvariantCulture;
        
        var match = PatternRegex.Match(input);
        if (!match.Success || match.Length != input.Length)
        {
            value = null;
            return false;
        }
        
        var numberString = match.Groups["VALUE"].Value;
        var currencyString = match.Groups["CURRENCY"].Value;
        
        // decimal.TryParse с InvariantCulture запятую просто игнорирует, для "1,1" возвращает 11
        if (Equals(formatProvider, CultureInfo.InvariantCulture) && numberString.Contains(',') ||
            !decimal.TryParse(numberString, formatProvider, out var number) || 
            !Currency.All.TryGet(currencyString, out var currency))
        {
            value = null;
            return false;
        }

        value = new Money(number, currency);
        return true;
    }
}
