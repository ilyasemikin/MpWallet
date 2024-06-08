using MpWallet.Currencies;
using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Context;
using MpWallet.Values.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MpWallet.Expressions;

public record ConstantExpression(Value Value) : Expression
{
    public override Expression Calculate(ExpressionCalculationContext context, Currency currency)
    {
        return this;
    }
}
