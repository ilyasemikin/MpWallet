namespace MpWallet.Currencies;

public sealed class CurrencyRatio : IEquatable<CurrencyRatio>
{
    public Currency Antecedent { get; }
    public Currency Consequent { get; }

    public CurrencyRatio(Currency antecedent, Currency consequent)
    {
        ArgumentNullException.ThrowIfNull(antecedent);
        ArgumentNullException.ThrowIfNull(consequent);
        
        Antecedent = antecedent;
        Consequent = consequent;
    }
    
    public bool Equals(CurrencyRatio? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;
        return Antecedent.Equals(other.Antecedent) && Consequent.Equals(other.Consequent);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is CurrencyRatio other && Equals(other);
    }
    
    public override string ToString()
    {
        return $"{{{Antecedent} : {Consequent}}}";
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Antecedent, Consequent);
    }
}