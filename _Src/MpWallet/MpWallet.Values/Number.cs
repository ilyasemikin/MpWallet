using MpWallet.Values.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MpWallet.Values;

public sealed record Number(decimal Value) : Value
{
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