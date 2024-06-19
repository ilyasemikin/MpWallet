using System.Globalization;
using MpWallet.Currencies;
using MpWallet.Expressions.Parsing.Parser.Abstractions;
using MpWallet.Expressions.Parsing.Syntax;
using MpWallet.Expressions.Parsing.Syntax.Nodes;
using MpWallet.Expressions.Parsing.Syntax.Nodes.Services;
using MpWallet.Expressions.Parsing.UnitTests.Parser.Abstractions.Base;

namespace MpWallet.Expressions.Parsing.UnitTests.Parser.Abstractions;

public abstract class MoneyExpressionParserTests<TExpressionParser> : ExpressionParserBaseTests<TExpressionParser>
    where TExpressionParser : IExpressionParser
{
    protected MoneyExpressionParserTests(Func<TExpressionParser> factory) 
        : base(factory)
    {
    }

    public static IEnumerable<object[]> ParseSuccessCases
    {
        get
        {
            {
                object[] CreateCase(
                    string input, 
                    Func<string, int>? beginGetter = null, 
                    Func<string, int>? endGetter = null)
                {
                    var begin = beginGetter?.Invoke(input) ?? 0;
                    var end = endGetter?.Invoke(input) ?? input.Length;
                    
                    var token = new Token(input, begin, end);
                    var expected = new MoneySyntaxNode(token);

                    return [input, expected];
                }
                
                var values = new[]
                {
                    -123,
                    123,
                    -123.5M,
                    123.5M
                };
                
                foreach (var currency in Currency.All)
                foreach (var value in values)
                {
                    var valueString = value.ToString(CultureInfo.InvariantCulture);
                    
                    yield return CreateCase(valueString + currency.Code);
                    yield return CreateCase(valueString + currency.Symbol);
                    yield return CreateCase(valueString + " " + currency.Code);
                    yield return CreateCase(valueString + " " + currency.Symbol);
                    
                    yield return CreateCase("(" + valueString + currency.Code + ")", _ => 1, i => i.Length - 1);
                    yield return CreateCase("(" + valueString + currency.Symbol + ")", _ => 1, i => i.Length - 1);
                    yield return CreateCase("(" + valueString + " " + currency.Code + ")", _ => 1, i => i.Length - 1);
                    yield return CreateCase("(" + valueString + " " + currency.Symbol + ")", _ => 1, i => i.Length - 1);
                }
            }
        }
    }

    [Theory]
    [MemberData(nameof(ParseSuccessCases))]
    public void Parse_ShouldSuccess_WhenInputValid(string input, MoneySyntaxNode expected)
    {
        var actual = Parser.Parse(input);

        Assert.NotNull(expected);
        Assert.IsType<MoneySyntaxNode>(actual);
        Assert.Equal(expected, actual, SyntaxNodeAbsoluteEqualityComparer.Instance);
    }
}