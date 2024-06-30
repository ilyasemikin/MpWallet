using MpWallet.Currencies;
using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Context;
using MpWallet.Values.Implementations;

namespace MpWallet.Expressions;

public sealed record MoneyExpression : ConstantExpression
{
    public override Money Value { get; }

    public MoneyExpression(Money value)
    {
        Value = value;
    }

    public MoneyExpression(decimal value, Currency currency)
    {
        Value = new Money(value, currency);
    }
    
    public override Expression Negotiate()
    {
        var value = new Money(-Value.Value, Value.Currency);
        return new MoneyExpression(value);
    }

    public override Expression Calculate(ExpressionsContext context, Currency currency)
    {
        return Value.TryConvertCurrency(context.CurrencyRatioProvider, currency, out var value)
            ? new MoneyExpression(value)
            : this;
    }
}