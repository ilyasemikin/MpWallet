using MpWallet.Currencies;
using MpWallet.Expressions.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpWallet.Expressions.Abstractions;

public abstract record Expression
{
    protected internal Expression()
    {
    }

    public abstract Expression Calculate(ExpressionCalculationContext context, Currency currency);

    public static Expression operator -(Expression argument)
    {
        return new NegationOperatorExpression(argument);
    }

    public static Expression operator +(Expression left, Expression right)
    {
        return new AdditionOperatorExpression(left, right);
    }

    public static Expression operator -(Expression left, Expression right)
    {
        return new SubtractionOperationExpression(left, right);
    }

    public static Expression operator *(Expression left, Expression right)
    {
        return new MultiplicationOperationExpression(left, right);
    }

    public static Expression operator /(Expression left, Expression right)
    {
        return new DivisionOperatorExpression(left, right);
    }
}
