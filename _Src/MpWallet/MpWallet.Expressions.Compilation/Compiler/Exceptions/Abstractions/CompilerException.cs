namespace MpWallet.Expressions.Compilation.Compiler.Exceptions.Abstractions;

public abstract class CompilerException : Exception
{
    protected CompilerException(string? message = null, Exception? inner = null) 
        : base(message, inner)
    {
    }
}