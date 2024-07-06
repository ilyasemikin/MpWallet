using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Compiled.Abstractions;

namespace MpWallet.Expressions.Compiled.Implementations.Constants;

public sealed record Constant : CompiledExpression
{
    public Constant(Expression expression) 
        : base(expression)
    {
    }
}