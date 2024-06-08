using MpWallet.Currencies;
using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Context;
using MpWallet.Values;
using MpWallet.Values.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MpWallet.Expressions;

public sealed record AdditionOperatorExpression(Expression Augend, Expression Addend) : Expression
{
    public override Expression Calculate(ExpressionCalculationContext context, Currency currency)
    {
        var augend = Augend.Calculate(context, currency);
        var addend = Addend.Calculate(context, currency);

        if (augend is ConstantExpression augendConstant && addend is ConstantExpression addendConstant && 
            Value.TryAdd(augendConstant.Value, addendConstant.Value, out var value))
            return new ConstantExpression(value);

        return augend + addend;
    }
}
