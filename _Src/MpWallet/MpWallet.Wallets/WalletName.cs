namespace MpWallet.Wallets;

public sealed class WalletName : IEquatable<WalletName>
{
    public const int MaxLength = 128;
    
    private readonly string _value;

    public WalletName(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value.Length, MaxLength);
        
        _value = value;
    }

    public bool Equals(WalletName? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is WalletName other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public override string ToString()
    {
        return _value;
    }

    public static bool operator ==(WalletName left, WalletName right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(WalletName left, WalletName right)
    {
        return !Equals(left, right);
    }
}