using MpWallet.Expressions.Parsing.Sprache.Parser.Implementations.Nodes.Abstractions;
using MpWallet.Expressions.Parsing.Syntax.Nodes;
using MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;
using Sprache;

namespace MpWallet.Expressions.Parsing.Sprache.Parser.Implementations.Nodes;

public sealed record NumberParserNode : ParserNode, IPositionAware<NumberParserNode>
{
    internal NumberParserNode()
    {
    }
    
    public override SyntaxNode ToSyntaxNode(string input)
    {
        var token = ToToken(input);
        return new NumberSyntaxNode(token);
    }

    public override ParserNode SetPos(Position startPos, int length)
    {
        return new NumberParserNode
        {
            Index = startPos.Pos,
            Length = length
        };
    }

    NumberParserNode IPositionAware<NumberParserNode>.SetPos(Position startPos, int length)
    {
        return new NumberParserNode
        {
            Index = startPos.Pos,
            Length = length
        };
    }
}