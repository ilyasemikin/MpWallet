namespace MpWallet.Operators.UnitTests;

public sealed class OperatorDetailsTests
{
    [Fact]
    public void Constructor_ShouldSuccess_WhenArgumentsCorrect()
    {
        const int priority = 10;
        const OperatorAssociativity associativity = OperatorAssociativity.Right;
        const OperatorArity arity = OperatorArity.Binary;
        
        OperatorDetails? details = null;

        var exception = Record.Exception(() => details = new OperatorDetails(priority, associativity, arity));
        
        Assert.Null(exception);
        Assert.NotNull(details);
        
        Assert.Equal(priority, details.Priority);
        Assert.Equal(associativity, details.Associativity);
        Assert.Equal(arity, details.Arity);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentOutOfRange_WhenPriorityNegative()
    {
        var exception = Record.Exception(
            () => new OperatorDetails(-1, OperatorAssociativity.Left, OperatorArity.Binary));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentOutOfRangeException>(exception);

        Assert.Equal("priority", ((ArgumentOutOfRangeException)exception).ParamName);
    }
}