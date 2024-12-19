using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace MpWallet.Currencies;

public sealed class CurrenciesCollection : IEnumerable<Currency>
{
    private readonly IReadOnlyList<Currency> _currencies;
    
    private readonly IReadOnlyDictionary<string, Currency> _currenciesByCode;
    private readonly IReadOnlyDictionary<string, Currency> _currenciesBySymbol;

    public int Count => _currencies.Count;
    
    internal CurrenciesCollection(IEnumerable<Currency> currencies)
    {
        _currencies = currencies.ToArray();
        
        _currenciesByCode = _currencies.ToDictionary(c => c.Code.ToLowerInvariant());
        _currenciesBySymbol = _currencies.ToDictionary(c => c.Symbol.ToLowerInvariant());
    }

    public bool TryGetByCode(string code, [NotNullWhen(true)] out Currency? currency)
    {
        code = code.ToLowerInvariant();
        return _currenciesByCode.TryGetValue(code, out currency);
    }

    public bool TryGetBySymbol(string symbol, [NotNullWhen(true)] out Currency? currency)
    {
        symbol = symbol.ToLowerInvariant();
        return _currenciesBySymbol.TryGetValue(symbol, out currency);
    }
    
    public IEnumerator<Currency> GetEnumerator()
    {
        return _currencies.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
