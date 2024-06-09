using MpWallet.Currencies;
using MpWallet.Expressions.UnitTests.Mocks;
using MpWallet.Values;
using Xunit;

namespace MpWallet.Expressions.UnitTests;

public class ConstantExpressionTests
{
    [Fact]
    public void Calculate_ShouldReturnSame_WhenValueIsNumber()
    {
        var number = new Number(1);
        var expression = new ConstantExpression(number);

        var result = expression.Calculate(MockCurrencyRatioProvider.ExpressionCalculationContext, Currency.BYN);
        
        Assert.True(result is ConstantExpression { Value: Number n } && n.Value == number.Value);
    }

    public static IEnumerable<object[]> CalculateReturnMoneyWithCurrencyTestCases =>
        MockCurrencyRatioProvider.Ratios.Keys.Select(ratio => new object[] { ratio });
    
    [Theory]
    [MemberData(nameof(CalculateReturnMoneyWithCurrencyTestCases))]
    public void Calculate_ShouldReturnMoneyWithCurrency_WhenValueIsMoney(CurrencyRatio ratio)
    {
        var money = new Money(1, ratio.Antecedent);
        var expression = new ConstantExpression(money);

        var expectedMoneyValue = MockCurrencyRatioProvider.Ratios[ratio] * money.Value;
        
        var result = expression.Calculate(MockCurrencyRatioProvider.ExpressionCalculationContext, ratio.Consequent);

        Assert.True(result is ConstantExpression { Value: Money m } &&
                    m.Value == expectedMoneyValue &&
                    m.Currency == ratio.Consequent);
    }

    [Fact]
    public void Negotiate_ShouldReturnNegotiateNumber_WhenValueIsNumber()
    {
        var number = new Number(1);
        var expression = new ConstantExpression(number);

        var result = expression.Negotiate();

        Assert.True(result.Value is Number n && n.Value == -number.Value);
    }

    public static IEnumerable<object[]> NegotiateReturnNegotiateMoneyValueTestCases =>
        Currency.All.Select(currency => new object[] { currency });
    
    [Theory]
    [MemberData(nameof(NegotiateReturnNegotiateMoneyValueTestCases))]
    public void Negotiate_ShouldReturnNegotiateMoneyValue_WhenValueIsMoney(Currency currency)
    {
        var money = new Money(1, currency);
        var expression = new ConstantExpression(money);

        var result = expression.Negotiate();

        Assert.True(result.Value is Money m && m.Value == -money.Value && m.Currency == money.Currency);
    }
}