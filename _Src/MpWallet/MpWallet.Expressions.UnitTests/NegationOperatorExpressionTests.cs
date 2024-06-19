using MpWallet.Currencies;
using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.UnitTests.Mocks;
using MpWallet.Values.Implementations;
using Xunit;

namespace MpWallet.Expressions.UnitTests;

public class NegationOperatorExpressionTests
{
    [Fact]
    public void Calculate_ShouldReturnNegotiate_WhenValueIsNumber()
    {
        var number = new Number(1);
        var constant = new NumberExpression(number);
        var expression = new NegationOperatorExpression(constant);

        var result = expression.Calculate(MockCurrencyRatioProvider.ExpressionCalculationContext, Currency.USD);
        
        Assert.True(result is ConstantExpression { Value: Number n } && n.Value == -number.Value);
    }

    public static IEnumerable<object[]> CalculateReturnNegotiateValueTestCases =>
        Currency.All.Select(currency => new object[] { currency });
    
    [Theory]
    [MemberData(nameof(CalculateReturnNegotiateValueTestCases))]
    public void Calculate_ShouldReturnNegotiateValue_WhenValueIsMoney(Currency currency)
    {
        var money = new Money(1, currency);
        var constant = new MoneyExpression(money);
        var expression = new NegationOperatorExpression(constant);

        var result = expression.Calculate(MockCurrencyRatioProvider.ExpressionCalculationContext, currency);

        Assert.True(result is ConstantExpression { Value: Money m } &&
                    m.Value == -money.Value &&
                    m.Currency == money.Currency);
    }

    public static IEnumerable<object[]> CalculateComplexArgumentTestCases
    {
        get
        {
            var number = new Number(1);
            var money = new Money(1, Currency.USD);

            var numberConstant = new NumberExpression(number);
            var moneyConstant = new MoneyExpression(money);

            yield return [new AdditionOperatorExpression(numberConstant, moneyConstant)];
            yield return [new SubtractionOperationExpression(numberConstant, moneyConstant)];
        }
    }
    
    [Theory]
    [MemberData(nameof(CalculateComplexArgumentTestCases))]
    public void Calculate_ShouldReturnNegationWithArgument_WhenArgumentIsComplex(Expression argument)
    {
        var expression = new NegationOperatorExpression(argument);

        var result = expression.Calculate(MockCurrencyRatioProvider.ExpressionCalculationContext, Currency.USD);

        Assert.True(result is NegationOperatorExpression e && e.Argument == argument);
    }

    [Theory]
    [MemberData(nameof(CalculateComplexArgumentTestCases))]
    public void Calculate_ShouldReturnArgument_WhenNegationApplyTwice(Expression argument)
    {
        var expression = new NegationOperatorExpression(argument);
        expression = new NegationOperatorExpression(expression);

        var result = expression.Calculate(MockCurrencyRatioProvider.ExpressionCalculationContext, Currency.USD);

        Assert.Equal(argument, result);
    }
}