using MpWallet.Expressions.Parsing.Sprache.Parser.Implementations.Nodes.Abstractions;
using MpWallet.Expressions.Parsing.Syntax.Nodes;
using MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;
using Sprache;

namespace MpWallet.Expressions.Parsing.Sprache.Parser.Implementations.Nodes;

public sealed record MoneyParserNode : ParserNode, IPositionAware<MoneyParserNode>
{
    internal MoneyParserNode()
    {
    }
    
    public override SyntaxNode ToSyntaxNode(string input)
    {
        var token = ToToken(input);
        return new MoneySyntaxNode(token);
    }

    public override ParserNode SetPos(Position startPos, int length)
    {
        return new MoneyParserNode
        {
            Index = startPos.Pos,
            Length = length
        };
    }

    MoneyParserNode IPositionAware<MoneyParserNode>.SetPos(Position startPos, int length)
    {
        return new MoneyParserNode
        {
            Index = startPos.Pos,
            Length = length
        };
    }
}