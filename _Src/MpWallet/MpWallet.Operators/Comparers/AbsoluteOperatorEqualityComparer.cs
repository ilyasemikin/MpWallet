namespace MpWallet.Operators.Comparers;

public class AbsoluteOperatorEqualityComparer : IEqualityComparer<Operator>
{
    public static AbsoluteOperatorEqualityComparer Instance { get; } = new();
    
    public bool Equals(Operator? x, Operator? y)
    {
        if (ReferenceEquals(x, y)) 
            return true;
        if (ReferenceEquals(x, null)) 
            return false;
        if (ReferenceEquals(y, null)) 
            return false;
        if (x.GetType() != y.GetType()) 
            return false;

        return x.Value == y.Value && AbsoluteOperatorDetailsEqualityComparer.Instance.Equals(x.Details, y.Details);
    }

    public int GetHashCode(Operator obj)
    {
        return HashCode.Combine(obj.Value, obj.Details);
    }
}