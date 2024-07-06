using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Compiled.Abstractions;
using MpWallet.Expressions.Compiled.Implementations.Constants;

namespace MpWallet.Expressions.Compilation.Compiler.Implementations;

internal class CompiledNode
{
    private readonly Expression? _expression;
    private readonly CompiledExpression? _compiledExpression;

    public Expression Expression => _expression ?? throw new InvalidOperationException();
    public CompiledExpression CompiledExpression => _compiledExpression ?? new Constant(_expression!);

    public CompiledNode(Expression expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        
        _expression = expression;
        _compiledExpression = null;
    }

    public CompiledNode(CompiledExpression expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        
        _expression = null;
        _compiledExpression = expression;
    }
}