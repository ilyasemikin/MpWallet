using MpWallet.Expressions.Comparers;

namespace MpWallet.Expressions.Context.Functions.Comparers;

public sealed class FunctionEqualityComparer : IEqualityComparer<Function>
{
    public static FunctionEqualityComparer Instance { get; } = new();
    
    public bool Equals(Function? x, Function? y)
    {
        if (x is null || y is null)
            return false;

        return x.Name == y.Name && 
               x.Parameters.Zip(y.Parameters).All(p => p.First == p.Second) &&
               ExpressionEqualityComparer.Instance.Equals(x.Expression, y.Expression);
    }

    public int GetHashCode(Function obj)
    {
        return obj.GetHashCode();
    }
}