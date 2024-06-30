using MpWallet.Expressions.Compilation.Compiler.Models.Abstractions;
using MpWallet.Expressions.Context;

namespace MpWallet.Expressions.Compilation.Compiler.Abstractions;

public interface IExpressionCompiler
{
    CompilationResult Compile(string input, ExpressionsContext context);
}