using MpWallet.Currencies;
using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Extensions;
using MpWallet.Expressions.UnitTests.Mocks;
using MpWallet.Values.Implementations;
using Xunit;

namespace MpWallet.Expressions.UnitTests;

public sealed class BinaryOperationExpressionTests
{
    public static IEnumerable<object[]> CalculateSuccessSimpleArgumentsTestCases
    {
        get
        {
            {
                object[] Create(
                    Func<Expression, Expression, Expression> expressionFactory,
                    Func<decimal, decimal, decimal> calculation)
                {
                    var number1 = new Number(4);
                    var number2 = new Number(2);
                
                    var numberConstant1 = number1.ToExpression();
                    var numberConstant2 = number2.ToExpression();

                    var expression = expressionFactory(numberConstant1, numberConstant2);
                    var expectedValue = calculation(number1.Value, number2.Value);
                    var expected = new Number(expectedValue).ToExpression();

                    return [expression, Currency.USD, expected];
                }

                yield return Create((l, r) => new AdditionOperatorExpression(l, r), (l, r) => l + r);
                yield return Create((l, r) => new SubtractionOperationExpression(l, r), (l, r) => l - r);
                yield return Create((l, r) => new MultiplicationOperationExpression(l, r), (l, r) => l * r);
                yield return Create((l, r) => new DivisionOperatorExpression(l, r), (l, r) => l / r);
            }

            {
                object[] Create(
                    Func<Expression, Expression, Expression> expressionFactory,
                    KeyValuePair<CurrencyRatio, decimal> pair,
                    Func<decimal, decimal, decimal> calculation)
                {
                    var money1 = new Money(5, pair.Key.Antecedent);
                    var money2 = new Money(2, pair.Key.Antecedent);
                    
                    var moneyConstant1 = money1.ToExpression();
                    var moneyConstant2 = money2.ToExpression();

                    var expression = expressionFactory(moneyConstant1, moneyConstant2);
                    var expectedValue = calculation(money1.Value, money2.Value) * pair.Value;
                    var expected = new Money(expectedValue, pair.Key.Consequent).ToExpression();

                    return [expression, pair.Key.Consequent, expected];
                }
                
                foreach (var pair in MockCurrencyRatioProvider.Ratios)
                {
                    yield return Create((l, r) => new AdditionOperatorExpression(l, r), pair, (l, r) => l + r);
                    yield return Create((l, r) => new SubtractionOperationExpression(l, r), pair, (l, r) => l - r);
                }
            }

            {
                IEnumerable<object[]> Create(
                    Func<Expression, Expression, Expression> expressionFactory,
                    KeyValuePair<CurrencyRatio, decimal> pair,
                    Func<decimal, decimal, decimal> calculation)
                {
                    var number = new Number(5);
                    var money = new Money(2, pair.Key.Antecedent);

                    var numberConstant = number.ToExpression();
                    var moneyConstant = money.ToExpression();

                    var expression = expressionFactory(numberConstant, moneyConstant);
                    var expectedValue = calculation(number.Value, money.Value * pair.Value);
                    var expected = new Money(expectedValue, pair.Key.Consequent).ToExpression();

                    yield return [expression, pair.Key.Consequent, expected];

                    expression = expressionFactory(moneyConstant, numberConstant);
                    expectedValue = calculation(money.Value * pair.Value, number.Value);
                    expected = new Money(expectedValue, pair.Key.Consequent).ToExpression();

                    yield return [expression, pair.Key.Consequent, expected];
                }

                foreach (var pair in MockCurrencyRatioProvider.Ratios)
                {
                    var cases = Create((l, r) => new MultiplicationOperationExpression(l, r), pair, (l, r) => l * r);
                    foreach (var @case in cases)
                        yield return @case;

                    cases = Create((l, r) => new DivisionOperatorExpression(l, r), pair, (l, r) => l / r);
                    foreach (var @case in cases)
                        yield return @case;
                }
            }
        }
    }
    
    [Theory]
    [MemberData(nameof(CalculateSuccessSimpleArgumentsTestCases))]
    public void Calculate_ShouldReturnConstantExpression_WhenArgumentsSimple(
        Expression expression, Currency currency, ConstantExpression expected)
    {
        var result = expression.Calculate(MockCurrencyRatioProvider.ExpressionCalculationContext, currency);
        
        Assert.Equal(expected, result);
    }

    public static IEnumerable<object[]> CalculateSuccessComplexArguments
    {
        get
        {
            {
                var number = new Number(1).ToExpression();
                var money = new Money(2, Currency.USD).ToExpression();

                yield return
                [
                    new AdditionOperatorExpression(number, money),
                    Currency.USD,
                    (Predicate<Expression>)(e => e is AdditionOperatorExpression addition &&
                                      addition.Augend == number &&
                                      addition.Addend == money)
                ];

                yield return
                [
                    new AdditionOperatorExpression(money, number),
                    Currency.USD,
                    (Predicate<Expression>)(e => e is AdditionOperatorExpression addition &&
                                      addition.Augend == money &&
                                      addition.Addend == number)
                ];
                
                yield return
                [
                    new SubtractionOperationExpression(number, money),
                    Currency.USD,
                    (Predicate<Expression>)(e => e is SubtractionOperationExpression subtraction &&
                          subtraction.Minuend == number &&
                          subtraction.Subtrahend == money)
                ];

                yield return
                [
                    new SubtractionOperationExpression(money, number),
                    Currency.USD,
                    (Predicate<Expression>)(e => e is SubtractionOperationExpression subtraction &&
                                      subtraction.Minuend == money &&
                                      subtraction.Subtrahend == number)
                ];
            }

            {
                var money1 = new Money(1, Currency.USD).ToExpression();
                var money2 = new Money(1, Currency.USD).ToExpression();

                yield return
                [
                    new MultiplicationOperationExpression(money1, money2),
                    Currency.USD,
                    (Predicate<Expression>)(e => e is MultiplicationOperationExpression multiplication &&
                                                 multiplication.Multiplier == money1 &&
                                                 multiplication.Multiplicand == money2)
                ];
                
                yield return
                [
                    new DivisionOperatorExpression(money1, money2),
                    Currency.USD,
                    (Predicate<Expression>)(e => e is DivisionOperatorExpression multiplication &&
                                                 multiplication.Numerator == money1 &&
                                                 multiplication.Denominator == money2)
                ];
            }
        }
    }
    
    [Theory]
    [MemberData(nameof(CalculateSuccessComplexArguments))]
    public void Calculate_ShouldReturnComplexExpression_WhenArgumentsComplex(
        Expression expression, Currency currency, Predicate<Expression> predicate)
    {
        var result = expression.Calculate(MockCurrencyRatioProvider.ExpressionCalculationContext, currency);

        Assert.True(predicate(result));
    }
}