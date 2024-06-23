using System.Diagnostics.CodeAnalysis;
using MpWallet.Collections.Immutable.Extensions;
using MpWallet.Currencies;
using MpWallet.Currencies.Services.Abstractions;
using MpWallet.Expressions.Context;
using MpWallet.Expressions.Context.Functions;
using MpWallet.Expressions.Context.Variables;

namespace MpWallet.Expressions.UnitTests.Mocks;

public sealed class MockCurrencyRatioProvider : ICurrencyRatioProvider
{
    public static ICurrencyRatioProvider Instance => new MockCurrencyRatioProvider();

    public static ExpressionCalculationContext ExpressionCalculationContext => new(Instance);
    
    public static IReadOnlyDictionary<CurrencyRatio, decimal> Ratios { get; } = new Dictionary<CurrencyRatio, decimal>
    {
        [new CurrencyRatio(Currency.EUR, Currency.USD)] = 1.08M,
        [new CurrencyRatio(Currency.USD, Currency.EUR)] = 0.92M,
        [new CurrencyRatio(Currency.EUR, Currency.RUB)] = 96.63M,
        [new CurrencyRatio(Currency.RUB, Currency.EUR)] = 0.01M,
        [new CurrencyRatio(Currency.USD, Currency.RUB)] = 89.44M,
        [new CurrencyRatio(Currency.RUB, Currency.USD)] = 0.011M
    };
    
    public bool TryGet(CurrencyRatio ratio, [NotNullWhen(true)] out decimal? value)
    {
        value = null;
        if (!Ratios.TryGetValue(ratio, out var nullableValue))
            return false;

        value = nullableValue;
        return true;
    }

    public static ExpressionCalculationContext CreateExpressionCalculationContext(
        IEnumerable<Variable>? variables = null, 
        IEnumerable<Function>? functions = null)
    {
        var variablesImmutableCollection = variables?.ToImmutableCollection();
        var functionsImmutableCollection = functions?.ToImmutableCollection();
        return new ExpressionCalculationContext(Instance, variablesImmutableCollection, functionsImmutableCollection);
    }
}