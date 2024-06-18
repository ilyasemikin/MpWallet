using MpWallet.Expressions.Parsing.Parser.Abstractions;

namespace MpWallet.Expressions.Parsing.UnitTests.Parser.Abstractions.Base;

public abstract class ExpressionParserBaseTests<TExpressionParser>
    where TExpressionParser : IExpressionParser
{
    protected TExpressionParser Parser { get; }

    protected ExpressionParserBaseTests(Func<TExpressionParser> factory)
    {
        Parser = factory();
    }
}