namespace MpWallet.Expressions.Operators.UnitTests;

public sealed class OperatorTests
{
    private const string Value = "+";

    private static readonly OperatorDetails Details = new(10, OperatorAssociativity.Left, OperatorArity.Binary);
    
    [Fact]
    public void Constructor_ShouldSuccess_WhenPassCorrectArguments()
    {
        Operator? @operator = null;
        
        var exception = Record.Exception(() => @operator = new Operator(Value, Details));
        
        Assert.Null(exception);
        Assert.NotNull(@operator);
        Assert.Equal(Value, @operator.Value);
        Assert.Equal(Details, @operator.Details);
    }

    public static IEnumerable<object?[]> ConstructorOneOfArgumentNullCases
    {
        get
        {
            yield return [null, Details, "value"];
            yield return [Value, null, "details"];
        }
    }
    
    [Theory]
    [MemberData(nameof(ConstructorOneOfArgumentNullCases))]
    public void Constructor_ShouldThrowArgumentNullException_WhenOneArgumentNull(
        string? value, OperatorDetails? details, string expectedParamName)
    {
        var exception = Record.Exception(() => new Operator(value!, details!));

        Assert.NotNull(exception);
        Assert.Equal(expectedParamName, ((ArgumentNullException)exception).ParamName);
    }
}