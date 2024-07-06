using MpWallet.Currencies;
using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Abstractions.Constants;
using MpWallet.Expressions.Abstractions.Operators;
using MpWallet.Expressions.Context;

namespace MpWallet.Expressions.Implementations.Operators;

public sealed record NegationOperatorExpression(Expression Argument) : OperatorExpression
{
    public override Expression Calculate(ExpressionsContext context, Currency currency)
    {
        return Argument.Calculate(context, currency) switch
        {
            ConstantExpression constant => constant.Negotiate(),
            NegationOperatorExpression negation => negation.Argument,
            { } e => new NegationOperatorExpression(e)
        };
    }
}
