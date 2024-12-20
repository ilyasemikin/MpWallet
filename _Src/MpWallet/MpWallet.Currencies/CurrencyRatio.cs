namespace MpWallet.Currencies;

public sealed class CurrencyRatio
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

    public override string ToString()
    {
        return $"{{{Antecedent} : {Consequent}}}";
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Antecedent, Consequent);
    }
}