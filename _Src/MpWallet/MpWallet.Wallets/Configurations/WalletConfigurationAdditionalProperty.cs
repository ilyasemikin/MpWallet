namespace MpWallet.Wallets.Configurations;

public sealed class WalletConfigurationAdditionalProperty : IEquatable<WalletConfigurationAdditionalProperty>
{
    public const int MaxKeyLength = 128;
    
    public string Key { get; }
    public string? Value { get; }

    public WalletConfigurationAdditionalProperty(string key, string? value)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(key.Length, MaxKeyLength);

        Key = key;
        Value = value;
    }

    public bool Equals(WalletConfigurationAdditionalProperty? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;
        return Key == other.Key && Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is WalletConfigurationAdditionalProperty other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Key, Value);
    }
}