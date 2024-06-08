using MpWallet.Currencies;
using MpWallet.Values.Abstractions;
using MpWallet.Values.UnitTests.TestCases.BinaryOperations.Abstractions;

namespace MpWallet.Values.UnitTests.TestCases.BinaryOperations;

public class MultipleOperationValueTestCases : IOperationValueTestCases
{
    public static IEnumerable<object[]> GenerateSuccessCases()
    {
        {
            var left = new Number(1);
            var right = new Number(2);
            var expected = new Number(2);

            yield return GenerateCase(left, right, expected);
            yield return GenerateCase(right, left, expected);
        }

        {
            var left = new Number(2);
            var right = new Number(3);
            var expected = new Number(6);

            yield return GenerateCase(left, right, expected);
            yield return GenerateCase(right, left, expected);
        }

        {
            var left = new Number(2);
            var right = new Money(3, Currency.EUR);
            var expected = new Money(6, Currency.EUR);

            yield return GenerateCase(left, right, expected);
            yield return GenerateCase(right, left, expected);
        }
        
        yield break;

        object[] GenerateCase(Value left, Value right, Value expected)
        {
            return [(BinaryOperationValueTests.BinaryOperation)Value.TryMultiple, left, right, expected];
        }
    }

    public static IEnumerable<object[]> GenerateFailureCases()
    {
        {
            var left = new Money(2, Currency.EUR);
            var right = new Money(3, Currency.EUR);

            yield return GenerateCase(left, right);
            yield return GenerateCase(right, left);
        }

        {
            var left = new Money(2, Currency.EUR);
            var right = new Money(3, Currency.USD);

            yield return GenerateCase(left, right);
            yield return GenerateCase(right, left);
        }
        
        yield break;

        object[] GenerateCase(Value left, Value right)
        {
            return [(BinaryOperationValueTests.BinaryOperation)Value.TryMultiple, left, right];
        }
    }
}