using MpWallet.Currencies;
using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Context;
using MpWallet.Values.Abstractions;
using MpWallet.Values;
using MpWallet.Values.Implementations;

namespace MpWallet.Expressions;

public record ConstantExpression(Value Value) : Expression
{
    public ConstantExpression Negotiate()
    {
        Value value = Value switch
        {
            Number number => new Number(-number.Value),
            Money money => new Money(-money.Value, money.Currency),
            _ => throw new ArgumentOutOfRangeException(nameof(Value))
        };

        return new ConstantExpression(value);
    }
    
    public override Expression Calculate(ExpressionCalculationContext context, Currency currency)
    {
        if (Value is Money money &&
            money.TryConvertCurrency(context.CurrencyRatioProvider, currency, out var converted))
            return new ConstantExpression(converted);

        return this;
    }
}
