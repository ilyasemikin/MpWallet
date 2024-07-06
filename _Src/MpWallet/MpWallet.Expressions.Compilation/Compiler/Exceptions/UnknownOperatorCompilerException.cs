using MpWallet.Expressions.Compilation.Compiler.Exceptions.Abstractions;
using MpWallet.Operators;

namespace MpWallet.Expressions.Compilation.Compiler.Exceptions;

public class UnknownOperatorCompilerException : CompilerException
{
    public Operator Operator { get; }

    public UnknownOperatorCompilerException(Operator @operator)
        : base($"Unknown operator \"{@operator.Value}\" with arity \"{@operator.Details.Arity}\"")
    {
        Operator = @operator;
    }
}