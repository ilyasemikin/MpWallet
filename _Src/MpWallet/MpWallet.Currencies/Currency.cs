using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpWallet.Currencies;

public sealed record Currency
{
    public string Code { get; }
    public string Symbol { get; }

    private Currency(string code, string symbol)
    {
        Code = code;
        Symbol = symbol;
    }

    public static Currency USD => new("USD", "$");
    public static Currency EUR => new("EUR", "€");
    public static Currency GBP => new("GBP", "£");
    public static Currency CHF => new("CHF", "₣");
    public static Currency CAD => new("CAD", "CA$");
    public static Currency AED => new("AED", "DH");
    public static Currency BYN => new("BYN", "Br");
    public static Currency RUB => new("RUB", "₽");

    public static CurrenciesCollection All { get; }

    static Currency()
    {
        var currencies = CollectCurrencies();
        All = new CurrenciesCollection(currencies);
        return;

        static IEnumerable<Currency> CollectCurrencies()
        {
            yield return USD;
            yield return EUR;
            yield return GBP;
            yield return CHF;
            yield return CAD;
            yield return AED;
            yield return BYN;
            yield return RUB;
        }
    }

    public override int GetHashCode()
    {
        return Code.GetHashCode();
    }

    public override string ToString()
    {
        return Code;
    }
}
