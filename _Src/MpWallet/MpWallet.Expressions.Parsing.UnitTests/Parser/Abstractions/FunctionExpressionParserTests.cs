using MpWallet.Expressions.Operators;
using MpWallet.Expressions.Parsing.Parser.Abstractions;
using MpWallet.Expressions.Parsing.Syntax;
using MpWallet.Expressions.Parsing.Syntax.Extensions;
using MpWallet.Expressions.Parsing.Syntax.Nodes;
using MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;
using MpWallet.Expressions.Parsing.Syntax.Nodes.Comparers;
using MpWallet.Expressions.Parsing.UnitTests.Parser.Abstractions.Base;
using MpWallet.Operators;
using MpWallet.Operators.Collections.Extensions;

namespace MpWallet.Expressions.Parsing.UnitTests.Parser.Abstractions;

public abstract class FunctionExpressionParserTests<TExpressionParser> : ExpressionParserBaseTests<TExpressionParser>
    where TExpressionParser : IExpressionParser
{
    protected FunctionExpressionParserTests(Func<TExpressionParser> factory) 
        : base(factory)
    {
    }

    public static IEnumerable<object[]> ParseSuccessCases
    {
        get
        {
            {
                const string input = "func()";

                var token = new Token(input, 0, input.Length);
                var expected = new FunctionSyntaxNode(token, "func");
                yield return [input, expected];
            }

            {
                const string input = "func(a)";

                var variableToken = new Token(input, 5, 6);
                var variable = new VariableSyntaxNode(variableToken);

                var token = new Token(input, 0, input.Length);
                var expected = new FunctionSyntaxNode(token, "func", new[] { variable });
                yield return [input, expected];
            }

            {
                const string input = "func(a, b)";

                var token = new Token(input, 0, input.Length);
                var expected = new FunctionSyntaxNode(token, "func", new[]
                {
                    new VariableSyntaxNode(new Token(input, 5, 6)),
                    new VariableSyntaxNode(new Token(input, 8, 9))
                });

                yield return [input, expected];
            }

            var addition = DefaultOperators.Collection.Get("+", OperatorArity.Binary);
            
            {
                const string input = "func(a + b)";

                var token = new Token(input, 0, input.Length);
                var expected = new FunctionSyntaxNode(token, "func", new[]
                {
                    new BinaryOperatorSyntaxNode(new Token(input, 7, 8), addition,
                        new VariableSyntaxNode(new Token(input, 5, 6)),
                        new VariableSyntaxNode(new Token(input, 9, 10)))
                });

                yield return [input, expected];
            }

            {
                const string input = "func(func())";

                var expected = new FunctionSyntaxNode(input.ToToken(), "func", new[]
                {
                    new FunctionSyntaxNode(input.ToToken(5, 11), "func")
                });

                yield return [input, expected];
            }

            {
                const string input = "func(func(), a)";

                var expected = new FunctionSyntaxNode(input.ToToken(), "func", new SyntaxNode[]
                {
                    new FunctionSyntaxNode(input.ToToken(5, 11), "func"),
                    new VariableSyntaxNode(input.ToToken(13, 14))
                });

                yield return [input, expected];
            }

            {
                const string input = "func(a, func())";
                
                var expected = new FunctionSyntaxNode(input.ToToken(), "func", new SyntaxNode[]
                {
                    new VariableSyntaxNode(input.ToToken(5, 6)),
                    new FunctionSyntaxNode(input.ToToken(8, 14), "func"),
                });

                yield return [input, expected];
            }
        }
    }

    [Theory]
    [MemberData(nameof(ParseSuccessCases))]
    public void Parse_ShouldSuccess_WhenInputValid(string input, FunctionSyntaxNode expected)
    {
        var node = Parser.Parse(input);

        Assert.NotNull(node);
        Assert.IsType<FunctionSyntaxNode>(node);
        Assert.Equal(expected, node, SyntaxNodeAbsoluteEqualityComparer.Instance);
    }
}