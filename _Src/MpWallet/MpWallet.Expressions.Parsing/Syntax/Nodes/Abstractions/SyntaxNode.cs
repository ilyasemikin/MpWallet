namespace MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;

public abstract record SyntaxNode
{
    public Token Token { get; }
    
    protected internal SyntaxNode(Token token)
    {
        ArgumentNullException.ThrowIfNull(token);
        
        Token = token;
    }
}