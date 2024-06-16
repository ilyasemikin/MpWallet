using MpWallet.Currencies;
using MpWallet.Values.Abstractions;
using MpWallet.Values.Implementations;
using MpWallet.Values.UnitTests.TestCases.BinaryOperations.Abstractions;

namespace MpWallet.Values.UnitTests.TestCases.BinaryOperations;

public class DivideOperationValueTestCases : IOperationValueTestCases
{
    public static IEnumerable<object[]> GenerateSuccessCases()
    {
        {
            var left = new Number(4);
            var right = new Number(2);

            yield return GenerateCase(left, right, new Number(2));
            yield return GenerateCase(right, left, new Number(0.5M));
        }

        {
            var left = new Number(5);
            var right = new Number(2);

            yield return GenerateCase(left, right, new Number(2.5M));
            yield return GenerateCase(right, left, new Number(0.4M));
        }

        {
            var left = new Number(5);
            var right = new Money(2, Currency.EUR);

            yield return GenerateCase(left, right, new Money(2.5M, Currency.EUR));
            yield return GenerateCase(right, left, new Money(0.4M, Currency.EUR));
        }
        
        yield break;
        
        object[] GenerateCase(Value left, Value right, Value expected)
        {
            return [(BinaryOperationValueTests.BinaryOperation)Value.TryDivide, left, right, expected];
        }
    }

    public static IEnumerable<object[]> GenerateFailureCases()
    {
        {
            var left = new Money(1, Currency.EUR);
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
            return [(BinaryOperationValueTests.BinaryOperation)Value.TryDivide, left, right];
        }
    }
}