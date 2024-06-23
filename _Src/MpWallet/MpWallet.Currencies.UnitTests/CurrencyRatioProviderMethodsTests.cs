using MpWallet.Currencies.Services.Abstractions;
using MpWallet.Currencies.Services.Abstractions.Extensions;
using MpWallet.Currencies.Services.Implementations;

namespace MpWallet.Currencies.UnitTests;

public sealed class CurrencyRatioProviderMethodsTests
{
    private static readonly IReadOnlyDictionary<CurrencyRatio, decimal> Ratios;

    private readonly ICurrencyRatioProvider _provider;

    static CurrencyRatioProviderMethodsTests()
    {
        Ratios = new Dictionary<CurrencyRatio, decimal>
        {
            [new CurrencyRatio(Currency.EUR, Currency.USD)] = 1.09M,
            [new CurrencyRatio(Currency.USD, Currency.RUB)] = 89.20M
        };

    }

    public CurrencyRatioProviderMethodsTests()
    {
        _provider = new CurrencyRatioProvider(Ratios);
    }

    public static IEnumerable<object[]> TryGetAlreadyAddedTestCases =>
        Ratios
            .Select(p => new object[] { p.Key, p.Value });

    [Theory]
    [MemberData(nameof(TryGetAlreadyAddedTestCases))]
    public void TryGet_ShouldReturn_WhenRequestsAlreadyAdded(CurrencyRatio ratio, decimal expected)
    {
        var result = _provider.TryGet(ratio, out var actual);

        Assert.True(result);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(TryGetAlreadyAddedTestCases))]
    public void TryGet_ShouldReturn_WhenRequestAlreadyAddedAndPassParts(CurrencyRatio ratio, decimal expected)
    {
        var (antecedent, consequent) = ratio;

        var result = _provider.TryGet(antecedent, consequent, out var actual);

        Assert.True(result);
        Assert.Equal(expected, actual);
    }
    
    [Theory]
    [MemberData(nameof(TryGetAlreadyAddedTestCases))]
    public void Get_ShouldReturn_WhenRequestsAlreadyAdded(CurrencyRatio ratio, decimal expected)
    {
        decimal actual = 0;
        
        var exception = Record.Exception(() => actual = _provider.Get(ratio));

        Assert.Null(exception);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(TryGetAlreadyAddedTestCases))]
    public void Get_ShouldReturn_WhenRequestAlreadyAddedAndPassParts(CurrencyRatio ratio, decimal expected)
    {
        var (antecedent, consequent) = ratio;
        decimal actual = 0;

        var exception = Record.Exception(() => actual = _provider.Get(antecedent, consequent));
        
        Assert.Null(exception);
        Assert.Equal(expected, actual);
    }

    public static IEnumerable<object[]> TryGetEqualPartsTestCases => Currency.All.Select(r => new object[] { r });

    [Theory]
    [MemberData(nameof(TryGetEqualPartsTestCases))]
    public void TryGet_ShouldReturn_WhenRequestsEqualPartsRatio(Currency currency)
    {
        var ratio = new CurrencyRatio(currency, currency);
        var result = _provider.TryGet(ratio, out var actual);
        
        Assert.True(result);
        Assert.Equal(1, actual);
    }
    
    [Theory]
    [MemberData(nameof(TryGetEqualPartsTestCases))]
    public void TryGet_ShouldReturn_WhenRequestsEqualPartsRatioAndPassParts(Currency currency)
    {
        var result = _provider.TryGet(currency, currency, out var actual);
        
        Assert.True(result);
        Assert.Equal(1, actual);
    }
    
    [Theory]
    [MemberData(nameof(TryGetEqualPartsTestCases))]
    public void Get_ShouldReturn_WhenRequestsEqualPartsRatio(Currency currency)
    {
        var ratio = new CurrencyRatio(currency, currency);
        decimal value = 0;

        var exception = Record.Exception(() => value = _provider.Get(ratio));
        
        Assert.Null(exception);
        Assert.Equal(1, value);
    }
    
    [Theory]
    [MemberData(nameof(TryGetEqualPartsTestCases))]
    public void Get_ShouldReturn_WhenRequestsEqualPartsRatioAndPassParts(Currency currency)
    {
        decimal value = 0;

        var exception = Record.Exception(() => value = _provider.Get(currency, currency));
        
        Assert.Null(exception);
        Assert.Equal(1, value);
    }

    public static IEnumerable<object[]> TryGetUnknownTestCases
    {
        get
        {
            yield return [new CurrencyRatio(Currency.AED, Currency.BYN)];
            yield return [new CurrencyRatio(Currency.EUR, Currency.RUB)];
        }
    }
    
    [Theory]
    [MemberData(nameof(TryGetUnknownTestCases))]
    public void TryGet_ShouldNotReturn_WhenRequestsUnknown(CurrencyRatio ratio)
    {
        var result = _provider.TryGet(ratio, out var actual);

        Assert.False(result);
        Assert.Null(actual);
    }

    [Theory]
    [MemberData(nameof(TryGetUnknownTestCases))]
    public void TryGet_ShouldNotReturn_WhenRequestsUnknownAndPassParts(CurrencyRatio ratio)
    {
        var (antecedent, consequent) = ratio;

        var result = _provider.TryGet(antecedent, consequent, out var actual);

        Assert.False(result);
        Assert.Null(actual);
    }

    [Theory]
    [MemberData(nameof(TryGetUnknownTestCases))]
    public void Get_ShouldNotReturn_WhenRequestsUnknown(CurrencyRatio ratio)
    {
        var exception = Record.Exception(() => _provider.Get(ratio));

        Assert.NotNull(exception);
    }
    
    [Theory]
    [MemberData(nameof(TryGetUnknownTestCases))]
    public void Get_ShouldNotReturn_WhenRequestsUnknownAndPassParts(CurrencyRatio ratio)
    {
        var (antecedent, consequent) = ratio;

        var exception = Record.Exception(() => _provider.Get(antecedent, consequent));

        Assert.NotNull(exception);
    }
}