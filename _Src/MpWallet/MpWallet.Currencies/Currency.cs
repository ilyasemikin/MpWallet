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

    public static Currency USD { get; } = new("USD", "$");
    public static Currency EUR { get; } = new("EUR", "€");
    public static Currency GBP { get; } = new("GBP", "£");
    public static Currency CHF { get; } = new("CHF", "₣");
    public static Currency CAD { get; } = new("CAD", "CA$");
    public static Currency AED { get; } = new("AED", "DH");
    public static Currency BYN { get; } = new("BYN", "Br");
    public static Currency TRY { get; } = new("TRY", "₺");
    public static Currency RUB { get; } = new("RUB", "₽");

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
            yield return TRY;
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
