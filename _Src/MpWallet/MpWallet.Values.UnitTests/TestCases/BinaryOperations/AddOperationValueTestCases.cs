using MpWallet.Currencies;
using MpWallet.Values.Abstractions;
using MpWallet.Values.Implementations;
using MpWallet.Values.UnitTests.TestCases.BinaryOperations.Abstractions;

namespace MpWallet.Values.UnitTests.TestCases.BinaryOperations;

public sealed class AddOperationValueTestCases : IOperationValueTestCases
{
    public static IEnumerable<object[]> GenerateSuccessCases()
    {
        {
            var left = new Number(1);
            var right = new Number(2);
            var expected = new Number(3);
            
            yield return GenerateCase(left, right, expected);
            yield return GenerateCase(right, left, expected);
        }

        {
            var left = new Number(2);
            var right = new Number(4);
            var expected = new Number(6);
            
            yield return GenerateCase(left, right, expected);
            yield return GenerateCase(right, left, expected);
        }

        {
            var left = new Money(5, Currency.EUR);
            var right = new Money(2, Currency.EUR);
            var expected = new Money(7, Currency.EUR);
            
            yield return GenerateCase(left, right, expected);
            yield return GenerateCase(right, left, expected);
        }
        
        yield break;

        static object[] GenerateCase(Value left, Value right, Value expected)
        {
            return [(BinaryOperationValueTests.BinaryOperation)Value.TryAdd, left, right, expected];
        }
    }

    public static IEnumerable<object[]> GenerateFailureCases()
    {
        {
            var left = new Number(1);
            var right = new Money(1, Currency.EUR);
            
            yield return GenerateCase(left, right);
            yield return GenerateCase(right, left);
        }

        {
            var left = new Money(1, Currency.AED);
            var right = new Money(2, Currency.BYN);

            yield return GenerateCase(left, right);
            yield return GenerateCase(right, left);
        }
        
        yield break;

        static object[] GenerateCase(Value left, Value right)
        {
            return [(BinaryOperationValueTests.BinaryOperation)Value.TryAdd, left, right];
        }
    }
}