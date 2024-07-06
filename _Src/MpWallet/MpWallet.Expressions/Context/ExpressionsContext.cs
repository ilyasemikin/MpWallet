using MpWallet.Currencies.Services.Abstractions;
using MpWallet.Expressions.Context.Functions;
using MpWallet.Expressions.Context.Variables;
using MpWallet.Collections.Immutable;
using MpWallet.Currencies.Services.Implementations;

namespace MpWallet.Expressions.Context;

public sealed class ExpressionsContext
{
    public ICurrencyRatioProvider CurrencyRatioProvider { get; }
    public ImmutableCollection<Variable> Variables { get; }
    public ImmutableCollection<Function> Functions { get; }
    
    public ExpressionsContext(
        ICurrencyRatioProvider currencyRatioProvider, 
        ImmutableCollection<Variable>? variables = null,
        ImmutableCollection<Function>? functions = null)
    {
        CurrencyRatioProvider = currencyRatioProvider;
        Variables = variables ?? new ImmutableCollection<Variable>(variable => variable.Name);
        Functions = functions ?? new ImmutableCollection<Function>(function => function.Name);
    }

    public ExpressionsContext WithVariables(params Variable[] variables)
    {
        return new ExpressionsContext(CurrencyRatioProvider, Variables.With(variables), Functions);
    }
    
    public ExpressionsContext WithVariables(ImmutableCollection<Variable> variables)
    {
        return ReferenceEquals(Variables, variables) 
            ? this 
            : new ExpressionsContext(CurrencyRatioProvider, Variables.With(variables), Functions);
    }

    public ExpressionsContext WithFunctions(params Function[] functions)
    {
        return new ExpressionsContext(CurrencyRatioProvider, Variables, Functions.With(functions));
    }
    
    public ExpressionsContext WithFunctions(ImmutableCollection<Function> functions)
    {
        return ReferenceEquals(Functions, functions)
            ? this
            : new ExpressionsContext(CurrencyRatioProvider, Variables, Functions.With(functions));
    }

    public static ExpressionsContext CreateEmpty()
    {
        var currencyRatioProvider = new CurrencyRatioProvider([]);
        return new ExpressionsContext(currencyRatioProvider);
    }
}
