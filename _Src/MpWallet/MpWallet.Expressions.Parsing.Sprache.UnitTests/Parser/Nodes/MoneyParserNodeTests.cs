using MpWallet.Expressions.Parsing.Sprache.Parser.Implementations.Nodes;
using MpWallet.Expressions.Parsing.Sprache.UnitTests.Parser.Nodes.Abstractions;

namespace MpWallet.Expressions.Parsing.Sprache.UnitTests.Parser.Nodes;

public sealed class MoneyParserNodeTests : ParserNodeTests<MoneyParserNode>
{
    protected override MoneyParserNode CreateEmptyNode()
    {
        return new MoneyParserNode();
    }
}