using System.Globalization;
using MpWallet.Expressions.Parsing.Syntax;
using MpWallet.Expressions.Parsing.Syntax.Nodes;

namespace MpWallet.Expressions.Parsing.UnitTests.Syntax.Nodes;

public class NumberSyntaxNodeTests
{
    public static IEnumerable<object?[]> SuccessCases
    {
        get
        {
            yield return [new Token("1", 0, 1), null, 1M];
            yield return [new Token("tra1sh", 3, 4), null, 1M];
            yield return [new Token("1.1", 0, 3), null, 1.1M];
            yield return [new Token("1,1", 0, 3), new CultureInfo("ru-RU"), 1.1M];
        }
    }
    
    [Theory]
    [MemberData(nameof(SuccessCases))]
    public void Constructor_ShouldSuccess_WhenArgumentCorrect(
        Token token, IFormatProvider? formatProvider, decimal expected)
    {
        var node = new NumberSyntaxNode(token, formatProvider);

        Assert.NotNull(node);
        Assert.Equal(token, node.Token);
        Assert.Equal(expected, node.Value);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenArgumentIsNull()
    {
        var exception = Record.Exception(() => new NumberSyntaxNode(null!));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal("token", ((ArgumentNullException)exception).ParamName);
    }

    public static IEnumerable<object[]> ConstructorThrowFormatExceptionCases
    {
        get
        {
            const string min = "-79228162514264337593543950336";
            const string max = "79228162514264337593543950336";
            
            yield return [new Token("1-1", 0, 3)];
            yield return [new Token(min, 0, min.Length)];
            yield return [new Token(max, 0, max.Length)];
        }
    }

    [Theory]
    [MemberData(nameof(ConstructorThrowFormatExceptionCases))]
    public void Constructor_ShouldThrowFormatException_WhenArgumentInvalid(Token token)
    {
        var exception = Record.Exception(() => new NumberSyntaxNode(token));

        Assert.NotNull(exception);
        Assert.IsType<FormatException>(exception);
    }
}