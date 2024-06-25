using MpWallet.Expressions.Parsing.Sprache.Parser.Implementations;
using MpWallet.Expressions.Parsing.UnitTests.Parser.Abstractions;

namespace MpWallet.Expressions.Parsing.Sprache.UnitTests.Parser;

public sealed class VariableExpressionParserTests : VariableExpressionParserTests<ExpressionParser>
{
    public VariableExpressionParserTests() 
        : base(() => new ExpressionParser())
    {
    }
}