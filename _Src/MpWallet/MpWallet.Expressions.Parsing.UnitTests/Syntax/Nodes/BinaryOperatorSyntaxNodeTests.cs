using MpWallet.Expressions.Operators;
using MpWallet.Expressions.Parsing.Syntax;
using MpWallet.Expressions.Parsing.Syntax.Extensions;
using MpWallet.Expressions.Parsing.Syntax.Nodes;
using MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;
using MpWallet.Operators;

namespace MpWallet.Expressions.Parsing.UnitTests.Syntax.Nodes;

public sealed class BinaryOperatorSyntaxNodeTests
{
    private const string Input = "1 + 2";
    
    private static readonly Token Token;
    private static readonly Operator Operator;
    private static readonly SyntaxNode LeftNode;
    private static readonly SyntaxNode RightNode;

    static BinaryOperatorSyntaxNodeTests()
    {
        Token = new Token(Input, 2, 3);
        Operator = new Operator("+", new OperatorDetails(0, OperatorAssociativity.Left, OperatorArity.Binary));
        LeftNode = new NumberSyntaxNode(new Token(Input, 0, 1));
        RightNode = new NumberSyntaxNode(new Token(Input, 4, Input.Length));
    }

    [Fact]
    public void Constructor_ShouldSuccess_WhenArgumentsCorrect()
    {
        var node = new BinaryOperatorSyntaxNode(Token, Operator, LeftNode, RightNode);
        
        Assert.Equal(Token, node.Token);
        Assert.Equal(Operator, node.Operator);
        Assert.Equal(LeftNode, node.LeftOperand);
        Assert.Equal(RightNode, node.RightOperand);
    }
    
    public static IEnumerable<object?[]> ConstructorArgumentNullCases
    {
        get
        {
            var @operator = DefaultOperators.Collection.First();
            
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

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenOperatorIsNotBinary()
    {
        var @operator = new Operator("+", new OperatorDetails(0, OperatorAssociativity.Left, OperatorArity.Unary));

        var exception = Record.Exception(() => new BinaryOperatorSyntaxNode(Token, @operator, LeftNode, RightNode));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.StartsWith("Operator must be binary", ((ArgumentException)exception).Message);
        Assert.Equal("operator", ((ArgumentException)exception).ParamName);
    }

    [Fact]
    public void Constructor_ShouldThrowInvalidOperationException_WhenOperatorAndTokenNotEquals()
    {
        var @operator = new Operator("-", new OperatorDetails(0, OperatorAssociativity.Left, OperatorArity.Binary));

        var exception = Record.Exception(() => new BinaryOperatorSyntaxNode(Token, @operator, LeftNode, RightNode));

        Assert.NotNull(exception);
        Assert.IsType<InvalidOperationException>(exception);
        Assert.Equal("Operator value and token value must equals", exception.Message);
    }

    public static TheoryData<Token, Operator, SyntaxNode, SyntaxNode> ConstructorInputsDifferentCases
    {
        get
        {
            {
                var data = new TheoryData<Token, Operator, SyntaxNode, SyntaxNode>();

                const string otherInput = "2 + 1";

                var left = new NumberSyntaxNode(otherInput.ToToken(4, 5));
                data.Add(Token, Operator, left, RightNode);

                var right = new NumberSyntaxNode(otherInput.ToToken(0, 1));
                data.Add(Token, Operator, LeftNode, right);
                
                return data;
            }
        }
    }
    
    [Theory]
    [MemberData(nameof(ConstructorInputsDifferentCases))]
    public void Constructor_ShouldThrowInvalidOperationException_WhenInputsDifferent(
        Token token, Operator @operator, SyntaxNode left, SyntaxNode right)
    {
        var exception = Record.Exception(() => new BinaryOperatorSyntaxNode(token, @operator, left, right));

        Assert.NotNull(exception);
        Assert.IsType<InvalidOperationException>(exception);
        Assert.Equal("Inputs must be equals", ((InvalidOperationException)exception).Message);
    }

    public static TheoryData<SyntaxNode, SyntaxNode, string> ConstructorOperandOnIncorrectSideCases =>
        new()
        {
            { LeftNode, LeftNode, "right" },
            { RightNode, RightNode, "left" }
        };
    
    [Theory]
    [MemberData(nameof(ConstructorOperandOnIncorrectSideCases))]
    public void Constructor_ShouldThrowArgumentException_WhenOperandOnIncorrectSide(
        SyntaxNode left, SyntaxNode right, string parameterName)
    {
        var exception = Record.Exception(() => new BinaryOperatorSyntaxNode(Token, Operator, left, right));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Contains("operator must be on", exception.Message);
        Assert.Equal(parameterName, ((ArgumentException)exception).ParamName);
    }
}