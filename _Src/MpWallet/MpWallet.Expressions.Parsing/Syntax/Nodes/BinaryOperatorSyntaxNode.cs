using MpWallet.Expressions.Operators;
using MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;

namespace MpWallet.Expressions.Parsing.Syntax.Nodes;

public sealed record BinaryOperatorSyntaxNode : SyntaxNode
{
    public Operator Operator { get; }
    public SyntaxNode LeftOperand { get; }
    public SyntaxNode RightOperand { get; }
    
    public BinaryOperatorSyntaxNode(Token token, Operator @operator, SyntaxNode left, SyntaxNode right) 
        : base(token)
    {
        ArgumentNullException.ThrowIfNull(@operator);
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        Operator = @operator;
        LeftOperand = left;
        RightOperand = right;
    }
}