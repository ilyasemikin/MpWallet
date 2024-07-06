namespace MpWallet.Operators.Comparers;

public class AbsoluteOperatorDetailsEqualityComparer : IEqualityComparer<OperatorDetails>
{
    public static AbsoluteOperatorDetailsEqualityComparer Instance { get; } = new();
    
    public bool Equals(OperatorDetails? x, OperatorDetails? y)
    {
        if (ReferenceEquals(x, y)) 
            return true;
        if (ReferenceEquals(x, null)) 
            return false;
        if (ReferenceEquals(y, null)) 
            return false;
        if (x.GetType() != y.GetType()) 
            return false;
        
        return x.Priority == y.Priority && x.Associativity == y.Associativity && x.Arity == y.Arity;
    }

    public int GetHashCode(OperatorDetails obj)
    {
        return HashCode.Combine(obj.Priority, obj.Associativity, obj.Arity);
    }
}