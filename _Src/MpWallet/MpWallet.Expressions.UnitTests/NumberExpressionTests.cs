using MpWallet.Currencies;
using MpWallet.Expressions.UnitTests.Mocks;
using MpWallet.Values.Implementations;
using Xunit;

namespace MpWallet.Expressions.UnitTests;

public sealed class NumberExpressionTests
{
    [Fact]
    public void Calculate_ShouldReturnSame_WhenValueIsNumber()
    {
        var number = new Number(1);
        var expression = new NumberExpression(number);

        var result = expression.Calculate(MockCurrencyRatioProvider.ExpressionCalculationContext, Currency.BYN);
        
        Assert.True(result is NumberExpression { Value: { } n } && n.Value == number.Value);
    }
    
    [Fact]
    public void Negotiate_ShouldReturnNegotiateNumber_WhenValueIsNumber()
    {
        var number = new Number(1);
        var expression = new NumberExpression(number);

        var result = expression.Negotiate();

        Assert.True(result is NumberExpression { Value: { } n } && n.Value == -number.Value);
    }
}