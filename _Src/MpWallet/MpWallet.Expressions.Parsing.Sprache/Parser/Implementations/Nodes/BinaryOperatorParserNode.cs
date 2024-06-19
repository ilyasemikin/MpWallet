using MpWallet.Expressions.Parsing.Sprache.Parser.Implementations.Nodes.Abstractions;
using MpWallet.Expressions.Parsing.Syntax.Nodes;
using MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;
using Sprache;

namespace MpWallet.Expressions.Parsing.Sprache.Parser.Implementations.Nodes;

internal record BinaryOperatorParserNode : ParserNode, IPositionAware<BinaryOperatorParserNode>
{
    public OperatorParserNode Operator { get; }
    public ParserNode Left { get; }
    public ParserNode Right { get; }
    
    public BinaryOperatorParserNode(OperatorParserNode @operator, ParserNode left, ParserNode right)
    {
        Operator = @operator;
        Left = left;
        Right = right;
    }
    
    public override SyntaxNode ToSyntaxNode(string input)
    {
        var token = Operator.ToToken(input);
        var left = Left.ToSyntaxNode(input);
        var right = Right.ToSyntaxNode(input);
        return new BinaryOperatorSyntaxNode(token, Operator.Value, left, right);
    }

    public override ParserNode SetPos(Position startPos, int length)
    {
        return new BinaryOperatorParserNode(Operator, Left, Right)
        {
            Index = startPos.Pos,
            Length = length
        };
    }

    BinaryOperatorParserNode IPositionAware<BinaryOperatorParserNode>.SetPos(Position startPos, int length)
    {
        return new BinaryOperatorParserNode(Operator, Left, Right)
        {
            Index = startPos.Pos,
            Length = length
        };
    }
}