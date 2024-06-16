using MpWallet.Currencies;
using MpWallet.Values.Abstractions;
using MpWallet.Values.Implementations;
using MpWallet.Values.UnitTests.TestCases.BinaryOperations.Abstractions;

namespace MpWallet.Values.UnitTests.TestCases.BinaryOperations;

public class SubtractOperationValueTestCases : IOperationValueTestCases
{
    public static IEnumerable<object[]> GenerateSuccessCases()
    {
        {
            var left = new Number(1);
            var right = new Number(2);

            yield return GenerateCase(left, right, new Number(-1));
            yield return GenerateCase(right, left, new Number(1));
        }

        {
            var left = new Number(6);
            var right = new Number(2);

            yield return GenerateCase(left, right, new Number(4));
            yield return GenerateCase(right, left, new Number(-4));
        }

        {
            var left = new Money(1, Currency.EUR);
            var right = new Money(2, Currency.EUR);

            yield return GenerateCase(left, right, new Money(-1, Currency.EUR));
            yield return GenerateCase(right, left, new Money(1, Currency.EUR));
        }

        {
            var left = new Money(6, Currency.USD);
            var right = new Money(4, Currency.USD);

            yield return GenerateCase(left, right, new Money(2, Currency.USD));
            yield return GenerateCase(right, left, new Money(-2, Currency.USD));
        }
        
        yield break;

        object[] GenerateCase(Value left, Value right, Value expected)
        {
            return [(BinaryOperationValueTests.BinaryOperation)Value.TrySubtract, left, right, expected];
        }
    }

    public static IEnumerable<object[]> GenerateFailureCases()
    {
        {
            var left = new Number(1);
            var right = new Money(2, Currency.EUR);

            yield return GenerateCase(left, right);
            yield return GenerateCase(right, left);
        }

        {
            var left = new Money(1, Currency.EUR);
            var right = new Money(2, Currency.USD);

            yield return GenerateCase(left, right);
            yield return GenerateCase(right, left);
        }
        
        yield break;

        object[] GenerateCase(Value left, Value right)
        {
            return [(BinaryOperationValueTests.BinaryOperation)Value.TrySubtract, left, right];
        }
    }
}