namespace MpWallet.Expressions.Operators;

public sealed record OperatorDetails
{
    public int Priority { get; }
    public OperatorAssociativity Associativity { get; }
    public OperatorArity Arity { get; }

    public OperatorDetails(int priority, OperatorAssociativity associativity, OperatorArity arity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(priority);

        Priority = priority;
        Associativity = associativity;
        Arity = arity;
    }
}