using System.Globalization;
using MpWallet.Expressions.Parsing.Parser.Abstractions;
using MpWallet.Expressions.Parsing.Syntax;
using MpWallet.Expressions.Parsing.Syntax.Nodes;
using MpWallet.Expressions.Parsing.Syntax.Nodes.Comparers;
using MpWallet.Expressions.Parsing.UnitTests.Parser.Abstractions.Base;

namespace MpWallet.Expressions.Parsing.UnitTests.Parser.Abstractions;

public abstract class NumberExpressionParserTests<TExpressionParser> : ExpressionParserBaseTests<TExpressionParser>
    where TExpressionParser : IExpressionParser
{
    protected NumberExpressionParserTests(Func<TExpressionParser> factory)
        : base(factory)
    {
    }
    
    public static IEnumerable<object[]> ParseSuccessCases
    {
        get
        {
            {
                var values = new[]
                {
                    -123,
                    123,
                    -123.5M,
                    123.5M,
                };

                foreach (var value in values)
                {
                    var input = value.ToString(CultureInfo.InvariantCulture);
                    var token = new Token(input, 0, input.Length);

                    var expected = new NumberSyntaxNode(token);

                    yield return [input, expected];
                }

                foreach (var value in values)
                {
                    var input = "(" + value.ToString(CultureInfo.InvariantCulture) + ")";
                    var token = new Token(input, 1, input.Length - 1);

                    var expected = new NumberSyntaxNode(token);

                    yield return [input, expected];
                }
                
                foreach (var value in values)
                {
                    var input = "((" + value.ToString(CultureInfo.InvariantCulture) + "))";
                    var token = new Token(input, 2, input.Length - 2);

                    var expected = new NumberSyntaxNode(token);

                    yield return [input, expected];
                }
            }

            {
                const string input = "  123.34 ";
                var token = new Token(input, 2, 8);
                var node = new NumberSyntaxNode(token);

                yield return [input, node];
            }
        }
    }

    [Theory]
    [MemberData(nameof(ParseSuccessCases))]
    public void Parse_ShouldSuccess_WhenInputValid(string input, NumberSyntaxNode expected)
    {
        var actual = Parser.Parse(input);

        Assert.NotNull(actual);
        Assert.IsType<NumberSyntaxNode>(actual);
        Assert.Equal(expected, actual, SyntaxNodeAbsoluteEqualityComparer.Instance);
    }
}