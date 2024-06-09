using MpWallet.Expressions.Abstractions;
using MpWallet.Values.Abstractions;

namespace MpWallet.Expressions.Extensions;

public static class ValueExtensions
{
    public static Expression ToExpression(this Value value)
    {
        return new ConstantExpression(value);
    }
}