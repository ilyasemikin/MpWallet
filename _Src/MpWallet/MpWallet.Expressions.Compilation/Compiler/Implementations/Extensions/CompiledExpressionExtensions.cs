using MpWallet.Expressions.Compiled.Abstractions;

namespace MpWallet.Expressions.Compilation.Compiler.Implementations.Extensions;

internal static class CompiledExpressionExtensions
{
    public static CompiledNode ToCompiledNode(this CompiledExpression expression)
    {
        return new CompiledNode(expression);
    }
}