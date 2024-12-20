using System.Diagnostics.CodeAnalysis;

namespace MpWallet.Currencies;

public sealed class Currency
{
    private static readonly IReadOnlyDictionary<string, Currency> Instances = new Dictionary<string, Currency>
    {
        [Codes.USD] = new(Codes.USD, "$"),
        [Codes.EUR] = new(Codes.EUR, "€"),
        [Codes.GBP] = new(Codes.GBP, "£"),
        [Codes.CHF] = new(Codes.CHF, "₣"),
        [Codes.RUB] = new(Codes.RUB, "₽")
    };
    
    public string Code { get; }
    public string Symbol { get; }

    public static Currency USD => Instances[Codes.USD];
    public static Currency EUR => Instances[Codes.EUR];
    public static Currency GBP => Instances[Codes.GBP];
    public static Currency CHF => Instances[Codes.CHF];
    public static Currency RUB => Instances[Codes.RUB];

    public static IEnumerable<Currency> All => Instances.Values;
    
    private Currency(string code, string symbol)
    {
        Code = code;
        Symbol = symbol;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Code, Symbol);
    }

    public override string ToString()
    {
        return Symbol;
    }

    public static bool TryGetByCode(string code, [NotNullWhen(true)] out Currency? currency)
    {
        ArgumentException.ThrowIfNullOrEmpty(code);
        
        return Instances.TryGetValue(code, out currency);
    }

    public static Currency GetByCode(string code)
    {
        return TryGetByCode(code, out Currency? currency)
            ? currency
            : throw new InvalidOperationException($"Currency \"{code}\" is unknown");
    }

    public static class Codes
    {
        public const string USD = "USD";
        public const string EUR = "EUR";
        public const string GBP = "GBP";
        public const string CHF = "CHF";
        public const string RUB = "RUB";
    }
}
