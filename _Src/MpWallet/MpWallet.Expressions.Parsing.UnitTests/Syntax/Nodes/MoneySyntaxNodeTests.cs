using MpWallet.Currencies;
using MpWallet.Expressions.Parsing.Syntax;
using MpWallet.Expressions.Parsing.Syntax.Nodes;
using MpWallet.Values.Implementations;

namespace MpWallet.Expressions.Parsing.UnitTests.Syntax.Nodes;

public sealed class MoneySyntaxNodeTests
{
    [Fact]
    public void Constructor_ShouldSuccess_WhenArgumentCorrect()
    {
        const string input = "1.1 $";
        var token = new Token(input, 0, input.Length);
        var expectedValue = new Money(1.1M, Currency.USD);
        
        var node = new MoneySyntaxNode(token);

        Assert.NotNull(node);
        Assert.Equal(token, node.Token);
        Assert.Equal(expectedValue, node.Value);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenArgumentIsNull()
    {
        var exception = Record.Exception(() => new MoneySyntaxNode(null!));
        
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal("token", ((ArgumentNullException)exception).ParamName);
    }

    public static IEnumerable<object[]> ConstructorThrowFormatExceptionCases
    {
        get
        {
            const string min = "-79228162514264337593543950336 $";
            const string max = "79228162514264337593543950336 $";
            const string invalidCurrency = "1 ABC";
            
            yield return [new Token("1-1 $", 0, 3)];
            yield return [new Token(min, 0, min.Length)];
            yield return [new Token(max, 0, max.Length)];
            yield return [new Token(invalidCurrency, 0, invalidCurrency.Length)];
        }
    }
    
    [Theory]
    [MemberData(nameof(ConstructorThrowFormatExceptionCases))]
    public void Constructor_ShouldThrowException_WhenArgumentInvalid(Token token)
    {
        var exception = Record.Exception(() => new MoneySyntaxNode(token));

        Assert.NotNull(exception);
        Assert.IsType<FormatException>(exception);
    }
}