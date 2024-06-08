using MpWallet.Currencies;
using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Context;
using MpWallet.Values;
using MpWallet.Values.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpWallet.Expressions;

public sealed record NegationOperatorExpression(Expression Argument) : Expression
{
    public override Expression Calculate(ExpressionCalculationContext context, Currency currency)
    {
        return Argument.Calculate(context, currency) switch
        {
            ConstantExpression constant => GetNegationConstant(constant.Value),
            { } e => e
        };
    }

    private static ConstantExpression GetNegationConstant(Value value)
    {
        Value negationValue = value switch
        {
            Number number => new Number(-number.Value),
            Money money => new Money(-money.Value, money.Currency),
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };

        return new ConstantExpression(negationValue);
    }
}
