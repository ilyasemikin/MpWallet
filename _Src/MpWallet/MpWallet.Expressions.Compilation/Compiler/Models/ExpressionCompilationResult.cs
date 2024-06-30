using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Compilation.Compiler.Models.Abstractions;

namespace MpWallet.Expressions.Compilation.Compiler.Models;

public sealed record ExpressionCompilationResult(Expression Expression) : CompilationResult;