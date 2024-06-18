using MpWallet.Expressions.Parsing.Parser.Abstractions;
using MpWallet.Expressions.Parsing.Parser.Exceptions;
using MpWallet.Expressions.Parsing.Syntax;
using MpWallet.Expressions.Parsing.Syntax.Nodes;
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
                var inputs = new string[]
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
        Assert.Equal(expected, actual);
    }
    
    public static IEnumerable<object[]> ParseFailureCases
    {
        get
        {
            yield break;
        }
    }
    
    [Theory]
    [MemberData(nameof(ParseFailureCases))]
    public void Parse_ShouldThrowException_WhenInputInvalid(string input)
    {
        var exception = Record.Exception(() => Parser.Parse(input));

        Assert.NotNull(exception);
        Assert.IsAssignableFrom<ExpressionParseException>(exception);
    }
}