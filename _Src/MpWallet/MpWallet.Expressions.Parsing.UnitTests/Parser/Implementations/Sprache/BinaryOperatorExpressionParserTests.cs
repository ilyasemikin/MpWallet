using MpWallet.Expressions.Parsing.Sprache.Parser.Implementations;
using MpWallet.Expressions.Parsing.UnitTests.Parser.Abstractions;

namespace MpWallet.Expressions.Parsing.UnitTests.Parser.Implementations.Sprache;

public class BinaryOperatorExpressionParserTests : BinaryOperatorExpressionParserTests<ExpressionParser>
{
    public BinaryOperatorExpressionParserTests() 
        : base(() => new ExpressionParser())
    {
    }
}