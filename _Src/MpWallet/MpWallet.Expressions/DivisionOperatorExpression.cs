﻿using MpWallet.Currencies;
using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Context;
using MpWallet.Expressions.Extensions;
using MpWallet.Values.Abstractions;

namespace MpWallet.Expressions;

public sealed record DivisionOperatorExpression(Expression Numerator, Expression Denominator) : Expression
{
    public override Expression Calculate(ExpressionCalculationContext context, Currency currency)
    {
        var numerator = Numerator.Calculate(context, currency);
        var denominator = Denominator.Calculate(context, currency);

        if (numerator is ConstantExpression numeratorConstant && denominator is ConstantExpression denominatorConstant &&
            Value.TryDivide(numeratorConstant.Value, denominatorConstant.Value, out var value))
            return value.ToExpression();

        return new DivisionOperatorExpression(numerator, denominator);
    }
}
