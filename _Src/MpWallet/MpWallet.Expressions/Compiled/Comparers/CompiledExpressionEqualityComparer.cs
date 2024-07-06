using MpWallet.Expressions.Comparers;
using MpWallet.Expressions.Compiled.Abstractions;
using MpWallet.Expressions.Compiled.Implementations.Constants;
using MpWallet.Expressions.Compiled.Implementations.Functions;
using MpWallet.Expressions.Compiled.Implementations.Variables;

namespace MpWallet.Expressions.Compiled.Comparers;

public sealed class CompiledExpressionEqualityComparer : IEqualityComparer<CompiledExpression>
{
    public static CompiledExpressionEqualityComparer Instance { get; } = new();

    public bool Equals(CompiledExpression? x, CompiledExpression? y)
    {
        if (ReferenceEquals(x, y)) 
            return true;
        if (ReferenceEquals(x, null)) 
            return false;
        if (ReferenceEquals(y, null)) 
            return false;
        if (x.GetType() != y.GetType()) 
            return false;

        return x switch
        {
            Constant constant => Equals(constant, (Constant)y),
            Variable variable => Equals(variable, (Variable)y),
            Function function => Equals(function, (Function)y),
            _ => throw new InvalidOperationException($"Unknown type \"{x.GetType().Name}\"")
        };
    }

    public int GetHashCode(CompiledExpression obj)
    {
        return obj.GetHashCode();
    }

    private static bool Equals(Constant x, Constant y)
    {
        return ExpressionEqualityComparer.Instance.Equals(x.Expression, y.Expression);
    }

    private static bool Equals(Variable x, Variable y)
    {
        return x.Name == y.Name && ExpressionEqualityComparer.Instance.Equals(x.Expression, y.Expression);
    }

    private static bool Equals(Function x, Function y)
    {
        return x.Name == y.Name &&
               ExpressionEqualityComparer.Instance.Equals(x.Expression, y.Expression) &&
               x.Parameters.Zip(y.Parameters).All(p => p.First == p.Second);
    }
}