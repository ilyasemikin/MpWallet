using MpWallet.Expressions.Parsing.Parser.Abstractions;
using MpWallet.Expressions.Parsing.Syntax.Extensions;
using MpWallet.Expressions.Parsing.Syntax.Nodes;
using MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;

namespace MpWallet.Expressions.Compilation.UnitTests.Compiler.Mocks;

internal sealed class InputSavedExpressionParserMock : IExpressionParser
{
    public string? Input { get; private set; }
        
    public SyntaxNode Parse(string input)
    {
        Input = input;
        var token = "123".ToToken();
        return new NumberSyntaxNode(token);
    }
}