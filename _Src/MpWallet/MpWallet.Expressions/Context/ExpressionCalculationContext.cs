using MpWallet.Currencies.Services.Abstractions;
using MpWallet.Expressions.Context.Functions;
using MpWallet.Expressions.Context.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpWallet.Expressions.Context;

public sealed class ExpressionCalculationContext
{
    public required ICurrencyRatioProvider CurrencyRatioProvider { get; init; }
    public required ImmutableVariablesCollection Variables { get; init; }
    public required ImmutableFunctionsCollection Functions { get; init; }

    public ExpressionCalculationContext WithVariables(ImmutableVariablesCollection variables)
    {
        return new ExpressionCalculationContext
        {
            CurrencyRatioProvider = CurrencyRatioProvider,
            Functions = Functions,
            Variables = variables
        };
    }

    public ExpressionCalculationContext WithFunctions(ImmutableFunctionsCollection functions)
    {
        return new ExpressionCalculationContext
        {
            CurrencyRatioProvider = CurrencyRatioProvider,
            Variables = Variables,
            Functions = functions
        };
    }
}
