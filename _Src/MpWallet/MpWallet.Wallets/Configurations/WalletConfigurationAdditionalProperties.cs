using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace MpWallet.Wallets.Configurations;

public sealed class WalletConfigurationAdditionalProperties : 
    IEnumerable<WalletConfigurationAdditionalProperty>, 
    IEquatable<WalletConfigurationAdditionalProperties>
{
    private readonly IReadOnlyDictionary<string, WalletConfigurationAdditionalProperty> _values;

    public int Count => _values.Count;
    
    public static WalletConfigurationAdditionalProperties Empty { get; } = new([]);
    
    public WalletConfigurationAdditionalProperties(IEnumerable<WalletConfigurationAdditionalProperty> properties)
    {
        _values = properties.ToDictionary(p => p.Key);
    }

    public bool TryGet(string key, [NotNullWhen(true)] out WalletConfigurationAdditionalProperty? value)
    {
        return _values.TryGetValue(key, out value);
    }
    
    public IEnumerator<WalletConfigurationAdditionalProperty> GetEnumerator()
    {
        return _values.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public bool Equals(WalletConfigurationAdditionalProperties? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;

        if (Count != other.Count)
            return false;

        var values = _values.Values.OrderBy(p => p.Key);
        var otherValues = _values.Values.OrderBy(p => p.Key);

        return values.SequenceEqual(otherValues);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is WalletConfigurationAdditionalProperties other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _values.GetHashCode();
    }
}