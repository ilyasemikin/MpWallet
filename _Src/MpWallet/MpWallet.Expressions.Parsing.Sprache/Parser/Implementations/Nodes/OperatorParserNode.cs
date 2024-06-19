using MpWallet.Expressions.Operators;
using MpWallet.Expressions.Parsing.Sprache.Parser.Implementations.Nodes.Abstractions;
using MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;
using Sprache;

namespace MpWallet.Expressions.Parsing.Sprache.Parser.Implementations.Nodes;

internal record OperatorParserNode : ParserNode, IPositionAware<OperatorParserNode>
{
    public Operator Value { get; }

    public OperatorParserNode(Operator value)
    {
        Value = value;
    }
    
    public override SyntaxNode ToSyntaxNode(string input)
    {
        throw new InvalidOperationException();
    }

    public override ParserNode SetPos(Position startPos, int length)
    {
        return new OperatorParserNode(Value)
        {
            Index = startPos.Pos,
            Length = length
        };
    }

    OperatorParserNode IPositionAware<OperatorParserNode>.SetPos(Position startPos, int length)
    {
        return new OperatorParserNode(Value)
        {
            Index = startPos.Pos,
            Length = length
        };
    }
}