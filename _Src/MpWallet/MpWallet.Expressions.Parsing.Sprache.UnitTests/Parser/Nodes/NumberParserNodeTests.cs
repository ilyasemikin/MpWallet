using MpWallet.Expressions.Parsing.Sprache.Parser.Implementations.Nodes;
using MpWallet.Expressions.Parsing.Sprache.UnitTests.Parser.Nodes.Abstractions;

namespace MpWallet.Expressions.Parsing.Sprache.UnitTests.Parser.Nodes;

public class NumberParserNodeTests : ParserNodeTests<NumberParserNode>
{
    protected override NumberParserNode CreateEmptyNode()
    {
        return new NumberParserNode();
    }
}