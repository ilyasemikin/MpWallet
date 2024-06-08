using System.Collections;
using System.Reflection;

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

    [Fact]
    public void CurrencyAll_ShouldEnumerableContainsAll()
    {
        var currenciesProperties = GetCurrencies();
        var currenciesAll = Currency.All;

        Assert.NotStrictEqual(currenciesProperties, (IEnumerable<Currency>)currenciesAll);
    }
}