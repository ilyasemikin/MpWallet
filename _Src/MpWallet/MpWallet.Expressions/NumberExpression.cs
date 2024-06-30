using MpWallet.Currencies;
using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Context;
using MpWallet.Values.Implementations;

namespace MpWallet.Expressions;

public record NumberExpression : ConstantExpression
{
    public override Number Value { get; }

    public NumberExpression(Number value)
    {
        Value = value;
    }

    public NumberExpression(decimal value)
    {
        Value = new Number(value);
    }

    public override Expression Negotiate()
    {
        var value = new Number(-Value.Value);
        return new NumberExpression(value);
    }

    public override Expression Calculate(ExpressionsContext context, Currency currency)
    {
        return this;
    }
}