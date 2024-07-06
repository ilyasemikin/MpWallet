using MpWallet.Expressions.Compiled.Abstractions;
using MpWallet.Expressions.Context;

namespace MpWallet.Expressions.Compilation.Compiler.Abstractions;

public interface IExpressionCompiler
{
    CompiledExpression Compile(string input, ExpressionsContext context);
}