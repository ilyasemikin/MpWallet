using MpWallet.Currencies;
using MpWallet.ExchangeRates;
using MpWallet.ExchangeRates.Exceptions;
using MpWallet.ExchangeRates.Implementations;

namespace MpWallet.Money.Tests;

public class MoneyTests
{
    private readonly FixedExchangeRatesGetter _exchangeRatesGetter;

    public MoneyTests()
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
    public void Constructor_ShouldThrowException_WhenCurrencyIsNull()
    {
        var exception = Record.Exception(() => new Money(0, null!));
        
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal("currency", ((ArgumentNullException)exception).ParamName);
    }
    
    [Theory]
    [InlineData(Currency.Codes.USD)]
    [InlineData(Currency.Codes.RUB)]
    [InlineData(Currency.Codes.EUR)]
    [InlineData(Currency.Codes.GBP)]
    [InlineData(Currency.Codes.CHF)]
    public async Task AsAsync_ShouldSuccess(string newCurrencyCode)
    {
        var newCurrency = Currency.GetByCode(newCurrencyCode);
        var ratio = new CurrencyRatio(Currency.USD, newCurrency);
        
        var money = new Money(1, Currency.USD);
        var expected = new Money(money.Amount * _exchangeRatesGetter.Rates[ratio].Value, newCurrency);

        var result = await money.AsAsync(_exchangeRatesGetter, newCurrency);
        
        Assert.Equal(expected, result);
    }

    public static TheoryData<string> AllCurrenciesCodes
    {
        get
        {
            var data = new TheoryData<string>();
            
            foreach (var code in Currency.Codes.All)
                data.Add(code);

            return data;
        }
    }
    
    [Theory]
    [MemberData(nameof(AllCurrenciesCodes))]
    public async Task AsAsync_ShouldSuccess_WhenAmountIsZero(string newCurrencyCode)
    {
        var newCurrency = Currency.GetByCode(newCurrencyCode);
        var money = new Money(0, Currency.USD);
        
        var expected = new Money(0, newCurrency);
        
        var result = await money.AsAsync(_exchangeRatesGetter, newCurrency);
        
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task AsAsync_ShouldThrowException_WhenRatioNotExisted()
    {
        var newCurrency = Currency.USD;
        var exchangeRatesGetter = new FixedExchangeRatesGetter();
        
        var money = new Money(1, Currency.EUR);
        
        var exception = await Record.ExceptionAsync(async() => await money.AsAsync(exchangeRatesGetter, newCurrency));
        
        Assert.IsType<ExchangeRateNotAvailableException>(exception);
    }
    
    [Fact]
    public void Equals_ShouldTrue_WhenCurrencyAndAmountAreTheSame()
    {
        var left = new Money(10, Currency.USD);
        var right = new Money(10, Currency.USD);

        var result = left.Equals(right);
        
        Assert.True(result);
    }

    [Fact]
    public void Equals_ShouldTrue_WhenAmountsAreZero()
    {
        var left = new Money(0, Currency.USD);
        var right = new Money(0, Currency.EUR);
        
        var result = left.Equals(right);
        
        Assert.True(result);
    }

    [Fact]
    public void Equals_ShouldFalse_WhenCurrenciesDifferent()
    {
        var left = new Money(10, Currency.USD);
        var right = new Money(10, Currency.EUR);
        
        var result = left.Equals(right);
        
        Assert.False(result);
    }
    
    [Fact]
    public void Equals_ShouldFalse_WhenAmountsDifferent()
    {
        var left = new Money(10, Currency.USD);
        var right = new Money(15, Currency.USD);
        
        var result = left.Equals(right);
        
        Assert.False(result);
    }
    
    [Theory]
    [InlineData(1, Currency.Codes.USD)]
    [InlineData(2, Currency.Codes.EUR)]
    public void ToString_ShouldSuccess(int amount, string currencyCode)
    {
        var currency = Currency.GetByCode(currencyCode);
        
        var money = new Money(amount, currency);
        var expected = $"{amount}{currency}";
        
        var result = money.ToString();
        
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(10, Currency.Codes.USD, 10, Currency.Codes.EUR)]
    [InlineData(2, Currency.Codes.USD, 5, Currency.Codes.USD)]
    [InlineData(150, Currency.Codes.RUB, 5, Currency.Codes.RUB)]
    [InlineData(10, Currency.Codes.RUB, 30, Currency.Codes.USD)]
    [InlineData(10, Currency.Codes.USD, 30, Currency.Codes.RUB)]
    public async Task AddAsync_ShouldSuccess(
        decimal leftAmount, string leftCurrencyCode,
        decimal rightAmount, string rightCurrencyCode)
    {
        var leftCurrency = Currency.GetByCode(leftCurrencyCode);
        var rightCurrency = Currency.GetByCode(rightCurrencyCode);

        var leftRatio = new CurrencyRatio(leftCurrency, Currency.RUB);
        var rightRatio = new CurrencyRatio(rightCurrency, Currency.RUB);
        
        var left = new Money(leftAmount, leftCurrency);
        var right = new Money(rightAmount, rightCurrency);

        var expectedAmount =
            leftAmount * _exchangeRatesGetter.Rates[leftRatio].Value +
            rightAmount * _exchangeRatesGetter.Rates[rightRatio].Value;
        var expected = new Money(expectedAmount, Currency.RUB);
        
        var result = await Money.AddAsync(left, right, _exchangeRatesGetter, Currency.RUB);
        
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(10, Currency.Codes.EUR, 10, Currency.Codes.USD)]
    [InlineData(10, Currency.Codes.USD, 10, Currency.Codes.EUR)]
    [InlineData(100, Currency.Codes.RUB, 10, Currency.Codes.RUB)]
    [InlineData(100, Currency.Codes.RUB, 1, Currency.Codes.EUR)]
    public async Task SubtractAsync_ShouldSuccess(
        decimal leftAmount, string leftCurrencyCode,
        decimal rightAmount, string rightCurrencyCode)
    {
        var leftCurrency = Currency.GetByCode(leftCurrencyCode);
        var rightCurrency = Currency.GetByCode(rightCurrencyCode);

        var leftRatio = new CurrencyRatio(leftCurrency, Currency.RUB);
        var rightRatio = new CurrencyRatio(rightCurrency, Currency.RUB);
        
        var left = new Money(leftAmount, leftCurrency);
        var right = new Money(rightAmount, rightCurrency);

        var expectedAmount =
            leftAmount * _exchangeRatesGetter.Rates[leftRatio].Value -
            rightAmount * _exchangeRatesGetter.Rates[rightRatio].Value;
        var expected = new Money(expectedAmount, Currency.RUB);
        
        var result = await Money.SubtractAsync(left, right, _exchangeRatesGetter, Currency.RUB);
        
        Assert.Equal(expected, result);
    }

    [Fact]
    public void MultiplyLeftOperator_ShouldSuccess()
    {
        var money = new Money(1, Currency.USD);
        
        var result = money * 2;
        
        Assert.Equal(Currency.USD, result.Currency);
        Assert.Equal(2, result.Amount);
    }
    
    [Fact]
    public void MultiplyRightOperator_ShouldSuccess()
    {
        var money = new Money(1, Currency.USD);
        
        var result = 2 * money;
        
        Assert.Equal(Currency.USD, result.Currency);
        Assert.Equal(2, result.Amount);
    }

    [Fact]
    public void DivisionLeftOperator_ShouldSuccess()
    {
        var money = new Money(4, Currency.USD);
        
        var result = money / 2;
        
        Assert.Equal(Currency.USD, result.Currency);
        Assert.Equal(2, result.Amount);
    }
}
