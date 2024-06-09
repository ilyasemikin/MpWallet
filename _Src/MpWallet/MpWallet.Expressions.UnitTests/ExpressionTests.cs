using MpWallet.Expressions.Abstractions;
using MpWallet.Values;
using Xunit;

namespace MpWallet.Expressions.UnitTests;

public class ExpressionTests
{
    [Fact]
    public static void UnaryNegation_ShouldReturnExpressionOfNegation()
    {
        var number = new Number(1);
        Expression expression = new ConstantExpression(number);

        var result = -expression;

        Assert.True(result is NegationOperatorExpression negation && negation.Argument == expression);
    }
    
    public static IEnumerable<object[]> BinaryOperatorReturnExpressionOfType
    {
        get
        {
            yield return
            [
                (Expression l, Expression r) => l + r,
                (Expression result, Expression l, Expression r) =>
                    result is AdditionOperatorExpression addition && addition.Augend == l && addition.Addend == r
            ];

            yield return
            [
                (Expression l, Expression r) => l - r,
                (Expression result, Expression l, Expression r) =>
                    result is SubtractionOperationExpression subtraction &&
                    subtraction.Minuend == l && subtraction.Subtrahend == r
            ];
            
            yield return
            [
                (Expression l, Expression r) => l * r,
                (Expression result, Expression l, Expression r) =>
                    result is MultiplicationOperationExpression multiplication && 
                    multiplication.Multiplier == l && multiplication.Multiplicand == r
            ];
            
            yield return
            [
                (Expression l, Expression r) => l / r,
                (Expression result, Expression l, Expression r) =>
                    result is DivisionOperatorExpression division && 
                    division.Numerator == l && division.Denominator == r
            ];
        }
    }

    [Theory]
    [MemberData(nameof(BinaryOperatorReturnExpressionOfType))]
    public void BinaryOperator_ShouldReturnExpressionOfType(
        Func<Expression, Expression, Expression> @operator,
        Func<Expression, Expression, Expression, bool> predicate)
    {
        var value = new Number(1);
        Expression left = new ConstantExpression(value);
        Expression right = new ConstantExpression(value);

        var result = @operator(left, right);

        Assert.True(predicate(result, left, right));
    }
}