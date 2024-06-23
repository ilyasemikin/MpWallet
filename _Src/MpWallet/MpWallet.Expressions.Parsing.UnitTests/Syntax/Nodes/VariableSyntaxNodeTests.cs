using MpWallet.Expressions.Parsing.Syntax;
using MpWallet.Expressions.Parsing.Syntax.Nodes;

namespace MpWallet.Expressions.Parsing.UnitTests.Syntax.Nodes;

public sealed class VariableSyntaxNodeTests
{
    private const string Input = "variable";

    public static IEnumerable<object[]> ConstructorSuccessCases
    {
        get
        {
            yield return [new Token(Input, 0, Input.Length)];
            yield return [new Token(Input, 1, Input.Length)];
            yield return [new Token(Input, 0, Input.Length - 1)];
        }
    }
    
    [Theory]
    [MemberData(nameof(ConstructorSuccessCases))]
    public void Constructor_ShouldSuccess_WhenArgumentCorrect(Token token)
    {
        var node = new VariableSyntaxNode(token);

        Assert.NotNull(node);
        Assert.Equal(token, node.Token);
        Assert.Equal(token.Value, node.Name);
    }

    [Fact]
    public void Constructor_ShouldThrowNullException_WhenArgumentIsNull()
    {
        var exception = Record.Exception(() => new VariableSyntaxNode(null!));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal("token", ((ArgumentNullException)exception).ParamName);
    }
}