using MpWallet.Currencies;
using MpWallet.Expressions.Context;
using MpWallet.Values.Abstractions;

namespace MpWallet.Expressions.Abstractions;

public abstract record ConstantExpression : Expression
{
    public abstract Value Value { get; }

    protected internal ConstantExpression()
    {
    }
    
    public abstract Expression Negotiate();
    public abstract override Expression Calculate(ExpressionsContext context, Currency currency);
}