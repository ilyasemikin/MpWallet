using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        
        _currenciesByCode = _currencies.ToDictionary(c => c.Code);
        _currenciesBySymbol = _currencies.ToDictionary(c => c.Symbol);
    }

    public bool TryGetByCode(string code, [NotNullWhen(true)] out Currency? currency)
    {
        return _currenciesByCode.TryGetValue(code, out currency);
    }

    public bool TryGetBySymbol(string code, [NotNullWhen(true)] out Currency? currency)
    {
        return _currenciesBySymbol.TryGetValue(code, out currency);
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
