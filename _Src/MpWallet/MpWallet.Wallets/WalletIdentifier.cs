namespace MpWallet.Wallets;

public sealed class WalletIdentifier : IEquatable<WalletIdentifier>
{
    public const int MaxLength = 32;
    
    private readonly string _value;
    
    public WalletIdentifier(string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value.Length, MaxLength);

        _value = value;
    }
    
    public bool Equals(WalletIdentifier? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is WalletIdentifier other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }
    
    public override string ToString()
    {
        return _value;
    }

    public static bool operator ==(WalletIdentifier left, WalletIdentifier right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(WalletIdentifier left, WalletIdentifier right)
    {
        return !Equals(left, right);
    }
}