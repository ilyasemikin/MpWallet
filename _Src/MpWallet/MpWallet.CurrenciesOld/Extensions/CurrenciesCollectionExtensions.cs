using System.Diagnostics.CodeAnalysis;

namespace MpWallet.Currencies.Extensions;

public static class CurrenciesCollectionExtensions
{
    public static bool TryGet(
        this CurrenciesCollection collection, 
        string input, 
        [NotNullWhen(true)] out Currency? currency)
    {
        return collection.TryGetByCode(input, out currency) || collection.TryGetBySymbol(input, out currency);
    }
}