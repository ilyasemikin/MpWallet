namespace MpWallet.Values.UnitTests.TestCases.BinaryOperations.Abstractions;

public interface IOperationValueTestCases
{
    static abstract IEnumerable<object[]> GenerateSuccessCases();
    static abstract IEnumerable<object[]> GenerateFailureCases();
}