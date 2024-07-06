namespace MpWallet.Operators;

public sealed class Operator : IEquatable<Operator>
{
    public string Value { get; }
    public OperatorDetails Details { get; }
    
    public Operator(string value, OperatorDetails details)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentNullException.ThrowIfNull(details);
        
        Value = value;
        Details = details;
    }

    public bool Equals(Operator? other)
    {
        if (ReferenceEquals(null, other)) 
            return false;
        
        if (ReferenceEquals(this, other)) 
            return true;
        
        return Value == other.Value && Details.Equals(other.Details);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Operator other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value, Details);
    }

    public static bool operator ==(Operator left, Operator right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Operator left, Operator right)
    {
        return !(left == right);
    }
}