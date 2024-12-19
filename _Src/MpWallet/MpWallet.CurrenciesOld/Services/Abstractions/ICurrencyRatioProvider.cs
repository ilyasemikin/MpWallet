using System.Diagnostics.CodeAnalysis;

namespace MpWallet.Currencies.Services.Abstractions;

public interface ICurrencyRatioProvider
{
    bool TryGet(CurrencyRatio ratio, [NotNullWhen(true)] out decimal? value);
}
