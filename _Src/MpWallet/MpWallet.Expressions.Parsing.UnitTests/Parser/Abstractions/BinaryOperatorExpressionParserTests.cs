using MpWallet.Expressions.Operators;
using MpWallet.Expressions.Operators.Collections.Extensions;
using MpWallet.Expressions.Parsing.Parser.Abstractions;
using MpWallet.Expressions.Parsing.Syntax;
using MpWallet.Expressions.Parsing.Syntax.Nodes;
using MpWallet.Expressions.Parsing.Syntax.Nodes.Services;
using MpWallet.Expressions.Parsing.UnitTests.Parser.Abstractions.Base;

namespace MpWallet.Expressions.Parsing.UnitTests.Parser.Abstractions;

public abstract class BinaryOperatorExpressionParserTests<TExpressionParser> : ExpressionParserBaseTests<TExpressionParser>
    where TExpressionParser : IExpressionParser
{
    protected BinaryOperatorExpressionParserTests(Func<TExpressionParser> factory) : base(factory)
    {
    }

    public static IEnumerable<object[]> ParseSuccessCases
    {
        get
        {
            foreach (var @operator in Operator.All)
            {
                var input = $"1 {@operator.Value} 1";

                var leftToken = new Token(input, 0, 1);
                var left = new NumberSyntaxNode(leftToken);

                var rightToken = new Token(input, 4, 5);
                var right = new NumberSyntaxNode(rightToken);

                var token = new Token(input, 2, 3);
                var expected = new BinaryOperatorSyntaxNode(token, @operator, left, right);
                
                yield return [input, expected];
            }

            foreach (var @operator in Operator.All)
            {
                var input = $"1$ {@operator.Value} 1$";

                var leftToken = new Token(input, 0, 2);
                var left = new MoneySyntaxNode(leftToken);

                var rightToken = new Token(input, 5, 7);
                var right = new MoneySyntaxNode(rightToken);

                var token = new Token(input, 3, 4);
                var expected = new BinaryOperatorSyntaxNode(token, @operator, left, right);

                yield return [input, expected];
            }

            foreach (var @operator in Operator.All)
            {
                var input = $"a {@operator.Value} b";

                var leftToken = new Token(input, 0, 1);
                var left = new VariableSyntaxNode(leftToken);

                var rightToken = new Token(input, 4, 5);
                var right = new VariableSyntaxNode(rightToken);

                var token = new Token(input, 2, 3);
                var expected = new BinaryOperatorSyntaxNode(token, @operator, left, right);

                yield return [input, expected];
            }

            foreach (var @operator in Operator.All)
            {
                var input = $"(1 {@operator.Value} 1)";

                var leftToken = new Token(input, 1, 2);
                var left = new NumberSyntaxNode(leftToken);

                var rightToken = new Token(input, 5, 6);
                var right = new NumberSyntaxNode(rightToken);

                var token = new Token(input, 3, 4);
                var expected = new BinaryOperatorSyntaxNode(token, @operator, left, right);

                yield return [input, expected];
            }

            var addition = Operator.All.Get("+", OperatorArity.Binary);
            var multiplication = Operator.All.Get("*", OperatorArity.Binary);
            
            {
                const string input = "1 + 2 * 3";

                var expected = new BinaryOperatorSyntaxNode(new Token(input, 2, 3), addition,
                    new NumberSyntaxNode(new Token(input, 0, 1)),
                    new BinaryOperatorSyntaxNode(new Token(input, 6, 7), multiplication,
                        new NumberSyntaxNode(new Token(input, 4, 5)),
                        new NumberSyntaxNode(new Token(input, 8, 9))));
                
                yield return [input, expected];
            }

            {
                const string input = "(1 + 2) * 3";

                var expected = new BinaryOperatorSyntaxNode(new Token(input, 8, 9), multiplication,
                    new BinaryOperatorSyntaxNode(new Token(input, 3, 4), addition,
                        new NumberSyntaxNode(new Token(input, 1, 2)),
                        new NumberSyntaxNode(new Token(input, 5, 6))),
                    new NumberSyntaxNode(new Token(input, 10, 11)));

                yield return [input, expected];
            }
        }
    }

    [Theory]
    [MemberData(nameof(ParseSuccessCases))]
    public void Parse_ShouldSuccess_WhenInputValid(string input, BinaryOperatorSyntaxNode expected)
    {
        var node = Parser.Parse(input);

        Assert.NotNull(node);
        Assert.IsType<BinaryOperatorSyntaxNode>(expected);
        Assert.Equal(expected, node, SyntaxNodeAbsoluteEqualityComparer.Instance);
    }
}