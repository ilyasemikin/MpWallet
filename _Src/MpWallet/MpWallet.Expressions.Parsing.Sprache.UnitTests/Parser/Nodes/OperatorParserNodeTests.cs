using MpWallet.Expressions.Operators;
using MpWallet.Expressions.Operators.Collections.Extensions;
using MpWallet.Expressions.Parsing.Sprache.Parser.Implementations.Nodes;
using MpWallet.Expressions.Parsing.Sprache.UnitTests.Parser.Nodes.Abstractions;
using Xunit;

namespace MpWallet.Expressions.Parsing.Sprache.UnitTests.Parser.Nodes;

public class OperatorParserNodeTests : ParserNodeTests<OperatorParserNode>
{
    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenOperatorIsNull()
    {
        var exception = Record.Exception(() => new OperatorParserNode(null!));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal("value", ((ArgumentNullException)exception).ParamName);
    }
    
    protected override OperatorParserNode CreateEmptyNode()
    {
        var @operator = Operator.All.Get("+", OperatorArity.Binary);
        return new OperatorParserNode(@operator);
    }
}