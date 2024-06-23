using MpWallet.Expressions.Parsing.Sprache.Parser.Implementations;
using MpWallet.Expressions.Parsing.UnitTests.Parser.Abstractions;

namespace MpWallet.Expressions.Parsing.UnitTests.Parser.Implementations.Sprache;

public sealed class FunctionExpressionParserTests : FunctionExpressionParserTests<ExpressionParser>
{
    public FunctionExpressionParserTests() 
        : base(() => new ExpressionParser())
    {
    }
}