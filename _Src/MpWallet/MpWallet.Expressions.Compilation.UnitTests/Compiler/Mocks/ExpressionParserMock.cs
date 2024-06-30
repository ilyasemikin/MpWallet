using MpWallet.Expressions.Parsing.Parser.Abstractions;
using MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;

namespace MpWallet.Expressions.Compilation.UnitTests.Compiler.Mocks;

internal sealed class ExpressionParserMock : IExpressionParser
{
    private readonly SyntaxNode _result;

    public ExpressionParserMock(SyntaxNode result)
    {
        _result = result;
    }
    
    public SyntaxNode Parse(string input)
    {
        return _result;
    }
}