using MpWallet.Currencies;
using MpWallet.Time.Implementations;

namespace MpWallet.ExchangeRates.Getters.CBR.IntegrationTests;

public sealed class CbrExchangeRateExternalSourceGetterTests
{
    [Theory]
    [InlineData(Currency.Codes.USD, 102.3438)]
    [InlineData(Currency.Codes.EUR, 106.5444)]
    [InlineData(Currency.Codes.GBP, 128.4517)]
    [InlineData(Currency.Codes.CHF, 114.3251)]
    public async Task GetAsync_ShouldSuccess_ForCurrency(string currencyCode, decimal expectedRate)
    {
        var currency = Currency.GetByCode(currencyCode);
        var time = new DateTimeOffset(2024, 12, 23, 0, 0, 0, TimeSpan.Zero);
        var clock = new FixedClock(time);
        var getter = new CbrExchangeRateExternalSourceGetter(currency, clock);

        var expectedRatio = new CurrencyRatio(currency, Currency.RUB);
        
        var rate = await getter.GetAsync();

        Assert.Equal(expectedRatio, getter.ProvidedRatio);
        Assert.Equal(expectedRate, rate);
    }
}
