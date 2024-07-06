using MpWallet.Currencies;
using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Abstractions.Constants;
using MpWallet.Expressions.Abstractions.Operators;
using MpWallet.Expressions.Context;
using MpWallet.Expressions.Extensions;
using MpWallet.Values.Abstractions;

namespace MpWallet.Expressions.Implementations.Operators;

public sealed record AdditionOperatorExpression(Expression Augend, Expression Addend) : OperatorExpression
{
    public override Expression Calculate(ExpressionsContext context, Currency currency)
    {
        var augend = Augend.Calculate(context, currency);
        var addend = Addend.Calculate(context, currency);

        if (augend is ConstantExpression augendConstant && addend is ConstantExpression addendConstant && 
            Value.TryAdd(augendConstant.Value, addendConstant.Value, out var value))
            return value.ToExpression();

        return augend + addend;
    }
}
