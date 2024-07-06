using MpWallet.Expressions.Compilation.Compiler.Results.Abstractions;
using MpWallet.Expressions.Context.Functions;

namespace MpWallet.Expressions.Compilation.Compiler.Results;

public sealed record FunctionCompilationResult(FunctionExpression Function) : CompilationResult;