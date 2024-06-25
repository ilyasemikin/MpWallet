using MpWallet.Expressions.Parsing.Sprache.Parser.Implementations.Nodes;
using MpWallet.Expressions.Parsing.Sprache.Parser.Implementations.Nodes.Abstractions;
using MpWallet.Expressions.Parsing.Sprache.UnitTests.Parser.Nodes.Abstractions;
using Xunit;

namespace MpWallet.Expressions.Parsing.Sprache.UnitTests.Parser.Nodes;

public class TermParsingNodeTests : ParserNodeTests<TermParsingNode>
{
    public static TheoryData<string?> GenerateArgumentExceptionWhenNameNullOrWhitespace =>
        new()
        {
            null,
            string.Empty,
            "   "
        };

    [Theory]
    [MemberData(nameof(GenerateArgumentExceptionWhenNameNullOrWhitespace))]
    public void Constructor_ShouldThrowArgumentException_WhenNameNullOrWhitespace(string? name)
    {
        var exception = Record.Exception(() => new TermParsingNode(name!));

        Assert.NotNull(exception);
        Assert.IsAssignableFrom<ArgumentException>(exception);
        Assert.Equal("name", ((ArgumentException)exception).ParamName);
    }

    public static TheoryData<IReadOnlyList<ParserNode?>> GenerateConstructorThrowArgumentNullWhenArgumentNull =>
        new()
        {
            new[] { (ParserNode?)null },
            new[] { new MoneyParserNode(), (ParserNode?)null },
            new[] { (ParserNode?)null, new MoneyParserNode() }
        };

    [Theory]
    [MemberData(nameof(GenerateConstructorThrowArgumentNullWhenArgumentNull))]
    public void Constructor_ShouldThrowArgumentNullException_WhenSomeArgumentIsNull(
        IReadOnlyList<ParserNode?> arguments)
    {
        var exception = Record.Exception(() => new TermParsingNode("name", arguments!));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
    }

    protected override TermParsingNode CreateEmptyNode()
    {
        return new TermParsingNode("Term");
    }
}