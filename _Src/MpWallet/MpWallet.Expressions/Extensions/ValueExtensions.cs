using MpWallet.Expressions.Abstractions;
using MpWallet.Values.Abstractions;
using MpWallet.Values.Implementations;

namespace MpWallet.Expressions.Extensions;

public static class ValueExtensions
{
    public static Expression ToExpression(this Value value)
    {
        return value switch
        {
            Number number => new NumberExpression(number),
            Money money => new MoneyExpression(money),
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    }
}