using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Context.Functions;
using MpWallet.Expressions.Context.Variables;
using MpWallet.Expressions.Extensions;
using MpWallet.Values;
using MpWallet.Values.Implementations;
using Xunit;

namespace MpWallet.Expressions.UnitTests.Context;

public class FunctionTests
{
    private const string FunctionName = "Name";

    private readonly Expression _expression = new Number(1).ToExpression();

    [Fact]
    public void Constructor_ShouldSuccess_WhenParametersUnique()
    {
        var parameters = new FunctionParameter[]
        {
            new("a"),
            new("b"),
            new("c"),
            new("d")
        };

        var function = new Function(FunctionName, parameters, _expression);
        
        Assert.Equal(FunctionName, function.Name);
        Assert.Equal(_expression, function.Expression);
        Assert.Equal(parameters, function.Parameters);
    }
    
    [Fact]
    public void Constructor_ShouldThrowException_WhenParametersNotUnique()
    {
        var parameters = new FunctionParameter[]
        {
            new("a"),
            new("b"),
            new("a")
        };
        
        var exception = Record.Exception(() => new Function(FunctionName, parameters, _expression));

        Assert.NotNull(exception);
        Assert.IsType<InvalidOperationException>(exception);
        Assert.Equal("Parameters must be unique", ((InvalidOperationException)exception).Message);
    }

    public static IEnumerable<object[]> ConstructorThrowArgumentNullException
    {
        get
        {
            const string name = "abc";
            var parameters = Array.Empty<FunctionParameter>();
            var expression = new NumberExpression(10);

            yield return [null!, parameters, expression, "name"];
            yield return [name, null!, expression, "parameters"];
            yield return [name, parameters, null!, "expression"];
        }
    }
    
    [Theory]
    [MemberData(nameof(ConstructorThrowArgumentNullException))]
    public void Constructor_ShouldThrowArgumentNullException_WhenOneOfArgumentsNull(
        string? name, IEnumerable<FunctionParameter>? parameters, Expression? expression, string expectedName)
    {
        var exception = Record.Exception(() => new Function(name!, parameters!, expression!));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal(expectedName, ((ArgumentNullException)exception).ParamName);
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

        var exception = Record.Exception(() => new Function(name, expression));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);

        var argumentException = (ArgumentException)exception;
        
        Assert.Equal("name", argumentException.ParamName);
        Assert.StartsWith("Does not match the pattern", argumentException.Message);
    }
    
    public static IEnumerable<object?[]> ConstructorNameWhitespaceThrowArgumentException
    {
        get
        {
            var names = new[]
            {
                string.Empty,
                "\t"
            };

            return names.Select(name => new object[] { name });
        }
    }
    
    [Theory]
    [MemberData(nameof(ConstructorNameWhitespaceThrowArgumentException))]
    public void Constructor_ShouldThrowArgumentException_WhenNameIsWhitespace(string name)
    {
        var expression = new NumberExpression(123);

        var exception = Record.Exception(() => new Function(name!, expression));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Equal("name", ((ArgumentException)exception).ParamName);
    }

    [Fact]
    public void NameRegexPattern_ShouldNotNull()
    {
        Assert.NotNull(Function.NameRegexPattern);
    }

    [Fact]
    public void NameRegexPattern_ShouldEqualVariableNameRegexPattern()
    {
        Assert.Equal(Function.NameRegexPattern.ToString(), Variable.NameRegexPattern.ToString());
    }
}