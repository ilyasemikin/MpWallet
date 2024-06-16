using MpWallet.Expressions.Parsing.Syntax;

namespace MpWallet.Expressions.Parsing.UnitTests.Syntax;

public class TokenTests
{
    public static IEnumerable<object[]> ConstructorSuccessCases
    {
        get
        {
            const string input = "some input data";
            
            yield return [input, 0, 1];
            yield return [input, 0, input.Length];
            yield return [input, input.Length - 1, input.Length];
        }
    }

    [Theory]
    [MemberData(nameof(ConstructorSuccessCases))]
    public void Constructor_ShouldSuccess_WhenPassValidParameters(string input, int begin, int end)
    {
        var expectedInput = input;
        var expectedValue = input.Substring(begin, end - begin);
        
        var token = new Token(input, begin, end);

        Assert.Equal(expectedInput, token.Input);
        Assert.Equal(begin, token.Begin);
        Assert.Equal(end, token.End);
        Assert.Equal(expectedValue, token.Value);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenInputIsNull()
    {
        var exception = Record.Exception(() => new Token(null!, 0, 1));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal("input", ((ArgumentNullException)exception).ParamName);
    }
    
    public static IEnumerable<object?[]> ConstructorExceptionCases
    {
        get
        {
            const string beginName = "begin";
            const string endName = "end";
            
            const string input = "some input data";
            yield return [input, -1, input.Length, beginName];
            yield return [input, 0, -1, endName];
            yield return [input, input.Length + 1, input.Length, beginName];
            yield return [input, input.Length, input.Length + 1, endName];
            yield return [input, 2, 1, beginName];
            yield return [input, input.Length, input.Length, beginName];
        }
    }
    
    [Theory]
    [MemberData(nameof(ConstructorExceptionCases))]
    public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenParametersInvalid(
        string input, int begin, int end, string parameterName)
    {
        var exception = Record.Exception(() => new Token(input, begin, end));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentOutOfRangeException>(exception);
        Assert.Equal(parameterName, ((ArgumentOutOfRangeException)exception).ParamName);
    }
}