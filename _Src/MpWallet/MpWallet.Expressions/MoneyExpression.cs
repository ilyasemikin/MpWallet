using MpWallet.Currencies;
using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Context;
using MpWallet.Values.Implementations;

namespace MpWallet.Expressions;

public sealed record MoneyExpression(Money Money) : ConstantExpression
{
    public override Money Value { get; } = Money;

    public override Expression Negotiate()
    {
        var value = new Money(-Money.Value, Money.Currency);
        return new MoneyExpression(value);
    }

    public override Expression Calculate(ExpressionCalculationContext context, Currency currency)
    {
        return Money.TryConvertCurrency(context.CurrencyRatioProvider, currency, out var value)
            ? new MoneyExpression(value)
            : this;
    }
}