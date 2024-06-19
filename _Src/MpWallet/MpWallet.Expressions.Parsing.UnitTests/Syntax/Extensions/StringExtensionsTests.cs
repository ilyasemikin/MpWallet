using MpWallet.Expressions.Parsing.Syntax;
using MpWallet.Expressions.Parsing.Syntax.Extensions;

namespace MpWallet.Expressions.Parsing.UnitTests.Syntax.Extensions;

public class StringExtensionsTests
{
    [Fact]
    public static void ToToken_ShouldSuccess_WhenArgumentsIsEmpty()
    {
        const string input = "input";

        var token = input.ToToken();

        Assert.NotNull(token);
    }

    [Fact]
    public static void ToToken_ShouldThrowArgumentException_WhenStringIsEmpty()
    {
        var input = string.Empty;

        var exception = Record.Exception(() => input.ToToken());

        Assert.NotNull(exception);
        Assert.IsAssignableFrom<ArgumentException>(exception);
    }

    public static IEnumerable<object[]> ToTokenWithArgumentsSuccessCases
    {
        get
        {
            yield return ["input", 0, 5, new Token("input", 0, 5)];
            yield return ["input", 1, 5, new Token("input", 1, 5)];
            yield return ["input", 0, 4, new Token("input", 0, 4)];
            yield return ["input", 1, 4, new Token("input", 1, 4)];
        }
    }
    
    [Theory]
    [MemberData(nameof(ToTokenWithArgumentsSuccessCases))]
    public static void ToToken_ShouldSuccess_WhenArgumentsPassed(
        string input, int begin, int end, Token expected)
    {
        var actual = input.ToToken(begin, end);

        Assert.NotNull(actual);
        Assert.Equal(expected, actual);
    }
}