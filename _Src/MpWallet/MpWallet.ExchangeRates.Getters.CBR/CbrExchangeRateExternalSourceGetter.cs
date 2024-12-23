using System.Net;
using MpWallet.CBR.Client;
using MpWallet.Currencies;
using MpWallet.ExchangeRates.Abstractions;
using MpWallet.Time.Abstractions;

namespace MpWallet.ExchangeRates.Getters.CBR;

public class CbrExchangeRateExternalSourceGetter : BaseExchangeRateExternalSourceGetter
{
    private readonly CbrClient _client;
    private readonly IClock _clock;
    
    public Currency Currency { get; }
    
    public CbrExchangeRateExternalSourceGetter(Currency currency, IClock clock) 
        : base(new CurrencyRatio(currency, Currency.RUB))
    {
        ArgumentNullException.ThrowIfNull(currency);
        
        _client = new CbrClient();
        _clock = clock;
        
        Currency = currency;
    }

    public override async Task<decimal> GetAsync(CancellationToken cancellationToken = default)
    {
        var now = _clock.Get();

        var response = await _client.GetCursOnDateAsync(now, cancellationToken);
        if (response.StatusCode is not HttpStatusCode.OK || !response.TryGetBody(out var body))
            throw new Exception();

        var curses = body.Result.Data.Valutes;
        var curs = curses.FirstOrDefault(curs => curs.ChCode == Currency.Code);
        if (curs is null)
            throw new Exception();

        return curs.UnitRate;
    }
}