using MpWallet.Currencies;
using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Context;
using MpWallet.Expressions.Extensions;
using MpWallet.Values.Abstractions;

namespace MpWallet.Expressions;

public sealed record SubtractionOperationExpression(Expression Minuend, Expression Subtrahend) : Expression
{
    public override Expression Calculate(ExpressionCalculationContext context, Currency currency)
    {
        var minuend = Minuend.Calculate(context, currency);
        var subtrahend = Subtrahend.Calculate(context, currency);

        if (minuend is ConstantExpression minuendConstant && subtrahend is ConstantExpression subtrahendConstant &&
            Value.TrySubtract(minuendConstant.Value, subtrahendConstant.Value, out var value))
            return value.ToExpression();

        return new SubtractionOperationExpression(minuend, subtrahend);
    }
}
