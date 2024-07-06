using MpWallet.Expressions.Abstractions;

namespace MpWallet.Expressions.Compilation.Compiler.Implementations.Extensions;

internal static class ExpressionExtensions
{
    public static CompiledNode ToCompiledNode(this Expression expression)
    {
        return new CompiledNode(expression);
    }
}