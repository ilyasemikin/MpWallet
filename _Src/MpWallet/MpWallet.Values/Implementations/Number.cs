using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using MpWallet.Values.Abstractions;

namespace MpWallet.Values.Implementations;

public sealed record Number(decimal Value) : Value
{
    public static decimal Min => decimal.MinValue;
    public static decimal Max => decimal.MaxValue;
    
    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }
    
    public override string ToString(string? format, IFormatProvider? formatProvider)
    {
        return Value.ToString(format, formatProvider);
    }

    public static bool TryParse(ReadOnlySpan<char> input, [NotNullWhen(true)] out Number? number)
    {
        return TryParse(input, CultureInfo.InvariantCulture, out number);
    }

    public static bool TryParse(ReadOnlySpan<char> input, IFormatProvider? format, [NotNullWhen(true)] out Number? number)
    {
        if (!decimal.TryParse(input, format, out var value))
        {
            number = null;
            return false;
        }

        number = new Number(value);
        return true;
    }
}