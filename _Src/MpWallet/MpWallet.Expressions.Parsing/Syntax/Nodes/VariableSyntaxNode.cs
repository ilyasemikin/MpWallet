using MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;
using MpWallet.Tokens;

namespace MpWallet.Expressions.Parsing.Syntax.Nodes;

public sealed record VariableSyntaxNode : SyntaxNode
{
    public string Name => Token.Value;
    
    public VariableSyntaxNode(Token token) : base(token)
    {
    }
}