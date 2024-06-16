using MpWallet.Currencies.Services.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace MpWallet.Currencies.Services.Implementations;

public class CurrencyRatioProvider : ICurrencyRatioProvider
{
    private readonly IDictionary<CurrencyRatio, decimal> _rations;

    public CurrencyRatioProvider(IEnumerable<KeyValuePair<CurrencyRatio, decimal>> pairs)
    {
        _rations = new Dictionary<CurrencyRatio, decimal>();
        foreach (var (ratio, value) in pairs)
        {
            if (ratio.Antecedent == ratio.Consequent && value != 1)
                throw new InvalidOperationException($"{ratio} ratio cannot be differ from 1");

            if (!_rations.TryAdd(ratio, value))
                throw new InvalidOperationException($"Ratio \"{ratio}\" already added");
        }
    }

    public bool TryGet(CurrencyRatio ratio, [NotNullWhen(true)] out decimal? value)
    {
        if (ratio.Antecedent == ratio.Consequent)
        {
            value = 1;
            return true;
        }

        if (_rations.TryGetValue(ratio, out var ratioValue))
        {
            value = ratioValue;
            return true;
        }

        value = null;
        return false;
    }
}
