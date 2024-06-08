using MpWallet.Currencies.Services.Implementations;

namespace MpWallet.Currencies.UnitTests;

public sealed class CurrencyRatioProviderTests
{
    [Fact]
    public void Constructor_ShouldCorrect_WhenPassDifferentRatios()
    {
        var ratios = new CurrencyRatio[]
        {
            new(Currency.USD, Currency.EUR),
            new(Currency.USD, Currency.AED),
            new(Currency.USD, Currency.BYN),
        };

        var pairs = ratios
            .Select(ratio => new KeyValuePair<CurrencyRatio, decimal>(ratio, 2))
            .ToArray();

         var exception = Record.Exception(() => new CurrencyRatioProvider(pairs));
         
         Assert.Null(exception);
    }
    
    [Fact]
    public void Constructor_ShouldThrow_WhenRatioWithEqualAntecedentAndConsequentDifferentFromZero()
    {
        var ratio = new CurrencyRatio(Currency.USD, Currency.USD);
        var pairs = new KeyValuePair<CurrencyRatio, decimal>[] { new(ratio, 2) };

        var exception = Record.Exception(() => new CurrencyRatioProvider(pairs));

        Assert.NotNull(exception);
        Assert.Contains("ratio cannot be differ from 1", exception.Message);
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenRationsHasDuplicates()
    {
        var ratio = new CurrencyRatio(Currency.EUR, Currency.USD);
        var pairs = new KeyValuePair<CurrencyRatio, decimal>[] { new(ratio, 2), new(ratio, 2) };

        var exception = Record.Exception(() => new CurrencyRatioProvider(pairs));

        Assert.NotNull(exception);
        Assert.Contains("already added", exception.Message);
    }
}