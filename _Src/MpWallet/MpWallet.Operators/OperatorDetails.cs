namespace MpWallet.Operators;

public sealed class OperatorDetails : IEquatable<OperatorDetails>
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

    public bool Equals(OperatorDetails? other)
    {
        if (ReferenceEquals(null, other)) 
            return false;
        
        if (ReferenceEquals(this, other)) 
            return true;
        
        return Associativity == other.Associativity && Arity == other.Arity;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is OperatorDetails other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Associativity, Arity);
    }

    public static bool operator ==(OperatorDetails left, OperatorDetails right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(OperatorDetails left, OperatorDetails right)
    {
        return !(left == right);
    }
}