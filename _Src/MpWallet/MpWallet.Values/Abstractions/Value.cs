using MpWallet.Currencies;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpWallet.Values.Abstractions;

public abstract record Value : IFormattable
{
    protected internal Value()
    {
    }

    public abstract string ToString(string? format, IFormatProvider? formatProvider);

    public static bool TryAdd(Value left, Value right, [NotNullWhen(true)] out Value? result)
    {
        var lMoney = left as Money;
        var rMoney = right as Money;

        var lNumber = left as Number;
        var rNumber = right as Number;

        result = null;
        if (lMoney is not null && rMoney is not null && lMoney.Currency == rMoney.Currency)
            result = new Money(lMoney.Value + rMoney.Value, lMoney.Currency);
        else if (lNumber is not null && rNumber is not null)
            result = new Number(lNumber.Value + rNumber.Value);

        return result is not null;
    }

    public static bool TrySubtract(Value left, Value right, [NotNullWhen(true)] out Value? result)
    {
        var lMoney = left as Money;
        var rMoney = right as Money;

        var lNumber = left as Number;
        var rNumber = right as Number;

        result = null;
        if (lMoney is not null && rMoney is not null && lMoney.Currency == rMoney.Currency)
            result = new Money(lMoney.Value - rMoney.Value, lMoney.Currency);
        else if (lNumber is not null && rNumber is not null)
            result = new Number(lNumber.Value - rNumber.Value);

        return result is not null;
    }

    public static bool TryMultiple(Value left, Value right, [NotNullWhen(true)] out Value? result)
    {
        var lMoney = left as Money;
        var rMoney = right as Money;

        var lNumber = left as Number;
        var rNumber = right as Number;

        result = null;
        if (lNumber is not null && rNumber is not null)
            result = new Number(lNumber.Value * rNumber.Value);
        else if (lMoney is not null && rNumber is not null)
            result = new Money(lMoney.Value * rNumber.Value, lMoney.Currency);
        else if (lNumber is not null && rMoney is not null)
            result = new Money(lNumber.Value * rMoney.Value, rMoney.Currency);

        return result is not null;
    }

    public static bool TryDivide(Value left, Value right, [NotNullWhen(true)] out Value? result)
    {
        var lMoney = left as Money;
        var rMoney = right as Money;

        var lNumber = left as Number;
        var rNumber = right as Number;

        result = null;
        if (lNumber is not null && rNumber is not null)
            result = new Number(lNumber.Value / rNumber.Value);
        else if (lMoney is not null && rNumber is not null)
            result = new Money(lMoney.Value / rNumber.Value, lMoney.Currency);
        else if (lNumber is not null && rMoney is not null)
            result = new Money(lNumber.Value / rMoney.Value, rMoney.Currency);

        return result is not null;
    }
}
