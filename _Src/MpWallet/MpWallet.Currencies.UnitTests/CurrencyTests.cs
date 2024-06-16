using System.Reflection;
using MpWallet.Currencies.Extensions;

namespace MpWallet.Currencies.UnitTests;

public sealed class CurrencyTests
{
    private static Currency[] GetCurrencies()
    {
        var currencies = typeof(Currency)
            .GetProperties(BindingFlags.Static | BindingFlags.Public)
            .Where(p => p.Name.All(char.IsUpper) && p.PropertyType == typeof(Currency))
            .Select(p => (Currency?)p.GetValue(null, null))
            .ToArray();

        if (currencies.Any(currency => currency is null))
            throw new InvalidOperationException();
        
        return (Currency[])currencies;
    }

    public static IEnumerable<object[]> CurrencyPropertiesTestCases =>
        GetCurrencies().Select(currency => new object[] { currency });
    
    [Theory]
    [MemberData(nameof(CurrencyPropertiesTestCases))]
    public void CurrencyProperties_ShouldPropertiesNotNull(Currency currency)
    {
        Assert.NotEmpty(currency.Code);
        Assert.NotEmpty(currency.Symbol);
    }

    [Fact]
    public void CurrencyAll_ShouldContainsAllCurrencies()
    {
        var currenciesProperties = GetCurrencies();
        var currenciesAll = Currency.All;
        
        Assert.Equal(currenciesProperties.Length, currenciesAll.Count);

        foreach (var currency in currenciesProperties)
        {
            var result = currenciesAll.TryGetByCode(currency.Code, out var allCurrency);
            
            Assert.True(result);
            Assert.Equal(currency, allCurrency);
        }

        foreach (var currency in currenciesAll)
        {
            var result = currenciesAll.TryGetBySymbol(currency.Symbol, out var allCurrency);
            
            Assert.True(result);
            Assert.Equal(currency, allCurrency);
        }
    }

    public static IEnumerable<object[]> TryGetByCodeUsdDifferentCases
    {
        get
        {
            yield return ["USD"];
            yield return ["Usd"];
            yield return ["uSd"];
            yield return ["usD"];
            yield return ["usd"];
        }
    }
    
    [Theory]
    [MemberData(nameof(TryGetByCodeUsdDifferentCases))]
    public void TryGetByCode_ShouldIgnoreCase_WhenPassDifferentCase(string input)
    {
        var result = Currency.All.TryGetByCode(input, out var actual);
        
        Assert.True(result);
        Assert.Equal(Currency.USD, actual);
    }

    public static IEnumerable<object[]> TryGetBySymbolCadDifferentCases
    {
        get
        {
            yield return ["CA$"];
            yield return ["cA$"];
            yield return ["Ca$"];
            yield return ["ca$"];
        }
    }
    
    [Theory]
    [MemberData(nameof(TryGetBySymbolCadDifferentCases))]
    public void TryGetBySymbol_ShouldIgnoreCase_WhenPassDifferentCase(string input)
    {
        var result = Currency.All.TryGetBySymbol(input, out var actual);

        Assert.True(result);
        Assert.Equal(Currency.CAD, actual);
    }

    public static IEnumerable<object[]> TryGetSuccessCases
    {
        get
        {
            var codes = Currency.All.Select(currency => (Value: currency.Code, Currency: currency));
            var symbols = Currency.All.Select(currency => (Value: currency.Symbol, Currency: currency));

            var cases = codes.Concat(symbols).Select(p => new object[] { p.Value, p.Currency });

            foreach (var @case in cases)
                yield return @case;
            
            foreach (var @case in TryGetByCodeUsdDifferentCases)
                yield return [@case[0], Currency.USD];

            foreach (var @case in TryGetBySymbolCadDifferentCases)
                yield return [@case[0], Currency.CAD];
        }
    }
    
    [Theory]
    [MemberData(nameof(TryGetSuccessCases))]
    public void TryGet_ShouldSuccess_WhenPassAnyStringOfCurrency(string input, Currency expected)
    {
        var result = Currency.All.TryGet(input, out var actual);
        
        Assert.True(result);
        Assert.NotNull(actual);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CurrencyAll_ShouldEnumerableContainsAll()
    {
        var currenciesProperties = GetCurrencies();
        var currenciesAll = Currency.All;

        Assert.Equal(currenciesProperties, (IEnumerable<Currency>)currenciesAll);
    }
}