using MpWallet.Expressions.Context.Functions;
using MpWallet.Expressions.Context.Variables;
using Xunit;

namespace MpWallet.Expressions.UnitTests.Context;

public sealed class VariableTests
{
    public static IEnumerable<object[]> ConstructorSuccessCases
    {
        get
        {
            var names = new[]
            {
                "first",
                "Second",
                "ThIrD",
                "_",
                "_123",
                "_abc",
                "_a1c",
                "f4"
            };

            return names.Select(name => new object[] { name });
        }
    }
    
    [Theory]
    [MemberData(nameof(ConstructorSuccessCases))]
    public void Constructor_ShouldSuccess_WhenArgumentsValid(string name)
    {
        var expression = new NumberExpression(1);
        
        var actual = new Variable(name, expression);
        
        Assert.Equal(name, actual.Name);
        Assert.Equal(expression, actual.Expression);
    }

    public static IEnumerable<object[]> ConstructorNameThrowArgumentException
    {
        get
        {
            var names = new[]
            {
                "123abc",
                "$123",
                "^123"
            };

            return names.Select(name => new object[] { name });
        }
    }
    
    [Theory]
    [MemberData(nameof(ConstructorNameThrowArgumentException))]
    public void Constructor_ShouldThrowArgumentException_WhenNamePatternInvalid(string name)
    {
        var expression = new NumberExpression(123);

        var exception = Record.Exception(() => new Variable(name, expression));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);

        var argumentException = (ArgumentException)exception;
        
        Assert.Equal("name", argumentException.ParamName);
        Assert.StartsWith("Does not match the pattern", argumentException.Message);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenExpressionIsNull()
    {
        var exception = Record.Exception(() => new Variable("a", null!));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal("expression", ((ArgumentNullException)exception).ParamName);
    }

    public static IEnumerable<object?[]> ConstructorNameNullOrWhitespaceThrowArgumentException
    {
        get
        {
            yield return [null, typeof(ArgumentNullException)];
            yield return [string.Empty, typeof(ArgumentException)];
            yield return ["\t", typeof(ArgumentException)];
        }
    }
    
    [Theory]
    [MemberData(nameof(ConstructorNameNullOrWhitespaceThrowArgumentException))]
    public void Constructor_ShouldThrowArgumentException_WhenNameIsNullOrWhitespace(
        string? name, Type expectedExceptionType)
    {
        var expression = new NumberExpression(123);

        var exception = Record.Exception(() => new Variable(name!, expression));

        Assert.NotNull(exception);
        Assert.IsType(expectedExceptionType, exception);
        Assert.Equal("name", ((ArgumentException)exception).ParamName);
    }
    
    [Fact]
    public void NameRegexPattern_ShouldNotNull()
    {
        Assert.NotNull(Variable.NameRegexPattern);
    }
    
    [Fact]
    public void NameRegexPattern_ShouldEqualFunctionNameRegexPattern()
    {
        Assert.Equal(Function.NameRegexPattern.ToString(), Variable.NameRegexPattern.ToString());
    }
}