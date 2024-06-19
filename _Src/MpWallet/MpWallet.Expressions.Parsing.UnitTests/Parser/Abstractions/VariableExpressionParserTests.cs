using MpWallet.Expressions.Parsing.Parser.Abstractions;
using MpWallet.Expressions.Parsing.Syntax;
using MpWallet.Expressions.Parsing.Syntax.Nodes;
using MpWallet.Expressions.Parsing.Syntax.Nodes.Services;
using MpWallet.Expressions.Parsing.UnitTests.Parser.Abstractions.Base;

namespace MpWallet.Expressions.Parsing.UnitTests.Parser.Abstractions;

public abstract class VariableExpressionParserTests<TExpressionParser> : ExpressionParserBaseTests<TExpressionParser>
    where TExpressionParser : IExpressionParser
{
    public VariableExpressionParserTests(Func<TExpressionParser> factory) 
        : base(factory)
    {
    }

    public static IEnumerable<object[]> ParseSuccessCase
    {
        get
        {
            {
                var inputs = new[]
                {
                    "a",
                    "abc",
                    "ABC",
                    "a123",
                    "x123"
                };

                foreach (var input in inputs)
                {
                    var token = new Token(input, 0, input.Length);
                    var expected = new VariableSyntaxNode(token);
                    yield return [input, expected];
                }
            }
        }
    }

    [Theory]
    [MemberData(nameof(ParseSuccessCase))]
    public void Parse_ShouldSuccess_WhenInputValid(string input, VariableSyntaxNode expected)
    {
        var actual = Parser.Parse(input);

        Assert.NotNull(actual);
        Assert.IsType<VariableSyntaxNode>(actual);
        Assert.Equal(expected, actual, SyntaxNodeAbsoluteEqualityComparer.Instance);
    }
}