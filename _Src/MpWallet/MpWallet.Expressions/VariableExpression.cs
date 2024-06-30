using MpWallet.Currencies;
using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Context;
using MpWallet.Expressions.Exceptions;

namespace MpWallet.Expressions;

public sealed record VariableExpression(string Name) : Expression
{
    public override Expression Calculate(ExpressionsContext context, Currency currency)
    {
        if (!context.Variables.TryGet(Name, out var variable))
            throw new VariableNotFoundException(Name);

        return variable.Expression.Calculate(context, currency);
    }
}
