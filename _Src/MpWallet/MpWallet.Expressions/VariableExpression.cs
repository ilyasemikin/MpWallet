using MpWallet.Currencies;
using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Context;
using MpWallet.Expressions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpWallet.Expressions;

public sealed record VariableExpression(string Name) : Expression
{
    public override Expression Calculate(ExpressionCalculationContext context, Currency currency)
    {
        if (!context.Variables.TryGet(Name, out var variable))
            throw new VariableNotFoundException(Name);

        return variable.Expression.Calculate(context, currency);
    }
}
