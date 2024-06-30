﻿using MpWallet.Currencies;
using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Context;
using MpWallet.Expressions.Extensions;
using MpWallet.Values.Abstractions;

namespace MpWallet.Expressions;

public sealed record MultiplicationOperationExpression(Expression Multiplier, Expression Multiplicand) : Expression
{
    public override Expression Calculate(ExpressionsContext context, Currency currency)
    {
        var multiplier = Multiplier.Calculate(context, currency);
        var multiplicand = Multiplicand.Calculate(context, currency);

        if (multiplier is ConstantExpression multiplierConstant && multiplicand is ConstantExpression multiplicandConstant &&
            Value.TryMultiple(multiplierConstant.Value, multiplicandConstant.Value, out var value))
            return value.ToExpression();

        return new MultiplicationOperationExpression(multiplier, multiplicand);
    }
}
