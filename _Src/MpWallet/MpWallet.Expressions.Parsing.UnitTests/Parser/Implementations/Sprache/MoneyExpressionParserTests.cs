using MpWallet.Expressions.Parsing.Sprache.Parser.Implementations;
using MpWallet.Expressions.Parsing.UnitTests.Parser.Abstractions;

namespace MpWallet.Expressions.Parsing.UnitTests.Parser.Implementations.Sprache;

public sealed class MoneyExpressionParserTests : MoneyExpressionParserTests<ExpressionParser>
{
    public MoneyExpressionParserTests() 
        : base(() => new ExpressionParser())
    {
    }
}