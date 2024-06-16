using MpWallet.Currencies;
using MpWallet.Expressions.UnitTests.Mocks;
using MpWallet.Values.Implementations;
using Xunit;

namespace MpWallet.Expressions.UnitTests;

public class MoneyExpressionTests
{
    public static IEnumerable<object[]> CalculateReturnMoneyWithCurrencyTestCases =>
        MockCurrencyRatioProvider.Ratios.Keys.Select(ratio => new object[] { ratio });
    
    [Theory]
    [MemberData(nameof(CalculateReturnMoneyWithCurrencyTestCases))]
    public void Calculate_ShouldReturnMoneyWithCurrency_WhenValueIsMoney(CurrencyRatio ratio)
    {
        var money = new Money(1, ratio.Antecedent);
        var expression = new MoneyExpression(money);

        var expectedMoneyValue = MockCurrencyRatioProvider.Ratios[ratio] * money.Value;
        
        var result = expression.Calculate(MockCurrencyRatioProvider.ExpressionCalculationContext, ratio.Consequent);

        Assert.True(result is MoneyExpression { Value: { } m } &&
                    m.Value == expectedMoneyValue &&
                    m.Currency == ratio.Consequent);
    }

    public static IEnumerable<object[]> NegotiateReturnNegotiateMoneyValueTestCases =>
        Currency.All.Select(currency => new object[] { currency });
    
    [Theory]
    [MemberData(nameof(NegotiateReturnNegotiateMoneyValueTestCases))]
    public void Negotiate_ShouldReturnNegotiateMoneyValue_WhenValueIsMoney(Currency currency)
    {
        var money = new Money(1, currency);
        var expression = new MoneyExpression(money);

        var result = expression.Negotiate();

        Assert.True(result is MoneyExpression { Value: { } m } && 
                    m.Value == -money.Value &&
                    m.Currency == money.Currency);
    }
}