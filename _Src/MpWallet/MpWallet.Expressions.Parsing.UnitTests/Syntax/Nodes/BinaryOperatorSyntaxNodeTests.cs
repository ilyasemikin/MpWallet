using MpWallet.Expressions.Parsing.Syntax;
using MpWallet.Expressions.Parsing.Syntax.Nodes;
using MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;

namespace MpWallet.Expressions.Parsing.UnitTests.Syntax.Nodes;

public class BinaryOperatorSyntaxNodeTests
{
    private const string Input = "1 + 1";
    
    private static readonly Token Token;
    private static readonly SyntaxNode LeftNode;
    private static readonly SyntaxNode RightNode;

    static BinaryOperatorSyntaxNodeTests()
    {
        Token = new Token(Input, 0, Input.Length);
        LeftNode = new NumberSyntaxNode(new Token(Input, 0, 1));
        RightNode = new NumberSyntaxNode(new Token(Input, 4, Input.Length));
    }

    [Fact]
    public void Constructor_ShouldSuccess_WhenArgumentsCorrect()
    {
        BinaryOperatorSyntaxNode? node = null;
        
        var exception = Record.Exception(() => node = new BinaryOperatorSyntaxNode(Token, LeftNode, RightNode));

        Assert.Null(exception);
        Assert.NotNull(node);
        
        Assert.Equal(Token, node.Token);
        Assert.Equal(LeftNode, node.LeftOperand);
        Assert.Equal(RightNode, node.RightOperand);
    }
    
    public static IEnumerable<object?[]> ConstructorArgumentNullCases
    {
        get
        {
            yield return [null, LeftNode, RightNode, "token"];
            yield return [Token, null, RightNode, "left"];
            yield return [Token, LeftNode, null, "right"];
        }
    }
    
    [Theory]
    [MemberData(nameof(ConstructorArgumentNullCases))]
    public void Constructor_ShouldThrowArgumentNull_WhenOneParameterNull(
        Token? token, SyntaxNode? left, SyntaxNode? right, string expectedName)
    {
        var exception = Record.Exception(() => new BinaryOperatorSyntaxNode(token!, left!, right!));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal(expectedName, ((ArgumentNullException)exception).ParamName);
    }
}