using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MpWallet.Currencies;

namespace MpWallet.ExchangeRates.Abstractions.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ExchangeRateExternalSourceGetterAttribute : Attribute
{
    public CurrencyRatio ProvidedRatio { get; }

    public ExchangeRateExternalSourceGetterAttribute(string antecedentCode, string consequentCode)
    {
        var antecedent = Currency.GetByCode(antecedentCode);
        var consequent = Currency.GetByCode(consequentCode);
        
        ProvidedRatio = new CurrencyRatio(antecedent, consequent);
    }

    public static bool TryExtract(Type type, [NotNullWhen(true)] out CurrencyRatio? ratio)
    {
        ArgumentNullException.ThrowIfNull(type);
        if (!type.IsAssignableTo(typeof(IExchangeRateExternalSourceGetter)) || type.IsAbstract)
        {
            ratio = null;
            return false;
        }

        var attribute = type.GetCustomAttribute<ExchangeRateExternalSourceGetterAttribute>();
        
        ratio = attribute?.ProvidedRatio;
        return ratio is not null;
    }
    
    public static CurrencyRatio Extract(Type type)
    {
        return TryExtract(type, out var ratio)
            ? ratio
            : throw new InvalidOperationException($"Implementation of {nameof(IExchangeRateExternalSourceGetter)} must have {nameof(ExchangeRateExternalSourceGetterAttribute)} attribute");
    }

    public static bool TryExtract<T>([NotNullWhen(true)] out CurrencyRatio? ratio)
        where T : IExchangeRateExternalSourceGetter, new()
    {
        return TryExtract(typeof(T), out ratio);
    }
    
    public static CurrencyRatio Extract<T>()
        where T : IExchangeRateExternalSourceGetter, new()
    {
        return Extract(typeof(T));
    }
    
    public static bool TryExtract<T>(T getter, [NotNullWhen(true)] out CurrencyRatio? ratio)
        where T : IExchangeRateExternalSourceGetter
    {
        return TryExtract(getter.GetType(), out ratio);
    }
    
    public static CurrencyRatio Extract<T>(T getter)
        where T : IExchangeRateExternalSourceGetter
    {
        return Extract(getter.GetType());
    }
}