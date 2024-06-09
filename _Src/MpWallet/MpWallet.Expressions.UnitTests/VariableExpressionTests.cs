using MpWallet.Currencies;
using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Context.Variables;
using MpWallet.Expressions.Exceptions;
using MpWallet.Expressions.UnitTests.Mocks;
using MpWallet.Values;
using Xunit;

namespace MpWallet.Expressions.UnitTests;

public class VariableExpressionTests
{
    public static IEnumerable<object[]> CalculateReturnVariableExpressionTestCases
    {
        get
        {
            const string name = "ExistedName";
            
            var number = new Number(1);
            var money = new Money(1, Currency.USD);

            var numberConstant = new ConstantExpression(number);
            var moneyConstant = new ConstantExpression(money);

            yield return [new Variable(name, numberConstant)];
            yield return [new Variable(name, moneyConstant)];
            yield return [new Variable(name, new AdditionOperatorExpression(numberConstant, moneyConstant))];
            yield return [new Variable(name, new SubtractionOperationExpression(numberConstant, moneyConstant))];
        }
    }
    
    [Theory]
    [MemberData(nameof(CalculateReturnVariableExpressionTestCases))]
    public void Calculate_ShouldReturnVariableExpression_WhenVariableExists(Variable variable)
    {
        var variables = new[] { variable };
        var context = MockCurrencyRatioProvider.CreateExpressionCalculationContext(variables);
        var expression = new VariableExpression(variable.Name);

        var result = expression.Calculate(context, Currency.USD);

        Assert.Equal(variable.Expression, result);
    }

    public static IEnumerable<object[]> CalculateValueOfVariableTestCases
    {
        get
        {
            const string name = "ExistedName";
            
            foreach (var (ratio, value) in MockCurrencyRatioProvider.Ratios)
            {
                var money = new Money(1, ratio.Antecedent);
                var constant = new ConstantExpression(money);

                var variable = new Variable(name, constant);

                var expectedMoney = new Money(MockCurrencyRatioProvider.Ratios[ratio], ratio.Consequent);
                var expected = new ConstantExpression(expectedMoney);
                
                yield return [variable, ratio.Consequent, expected];
            }

            {
                var number = new Number(1);
                var expression = new ConstantExpression(number);

                var variable = new Variable(name, expression);
                
                yield return [variable, Currency.USD, expression];
            }

            {
                var number = new Number(1);
                var constant = new ConstantExpression(number);

                var expression = new AdditionOperatorExpression(constant, constant);
                var variable = new Variable(name, expression);
                
                var expectedNumber = new Number(2);
                var expected = new ConstantExpression(expectedNumber);

                yield return [variable, Currency.USD, expected];
            }
        }
    }
    
    [Theory]
    [MemberData(nameof(CalculateValueOfVariableTestCases))]
    public void Calculate_ShouldCalculateValueOfVariable(Variable variable, Currency currency, Expression expected)
    {
        var variables = new[] { variable };
        var context = MockCurrencyRatioProvider.CreateExpressionCalculationContext(variables);
        var expression = new VariableExpression(variable.Name);

        var result = expression.Calculate(context, currency);
        
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void Calculate_ShouldThrowException_WhenVariableNotFoundInContext()
    {
        const string name = "NotExistedName";
        var expression = new VariableExpression(name);

        var exception = Record.Exception(
            () => expression.Calculate(MockCurrencyRatioProvider.ExpressionCalculationContext, Currency.EUR));

        Assert.NotNull(exception);
        Assert.True(exception is VariableNotFoundException { VariableName: name });
    }
}