using MpWallet.Currencies;
using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Context;
using MpWallet.Expressions.Context.Functions;
using MpWallet.Expressions.Exceptions;
using MpWallet.Expressions.Extensions;
using MpWallet.Expressions.UnitTests.Mocks;
using MpWallet.Values;
using Xunit;

namespace MpWallet.Expressions.UnitTests;

public class FunctionCallExpressionTests
{
    private const string FunctionWithParametersName = "FunctionWithParameters";
    private const string FunctionWithoutParametersName = "FunctionWithoutParameters";
    
    private readonly ExpressionCalculationContext _context;

    public FunctionCallExpressionTests()
    {
        var functions = new List<Function>();

        {
            var functionParameters = new FunctionParameter[]
            {
                new("x"),
                new("y")
            };

            var functionExpression =
                new MultiplicationOperationExpression(
                    new AdditionOperatorExpression(
                        new VariableExpression("x"),
                        new VariableExpression("y")),
                    new Number(2).ToExpression());

            var function = new Function(FunctionWithParametersName, functionParameters, functionExpression);
            functions.Add(function);
        }

        {
            var functionExpression = new Number(1).ToExpression();

            var function = new Function(FunctionWithoutParametersName, functionExpression);
            functions.Add(function);
        }

        _context = MockCurrencyRatioProvider.CreateExpressionCalculationContext(functions: functions);
    }
    
    [Fact]
    public void Calculate_ShouldThrowException_WhenFunctionNotFoundInContext()
    {
        const string name = "NotExistedName";
        var expression = new FunctionCallExpression(name);

        var exception = Record.Exception(() =>
            expression.Calculate(MockCurrencyRatioProvider.ExpressionCalculationContext, Currency.USD));

        Assert.NotNull(exception);
        Assert.True(exception is FunctionNotFoundException { FunctionName: name });
    }

    public static IEnumerable<object?[]> CalculateThrowExceptionWithInvalidCall
    {
        get
        {
            yield return Create(null);
            yield return Create(new Number(1).ToExpression());
            yield return Create(new Money(1, Currency.EUR).ToExpression());
            yield return Create(
                new Number(1).ToExpression(),
                new Number(2).ToExpression(),
                new Number(3).ToExpression());
            
            yield break;

            object?[] Create(params Expression[]? expressions)
            {
                return [expressions];
            }
        }
    }
    
    [Theory]
    [MemberData(nameof(CalculateThrowExceptionWithInvalidCall))]
    public void Calculate_ShouldThrowException_WhenFunctionCallIsInvalid(IEnumerable<Expression> arguments)
    {
        var expression = new FunctionCallExpression(FunctionWithParametersName, arguments);

        var exception = Record.Exception(() => expression.Calculate(_context, Currency.EUR));

        Assert.NotNull(exception);
        Assert.IsType<InvalidFunctionCallException>(exception);
    }

    public static IEnumerable<object[]> CalculateSuccessWithParameters
    {
        get
        {
            {
                var arguments = new[]
                {
                    new Number(1).ToExpression(),
                    new Number(2).ToExpression()
                };
                yield return [arguments, new Number(6).ToExpression()];
            }

            {
                var arguments = new[]
                {
                    new Money(1, Currency.USD).ToExpression(),
                    new Money(2, Currency.USD).ToExpression()
                };
                yield return [arguments, new Money(6, Currency.USD).ToExpression()];
            }
        }
    }
    
    [Theory]
    [MemberData(nameof(CalculateSuccessWithParameters))]
    public void Calculate_ShouldSuccess_WhenFunctionWithParameters(
        IEnumerable<Expression> arguments, Expression expected)
    {
        var expression = new FunctionCallExpression(FunctionWithParametersName, arguments);

        var result = expression.Calculate(_context, Currency.USD);
        
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Calculate_ShouldSuccess_WhenFunctionWithoutParameters()
    {
        var expected = new Number(1).ToExpression();
        var expression = new FunctionCallExpression(FunctionWithoutParametersName);

        var result = expression.Calculate(_context, Currency.USD);

        Assert.Equal(expected, result);
    }
}