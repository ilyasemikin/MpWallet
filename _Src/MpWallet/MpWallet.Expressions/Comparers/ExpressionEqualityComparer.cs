using MpWallet.Expressions.Abstractions;

namespace MpWallet.Expressions.Comparers;

public sealed class ExpressionEqualityComparer : IEqualityComparer<Expression>
{
    public static ExpressionEqualityComparer Instance { get; } = new();
    
    public bool Equals(Expression? x, Expression? y)
    {
        if (x is null || y is null)
            return false;

        if (x.GetType() != y.GetType())
            return false;

        return x switch
        {
            NumberExpression node => Equals(node, (NumberExpression)y),
            MoneyExpression node => Equals(node, (MoneyExpression)y),
            VariableExpression node => Equals(node, (VariableExpression)y),
            FunctionCallExpression node => Equals(node, (FunctionCallExpression)y),
            AdditionOperatorExpression node => Equals(node, (AdditionOperatorExpression)y),
            SubtractionOperationExpression node => Equals(node, (SubtractionOperationExpression)y),
            MultiplicationOperationExpression node => Equals(node, (MultiplicationOperationExpression)y),
            DivisionOperatorExpression node => Equals(node, (DivisionOperatorExpression)y),
            _ => throw new InvalidOperationException("Unknown expression type")
        };
    }

    public int GetHashCode(Expression obj)
    {
        return obj.GetHashCode();
    }

    private bool Equals(AdditionOperatorExpression x, AdditionOperatorExpression y)
    {
        return Equals(x.Addend, y.Addend) && Equals(x.Augend, y.Augend) ||
               Equals(x.Addend, y.Augend) && Equals(x.Augend, y.Addend);
    }

    private bool Equals(SubtractionOperationExpression x, SubtractionOperationExpression y)
    {
        return Equals(x.Minuend, y.Minuend) && Equals(x.Subtrahend, y.Subtrahend);
    }

    private bool Equals(MultiplicationOperationExpression x, MultiplicationOperationExpression y)
    {
        return Equals(x.Multiplier, y.Multiplier) && Equals(x.Multiplicand, y.Multiplicand) ||
               Equals(x.Multiplier, y.Multiplicand) && Equals(x.Multiplicand, y.Multiplier);
    }

    public bool Equals(DivisionOperatorExpression x, DivisionOperatorExpression y)
    {
        return Equals(x.Numerator, y.Numerator) && Equals(x.Denominator, y.Denominator);
    }
    
    private bool Equals(FunctionCallExpression x, FunctionCallExpression y)
    {
        return x.Name == y.Name &&
               x.Arguments.Count == y.Arguments.Count &&
               x.Arguments.Zip(y.Arguments).All(p => Equals(p.First, p.Second));
    }
    
    private static bool Equals(VariableExpression x, VariableExpression y)
    {
        return x.Name == y.Name;
    }
    
    private static bool Equals(MoneyExpression x, MoneyExpression y)
    {
        return x.Value == y.Value;
    }
    
    private static bool Equals(NumberExpression x, NumberExpression y)
    {
        return x.Value == y.Value;
    }
}