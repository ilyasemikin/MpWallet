using MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;

namespace MpWallet.Expressions.Parsing.Syntax.Nodes.Services;

public class SyntaxNodeAbsoluteEqualityComparer : IEqualityComparer<SyntaxNode>
{
    public static SyntaxNodeAbsoluteEqualityComparer Instance { get; } = new();
    
    public bool Equals(SyntaxNode? x, SyntaxNode? y)
    {
        if (x == null || y == null)
            return false;

        if (x.GetType() != y.GetType())
            return false;

        return x switch
        {
            BinaryOperatorSyntaxNode node => Equals(node, (BinaryOperatorSyntaxNode)y),
            FunctionSyntaxNode node => Equals(node, (FunctionSyntaxNode)y),
            MoneySyntaxNode node => Equals(node, (MoneySyntaxNode)y),
            NumberSyntaxNode node => Equals(node, (NumberSyntaxNode)y),
            VariableSyntaxNode node => Equals(node, (VariableSyntaxNode)y),
            _ => throw new InvalidOperationException($"Unknown type of SyntaxNode \"{x.GetType().Name}\"")
        };
    }

    public int GetHashCode(SyntaxNode obj)
    {
        return obj.GetHashCode();
    }

    private bool Equals(BinaryOperatorSyntaxNode x, BinaryOperatorSyntaxNode y)
    {
        return x.Token == y.Token &&
               x.Operator == y.Operator &&
               Equals(x.LeftOperand, y.LeftOperand) &&
               Equals(x.RightOperand, y.RightOperand);
    }

    private bool Equals(FunctionSyntaxNode x, FunctionSyntaxNode y)
    {
        return x.Token == y.Token &&
               x.Name == y.Name &&
               x.Arguments.Zip(y.Arguments).All(pair => Equals(pair.First, pair.Second));
    }

    private static bool Equals(MoneySyntaxNode x, MoneySyntaxNode y)
    {
        return x.Token == y.Token && x.Value == y.Value;
    }

    private static bool Equals(NumberSyntaxNode x, NumberSyntaxNode y)
    {
        return x.Token == y.Token && x.Value == y.Value;
    }

    private static bool Equals(VariableSyntaxNode x, VariableSyntaxNode y)
    {
        return x.Token == y.Token && x.Name == y.Name;
    }
}