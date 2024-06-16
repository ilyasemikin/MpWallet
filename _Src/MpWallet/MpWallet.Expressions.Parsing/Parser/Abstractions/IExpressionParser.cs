using MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;

namespace MpWallet.Expressions.Parsing.Parser.Abstractions;

public interface IExpressionParser
{
    SyntaxNode Parse(string input);
}