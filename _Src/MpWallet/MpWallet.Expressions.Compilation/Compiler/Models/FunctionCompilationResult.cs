using MpWallet.Expressions.Compilation.Compiler.Models.Abstractions;
using MpWallet.Expressions.Context.Functions;

namespace MpWallet.Expressions.Compilation.Compiler.Models;

public sealed record FunctionCompilationResult(Function Function) : CompilationResult;