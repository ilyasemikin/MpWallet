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

public sealed record DivisionOperatorExpression(Expression Numerator, Expression Denominator) : Expression
{
    public override Expression Calculate(ExpressionCalculationContext context, Currency currency)
    {
        var numerator = Numerator.Calculate(context, currency);
        var denominator = Denominator.Calculate(context, currency);

        if (numerator is ConstantExpression numeratorConstant && denominator is ConstantExpression denominatorConstant &&
            Value.TryDivide(numeratorConstant.Value, denominatorConstant.Value, out var value))
            return new ConstantExpression(value);

        return new DivisionOperatorExpression(numerator, denominator);
    }
}
