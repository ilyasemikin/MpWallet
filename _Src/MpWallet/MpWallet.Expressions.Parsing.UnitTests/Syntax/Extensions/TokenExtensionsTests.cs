using MpWallet.Expressions.Parsing.Syntax;
using MpWallet.Expressions.Parsing.Syntax.Extensions;

namespace MpWallet.Expressions.Parsing.UnitTests.Syntax.Extensions;

public class TokenExtensionsTests
{
    public static IEnumerable<object[]> TryTrimSuccessCases
    {
        get
        {
            {
                const string input = "input";
                var token = new Token(input, 0, input.Length);
                
                yield return [token, token];
            }

            {
                const string input = "  input";
                
                var token = new Token(input, 0, input.Length);
                var expected = new Token(input, 2, input.Length);

                yield return [token, expected];
            }

            {
                const string input = "input   ";
                
                var token = new Token(input, 0, input.Length);
                var expected = new Token(input, 0, 5);

                yield return [token, expected];
            }

            {
                const string input = "   input    ";

                var token = new Token(input, 0, input.Length);
                var expected = new Token(input, 3, 8);

                yield return [token, expected];
            }

            {
                const string input = " fdas  input fas ";

                var token = new Token(input, 5, 13);
                var expected = new Token(input, 7, 12);

                yield return [token, expected];
            }
        }
    }
    
    [Theory]
    [MemberData(nameof(TryTrimSuccessCases))]
    public void TryTrim_ShouldSuccess_WhenTrimmedNotEmpty(Token token, Token expected)
    {
        var result = token.TryTrim(out var actual);
        
        Assert.True(result);
        Assert.NotNull(actual);

        Assert.Equal(token.Input, expected.Input);
        Assert.Equal(token.Input, actual.Input);
        Assert.Equal(expected, actual);
    }

    public static IEnumerable<object[]> TryTrimFailureCases
    {
        get
        {
            const string input = "    ";
            var token = new Token(input, 0, input.Length);

            yield return [token];
        }
    }
    
    [Theory]
    [MemberData(nameof(TryTrimFailureCases))]
    public void TryTrim_ShouldFailure_WhenTrimmedEmpty(Token token)
    {
        var result = token.TryTrim(out var actual);
        
        Assert.False(result);
        Assert.Null(actual);
    }

    public static IEnumerable<object[]> TrimSuccessCases => TryTrimSuccessCases;

    [Theory]
    [MemberData(nameof(TrimSuccessCases))]
    public void Trim_ShouldSuccess_WhenTrimmedNotEmpty(Token token, Token expected)
    {
        var actual = token.Trim();

        Assert.Equal(token.Input, expected.Input);
        Assert.Equal(token.Input, actual.Input);
        Assert.Equal(expected, actual);
    }

    public static IEnumerable<object[]> TrimFailureCases => TryTrimFailureCases;

    [Theory]
    [MemberData(nameof(TrimFailureCases))]
    public void Trim_ShouldThrowArgumentException_WhenTrimmedEmpty(Token token)
    {
        var exception = Record.Exception(token.Trim);

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Equal("token", ((ArgumentException)exception).ParamName);
    }
}