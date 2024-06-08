using MpWallet.Currencies;
using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Context;
using MpWallet.Values.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpWallet.Expressions;

public sealed record SubtractionOperationExpression(Expression Minuend, Expression Subtrahend) : Expression
{
    public override Expression Calculate(ExpressionCalculationContext context, Currency currency)
    {
        var minuend = Minuend.Calculate(context, currency);
        var subtrahend = Subtrahend.Calculate(context, currency);

        if (minuend is ConstantExpression minuendConstant && subtrahend is ConstantExpression subtrahendConstant &&
            Value.TrySubtract(minuendConstant.Value, subtrahendConstant.Value, out var value))
            return new ConstantExpression(value);

        return new SubtractionOperationExpression(minuend, subtrahend);
    }
}
