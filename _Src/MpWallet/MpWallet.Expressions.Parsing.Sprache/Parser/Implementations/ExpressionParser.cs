using MpWallet.Expressions.Parsing.Parser.Abstractions;
using MpWallet.Expressions.Parsing.Sprache.Parser.Implementations.Nodes;
using MpWallet.Expressions.Parsing.Sprache.Parser.Implementations.Nodes.Abstractions;
using MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;
using MpWallet.Values.Implementations;
using Sprache;
using SpracheLib = Sprache;

namespace MpWallet.Expressions.Parsing.Sprache.Parser.Implementations;

public class ExpressionParser : IExpressionParser
{
    private Parser<ParserNode> NumberParser =>
    (
        from sign in SpracheLib.Parse.Char('-').Token().Optional()
        from value in SpracheLib.Parse.DecimalInvariant.Token()
        select new NumberParserNode()
    ).Positioned();

    private Parser<ParserNode> MoneyParser =>
    (
        from money in SpracheLib.Parse.RegexMatch(Money.PatternRegex).Token()
        select new MoneyParserNode()
    ).Positioned();

    private Parser<ParserNode> ExpressionInParenthesesParser =>
        from left in SpracheLib.Parse.Char('(')
        from expr in Parser
        from right in SpracheLib.Parse.Char(')')
        select expr;

    private Parser<ParserNode> Parser => MoneyParser.Or(NumberParser).XOr(ExpressionInParenthesesParser);

    public ExpressionParser()
    {
    }

    public SyntaxNode Parse(string input)
    {
        return Parser
            .Parse(input)
            .ToSyntaxNode(input);
    }
}