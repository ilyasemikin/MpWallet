using MpWallet.Expressions.Abstractions;

namespace MpWallet.Expressions.Compiled.Abstractions;

public abstract record CompiledExpression
{
    public Expression Expression { get; }

    protected internal CompiledExpression(Expression expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        
        Expression = expression;
    }
}