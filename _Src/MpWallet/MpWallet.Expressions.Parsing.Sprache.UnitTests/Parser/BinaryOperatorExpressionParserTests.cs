using MpWallet.Expressions.Parsing.Sprache.Parser.Implementations;
using MpWallet.Expressions.Parsing.UnitTests.Parser.Abstractions;

namespace MpWallet.Expressions.Parsing.Sprache.UnitTests.Parser;

public sealed class BinaryOperatorExpressionParserTests : BinaryOperatorExpressionParserTests<ExpressionParser>
{
    public BinaryOperatorExpressionParserTests() 
        : base(() => new ExpressionParser())
    {
    }
}