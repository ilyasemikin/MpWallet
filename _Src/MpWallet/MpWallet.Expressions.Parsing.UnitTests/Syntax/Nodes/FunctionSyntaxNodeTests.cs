using MpWallet.Expressions.Parsing.Syntax;
using MpWallet.Expressions.Parsing.Syntax.Nodes;
using MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;

namespace MpWallet.Expressions.Parsing.UnitTests.Syntax.Nodes;

public class FunctionSyntaxNodeTests
{
    private const string Input = "func(a, b, c)";
    private const string ArgumentInput = "a";

    private static readonly Token Token = new(Input, 0, Input.Length);
    private static readonly Token ArgumentToken = new(ArgumentInput, 0, ArgumentInput.Length);
    
    public static IEnumerable<object?[]> ConstructorSuccessCases
    {
        get
        {
            var argumentNode = new VariableSyntaxNode(ArgumentToken);
            
            yield return [Token, "func", null];
            yield return [Token, "func", new[] { argumentNode }];
            yield return [Token, "func", new[] { argumentNode, argumentNode }];
        }
    }
    
    [Theory]
    [MemberData(nameof(ConstructorSuccessCases))]
    public void Constructor_ShouldSuccess_WhenArgumentsCorrect(
        Token token, string name, IReadOnlyList<SyntaxNode>? arguments)
    {
        var node = new FunctionSyntaxNode(token, name, arguments);

        Assert.NotNull(node);
        Assert.Equal(token, node.Token);
        Assert.Equal(name, node.Name);

        if (arguments is not null)
            Assert.Equal<IEnumerable<SyntaxNode>>(arguments, node.Arguments);
    }

    public static IEnumerable<object?[]> ConstructorInvalidNameCases
    {
        get
        {
            yield return [null];
            yield return [string.Empty];
            yield return ["\t"];
        }
    }
    
    [Theory]
    [MemberData(nameof(ConstructorInvalidNameCases))]
    public void Constructor_ShouldThrowArgumentException_WhenNameInvalid(string name)
    {
        var exception = Record.Exception(() => new FunctionSyntaxNode(Token, name));

        Assert.NotNull(exception);
        Assert.IsAssignableFrom<ArgumentException>(exception);
        Assert.Equal("name", ((ArgumentException)exception).ParamName);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenArgumentsHasNull()
    {
        var exception = Record.Exception(() => new FunctionSyntaxNode(Token, "func", new SyntaxNode[] { null! }));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
    }
}