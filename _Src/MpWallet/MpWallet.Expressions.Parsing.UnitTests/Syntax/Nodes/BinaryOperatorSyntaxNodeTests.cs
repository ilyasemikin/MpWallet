using MpWallet.Expressions.Operators;
using MpWallet.Expressions.Parsing.Syntax;
using MpWallet.Expressions.Parsing.Syntax.Nodes;
using MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;

namespace MpWallet.Expressions.Parsing.UnitTests.Syntax.Nodes;

public sealed class BinaryOperatorSyntaxNodeTests
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

    public static IEnumerable<object[]> ConstructorSuccessCases =>
        Operator.All.Select(@operator => new object[] { @operator });

    [Theory]
    [MemberData(nameof(ConstructorSuccessCases))]
    public void Constructor_ShouldSuccess_WhenArgumentsCorrect(Operator @operator)
    {
        var node = new BinaryOperatorSyntaxNode(Token, @operator, LeftNode, RightNode);
        
        Assert.Equal(Token, node.Token);
        Assert.Equal(@operator, node.Operator);
        Assert.Equal(LeftNode, node.LeftOperand);
        Assert.Equal(RightNode, node.RightOperand);
    }
    
    public static IEnumerable<object?[]> ConstructorArgumentNullCases
    {
        get
        {
            var @operator = Operator.All.First();
            
            yield return [null, @operator, LeftNode, RightNode, "token"];
            yield return [Token, null, LeftNode, RightNode, "@operator"];
            yield return [Token, @operator, null, RightNode, "left"];
            yield return [Token, @operator, LeftNode, null, "right"];
        }
    }
    
    [Theory]
    [MemberData(nameof(ConstructorArgumentNullCases))]
    public void Constructor_ShouldThrowArgumentNull_WhenOneParameterNull(
        Token? token, Operator? @operator, SyntaxNode? left, SyntaxNode? right, string expectedName)
    {
        var exception = Record.Exception(() => new BinaryOperatorSyntaxNode(token!, @operator!, left!, right!));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal(expectedName, ((ArgumentNullException)exception).ParamName);
    }
}