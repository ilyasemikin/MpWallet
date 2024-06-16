using MpWallet.Values.Abstractions;
using System.Diagnostics.CodeAnalysis;
using MpWallet.Currencies;
using MpWallet.Values.Implementations;
using MpWallet.Values.UnitTests.TestCases.BinaryOperations;

namespace MpWallet.Values.UnitTests;

public class BinaryOperationValueTests
{
    public delegate bool BinaryOperation(Value left, Value right, [NotNullWhen(true)] out Value? result);
    
    [Theory]
    [MemberData(nameof(AddOperationValueTestCases.GenerateSuccessCases), MemberType = typeof(AddOperationValueTestCases))]
    [MemberData(nameof(SubtractOperationValueTestCases.GenerateSuccessCases), MemberType = typeof(SubtractOperationValueTestCases))]
    [MemberData(nameof(MultipleOperationValueTestCases.GenerateSuccessCases), MemberType = typeof(MultipleOperationValueTestCases))]
    [MemberData(nameof(DivideOperationValueTestCases.GenerateSuccessCases), MemberType = typeof(DivideOperationValueTestCases))]
    public void Operation_ShouldSuccess(BinaryOperation operation, Value left, Value right, Value expected)
    {
        var result = operation(left, right, out var actual);
        
        Assert.True(result);
        Assert.NotNull(actual);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(AddOperationValueTestCases.GenerateFailureCases), MemberType = typeof(AddOperationValueTestCases))]
    [MemberData(nameof(SubtractOperationValueTestCases.GenerateFailureCases), MemberType = typeof(SubtractOperationValueTestCases))]
    [MemberData(nameof(MultipleOperationValueTestCases.GenerateFailureCases), MemberType = typeof(MultipleOperationValueTestCases))]
    [MemberData(nameof(DivideOperationValueTestCases.GenerateFailureCases), MemberType = typeof(DivideOperationValueTestCases))]
    public void Operation_ShouldFailure(BinaryOperation operation, Value left, Value right)
    {
        var result = operation(left, right, out var actual);
        
        Assert.False(result);
        Assert.Null(actual);
    }

    [Fact]
    public void TryDivide_ShouldThrow_WhenDivideByZero()
    {
        var numberLeft = new Number(1);
        var moneyLeft = new Money(1, Currency.USD);
        
        var right = new Number(0);

        Assert.Throws<DivideByZeroException>(() => Value.TryDivide(numberLeft, right, out _));
        Assert.Throws<DivideByZeroException>(() => Value.TryDivide(moneyLeft, right, out _));
    }
}