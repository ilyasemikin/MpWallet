using MpWallet.Expressions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpWallet.Expressions.Extensions;

internal static class ExpressionExtensions
{
    public static bool HasChild(this Expression expression, Predicate<Expression> predicate)
    {
        return expression switch
        {
            ConstantExpression constant => predicate(constant),
            VariableExpression variable => predicate(variable),
            NegationOperatorExpression negotion => predicate(negotion) || predicate(negotion.Argument),
            AdditionOperatorExpression addition => predicate(addition) || addition.Augend.HasChild(predicate) || addition.Addend.HasChild(predicate),
            MultiplicationOperationExpression multiplication => predicate(multiplication) || multiplication.Multiplier.HasChild(predicate) || multiplication.Multiplicand.HasChild(predicate),
            FunctionCallExpression call => predicate(call),
            _ => throw new ArgumentOutOfRangeException(nameof(expression), expression.GetType(), $"Unktnown implementation type of interface \"{nameof(Expression)}\"")
        };
    }

    public static bool HasChild<TExpression>(this Expression expression)
        where TExpression : Expression
    {
        return expression.HasChild(e => e is TExpression);
    }

    public static bool HasChild<TExpression>(this Expression expression, Predicate<TExpression> predicate)
        where TExpression : Expression
    {
        return expression.HasChild(e => e is TExpression concrete && predicate(concrete));
    }
}
