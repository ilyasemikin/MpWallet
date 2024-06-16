using MpWallet.Currencies;
using MpWallet.Currencies.Services.Abstractions;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using MpWallet.Values.Implementations;
using NuGet.Frameworks;

namespace MpWallet.Values.UnitTests;

public sealed class MoneyTests
{
    private sealed class MockCurrencyRatioProvider : ICurrencyRatioProvider
    {
        public const decimal UsdRubRatio = 90M;
        public const decimal EurRubRatio = 98M;
        public const decimal RubUsdRatio = 0.011M;
        public const decimal RubEurRatio = 0.010M;

        private IReadOnlyDictionary<CurrencyRatio, decimal> Rations { get; } = new Dictionary<CurrencyRatio, decimal>
        {
            [new CurrencyRatio(Currency.USD, Currency.RUB)] = UsdRubRatio,
            [new CurrencyRatio(Currency.EUR, Currency.RUB)] = EurRubRatio,
            [new CurrencyRatio(Currency.RUB, Currency.USD)] = RubUsdRatio,
            [new CurrencyRatio(Currency.RUB, Currency.EUR)] = RubEurRatio,
        };

        public bool TryGet(CurrencyRatio ratio, [NotNullWhen(true)] out decimal? value)
        {
            value = null;
            if (Rations.TryGetValue(ratio, out var ratioValue))
                value = ratioValue;

            return value is not null;
        }
    }

    [Fact]
    public void Constructor_ShouldCorrectCreate()
    {
        var value = 1.5M;
        var currency = Currency.USD;

        var money = new Money(value, currency);

        Assert.Equal(value, money.Value);
        Assert.Equal(currency, money.Currency);
    }

    public static IEnumerable<object[]> TryConvertCurrencyCorrectCases =>
        [
            [1M, Currency.USD, 1M * MockCurrencyRatioProvider.UsdRubRatio, Currency.RUB],
            [1M, Currency.EUR, 1M * MockCurrencyRatioProvider.EurRubRatio, Currency.RUB],
            [1M, Currency.RUB, 1M * MockCurrencyRatioProvider.RubUsdRatio, Currency.USD],
            [1M, Currency.RUB, 1M * MockCurrencyRatioProvider.RubEurRatio, Currency.EUR]
        ];

    [Theory]
    [MemberData(nameof(TryConvertCurrencyCorrectCases))]
    public void TryConvertCurrency_ShouldConvert_WhenRatioExists(decimal value, Currency currency, decimal expectedValue, Currency expectedCurrency)
    {
        var currencyRatioGetter = new MockCurrencyRatioProvider();

        var money = new Money(value, currency);

        var result = money.TryConvertCurrency(currencyRatioGetter, expectedCurrency, out var convertedMoney);

        Assert.True(result);
        Assert.NotNull(convertedMoney);

        Assert.Equal(expectedValue, convertedMoney.Value);
        Assert.Equal(expectedCurrency, convertedMoney.Currency);
    }

    [Fact]
    public void TryConvertCurrency_ShouldNotConvert_WhenRatioNotExists()
    {
        var currencyRatioGetter = new MockCurrencyRatioProvider();

        var money = new Money(1M, Currency.RUB);

        var result = money.TryConvertCurrency(currencyRatioGetter, Currency.GBP, out var convertedMoney);

        Assert.False(result);
        Assert.Null(convertedMoney);
    }

    public static IEnumerable<object?[]> FormattableToStringTestCases
    {
        get
        {
            foreach (var currency in Currency.All)
            {
                var value = (decimal)Random.Shared.NextDouble();
                var money = new Money(value, currency);

                yield return [money, null, CultureInfo.InvariantCulture];
                yield return [money, null, CultureInfo.CurrentCulture];
            }
        }
    }

    [Theory]
    [MemberData(nameof(FormattableToStringTestCases))]
    public void FormattableToString_ShouldCorrect(Money money, string format, IFormatProvider formatProvider)
    {
        var expectedString = $"{money.Value.ToString(formatProvider)}{money.Currency.Symbol}";

        var @string = money.ToString(format, formatProvider);

        Assert.Equal(@string, expectedString);
    }

    public static IEnumerable<object[]> TryParseSuccessCases
    {
        get
        {
            foreach (var currency in Currency.All)
            {
                yield return [$"100 {currency.Symbol}", new Money(100M, currency)];
                yield return [$"100 {currency.Code}", new Money(100M, currency)];
            }
            
            yield return ["1.1$", new Money(1.1M, Currency.USD)];
            yield return ["1.1USD", new Money(1.1M, Currency.USD)];
            yield return ["1.1 $", new Money(1.1M, Currency.USD)];
            yield return ["10000.1 $", new Money(10000.1M, Currency.USD)];
            yield return ["1.10000000 $", new Money(1.1M, Currency.USD)];
            yield return ["1.10000001 $", new Money(1.10000001M, Currency.USD)];
            yield return ["1   $", new Money(1, Currency.USD)];
        }
    }
    
    [Theory]
    [MemberData(nameof(TryParseSuccessCases))]
    public void TryParse_ShouldSuccess_WhenPassCorrect(string input, Money expected)
    {
        var result = Money.TryParse(input, out var actual);
        
        Assert.True(result);
        Assert.NotNull(actual);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TryParse_ShouldSuccess_WhenPassCorrectWithFormatProvider()
    {
        var expected = new Money(1.1M, Currency.USD);
        
        var result = Money.TryParse("1,1 $", new CultureInfo("ru-RU"), out var actual);

        Assert.True(result);
        Assert.NotNull(actual);
        Assert.Equal(expected, actual);
    }

    public static IEnumerable<object[]> TryParseFailureCases
    {
        get
        {
            yield return ["1,1 $"];
            yield return ["1.1.1 $"];
            yield return ["1,1,1 $"];
            yield return ["1 1 $"];
            yield return ["1"];
            yield return [string.Empty];
            yield return [".5 $"];
            yield return ["1. $"];
        }
    }
    
    [Theory]
    [MemberData(nameof(TryParseFailureCases))]
    public void TryParse_ShouldFailure_WhenPassInvalid(string input)
    {
        var result = Money.TryParse(input, out var actual);
        
        Assert.False(result);
        Assert.Null(actual);
    }
}
