using MpWallet.Expressions.Context.Variables;
using MpWallet.Expressions.Operators;
using MpWallet.Expressions.Operators.Collections;
using MpWallet.Expressions.Parsing.Parser.Abstractions;
using MpWallet.Expressions.Parsing.Sprache.Parser.Implementations.Nodes;
using MpWallet.Expressions.Parsing.Sprache.Parser.Implementations.Nodes.Abstractions;
using MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;
using MpWallet.Values.Implementations;
using Sprache;
using SpracheLib = Sprache;

namespace MpWallet.Expressions.Parsing.Sprache.Parser.Implementations;

public sealed class ExpressionParser : IExpressionParser
{
    private readonly OperatorsCollection _operators;
    
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

    private Parser<char> FunctionArgumentsDelimiterParser =>
        SpracheLib.Parse
            .Char(',')
            .Token();

    private Parser<IEnumerable<ParserNode>> FunctionArgumentsParser =>
        from left in SpracheLib.Parse.Char('(')
        from expr in Parser.DelimitedBy(FunctionArgumentsDelimiterParser).Optional()
        from right in SpracheLib.Parse.Char(')')
        select expr.GetOrElse([]);
    
    private Parser<ParserNode> TermParser =>
    (
        from name in SpracheLib.Parse.Regex(Variable.NameRegexPattern)
        from arguments in FunctionArgumentsParser.Optional()
        select new TermParsingNode(name, arguments.GetOrDefault())
    ).Positioned();

    private Parser<ParserNode> BinaryOperatorParser =>
        CreateBinaryOperatorParser(
            TermParser.Or(MoneyParser).Or(NumberParser).XOr(ExpressionInParenthesesParser),
            _operators);

    private Parser<ParserNode> ExpressionInParenthesesParser =>
        from left in SpracheLib.Parse.Char('(')
        from expr in Parser
        from right in SpracheLib.Parse.Char(')')
        select expr;

    private Parser<ParserNode> Parser => BinaryOperatorParser;
    
    private Parser<ParserNode> PrecalculatedParser { get; }

    public ExpressionParser(OperatorsCollection? operators = null)
    {
        _operators = operators ?? Operator.All;

        PrecalculatedParser = Parser;
    }

    public SyntaxNode Parse(string input)
    {
        return PrecalculatedParser
            .End()
            .Parse(input)
            .ToSyntaxNode(input);
    }

    private static Parser<ParserNode> CreateBinaryOperatorParser(Parser<ParserNode> @base, OperatorsCollection operators)
    {
        var groups = operators
            .Where(@operator => @operator.Details.Arity is OperatorArity.Binary)
            .GroupBy(@operator => @operator.Details.Priority)
            .OrderByDescending(group => group.Key);

        foreach (var group in groups)
        {
            var left = new List<Parser<OperatorParserNode>>();
            var right = new List<Parser<OperatorParserNode>>();

            foreach (var @operator in group)
            {
                var parser = CreateOperatorParser(@operator);

                switch (@operator.Details.Associativity)
                {
                    case OperatorAssociativity.Left:
                        left.Add(parser);
                        break;
                    case OperatorAssociativity.Right:
                        right.Add(parser);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            if (left.Count > 0)
            {
                var parser = AggregateParser(left);
                @base = SpracheLib.Parse.ChainOperator(parser, @base, CreateBinaryOperator);
            }

            if (right.Count > 0)
            {
                var parser = AggregateParser(right);
                @base = SpracheLib.Parse.ChainRightOperator(parser, @base, CreateBinaryOperator);
            }
        }

        return @base;

        static Parser<OperatorParserNode> CreateOperatorParser(Operator @operator)
        {
            return SpracheLib.Parse
                .String(@operator.Value)
                .Token()
                .Text()
                .Select(_ => new OperatorParserNode(@operator))
                .Positioned();
        }

        static Parser<OperatorParserNode> AggregateParser(IEnumerable<Parser<OperatorParserNode>> parsers)
        {
            return parsers.Aggregate((last, current) => last.Or(current));
        }

        static BinaryOperatorParserNode CreateBinaryOperator(
            OperatorParserNode @operator, ParserNode left, ParserNode right)
        {
            return new BinaryOperatorParserNode(@operator, left, right);
        }
    }
}