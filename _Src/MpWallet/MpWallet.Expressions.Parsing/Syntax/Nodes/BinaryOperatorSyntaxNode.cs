using MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;

namespace MpWallet.Expressions.Parsing.Syntax.Nodes;

public sealed record BinaryOperatorSyntaxNode : SyntaxNode
{
    public SyntaxNode LeftOperand { get; }
    public SyntaxNode RightOperand { get; }
    
    public BinaryOperatorSyntaxNode(Token token, SyntaxNode left, SyntaxNode right) 
        : base(token)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        
        LeftOperand = left;
        RightOperand = right;
    }
}