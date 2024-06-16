using MpWallet.Currencies;
using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Context;
using MpWallet.Values.Abstractions;
using MpWallet.Values.Implementations;

namespace MpWallet.Expressions;

public record NumberExpression(Number Number) : ConstantExpression
{
    public override Number Value { get; } = Number;

    public override Expression Negotiate()
    {
        var value = new Number(-Number.Value);
        return new NumberExpression(value);
    }

    public override Expression Calculate(ExpressionCalculationContext context, Currency currency)
    {
        return this;
    }
}