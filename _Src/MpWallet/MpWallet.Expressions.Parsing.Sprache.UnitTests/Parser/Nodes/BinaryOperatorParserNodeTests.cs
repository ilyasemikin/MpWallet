using MpWallet.Expressions.Operators;
using MpWallet.Expressions.Parsing.Sprache.Parser.Implementations.Nodes;
using MpWallet.Expressions.Parsing.Sprache.Parser.Implementations.Nodes.Abstractions;
using MpWallet.Expressions.Parsing.Sprache.UnitTests.Parser.Nodes.Abstractions;
using MpWallet.Operators;
using MpWallet.Operators.Collections.Extensions;
using Xunit;

namespace MpWallet.Expressions.Parsing.Sprache.UnitTests.Parser.Nodes;

public class BinaryOperatorParserNodeTests : ParserNodeTests<BinaryOperatorParserNode>
{
    public static TheoryData<OperatorParserNode?, ParserNode?, ParserNode?, string>
        GenerateConstructorArgumentNullExceptionCases
    {
        get
        {
            var @operator = new OperatorParserNode(DefaultOperators.Collection.Get("+", OperatorArity.Binary));
            var left = new MoneyParserNode();
            var right = new MoneyParserNode();
            
            return new TheoryData<OperatorParserNode?, ParserNode?, ParserNode?, string>
            {
                { null, left, right, "@operator" },
                { @operator, null, right, "left" },
                { @operator, left, null, "right" }
            };
        }
    }

    [Theory]
    [MemberData(nameof(GenerateConstructorArgumentNullExceptionCases))]
    public void Constructor_ShouldThrowArgumentNullException_WhenArgumentIsNull(
        OperatorParserNode? @operator, ParserNode? left, ParserNode? right, string paramName)
    {
        var exception = Record.Exception(() => new BinaryOperatorParserNode(@operator!, left!, right!));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal(paramName, ((ArgumentNullException)exception).ParamName);
    }

    protected override BinaryOperatorParserNode CreateEmptyNode()
    {
        var @operator = DefaultOperators.Collection.Get("+", OperatorArity.Binary);
        var operatorNode = new OperatorParserNode(@operator);

        var left = new MoneyParserNode();
        var right = new MoneyParserNode();

        return new BinaryOperatorParserNode(operatorNode, left, right);
    }
}