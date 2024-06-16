using MpWallet.Currencies.Services.Abstractions;
using MpWallet.Expressions.Context.Functions;
using MpWallet.Expressions.Context.Variables;
using MpWallet.Collections.Immutable;

namespace MpWallet.Expressions.Context;

public sealed class ExpressionCalculationContext
{
    public ICurrencyRatioProvider CurrencyRatioProvider { get; }
    public ImmutableCollection<Variable> Variables { get; }
    public ImmutableCollection<Function> Functions { get; }
    
    public ExpressionCalculationContext(
        ICurrencyRatioProvider currencyRatioProvider, 
        ImmutableCollection<Variable>? variables = null,
        ImmutableCollection<Function>? functions = null)
    {
        CurrencyRatioProvider = currencyRatioProvider;
        Variables = variables ?? ImmutableCollection<Variable>.Empty;
        Functions = functions ?? ImmutableCollection<Function>.Empty;
    }

    public ExpressionCalculationContext WithVariables(ImmutableCollection<Variable> variables)
    {
        return ReferenceEquals(Variables, variables) 
            ? this 
            : new ExpressionCalculationContext(CurrencyRatioProvider, variables, Functions);
    }

    public ExpressionCalculationContext WithFunctions(ImmutableCollection<Function> functions)
    {
        return ReferenceEquals(Functions, functions)
            ? this
            : new ExpressionCalculationContext(CurrencyRatioProvider, Variables, functions);
    }
}
