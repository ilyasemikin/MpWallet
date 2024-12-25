using MpWallet.Currencies;
using MpWallet.ExchangeRates;
using MpWallet.ExchangeRates.Implementations;

namespace MpWallet.Money.Tests;

public sealed class MultiCurrencyMoneyTests
{
    private readonly FixedExchangeRatesGetter _exchangeRatesGetter;

    public MultiCurrencyMoneyTests()
    {
        _exchangeRatesGetter = new FixedExchangeRatesGetter(
            new ExchangeRate(Currency.USD, Currency.USD, 1m),
            new ExchangeRate(Currency.USD, Currency.RUB, 102.3438m),
            new ExchangeRate(Currency.USD, Currency.EUR, 0.96m),
            new ExchangeRate(Currency.USD, Currency.GBP, 0.80m),
            new ExchangeRate(Currency.USD, Currency.CHF, 0.90m),
            new ExchangeRate(Currency.EUR, Currency.RUB, 106.5444m),
            new ExchangeRate(Currency.GBP, Currency.RUB, 128.4517m),
            new ExchangeRate(Currency.CHF, Currency.RUB, 114.3251m),
            new ExchangeRate(Currency.RUB, Currency.RUB, 1m));
    }
    
    [Fact]
    public void Constructor_ShouldThrowException_WhenMoneyIsNull()
    {
        var exception = Record.Exception(() => new MultiCurrencyMoney(null!));
        
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal("money", ((ArgumentNullException)exception).ParamName);
    }

    [Fact]
    public void Parts_ShouldCombineSameCurrencies_WhenPassMultipleSame()
    {
        var parts = new Money[]
        {
            new(10, Currency.EUR),
            new(15, Currency.EUR),
            new(20, Currency.EUR),
        };
        
        var money = new MultiCurrencyMoney(parts);

        Assert.Single(money.Parts);
        Assert.Equal(45, money.Parts[0].Amount);
        Assert.Equal(Currency.EUR, money.Parts[0].Currency);
    }

    [Fact]
    public void Parts_ShouldCombineDifferentCurrencies_WhenPassDifferent()
    {
        var parts = new Money[]
        {
            new(10, Currency.EUR),
            new(15, Currency.USD),
            new(20, Currency.RUB),
            new(25, Currency.USD),
        };

        var expectedParts = new Money[]
        {
            new(10, Currency.EUR),
            new(40, Currency.USD),
            new(20, Currency.RUB),
        };
        
        var money = new MultiCurrencyMoney(parts);

        Assert.Equal(3, money.Parts.Count);
        Assert.Contains(money.Parts, e => e == expectedParts[0]);
        Assert.Contains(money.Parts, e => e == expectedParts[1]);
        Assert.Contains(money.Parts, e => e == expectedParts[2]);
    }

    [Fact]
    public void Parts_ShouldEmpty_WhenMoneyIsEmpty()
    {
        var money = new MultiCurrencyMoney([]);
        
        Assert.Empty(money.Parts);
    }

    [Fact]
    public async Task AsAsync_ShouldSuccess_WhenContainsDifferentCurrencies()
    {
        var parts = new Money[]
        {
            new(1, Currency.EUR),
            new(1, Currency.USD),
        };
        
        var expected = new Money(208.8882m, Currency.RUB);
        
        var money = new MultiCurrencyMoney(parts);

        var result = await money.AsAsync(_exchangeRatesGetter, Currency.RUB);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(Currency.Codes.CHF)]
    [InlineData(Currency.Codes.EUR)]
    [InlineData(Currency.Codes.USD)]
    [InlineData(Currency.Codes.RUB)]
    public async Task AsAsync_ShouldSuccess_WhenPartsIsEmpty(string newCurrencyCode)
    {
        var newCurrency = Currency.GetByCode(newCurrencyCode);
        var money = new MultiCurrencyMoney([]);

        var expected = new Money(0, newCurrency);
        
        var result = await money.AsAsync(_exchangeRatesGetter, newCurrency);
        
        Assert.Equal(expected, result);
    }
}