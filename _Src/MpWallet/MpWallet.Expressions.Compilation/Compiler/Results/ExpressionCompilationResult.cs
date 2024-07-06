using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Compilation.Compiler.Results.Abstractions;

namespace MpWallet.Expressions.Compilation.Compiler.Results;

public sealed record ExpressionCompilationResult(Expression Expression) : CompilationResult;