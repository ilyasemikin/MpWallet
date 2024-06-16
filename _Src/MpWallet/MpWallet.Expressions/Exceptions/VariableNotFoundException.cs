namespace MpWallet.Expressions.Exceptions;

public sealed class VariableNotFoundException : Exception
{
    public string VariableName { get; }

    public VariableNotFoundException(string name)
        : base($"Variable \"{name}\" not found")
    {
        VariableName = name;
    }
}
