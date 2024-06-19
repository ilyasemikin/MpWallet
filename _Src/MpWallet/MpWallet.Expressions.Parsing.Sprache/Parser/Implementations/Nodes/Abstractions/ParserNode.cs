using MpWallet.Expressions.Parsing.Syntax;
using MpWallet.Expressions.Parsing.Syntax.Extensions;
using MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;
using Sprache;

namespace MpWallet.Expressions.Parsing.Sprache.Parser.Implementations.Nodes.Abstractions;

internal abstract record ParserNode : IPositionAware<ParserNode>
{
    public int? Index { get; init; }
    public int? Length { get; init; }

    public Token ToToken(string input)
    {
        if (Index is null || Length is null)
            throw new InvalidOperationException();

        var token = new Token(input, Index.Value, Index.Value + Length.Value);
        return token.Trim();
    }
    
    public abstract SyntaxNode ToSyntaxNode(string input);
    
    public abstract ParserNode SetPos(Position startPos, int length);
}