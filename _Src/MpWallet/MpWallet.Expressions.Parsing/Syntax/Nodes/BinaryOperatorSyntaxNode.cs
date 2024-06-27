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
        
        if (@operator.Value != token.Value)
            throw new InvalidOperationException("Operator value and token value must equals");

        if (@operator.Details.Arity is not OperatorArity.Binary)
            throw new ArgumentException("Operator must be binary", nameof(@operator));

        if (token.Input != left.Token.Input || left.Token.Input != right.Token.Input)
            throw new InvalidOperationException("Inputs must be equals");

        if (left.Token.End >= token.Begin)
            throw new ArgumentException("Left operator must be on left", nameof(left));
        if (right.Token.Begin <= token.End)
            throw new ArgumentException("Right operator must be on right", nameof(right));
        
        Operator = @operator;
        LeftOperand = left;
        RightOperand = right;
    }
}