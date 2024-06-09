using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Context.Functions;
using MpWallet.Expressions.Extensions;
using MpWallet.Values;
using Xunit;

namespace MpWallet.Expressions.UnitTests.Context;

public class FunctionTests
{
    private const string FunctionName = "Name";

    private readonly Expression _expression = new Number(1).ToExpression();
    
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
    }

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
}